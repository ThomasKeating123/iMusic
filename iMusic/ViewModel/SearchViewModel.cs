using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMusic.Model;

namespace iMusic.ViewModel
{
    class SearchViewModel
    {
        public SearchViewModel()
        {
            using (var db = new iMusicEntities())
            {
                List<Sale> yourSales = new List<Sale>();
                List<Music> yourMusic = new List<Music>();
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

                List<string> songs = new List<string>();
                foreach (Music song in Music)
                {
                    songs.Add(song.MusicTitle);
                }

                songNames = songs;
            }
        }

        private List<string> songNames { get; set; }
    }
}
