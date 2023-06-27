using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CollegeApp.Models
{
    public class TeacherSpecialization
    {
        [PrimaryKey, AutoIncrement]
        public int TeacherSpecializationId { get; set; }
        public int TeacherId { get; set; }
        public int SpecialtyId { get; set; }
    }
}
