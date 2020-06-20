using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class Pregnancy: Entity
    {
        [Required]
        public Guid AnimalId { get; set; }
        public Animal Animal { get; set; }
        public Guid? ProgenitorId { get; set; }
        public Animal Progenitor { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        [Required]
        public bool Finished { get; set; }
    }
}
