using AutoMapper;
using Data;
using Data.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModels.Data.ViewModels;

namespace Service
{
    public class SongService: IBaseService<SongViewModel>
    {
        private readonly SongContext _context;
        private readonly IMapper mapper;

        public SongService(SongContext context, IMapper mapper)
        {

            _context = context;
            this.mapper = mapper;
        }

        public bool Create(SongViewModel obj)
        {
            bool result = false;
            Song song = mapper.Map<Song>(obj);
            bool isSongInData = _context.Songs.Select(a => a.Title).Contains(song.Title);
            int artistId = _context.Artists.Where(a => a.Name == obj.ArtistName).FirstOrDefault()?.Id ?? 0;
            if (artistId > 0 && !isSongInData)
            {
                song.ArtistId = artistId;
                _context.Add(song);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public List<string> GetAll()
        {
            List<string> list = new List<string>();
            List<Song> listOfSongs = _context.Songs.ToList();
            foreach (Song song in listOfSongs)
            {
                string? artistName = _context.Artists.Where(x => x.Id == song.ArtistId).FirstOrDefault()?.Name;
                SongViewModel songView = mapper.Map<SongViewModel>(song);
                songView.ArtistName = artistName;
                string json = JsonSerializer.Serialize(songView);
                list.Add(json);
            }
            return list;
        }

        public string GetByName(string name)
        {
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            string json = "";
            if(song != null)
            {
                string? artistName = _context.Artists.Where(x => x.Id == song.ArtistId).FirstOrDefault()?.Name;
                SongViewModel songView = mapper.Map<SongViewModel>(song);
                songView.ArtistName = artistName;
                json = JsonSerializer.Serialize(songView);
            }
            return json;
        }

        public bool Update(SongViewModel obj, string name)
        {
            bool result = false;
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            if (song != null)
            {
                int artistId = _context.Artists.Where(x => x.Name == obj.ArtistName).FirstOrDefault()?.Id ?? 0;
                song.Title = (obj.Title != "string" && obj.Title != "" && obj.Title != null) ? obj.Title : song.Title;
                song.Genre = (obj.Genre != "string" && obj.Genre != "" && obj.Genre != null) ? obj.Genre : song.Genre;
                song.Year =  obj.Year != 0 ? obj.Year : song.Year;
                song.ArtistId = artistId != 0 ? artistId : song.ArtistId;
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
        public bool Delete(string name)
        {
            bool result = false;
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            if (song != null)
            {
                SongSpecification? specification = _context.Specifications.Where(x => x.SongId == song.Id).FirstOrDefault();
                if (specification != null)
                {
                    _context.Remove(specification);
                }
                _context.Remove(song);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
