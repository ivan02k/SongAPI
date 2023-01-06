using CsvHelper;
using Data;
using Data.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Service.CSV
{
    public class CSVService : ICSVService
    {
        private readonly SongContext _context;

        public CSVService(SongContext context)
        {
            _context = context;
        }

        public List<CSVViewModel> ReadCSV(string path)
        {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<CSVViewModel>().ToList();
                    List<CSVViewModel> result = new List<CSVViewModel>();
                    foreach (var record in records)
                    {
                      if(!result.Select(r => r.Title).Contains(record.Title))
                      {
                        result.Add(record);
                      }
                    }
                    return result;
                }

        }
        public void CSVDataFilling(List<CSVViewModel> record)
        {
            Dictionary < string, List<object>> artists = new Dictionary<string, List<object>>();
            
            foreach (CSVViewModel song in record)
            {
                string? artist = song.Artist;
                if (artist != null)
                {
                    if (!artists.ContainsKey(artist))
                    {
                        artists[artist] = new List<object>();
                    }
                    artists[artist].Add(song);
                }
                
            }
            int counter = 0;

            //Artist
            bool haveArtistInData = (_context.Artists.Count() > 0);
            if (!haveArtistInData)
            {
                foreach (string artistName in artists.Keys)
                {
                  _context.Artists.Add(new Artist() { Name = artistName });
                  counter++;                    
                }
                if (counter > 0)
                {
                    _context.SaveChanges();
                    counter = 0;
                }
            }

            //Song
            bool haveSongInData = (_context.Songs.Count() > 0);
            if(!haveSongInData)
            {
                foreach (string artistName in artists.Keys)
                {
                    foreach (CSVViewModel model in artists[artistName])
                    {
                        int artistId = _context.Artists.Where(a => a.Name == artistName).FirstOrDefault()?.Id ?? 0;

                        if (artistId != 0)
                        {  
                            _context.Songs.Add(new Song()
                            {
                                Title = model.Title,
                                Genre = model.Genre,
                                Year = model.Year,
                                ArtistId = artistId
                            });
                            counter++;
                        }
                    }
                }
                if (counter > 0)
                {
                    _context.SaveChanges();
                    counter = 0;
                }
            }

            //Specification
            bool haveSongSpecificationInData = (_context.Specifications.Count() > 0);
            if(!haveSongSpecificationInData)
            {
                foreach (string artistName in artists.Keys)
                {
                    foreach (CSVViewModel model in artists[artistName])
                    {
                        int songId = _context.Songs.Where(a => a.Title == model.Title).FirstOrDefault()?.Id ?? 0;

                        if (songId != 0)
                        {
                            _context.Specifications.Add(new SongSpecification()
                            {
                                Beats = (model.BeatsPerMinute != null) ? (int)model.BeatsPerMinute : 0,
                                Energy = (model.Energy != null) ? (int)model.Energy : 0,
                                Danceability = (model.Danceability != null) ? (int)model.Danceability : 0,
                                Valence = (model.Valence != null) ? (int)model.Valence : 0,
                                Length = (model.Length != null) ? (int)model.Length : 0,
                                Acousticness = (model.Acousticness != null) ? (int)model.Acousticness : 0,
                                SongId = songId
                            });
                            counter++;
                        }
                    }
                }
                if (counter > 0)
                {
                    _context.SaveChanges();
                    counter = 0;
                }
            }

            //foreach (string artistName in artists.Keys)
            //{
            //    int artistId = artistIndex;
            //    bool isArtistInData = (_context.Artists.Count() > 0)
            //            ? _context.Artists.ToList().Select(a => a.Name).Contains(artistName)
            //            : false;

            //    if (!isArtistInData)
            //    {
            //        _context.Artists.Add(new Artist() { Name = artistName });
            //    }
            //    artistIndex++;
            //    foreach (var song in artists[artistName])
            //    {
            //        int songId = songIndex;

            //        string title = (song.GetType().GetProperties().First(x => x.Name == "Title").GetValue(song)).ToString();
            //        string genre = (song.GetType().GetProperties().First(x => x.Name == "Genre").GetValue(song)).ToString();
            //        int year = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Year").GetValue(song)).ToString());
            //        int beatsPerMinute = int.Parse((song.GetType().GetProperties().First(x => x.Name == "BeatsPerMinute").GetValue(song)).ToString());
            //        int energy = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Energy").GetValue(song)).ToString());
            //        int danceability = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Danceability").GetValue(song)).ToString());
            //        int valence = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Valence").GetValue(song)).ToString());
            //        int length = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Length").GetValue(song)).ToString());
            //        int acousticness = int.Parse((song.GetType().GetProperties().First(x => x.Name == "Acousticness").GetValue(song)).ToString());

            //        bool isSongInData = (_context.Songs != null)
            //        ? _context.Songs.ToList().Select(a => a.Title).Contains(title)
            //        : false;

            //        if (!isSongInData)
            //        {
            //            _context.Songs.Add(new Song()
            //            {
            //                Title = title,
            //                Genre = genre,
            //                Year = year,
            //                ArtistId = artistId
            //            });

            //            _context.Specifications.Add(new SongSpecification()
            //            {
            //                Beats = beatsPerMinute,
            //                Energy = energy,
            //                Danceability = danceability,
            //                Valence = valence,
            //                Length = length,
            //                Acousticness = acousticness,
            //                SongId = songId
            //            });
            //        }
            //        songIndex++;
            //    }
            //}
            //_context.SaveChanges();
        }
    }

}

