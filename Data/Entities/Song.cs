using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Song:BaseClass
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int Year { get; set; }
        public int? ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }
}
