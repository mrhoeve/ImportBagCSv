using ImportBagCsv.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ImportBagCsv.Models
{
    public static class AdresBuilder
    {
        public static void Add(string straat, int huisnummer, string huisletter, string huisnummertoevoeging, string postcode,
            string plaats, string gemeente, string provincie, Context _context)
        {
            Provincie _provincie = _context.Provincies.FirstOrDefault(p => p.Naam == provincie);
            if (_provincie == null)
            {
                Console.WriteLine($"Bezig met verwerken van {provincie}...");
                _provincie = new Provincie() {Naam = provincie};
                _context.Provincies.Add(_provincie);
                _context.SaveChanges();
            }

            Gemeente _gemeente = _context.Gemeenten.Include(g => g.Plaatsen).FirstOrDefault(g => g.Naam == gemeente);
            if (_gemeente == null)
            {
                Console.WriteLine($"Bezig met verwerken van {gemeente}...");
                _gemeente = new Gemeente() {Naam = gemeente, Provincie = _provincie};
                _context.Gemeenten.Add(_gemeente);
                _context.SaveChanges();
            }

            Plaats _plaats = _gemeente.Plaatsen?.FirstOrDefault(p => p.Naam == plaats);
            if (_plaats == null)
            {
                _plaats = new Plaats() {Naam = plaats, Gemeente = _gemeente};
                _context.Plaatsen.Add(_plaats);
                _context.SaveChanges();
            }

            Adres _adres = _context.Adressen.Include(a => a.Nummers).FirstOrDefault(a => a.Plaats == _plaats && a.Straat == straat && a.Postcode == postcode);
            if (_adres == null)
            {
                _adres = new Adres() {Straat = straat, Postcode = postcode, Plaats = _plaats};
                _context.Adressen.Add(_adres);
                _context.SaveChanges();
            }

            Nummer _nummer = _adres.Nummers?.FirstOrDefault(n =>
                n.Huisnummer == huisnummer && n.Huisletter == huisletter &&
                n.Huisnummertoevoeging == huisnummertoevoeging);
            if (_nummer == null)
            {
                _nummer = new Nummer()
                    {Huisnummer = huisnummer, Huisletter = huisletter, Huisnummertoevoeging = huisnummertoevoeging, Adres = _adres};
                _context.Nummers.Add(_nummer);
            }
        }
    }
}
