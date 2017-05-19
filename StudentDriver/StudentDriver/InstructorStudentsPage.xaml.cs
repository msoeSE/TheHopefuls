using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDriver.Models;
using Xamarin.Forms;

namespace StudentDriver
{
	public partial class InstructorStudentsPage : ContentPage
	{
		private readonly ObservableCollection<User> _students;
		public InstructorStudentsPage()
		{
			InitializeComponent();
			_students = new ObservableCollection<User>();
		}

		private async Task StudentTapped(object sender, ItemTappedEventArgs e)
		{
			var user = e.Item as User;
			if (user != null)
			{
				await Navigation.PushAsync(new StatsPage(user.ServerId));
			}
		}

		protected override async void OnAppearing()
		{
			var users = await App.ServiceController.GetStudents();
			if (users == null) return;
			_students.Clear();
			foreach (var user in users) { _students.Add(user); }
			StudentsListView.ItemsSource = _students;
			StudentsListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				await StudentTapped(sender, e);
			};
		}
	}
}
