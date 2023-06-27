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
	public partial class RegisterPage : ContentPage
	{
		private CollegeDatabase _database;
		public RegisterPage ()
		{
			InitializeComponent ();
			_database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "collegeDB"));
		}

		private async void Regiter_Clicked(object sender, EventArgs e)
		{
			string fullName = fullNameEntry.Text;
			string dateOfBirth = dateOfBirthDatePicker.Date.ToString("dd/MM/yyyy");
			bool budget = budgetCheckBox.IsChecked;
			string course = numberCourseEntry.Text;
			string login = loginEntry.Text;
			string password = passwordEntry.Text;
			string repitPassword = repitpasswordEntry.Text;
			Student student = await _database.FindStudentByLoginAndPasswordAsync(login, password);

			int groupID = int.Parse(groupIdEntry.Text);

			if (string.IsNullOrEmpty(course) || string.IsNullOrEmpty(dateOfBirth) ||
				string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(login) ||
				string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repitPassword) || string.IsNullOrEmpty(groupIdEntry.Text))
			{
				await DisplayAlert("Ошибка", "Заполните поля", "OK");
				return;
			}
			else if (password != repitPassword)
			{
				await DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
				return;
			}

			if (student != null)
			{
				await DisplayAlert("Ошибка", "Студент с таким логином уже есть", "OK");
				return;
			}
			else
			{
				Student studentAdd = new Student
				{
					FullName = fullName,
					DateOfBirth = dateOfBirth,
					Course = int.Parse(course),
					Login = login,
					Password = password,
					GroupId = groupID,
					IsBudget = budget,
				};
				await _database.SaveStudentAsync(studentAdd);

				await DisplayAlert("Информация", "Регистрация прошла успешно", "OK");

				fullNameEntry.Text = "";
				dateOfBirthDatePicker = null;
				budgetCheckBox.IsChecked = false;
				numberCourseEntry.Text = "";
				loginEntry.Text = "";
				passwordEntry.Text = "";
				repitpasswordEntry.Text = "";
			}
		}

		private async void BackToLogin_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new MainPage());
		}
	}
}