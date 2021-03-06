﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoryan.Models
{
    public class Drugs
    {
        public int id { get; set; }
        public string name { get; set; }
        public int officialPrice { get; set; }
		public List<string> imgsUrls { get; set; }
		public List<int> categoriesIds { get; set; }
		public List<string> effectiveSubstances { get; set; }
    }
}
