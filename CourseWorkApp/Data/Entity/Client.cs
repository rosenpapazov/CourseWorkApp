﻿using System.ComponentModel.DataAnnotations;

namespace CourseWorkApp.Data.Entity
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
    }
}
