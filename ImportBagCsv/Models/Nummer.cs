using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImportBagCsv.Models
{
    public class Nummer
    {
        [Key]
        public long Id { get; set; }
        public int Huisnummer { get; set; }
        public string Huisletter { get; set; }
        public string Huisnummertoevoeging { get; set; }
        public long AdresId { get; set; }
        public Adres Adres { get; set; }
    }
}
