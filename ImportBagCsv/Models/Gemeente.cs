using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImportBagCsv.Models
{
    public class Gemeente
    {
        [Key]
        public long Id { get; set; }
        public string Naam { get; set; }
        public long ProvincieId { get; set; }

        public Provincie Provincie { get; set; }
        public ICollection<Plaats> Plaatsen { get; set; }
    }
}
