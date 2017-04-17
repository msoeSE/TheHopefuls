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
        public InstructorStudentsPage()
        {
            InitializeComponent();
            var students = new ObservableCollection<User>();
            var users = new List<User> {new User {FirstName = "Fuck", LastName = "Face"}, new User { FirstName = "Ur", LastName = "Mom" } };
            foreach (var user in users) { students.Add(user); }
            StudentsListView.ItemsSource = students;
            StudentsListView.ItemTapped += StudentTapped;
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
            
        }
    }
}
