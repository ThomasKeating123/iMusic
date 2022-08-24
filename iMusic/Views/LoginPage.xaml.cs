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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnRegister_OnClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views\\RegisterPage.xaml", UriKind.Relative));
        }
        //Triggered when the login button is clicked
        private void BtnLogging_OnClick(object sender, RoutedEventArgs e)
        {
            //Uses and creates a new iMusicEntitiis
            using (var db = new iMusicEntities())
            {
                //If either textbox is null the output a message
                if (string.IsNullOrWhiteSpace(TxtUsername.Text) ||
                    string.IsNullOrWhiteSpace(PbPassword.Password))
                {
                    //Outputs a message to the user
                    TbMessage.Text = "Make sure all information is filled in correctly";
                }
                else
                {
                    //Creates a query to get the user from the database
                    var query = from u in db.Users
                        where u.Username == TxtUsername.Text && u.Password == PbPassword.Password
                        select u;
                    //Rus the query and checks if it is not null
                    if (query.SingleOrDefault() != null)
                    {
                        //Adds a property to the current application
                        App.Current.Properties.Add("CurrentUser", TxtUsername.Text);
                        //Navigates the user to the library page
                        NavigationService.Navigate(new Uri("Views\\LibraryPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        //Outputs a message to the user
                        TbMessage.Text = "User doesn't exist";
                    }
                }
            }
        }
    }
}
