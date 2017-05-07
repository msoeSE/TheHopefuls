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

        private void StudentTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as User;
            if (user != null)
            {
                App.StatsPageAction(user.ServerId).Invoke();
            }
        }

        protected override async void OnAppearing()
        {
            //var users = new List<User> { new User { FirstName = "Fuck", LastName = "Face" }, new User { FirstName = "Ur", LastName = "Mom" } };
            var users = await App.ServiceController.GetStudents();
            if (users == null) return;
            foreach (var user in users) { _students.Add(user); }
            StudentsListView.ItemsSource = _students;
            StudentsListView.ItemTapped += StudentTapped;
        }
    }
}
