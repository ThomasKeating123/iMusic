using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iMusic.Model;

namespace iMusic.Views
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\LoginPage.xaml", UriKind.Relative));
        }

        private void BtnRegistering_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new iMusicEntities())
            {
                if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                    string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                    string.IsNullOrWhiteSpace(TxtUsername.Text) ||
                    (string.IsNullOrWhiteSpace(TxtEmail.Text) && ! TxtEmail.Text.Contains("@") &&
                     ! TxtEmail.Text.Contains(".")) ||
                    string.IsNullOrWhiteSpace(PbPassword.Password))
                {
                    TbMessage.Text = "Make sure all information is filled in correctly";
                }
                else
                {
                    if (db.Users.Any(x => x.Username == TxtUsername.Text))
                    {
                        TbMessage.Text = "Username is already taken";
                    }
                    else
                    {
                        User user = new User();
                        user.FirstName = TxtFirstName.Text;
                        user.LastName = TxtLastName.Text;
                        user.Username = TxtUsername.Text;
                        user.Email = TxtEmail.Text;
                        user.Password = PbPassword.Password;
                        user.Balance = 0;

                        db.Users.Add(user);
                        db.SaveChanges();
                        TbMessage.Text = "Successfully registered";
                    }
                    }

            }
        }
    }
}
