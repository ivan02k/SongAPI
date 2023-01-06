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
    public class ArtistService : IBaseService<ArtistViewModel>
    {
        private readonly SongContext _context;
        private readonly IMapper mapper;

        public ArtistService(SongContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public bool Create(ArtistViewModel obj)
        {
            bool result = false;
            Artist artist = mapper.Map<Artist>(obj);
            bool isArtistInData = _context.Artists.Select(a => a.Name).Contains(artist.Name);
            if (!isArtistInData)
            {
                _context.Add(artist);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public List<string> GetAll()
        {
            List<string> list = new List<string>();
            List<Artist> listOfArtists = _context.Artists.ToList();
            foreach(Artist artist in listOfArtists)
            {
                ArtistViewModel artistView = mapper.Map<ArtistViewModel>(artist);
                string json = JsonSerializer.Serialize(artistView).ToString();
                list.Add(json);
            }
            return list;
        }

        public string GetByName(string name)
        {
            Artist? artist = _context.Artists.Where(x => x.Name == name).FirstOrDefault();
            string json = "";
            if(artist != null)
            {
                ArtistViewModel artistView = mapper.Map<ArtistViewModel>(artist);
                json = JsonSerializer.Serialize(artistView);
            }
            return json;
        }

        public bool Update(ArtistViewModel obj,string name)
        {
            bool result = false;
            Artist? artist = _context.Artists.Where(x => x.Name == name).FirstOrDefault();
            if (artist != null)
            {
                artist.Name = obj.Name;
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool Delete(string name)
        {
            bool result = false;
            Artist? artist = _context.Artists.Where(x => x.Name == name).FirstOrDefault();
            if (artist != null)
            {
                List<Song> songs = _context.Songs.Where(x => x.ArtistId == artist.Id).ToList();
                foreach (Song song in songs)
                {
                    SongSpecification? specification = _context.Specifications.Where(x => x.SongId == song.Id).FirstOrDefault();
                    if (specification != null)
                    {
                        _context.Remove(specification);
                    }
                    _context.Remove(song);
                    
                }
                _context.Remove(artist);
                 _context.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}
