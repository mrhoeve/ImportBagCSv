using System;
using System.Collections.Generic;
using System.Text;

namespace ImportBagCsv.Models
{
    public class Plaats
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public long GemeenteId { get; set; }

        public Gemeente Gemeente { get; set; }
        public ICollection<Adres> Adressen { get; set; }

    }
}
