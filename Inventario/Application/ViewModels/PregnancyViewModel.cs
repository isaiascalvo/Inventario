using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class PregnancyViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModificationAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid AnimalId { get; set; }
        public Guid? ProgenitorId { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        public bool Finished { get; set; }
    }
}
