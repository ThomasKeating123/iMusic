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
    /// Interaction logic for AcoountPage.xaml
    /// </summary>
    public partial class AcoountPage : Page
    {
        public AcoountPage()
        {
            InitializeComponent();
        }

        private void BtnAddBalance_OnClick(object sender, RoutedEventArgs e)
        {
            decimal bal = Convert.ToDecimal(TxtBalance.Text);

            bal = bal + 1;

            TxtBalance.Text = bal.ToString();
        }

        private void BtnSaveInfo_OnClick(object sender, RoutedEventArgs e)
        {
            using (var db = new iMusicEntities())
            {
                if (string.IsNullOrWhiteSpace(TxtEditFirstName.Text) ||
                    string.IsNullOrWhiteSpace(TxtEditLastName.Text) ||
                    string.IsNullOrWhiteSpace(TxtEditUsername.Text) ||
                    string.IsNullOrWhiteSpace(TxtEditEmail.Text) ||
                    string.IsNullOrWhiteSpace(PbEditPassword.Password) ||
                    string.IsNullOrWhiteSpace(TxtBalance.Text))
                {
                    TbMessage.Text = "Please make sure all information is fill in";
                }
                else
                {
                    string CurrentUser = App.Current.Properties["CurrentUser"].ToString();
                    
                    if (db.Users.Any(x => x.Username == TxtEditUsername.Text) && TxtEditUsername.Text != CurrentUser)
                    {
                        TbMessage.Text = "Username is already taken";
                    }
                    else
                    {
                        User user = db.Users.Where(x => x.Username == CurrentUser).First();
                        user.FirstName = TxtEditFirstName.Text;
                        user.LastName = TxtEditLastName.Text;
                        user.Username = TxtEditUsername.Text;
                        user.Email = TxtEditEmail.Text;
                        user.Password = PbEditPassword.Password;

                        decimal bal = user.Balance + Convert.ToDecimal(TxtBalance.Text);
                        user.Balance = bal;
                        db.SaveChanges();
                        TbMessage.Text = "Successfully updated information";
                    }
                }
            }
        }

        private void BtnLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\LibraryPage.xaml", UriKind.Relative));
        }

        private void BtnStore_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\StorePage.xaml", UriKind.Relative));
        }

        private void BtnLogout_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.Properties.Remove("CurrentUser");
            NavigationService.Navigate(new Uri("Views\\LoginPage.xaml", UriKind.Relative));
        }

        private void BtnAddMusic_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\AddMusicPage.xaml", UriKind.Relative));
        }

        private void AcoountPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            string CurrentUser = App.Current.Properties["CurrentUser"].ToString();
            LblUsername.Content = CurrentUser;

            if (CurrentUser == "Admin")
            {
                BtnAddMusic.Visibility = Visibility.Visible;
            }

            using (var db = new iMusicEntities())
            {
                User user = db.Users.Where(x => x.Username == CurrentUser).First();
                TxtEditFirstName.Text = user.FirstName;
                TxtEditLastName.Text = user.LastName;
                TxtEditUsername.Text = user.Username;
                TxtEditEmail.Text = user.Email;
                PbEditPassword.Password = user.Password;
            }
        }
    }
}
