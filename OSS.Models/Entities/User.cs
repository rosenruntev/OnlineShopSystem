﻿namespace OSS.Models.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public decimal BankBalance { get; set; }
    }
}
