using System.ComponentModel.DataAnnotations;
using Orrery;
using OrreryPlotter;

namespace OrreryUI.Data
{
    public class OrbitParams
    {
        [Required]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between plus or minus 90 degrees")]
        public double Latitude { get; set; }

        [Required]
        [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between plus or minus 180 degrees")]
        public double Longitude { get; set; }
       
        [Required]
        public DateTime Start { get; set; }
      
        [Required]
        public DateTime End { get; set; }

        public string Interval { get; set; } 

        public List<string> Bodies { get; set; }

        public string GetSkySvg(IEnumerable<string> planetNames)
        {
            // Convert the UI interval to a .NET duration type

            TimeSpan interval = Interval switch
            {
                "year" => TimeSpan.FromDays(365.25),
                "week" => TimeSpan.FromDays(7),
                "sidereal" => Units.SiderealDay,
                "day" => TimeSpan.FromDays(1),
                "hour" => TimeSpan.FromHours(1),
                "minute" => TimeSpan.FromSeconds(60),
                _ => TimeSpan.FromDays(36525)
            };

            SkyChart sc = new();
            sc.Start = ConvertTime(Start);
            sc.End = ConvertTime(End);
            sc.Interval = interval;
            sc.LongLat = new((float)Longitude, (float)Latitude);
            sc.BodyNames.Clear();
            sc.BodyNames.AddRange(planetNames);
            return sc.PlotSvg();
        }

        // Adjust to current time of day and convert to UTC

        private static DateTimeOffset ConvertTime(DateTime when)
        {
            when = when.Date; // Map to previous midnight
            TimeSpan timeOfDay = DateTime.Now - DateTime.Today;
            return new DateTimeOffset((when + timeOfDay).ToUniversalTime());
        }
    }
}
