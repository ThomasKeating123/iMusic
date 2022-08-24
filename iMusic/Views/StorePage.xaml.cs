using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using iMusic.ViewModel;

namespace iMusic.Views
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        public StorePage()
        {
            InitializeComponent();
        }

        private void LbStore_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var db = new iMusicEntities())
            {
                int index = 0;
                foreach (Music song in LbStore.Items)
                {
                    int i = LbStore.SelectedIndex;
                    if (index == LbStore.SelectedIndex)
                    {
                        string CurrentUser = App.Current.Properties["CurrentUser"].ToString();

                        User user = db.Users.Where(x => x.Username == CurrentUser).First();
                        int userID = user.ID;
                        int songID = song.ID;

                        if (user.Balance >= song.Cost)
                        {
                            Sale sale = new Sale() {UserID = userID, SongID = songID};

                            db.Sales.Add(sale);

                            user.Balance = user.Balance - song.Cost;
                            LblBalance.Content = "Balance: £" + user.Balance;
                            db.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("You do not have enough money in your account", "Not enough money",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    index++;
                }
            }
            StorePage_OnLoaded(sender, e);
        }

        private void StorePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            string CurrentUser = App.Current.Properties["CurrentUser"].ToString();
            LblUsername.Content = CurrentUser;

            if (CurrentUser == "Admin")
            {
                BtnAddMusic.Visibility = Visibility.Visible;
            }

            using (var db = new iMusicEntities())
            {
                List<Sale> yourSales = new List<Sale>();
                List<Music> yourMusic = new List<Music>();
                List<string> category = new List<string>();

                User user = db.Users.Where(x => x.Username == CurrentUser).First();

                yourSales = db.Sales.Where(x => x.UserID == user.ID).ToList();

                foreach (Sale sale in yourSales)
                {
                    Music song = db.Musics.Where(x => x.ID == sale.SongID).FirstOrDefault();
                    yourMusic.Add(song);
                }

                List<Music> Music = new List<Music>();

                Music = db.Musics.ToList();

                for (int i = 0; i < yourMusic.Count; i++)
                {
                    if (Music.Contains(yourMusic[i]))
                    {
                        Music.Remove(yourMusic[i]);
                    }
                }

                foreach (Music song in Music)
                {
                    if (category.Contains(song.Category))
                    {

                    }
                    else
                    {
                        category.Add(song.Category);
                    }
                }
                
                CbFilter.ItemsSource = category;
                LbStore.ItemsSource = Music;

                LblBalance.Content = "Balance: £" + user.Balance;

            }
        }

        private void BtnLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\LibraryPage.xaml", UriKind.Relative));
        }

        private void BtnAccount_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\AccountPage.xaml", UriKind.Relative));
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

        private void CbFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxtSearch.Text = "";

            using (var db = new iMusicEntities())
            {
                List<Sale> yourSales = new List<Sale>();
                List<Music> yourMusic = new List<Music>();
                List<Music> filteredMusic = new List<Music>();

                string CurrentUser = App.Current.Properties["CurrentUser"].ToString();

                User user = db.Users.Where(x => x.Username == CurrentUser).First();

                yourSales = db.Sales.Where(x => x.UserID == user.ID).ToList();

                foreach (Sale sale in yourSales)
                {
                    Music song = db.Musics.Where(x => x.ID == sale.SongID).FirstOrDefault();
                    yourMusic.Add(song);
                }

                List<Music> Music = new List<Music>();

                Music = db.Musics.ToList();

                for (int i = 0; i < yourMusic.Count; i++)
                {
                    if (Music.Contains(yourMusic[i]))
                    {
                        Music.Remove(yourMusic[i]);
                    }
                }

                foreach (Music song in Music)
                {
                    if (song.Category == CbFilter.SelectedValue.ToString())
                    {
                        filteredMusic.Add(song);
                    }
                }

                LbStore.ItemsSource = filteredMusic;

                LblBalance.Content = "Balance: £" + user.Balance;
            }
        }

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            using (var db = new iMusicEntities())
            {
                List<Sale> yourSales = new List<Sale>();
                List<Music> yourMusic = new List<Music>();
                List<Music> filteredMusic = new List<Music>();

                string CurrentUser = App.Current.Properties["CurrentUser"].ToString();

                User user = db.Users.Where(x => x.Username == CurrentUser).First();

                yourSales = db.Sales.Where(x => x.UserID == user.ID).ToList();

                foreach (Sale sale in yourSales)
                {
                    Music song = db.Musics.Where(x => x.ID == sale.SongID).FirstOrDefault();
                    yourMusic.Add(song);
                }

                List<Music> Music = new List<Music>();

                Music = db.Musics.ToList();

                for (int i = 0; i < yourMusic.Count; i++)
                {
                    if (Music.Contains(yourMusic[i]))
                    {
                        Music.Remove(yourMusic[i]);
                    }
                }

                foreach (Music song in Music)
                {
                    if (song.MusicTitle.ToLower().Contains(TxtSearch.Text.ToLower()))
                    {
                        filteredMusic.Add(song);
                    }
                }
                
                LbStore.ItemsSource = filteredMusic;

                LblBalance.Content = "Balance: £" + user.Balance;
            }
        }
    }
}
