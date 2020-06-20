using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Util.Enums;

namespace Data
{
    public class Animal: Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public Guid? FatherId { get; set; }
        public Animal Father { get; set; }
        public Guid? MotherId { get; set; }
        public Animal Mother { get; set; }
        [Required]
        public Guid RaceId { get; set; }
        public Race Race { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public eSex Sex { get; set; }
        public string LeftProfileImg { get; set; }
        public string FrontProfileImg { get; set; }
        public string RightProfileImg { get; set; }
        public string BirthObservations { get; set; }
        [Required]
        public bool Alive { get; set; }
    }
}
