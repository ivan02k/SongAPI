﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Data.ViewModels
{
    public class SongSpecificationViewModel
    {
        public int Beats { get; set; }
        public int Energy { get; set; }
        public int Danceability { get; set; }
        public int Valence { get; set; }
        public int Length { get; set; }
        public int Acousticness { get; set; }
    }
}
