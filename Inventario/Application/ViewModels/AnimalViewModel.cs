using Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Util.Enums;

namespace Application.ViewModels
{
    public class AnimalViewModel
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
        public Guid? RaceId { get; set; }
        public string Number { get; set; }
        public eSex Sex { get; set; }
        public string LeftProfileImg { get; set; }
        public string FrontProfileImg { get; set; }
        public string RightProfileImg { get; set; }
        public string BirthObservations { get; set; }
        public bool Alive { get; set; }
        public RaceViewModel Race { get; set; }
        public IEnumerable<AnimalDiseaseViewModel> AnimalDiseases { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
    }
}
