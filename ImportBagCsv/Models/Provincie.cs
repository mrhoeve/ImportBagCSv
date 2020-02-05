using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImportBagCsv.Models
{
    public class Provincie
    {
        [Key]
        public long Id { get; set; }
        public string Naam { get; set; }

        public ICollection<Gemeente> Gemeenten { get; set; }
    }
}
