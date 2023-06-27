using CollegeApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CollegeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentPage : ContentPage
    {
        private Student student;
        private CollegeDatabase _database;

        public StudentPage(Student student)
        {
            InitializeComponent();
            _database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "collegeDB"));

            this.student = student;
            Title = "Добро пожаловать, студент!";
            BindingContext = this.student;

            _ = LoadStudentSpecialtiesAsync();
            _ = LoadGroupNameAsync();
        }

        private async Task LoadStudentSpecialtiesAsync()
        {
            List<Specialization> studentSpecialties = await _database.GetStudentSpecialtiesAsync(student.StudentId);
            string specialtiesText = string.Join(", ", studentSpecialties.Select(s => s.Name));
            StudentSpecialties.Text = specialtiesText;
        }
        private async Task LoadGroupNameAsync()
        {
            string groupName = await _database.GetGroupNameAsync(student.GroupId);
            GroupName.Text = groupName;
        }
    }
}