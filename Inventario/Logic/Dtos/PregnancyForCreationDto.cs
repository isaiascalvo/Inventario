using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class PregnancyForCreationDto
    {
        public Guid AnimalId { get; set; }
        public Guid? ProgenitorId { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public bool Finished { get; set; }
    }
}
