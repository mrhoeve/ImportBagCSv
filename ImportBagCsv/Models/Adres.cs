using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImportBagCsv.Models
{
    public class Adres
    {
        [Key]
        public long Id { get; set; }
        public string Straat { get; set; }
        public string Postcode { get; set; }
        public long PlaatsId { get; set; }

        public Plaats Plaats { get; set; }
        public ICollection<Nummer> Nummers { get; set; }
    }
}
