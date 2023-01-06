using Data;
using Data.Entities;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DataService : IDataService
    {
        private readonly SongContext _context;
        public DataService(SongContext context)
        {
            _context = context;
        }

        public bool DeleteAll()
        {
            bool result = false;
            List<Artist> listOfArtists = _context.Artists.ToList();
            foreach (Artist artist in listOfArtists)
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
                result = true;
            }
            var res = _context.SaveChanges();
            return result;
        }
    }
}
