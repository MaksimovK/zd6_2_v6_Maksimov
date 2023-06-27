using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using CollegeApp.Models;

namespace CollegeApp.Models
{
    public class Specialization
    {
        [PrimaryKey, AutoIncrement]
        public int SpecializationId { get; set; }
        public string Name { get; set; }
    }
}
