using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity_CodeAlong.Models
{
    public class GymClassIndexModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime EndTime { get { return StartTime + Duration; } }
        public string Description { get; set; }
        public string Attending { get; set; }


    }
}