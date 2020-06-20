using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class AnimalDiseaseForCreationDto
    {
        public Guid AnimalId { get; set; }
        public Guid DiseaseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Treatement { get; set; }
        public string Observations { get; set; }
    }
}
