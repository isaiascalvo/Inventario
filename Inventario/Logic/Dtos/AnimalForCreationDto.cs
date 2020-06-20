using System;
using System.Collections.Generic;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class AnimalForCreationDto
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid? FatherId { get; set; }
        public Guid? MotherId { get; set; }
        public Guid SpeciesId { get; set; }
        public Guid RaceId { get; set; }
        public string Number { get; set; }
        public eSex Sex { get; set; }
        public string LeftProfileImg { get; set; }
        public string FrontProfileImg { get; set; }
        public string RightProfileImg { get; set; }
        public string BirthObservations { get; set; }
        public bool Alive { get; set; }
    }
}
