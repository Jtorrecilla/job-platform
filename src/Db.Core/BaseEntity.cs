using System;

namespace Db.Core
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreationDate = DateTime.Now;
        }
        public T Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeletionDate { get; set; }

    }
}
