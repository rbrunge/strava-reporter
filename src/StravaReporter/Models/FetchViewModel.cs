using System;
using System.ComponentModel.DataAnnotations;

namespace StravaReporter.Models
{
    public class FetchViewModel
    {
        public const int FetchMax = 10;

        [Display(Name = "Dato der hentes aktiviteter efter", 
            Description = "Nyeste aktiviteter hentes først")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public int NumberToFetch { get; set; } = FetchMax;
    }
}
