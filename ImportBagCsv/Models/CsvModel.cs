using System;
using System.Collections.Generic;
using System.Text;
using FileHelpers;

namespace ImportBagCsv.Models
{
    [DelimitedRecord(";")]
    [IgnoreFirst]
    public class CsvModel
    {
        public string Straat;
        public int Huisnummer;
        public string Huisletter;
        public string Huisnummertoevoeging;
        public string Postcode;
        public string Woonplaats;
        public string Gemeente;
        public string Provincie;
        public string Nummeraanduiding;
        public string Verblijfsobjectgebruiksdoel;
        public string Oppervlakteverblijfsobject;
        public string verblijfsobjectstatus;
        public string ObjectId;
        public string ObjectType;
        public string Nevenadres;
        public string Pandid;
        public string Pandstatus;
        public string Pandbouwjaar;
        public string x;
        public string y;
        public string lon;
        public string lat;
        public string verkorteopenbareruimte;
    }
}
