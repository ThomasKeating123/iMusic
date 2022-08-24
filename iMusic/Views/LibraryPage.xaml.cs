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
    /// Interaction logic for LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
        public LibraryPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string CurrentUser = App.Current.Properties["CurrentUser"].ToString();
            LblUsername.Content = CurrentUser;

            if (CurrentUser == "Admin")
            {
                BtnAddMusic.Visibility = Visibility.Visible;
            }

            try
            {
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
                        if (category.Contains(song.Category))
                        {

                        }
                        else
                        {
                            category.Add(song.Category);
                        }
                    }

                    CbFilter.ItemsSource = category;
                    LbLibrary.ItemsSource = yourMusic;

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void MiDeleteSong_OnClick(object sender, RoutedEventArgs e)
        {
            int index = 0;

            foreach (Music song in LbLibrary.Items)
            {

                if (index == LbLibrary.SelectedIndex)
                {
                    using (var db = new iMusicEntities())
                    {
                        string CurrentUser = App.Current.Properties["CurrentUser"].ToString();

                        User user = db.Users.Where(x => x.Username == CurrentUser).FirstOrDefault();
                        int userID = user.ID;

                        Sale sale = db.Sales.Where(x => x.SongID == song.ID).Where(x => x.UserID == userID).FirstOrDefault();

                        db.Sales.Remove(sale);
                        db.SaveChanges();
                    }
                }

                index++;
            }
            Page_Loaded(sender, e);
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

                User user = db.Users.Where(x => x.Username == CurrentUser).FirstOrDefault();

                yourSales = db.Sales.Where(x => x.UserID == user.ID).ToList();

                foreach (Sale sale in yourSales)
                {
                    Music song = db.Musics.Where(x => x.ID == sale.SongID).FirstOrDefault();
                    yourMusic.Add(song);
                }

                foreach (Music song in yourMusic)
                {
                    if (song.Category == CbFilter.SelectedValue.ToString())
                    {
                        filteredMusic.Add(song);
                    }
                }


                LbLibrary.ItemsSource = filteredMusic;
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

                User user = db.Users.Where(x => x.Username == CurrentUser).FirstOrDefault();

                yourSales = db.Sales.Where(x => x.UserID == user.ID).ToList();

                foreach (Sale sale in yourSales)
                {
                    Music song = db.Musics.Where(x => x.ID == sale.SongID).FirstOrDefault();
                    yourMusic.Add(song);
                }

                foreach (Music song in yourMusic)
                {
                    if (song.MusicTitle.ToLower().Contains(TxtSearch.Text.ToLower()))
                    {
                        filteredMusic.Add(song);
                    }
                }


                LbLibrary.ItemsSource = filteredMusic;
            }
        }

        private void BtnStore_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\StorePage.xaml", UriKind.Relative));
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
    }
}
