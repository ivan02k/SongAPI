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
    public class SongSpecificationService: IBaseService<SongSpecificationViewModel>
    {
        private readonly SongContext _context;
        private readonly IMapper mapper;

        public SongSpecificationService(SongContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public bool Create(SongSpecificationViewModel obj)
        {
            bool result = false;
            SongSpecification songSpecification = mapper.Map<SongSpecification>(obj);
            int songId = _context.Songs.Where(a => a.Title == obj.SongName).FirstOrDefault()?.Id ?? 0;
            bool isSongSpecificationInData = _context.Specifications.ToList().Select(a => a.SongId).Contains(songId);

            if (songId > 0 && !isSongSpecificationInData)
            {
                
                    songSpecification.SongId = songId;
                    _context.Add(songSpecification);
                    _context.SaveChanges();
                    result = true;

            }
            return result;
        }
        public List<string> GetAll()
        {
            List<string> list = new List<string>();
            List<SongSpecification> listOfSpecification = _context.Specifications.ToList();
            foreach (SongSpecification specification in listOfSpecification)
            {
                string? songTitle = _context.Songs.Where(x => x.Id == specification.SongId).FirstOrDefault()?.Title;
                SongSpecificationViewModel specificationView = mapper.Map<SongSpecificationViewModel>(specification);
                specificationView.SongName = songTitle;
                string json = JsonSerializer.Serialize(specificationView);
                list.Add(json);
            }
            return list;
        }

        public string GetByName(string name)
        {
            string json = "";
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            if(song != null)
            {
                SongSpecification? specification = _context.Specifications.Where(x => x.SongId == song.Id).FirstOrDefault();
                if (specification != null)
                {
                    SongSpecificationViewModel specificationView = mapper.Map<SongSpecificationViewModel>(specification);
                    specificationView.SongName = name;
                    json = JsonSerializer.Serialize(specificationView);
                }
            }
            return json;
        }

        public bool Update(SongSpecificationViewModel obj, string name)
        {
            bool result = false;
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            if( song != null)
            {
                SongSpecification? specification = _context.Specifications.Where(x => x.SongId == song.Id).FirstOrDefault();
                if (specification != null)
                {
                    specification.Acousticness = obj.Acousticness != 0 ? obj.Acousticness : specification.Acousticness;
                    specification.Valence = obj.Valence != 0 ? obj.Valence : specification.Valence;
                    specification.Danceability = obj.Danceability != 0 ? obj.Danceability : specification.Danceability;
                    specification.Beats = obj.Beats != 0 ? obj.Beats : specification.Beats;
                    specification.Energy = obj.Energy != 0 ? obj.Energy : specification.Energy;
                    specification.Length = obj.Length != 0 ? obj.Length : specification.Length;
                    _context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
        public bool Delete(string name)
        {
            bool result = false;
            Song? song = _context.Songs.Where(x => x.Title == name).FirstOrDefault();
            if(song != null)
            {
                SongSpecification? specification = _context.Specifications.Where(x => x.SongId == song.Id).FirstOrDefault();
                if (specification != null)
                {
                    _context.Remove(specification);
                    _context.SaveChanges();
                    result = true;
                }
            }
            return result;
        }
    }
}
