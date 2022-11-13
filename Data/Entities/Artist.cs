using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Artist:BaseClass
    {
        public string? Name { get; set; }
        public List<Song>? Song { get; set; }
    }
}
