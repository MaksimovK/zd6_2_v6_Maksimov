using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CollegeApp.Models
{
    public class StudentSpecialization
    {
        [PrimaryKey, AutoIncrement]
        public int StudentSpecializationId { get; set; }
        public int StudentId { get; set; }
        public int SpecializationId { get; set; }
    }
}
