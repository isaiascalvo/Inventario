using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IEntity
    {
        Guid Id { get; set; }
        Guid CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        Guid? LastModificationBy { get; set; }
        DateTime? LastModificationAt { get; set; }
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        Guid? DeletedBy { get; set; }
    }
}
