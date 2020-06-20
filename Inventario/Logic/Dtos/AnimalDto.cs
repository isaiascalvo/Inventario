using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Util.Enums;

namespace Logic.Dtos
{
    public class AnimalDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModificationAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
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
        public RaceDto Race { get; set; }
        public AnimalDto Mother { get; set; }
        public AnimalDto Father { get; set; }
        public IEnumerable<AnimalDiseaseDto> AnimalDiseases { get; set; }
    }
}
