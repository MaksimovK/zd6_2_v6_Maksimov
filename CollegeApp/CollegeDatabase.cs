using CollegeApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeApp
{
    public class CollegeDatabase
    {
        readonly SQLiteAsyncConnection connect;
        public CollegeDatabase(string path)
        {
            connect = new SQLiteAsyncConnection(path);
            connect.CreateTableAsync<Student>().Wait();
            connect.CreateTableAsync<Group>().Wait();
            connect.CreateTableAsync<Specialization>().Wait();
            connect.CreateTableAsync<Teacher>().Wait();
            connect.CreateTableAsync<StudentSpecialization>().Wait();
            connect.CreateTableAsync<TeacherSpecialization>().Wait();

            if (connect.Table<Student>().CountAsync().Result == 0)
            {
                var specialtyes = new List<Specialization>
                {
                    new Specialization { Name = "Информационные системы и программирование" },
                    new Specialization { Name = "Технология аналитического контроля химических соединений" },
                };
                connect.InsertAllAsync(specialtyes).Wait();

                var groups = new List<Group>
                {
                    new Group { Name = "ПР-21", SpecializationId = 1 },
                    new Group { Name = "ДЗ-11", SpecializationId = 1 },
                };
                connect.InsertAllAsync(groups).Wait();

                var teachers = new List<Teacher>
                {
                    new Teacher { FullName = "Ватолина Татьяна Александровна", Workload = 100, Login = "Teacher1", IsAdmission = false, Password = "qwerty" },
                    new Teacher { FullName = "Мирошниченко Галина Викторовна", Workload = 200, Login = "Teacher2", IsAdmission = true, Password = "qwerty" },
                };
                connect.InsertAllAsync(teachers).Wait();

                var students = new List<Student>
                {
                    new Student { FullName = "Максимов Кирилл Эдуардович", DateOfBirth = "30.05.2005", IsBudget = true, Course = 2, GroupId = 1, Login = "Maksimov", Password = "qwerty"},
                    new Student { FullName = "Шайдулин Артём Ришатович", DateOfBirth = "26.04.2005", IsBudget = true, Course = 1, GroupId = 2, Login = "Student2", Password = "qwerty"},
                };
                connect.InsertAllAsync(students).Wait();

                var studentSpecialties = new List<StudentSpecialization>
                {
                    new StudentSpecialization { StudentId = 1, SpecializationId = 1 },
                    new StudentSpecialization { StudentId = 2, SpecializationId = 1 },
                    new StudentSpecialization { StudentId = 2, SpecializationId = 2 },
                };
                connect.InsertAllAsync(studentSpecialties).Wait();

                var teacherSpecialties = new List<TeacherSpecialization>
                {
                    new TeacherSpecialization { TeacherId = 1, SpecialtyId = 1 },
                    new TeacherSpecialization { TeacherId = 2, SpecialtyId = 1 },
                };
                connect.InsertAllAsync(teacherSpecialties).Wait();
            }
        }

        public async Task<Student> FindStudentByLoginAndPasswordAsync(string login, string password)
        {
            return await connect.Table<Student>()
                .Where(s => s.Login == login && s.Password == password)
                .FirstOrDefaultAsync();
        }
        public async Task<Teacher> FindTeacherByLoginAndPasswordAsync(string login, string password)
        {
            return await connect.Table<Teacher>()
                .Where(t => t.Login == login && t.Password == password)
                .FirstOrDefaultAsync();
        }
        public Task<List<Specialization>> GetStudentSpecialtiesAsync(int studentId)
        {
            return connect.QueryAsync<Specialization>(
                "SELECT Specialty.* FROM Specialty " +
                "JOIN StudentSpecialty ON Specialty.SpecialtyId = StudentSpecialty.SpecialtyId " +
                "WHERE StudentSpecialty.StudentId = ?", studentId);
        }
        public Task<string> GetGroupNameAsync(int groupId)
        {
            return connect.ExecuteScalarAsync<string>(
                "SELECT Name FROM [Group] WHERE GroupId = ?", groupId);
        }
        public async Task<List<string>> GetAllGroupNamesAsync()
        {
            var groupNames = await connect.Table<Group>().ToListAsync();
            return groupNames.Select(group => group.Name).ToList();
        }
        public async Task<List<Student>> GetStudentsByGroupNameAsync(string groupName)
        {
            var group = await connect.Table<Group>().FirstOrDefaultAsync(g => g.Name == groupName);
            if (group != null)
            {
                var students = await connect.Table<Student>().Where(s => s.GroupId == group.GroupId).ToListAsync();
                return students;
            }
            return new List<Student>();
        }
        public async Task<List<Student>> GetStudentsAsync()
        {
            return await connect.Table<Student>().ToListAsync();
        }
        public async Task SaveStudentAsync(Student student)
        {
            if (student.StudentId != 0)
            {
                await connect.UpdateAsync(student);
            }
            else
            {
                await connect.InsertAsync(student);
            }
        }
        public async Task DeleteStudentAsync(Student student)
        {
            if (student != null)
            {
                await connect.DeleteAsync(student);
            }
        }

        public Task<int> SaveTeacherAsync(Teacher teacher)
        {
            if (teacher.TeacherId != 0)
            {
                return connect.UpdateAsync(teacher);
            }
            else
            {
                return connect.InsertAsync(teacher);
            }
        }

        public Task<List<Teacher>> GetTeachersAsync()
        {
            return connect.Table<Teacher>().ToListAsync();
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await connect.Table<Group>().ToListAsync();
        }

        public async Task<Group> GetGroupByNameAsync(string groupName)
        {
            return await connect.Table<Group>().FirstOrDefaultAsync(g => g.Name == groupName);
        }
    }
}
