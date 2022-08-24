using System;
using System.Collections.Generic;
using System.Drawing;
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
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;

namespace iMusic.Views
{
    /// <summary>
    /// Interaction logic for AddMusicPage.xaml
    /// </summary>
    public partial class AddMusicPage : Page
    {
        public string tempFilePath;

        public AddMusicPage()
        {
            InitializeComponent();
        }

        private void AddMusicPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            string CurrentUser = App.Current.Properties["CurrentUser"].ToString();
            LblUsername.Content = CurrentUser;

            if (CurrentUser == "Admin")
            {
                BtnAddMusic.Visibility = Visibility.Visible;
            }
        }

        private void BtnSelectImage_OnClick(object sender, RoutedEventArgs e)
        {
            FileDialog img = new OpenFileDialog();
            img.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

            Nullable<bool> result = img.ShowDialog();

            if (result == true)
            {
                string filePath = img.FileName;
                BitmapImage image = new BitmapImage(new Uri(filePath));
                ImgCover.Source = image;
                tempFilePath = filePath;
            }
        }

        private void BtnAddSong_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtCategory.Text) ||
                string.IsNullOrWhiteSpace(TxtRecordingStudio.Text) ||
                string.IsNullOrWhiteSpace(TxtMusicTitle.Text) ||
                string.IsNullOrWhiteSpace(ImgCover.Source.ToString()) ||
                string.IsNullOrWhiteSpace(TxtCost.Text) ||
                !DpReleaseDate.SelectedDate.HasValue)
            {
                TbMessage.Text = "Song has not been added";
            }
            else
            {
                Bitmap cover = new Bitmap(tempFilePath);

                ImageConverter converter = new ImageConverter();

                Image coverImage = (Image)converter.ConvertTo(cover, typeof(Image));

                byte[] image = (byte[])converter.ConvertTo(coverImage,
                    typeof(byte[]));

                using (var db = new iMusicEntities())
                {
                    Music song = new Music()
                    {
                        Category = TxtCategory.Text,
                        RecordingStudio = TxtRecordingStudio.Text,
                        MusicTitle = TxtMusicTitle.Text,
                        Image = image,
                        Cost = Convert.ToDecimal(TxtCost.Text),
                        ReleaseDate = DpReleaseDate.SelectedDate.Value,
                        Availability = true
                    };

                    if (db.Musics.Any(x => x == song))
                    {
                        TbMessage.Text = "Song already exsists";
                    }
                    else
                    {
                        db.Musics.Add(song);
                        db.SaveChanges();
                        TbMessage.Text = "Song Added";
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

        private void BtnAccount_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views\\AccountPage.xaml", UriKind.Relative));
        }

        private void BtnLogout_OnClick(object sender, RoutedEventArgs e)
        {
            App.Current.Properties.Remove("CurrentUser");
            NavigationService.Navigate(new Uri("Views\\LoginPage.xaml", UriKind.Relative));
        }
    }
}
