using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using ImportBagCsv.DAL;
using ImportBagCsv.Models;
using Microsoft.EntityFrameworkCore;

namespace ImportBagCsv
{
    public class ImportCSV
    {
        private Dictionary<string, IList<Plaats>> _plaatsen;
        private Dictionary<string, IList<Gemeente>> _gemeenten;
        private Dictionary<string, Provincie> _provincies;
        private Dictionary<string, IList<Adres>> _adressen;
        private Plaats _huidigePlaats;
        private Adres _huidigAdres;

        public ImportCSV()
        {
            _adressen = new Dictionary<string, IList<Adres>>();
            _plaatsen = new Dictionary<string, IList<Plaats>>();
            _gemeenten = new Dictionary<string, IList<Gemeente>>();
            _provincies = new Dictionary<string, Provincie>();
            _huidigAdres = null;
            _huidigePlaats = null;
        }

        public void DoImport()
        {
            long counter = 0L;
            long curCounter = 0L;

            var engine = new FileHelperAsyncEngine<CsvModel>();

            // Read
            using (engine.BeginReadFile(@"D:\Bag\bagadres-full.csv"))
            {
                using (var context = new Context())
                {
                    // The engine is IEnumerable
                    foreach (CsvModel model in engine)
                    {
                        Add(model.Straat, model.Huisnummer, model.Huisletter,
                            model.Huisnummertoevoeging,
                            model.Postcode, model.Woonplaats, model.Gemeente, model.Provincie, context);
                        if (model.Postcode.Equals("7944TV") && model.Huisnummer == 151)
                        {
                            Console.WriteLine(
                                $"Gevonden!\n{model.Straat} {model.Huisnummer}, {model.Postcode} {model.Woonplaats}, {model.Gemeente}, {model.Provincie}");
                        }

                        counter++;
                        curCounter++;
                        if (curCounter == 50000L)
                        {
                            context.SaveChanges();
                            curCounter = 0L;
                        }
                    }
                    context.SaveChanges();
                }

                Console.WriteLine($"Er zijn {counter.ToString()} records gelezen!");
            }


        }

        public void Add(string straat, int huisnummer, string huisletter, string huisnummertoevoeging, string postcode,
    string plaats, string gemeente, string provincie, Context _context)
        {


            Adres _adres = GetAdres(_context, straat, postcode, plaats, gemeente, provincie);

            Nummer _nummer = _adres.Nummers?.FirstOrDefault(n =>
                n.Huisnummer == huisnummer && n.Huisletter == huisletter &&
                n.Huisnummertoevoeging == huisnummertoevoeging);
            if (_nummer == null)
            {
                _nummer = new Nummer()
                { Huisnummer = huisnummer, Huisletter = huisletter, Huisnummertoevoeging = huisnummertoevoeging, Adres = _adres };
                _context.Nummers.Add(_nummer);
            }
        }

        private Adres GetAdres(Context _context, string straat, string postcode, string plaatsNaam, string gemeenteNaam, string provincieNaam)
        {
            Adres _adres = null;
            if (_huidigAdres != null)
            {
                if(_huidigAdres.Straat == straat && _huidigAdres.Postcode == postcode && _huidigAdres.Plaats.Naam == plaatsNaam && _huidigAdres.Plaats.Gemeente.Naam == gemeenteNaam && _huidigAdres.Plaats.Gemeente.Provincie.Naam == provincieNaam)
                {
                    _adres = _huidigAdres;
                }
            }

            if(_adres == null)
            {
                List<Adres> adressen = _adressen.GetValueOrDefault(postcode)?.ToList();
                if (adressen != null && adressen.Count > 0)
                {
                    foreach(Adres adres in adressen)
                    {
                        if(adres.Straat == straat && adres.Plaats.Naam == plaatsNaam && adres.Plaats.Gemeente.Naam == gemeenteNaam && adres.Plaats.Gemeente.Provincie.Naam == provincieNaam)
                        {
                            _adres = adres;
                        }
                    }
                }

                if (_adres == null)
                {
                    Plaats _plaats = getPlaats(_context, plaatsNaam, gemeenteNaam, provincieNaam);
                    _adres = new Adres() { Straat = straat, Postcode = postcode, Plaats = _plaats };
                    _context.Adressen.Add(_adres);
                    if (adressen == null)
                    {
                        _adressen.Add(postcode, new List<Adres>() { _adres });
                    }
                    else
                    {
                        adressen.Add(_adres);
                    }
                }
            }

            _huidigAdres = _adres;
            return _adres;
        }

        private Plaats getPlaats(Context _context, string plaatsNaam, string gemeenteNaam, string provincieNaam)
        {
            Plaats _plaats = null;
            List<Plaats> plaatsen = _plaatsen.GetValueOrDefault(plaatsNaam)?.ToList();
            if(plaatsen != null && plaatsen.Count>0)
            {
                foreach(Plaats plaats in plaatsen)
                {
                    if(plaats.Naam == plaatsNaam && plaats.Gemeente.Naam == gemeenteNaam && plaats.Gemeente.Provincie.Naam == provincieNaam)
                    {
                        _plaats = plaats;
                    }
                }
            }

            if (_plaats == null)
            {
                Gemeente _gemeente = GetGemeente(_context, gemeenteNaam, provincieNaam);
                _plaats = new Plaats() { Naam = plaatsNaam, Gemeente = _gemeente };
                _context.Plaatsen.Add(_plaats);
                if(plaatsen == null)
                {
                    _plaatsen.Add(plaatsNaam, new List<Plaats>() { _plaats });
                } else
                {
                    plaatsen.Add(_plaats);
                }
            }

            _huidigePlaats = _plaats;
            return _plaats;
        }

        private Gemeente GetGemeente(Context _context, string gemeenteNaam, string provincieNaam)
        {
            Gemeente _gemeente = null;
            List<Gemeente> gemeenten = _gemeenten.GetValueOrDefault(gemeenteNaam)?.ToList();
            if(gemeenten != null && gemeenten.Count > 0)
            {
                foreach(Gemeente gemeente in gemeenten)
                {
                    if(gemeente.Provincie.Naam == provincieNaam)
                    {
                        _gemeente = gemeente;
                    }
                }
            }
            
            if (_gemeente == null)
            {
                Console.WriteLine($"Bezig met verwerken van {gemeenteNaam}...");
                Provincie _provincie = GetProvincie(_context, provincieNaam);
                _gemeente = new Gemeente() { Naam = gemeenteNaam, Provincie = _provincie };
                _context.Gemeenten.Add(_gemeente);
                if (gemeenten == null)
                {
                    _gemeenten.Add(gemeenteNaam, new List<Gemeente>() { _gemeente });
                } else
                {
                    gemeenten.Add(_gemeente);
                }
            }

            return _gemeente;
        }

        private Provincie GetProvincie(Context _context, string provincie)
        {
           Provincie _provincie = _provincies.GetValueOrDefault(provincie);
            if (_provincie == null)
            {
                Console.WriteLine($"Bezig met verwerken van {provincie}...");
                _provincie = new Provincie() { Naam = provincie };
                _context.Provincies.Add(_provincie);
                _provincies.Add(provincie, _provincie);
            }
            return _provincie;
        }

    }
}
