using CsvHelper;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
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

        public List<CSVViewModel> DownloadCSV(Stream file)
        {
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<CSVViewModel>().ToList();

            List<CSVViewModel> filterRecords = new List<CSVViewModel>();
            foreach (var record in records)
            {
                bool isSongInData = _context.Songs.ToList().Select(a => a.Title).Contains(record.Title);
                bool isRecordInFilter = filterRecords.Select(r => r.Title).Contains(record.Title);
                if (!isRecordInFilter && !isSongInData)
                {
                    filterRecords.Add(record);
                }
            }
            return filterRecords;
        }
        public IActionResult WriteCSV(List<CSVViewModel> records)
        {

            var memoryStream = new MemoryStream();

                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteHeader<CSVViewModel>();
                        csv.NextRecord();
                        foreach (CSVViewModel record in records)
                        {
                            csv.WriteRecord(record);
                            csv.NextRecord();
                        }
                        writer.Flush();
                    }
                
                }
            FileStreamResult fsr = new FileStreamResult(new MemoryStream(memoryStream.ToArray()), "application/csv");
            fsr.FileDownloadName = "Data.csv";
            return fsr;
        }

        public List<CSVViewModel> ConvertDataToCSV()
        {
            List<CSVViewModel> dataCSV = new List<CSVViewModel>();

            List<Artist> artists = _context.Artists.ToList();
            foreach (var artist in artists)
            {
                List<Song> songs = _context.Songs.Where(s => s.ArtistId == artist.Id).ToList();
                foreach (Song song in songs)
                {
                    SongSpecification? specification = _context.Specifications.Where(s => s.SongId == song.Id).FirstOrDefault();
                    if (specification ==  null)
                    {
                        specification.Beats = 0;
                        specification.Energy = 0;
                        specification.Danceability = 0;
                        specification.Valence = 0;
                        specification.Length = 0;
                        specification.Acousticness = 0;
                    }
                    CSVViewModel model = new CSVViewModel()
                    {
                        Title = song.Title,
                        Artist = artist.Name,
                        Genre = song.Genre,
                        Year = song.Year,
                        BeatsPerMinute = specification.Beats,
                        Energy = specification.Energy,
                        Danceability = specification.Danceability,
                        Valence = specification.Valence,
                        Length = specification.Length,
                        Acousticness = specification.Acousticness,
                    };
                    dataCSV.Add(model);
                }
            }
            return dataCSV;
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
        public void CSVDataFilling(List<CSVViewModel> record,bool isDownloadCSV)
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
            bool haveChange = false;

            //Artist
            bool haveArtistInData = (_context.Artists.Count() > 0);
            if (isDownloadCSV) haveArtistInData = false;
            if (!haveArtistInData)
            {
                foreach (string artistName in artists.Keys)
                {
                    bool isArtistInData = _context.Artists.ToList().Select(a => a.Name).Contains(artistName);
                    if(!isArtistInData)
                    {
                        _context.Artists.Add(new Artist() { Name = artistName });
                        haveChange = true;
                    }                   
                }
                if (haveChange)
                {
                    _context.SaveChanges();
                    haveChange = false;
                }
            }

            //Song
            bool haveSongInData = (_context.Songs.Count() > 0);
            if(isDownloadCSV) haveSongInData = false;
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
                            haveChange = true;
                        }
                    }
                }
                if (haveChange)
                {
                    _context.SaveChanges();
                    haveChange= false;
                }
            }

            //Specification
            bool haveSongSpecificationInData = (_context.Specifications.Count() > 0);
            if(isDownloadCSV) haveSongSpecificationInData=false;
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
                            haveChange = true;
                        }
                    }
                }
                if (haveChange)
                {
                    _context.SaveChanges();
                    haveChange = false;
                }
            }
        }
    }

}

