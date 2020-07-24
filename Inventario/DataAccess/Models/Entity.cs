using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class Entity: IEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? LastModificationBy { get; set; }
        public DateTime? LastModificationAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
