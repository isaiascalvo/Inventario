using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class AnimalDiseaseForCreationViewModel
    {
        public Guid AnimalId { get; set; }
        public Guid DiseaseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Treatement { get; set; }
        public string Observations { get; set; }
    }
}
