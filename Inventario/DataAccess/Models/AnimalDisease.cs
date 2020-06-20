using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data
{
    public class AnimalDisease: Entity
    {
        [Required]
        public Guid AnimalId { get; set; }
        public Animal Animal { get; set; }
        [Required]
        public Guid DiseaseId { get; set; }
        public Disease Disease { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Treatement { get; set; }
        public string Observations { get; set; }
    }
}
