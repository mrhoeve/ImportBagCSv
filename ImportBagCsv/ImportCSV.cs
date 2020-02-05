using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
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
        private string _currentMelding;

        public ImportCSV()
        {
            _adressen = new Dictionary<string, IList<Adres>>(500009);
            _plaatsen = new Dictionary<string, IList<Plaats>>(7901);
            _gemeenten = new Dictionary<string, IList<Gemeente>>(601);
            _provincies = new Dictionary<string, Provincie>(13);
            _huidigAdres = null;
            _huidigePlaats = null;
            _currentMelding = "";
        }

        public void DoImport()
        {
            long counter = 0L;
            long curCounter = 0L;
            DateTime startTime;
            DateTime endTime;
            var engine = new FileHelperAsyncEngine<CsvModel>();

            // Read
            startTime=DateTime.Now;
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
                        // if (model.Postcode.Equals("7944TV") && model.Huisnummer == 151)
                        // {
                        //     LeegRegel();
                        //     Console.WriteLine(
                        //         $"Gevonden!\n{model.Straat} {model.Huisnummer}, {model.Postcode} {model.Woonplaats}, {model.Gemeente}, {model.Provincie}");
                        // }

                        counter++;
                        curCounter++;
                        if (curCounter == 99999L)
                        {
                            context.SaveChanges();
                            curCounter = 0L;
                        }
                    }
                    context.SaveChanges();
                }
                endTime=DateTime.Now;
                TimeSpan benodigdeTijd = endTime - startTime;
                int uren = benodigdeTijd.Hours;
                int minuten = benodigdeTijd.Minutes;
                int seconden = benodigdeTijd.Seconds % 60;
                LeegRegel();
                Console.WriteLine($"\rEr zijn {counter.ToString()} records gelezen in {uren} { (uren>1 ? "uren" : "uur" )}, {minuten} { (minuten >1 ? "minuten" : "minuut")} en {seconden} { (seconden > 1 ? "seconden" : "seconde")}!");
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
                        _adressen.Remove(postcode);
                        _adressen.Add(postcode, adressen);
                    }
                }
            }

            _huidigAdres = _adres;
            return _adres;
        }

        private Plaats getPlaats(Context _context, string plaatsNaam, string gemeenteNaam, string provincieNaam)
        {
            Plaats _plaats = null;

            if (_huidigePlaats != null && _huidigePlaats.Naam == plaatsNaam &&
                _huidigePlaats.Gemeente.Naam == gemeenteNaam && _huidigePlaats.Gemeente.Provincie.Naam == provincieNaam)
            {
                _plaats = _huidigePlaats;
            }

            if (_plaats == null)
            {
                List<Plaats> plaatsen = _plaatsen.GetValueOrDefault(plaatsNaam)?.ToList();
                if (plaatsen != null && plaatsen.Count > 0)
                {
                    foreach (Plaats plaats in plaatsen)
                    {
                        if (plaats.Naam == plaatsNaam && plaats.Gemeente.Naam == gemeenteNaam &&
                            plaats.Gemeente.Provincie.Naam == provincieNaam)
                        {
                            _plaats = plaats;
                            break;
                        }
                    }
                }

                if (_plaats == null)
                {
                    Gemeente _gemeente = GetGemeente(_context, gemeenteNaam, provincieNaam);
                    _plaats = new Plaats() {Naam = plaatsNaam, Gemeente = _gemeente};
                    _context.Plaatsen.Add(_plaats);
                    if (plaatsen == null)
                    {
                        _plaatsen.Add(plaatsNaam, new List<Plaats>() {_plaats});
                    }
                    else
                    {
                        plaatsen.Add(_plaats);
                        _plaatsen.Remove(plaatsNaam);
                        _plaatsen.Add(plaatsNaam, plaatsen);
                    }
                }

                _huidigePlaats = _plaats;
            }

            return _plaats;
        }

        private Gemeente GetGemeente(Context _context, string gemeenteNaam, string provincieNaam)
        {
            string NieuweMelding = "";
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
                Provincie _provincie = GetProvincie(_context, provincieNaam);
                _gemeente = new Gemeente() { Naam = gemeenteNaam, Provincie = _provincie };
                _context.Gemeenten.Add(_gemeente);
                if (gemeenten == null)
                {
                    _gemeenten.Add(gemeenteNaam, new List<Gemeente>() { _gemeente });
                } else
                {
                    gemeenten.Add(_gemeente);
                    _gemeenten.Remove(gemeenteNaam);
                    _gemeenten.Add(gemeenteNaam, gemeenten);
                }

                NieuweMelding = string.Format("Bezig met verwerken van {0} (NIEUW) in de provincie {1}...", gemeenteNaam, _gemeente.Provincie.Naam);
                Console.Write("\rBezig met verwerken van {0} (NIEUW) in de provincie {1}...");
            }
            else
            {
                NieuweMelding = string.Format("Bezig met verwerken van {0} in de provincie {1}...",
                    gemeenteNaam, _gemeente.Provincie.Naam);
            }
            LeegRegel(NieuweMelding);
            return _gemeente;
        }

        private Provincie GetProvincie(Context _context, string provincie)
        {
           Provincie _provincie = _provincies.GetValueOrDefault(provincie);
            if (_provincie == null)
            {
                _provincie = new Provincie() { Naam = provincie };
                _context.Provincies.Add(_provincie);
                _provincies.Add(provincie, _provincie);
            }
            return _provincie;
        }

        private void LeegRegel()
        {
            Console.Write(string.Format("\r{0}\r", "".PadRight(_currentMelding.Length)));
        }

        private void LeegRegel(string nieuweMelding)
        {
            LeegRegel();
            _currentMelding = nieuweMelding;
            Console.Write(_currentMelding);
        }
    }
}
