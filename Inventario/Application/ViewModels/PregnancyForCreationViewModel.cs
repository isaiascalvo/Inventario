using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PregnancyForCreationViewModel
    {
        public Guid AnimalId { get; set; }
        public Guid? ProgenitorId { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public bool Finished { get; set; }
    }
}
