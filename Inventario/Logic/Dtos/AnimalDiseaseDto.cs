using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class AnimalDiseaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModificationAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid AnimalId { get; set; }
        public AnimalDto Animal { get; set; }
        public Guid DiseaseId { get; set; }
        public DiseaseDto Disease { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Treatement { get; set; }
        public string Observations { get; set; }
    }
}
