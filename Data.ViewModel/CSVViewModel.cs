using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CSVViewModel
    {
        [Name("title")]
        public string? Title { get; set; }
        [Name("artist")]
        public string? Artist { get; set; }
        [Name("genre")]
        public string? Genre { get; set; }
        [Name("year")]
        public int? Year { get; set; }
        [Name("beatsPerMinute")]
        public int? BeatsPerMinute { get; set; }
        [Name("energy")]
        public int? Energy { get; set; }
        [Name("danceability")]
        public int? Danceability { get; set; }
        [Name("valence")]
        public int? Valence { get; set; }
        [Name("length")]
        public int? Length { get; set; }
        [Name("acousticness")]
        public int? Acousticness { get; set; }
    }
}
