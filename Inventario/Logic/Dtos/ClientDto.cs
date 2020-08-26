using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Dtos
{
    public class ClientDto: Entity
    {
        //public Guid Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? LastModificationAt { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Dni { get; set; }
        public string Phone { get; set; }
        public string? Mail { get; set; }
        public bool Debtor { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
