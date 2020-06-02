using System;

namespace OSS.Business.DTOs
{
    public abstract class BaseDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public abstract bool IsValid();
    }
}
