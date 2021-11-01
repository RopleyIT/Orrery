namespace Orrery
{
    /// <summary>
    /// Positional and other data found for a
    /// celestial body at a specified time and date
    /// </summary>

    public class BodyLocation
    {
        public double JulianDate { get; set; }
        public double RightAscension { get; set; }
        public double Declination { get; set; }
        public double Longitude { get; set; }
        public double Azimuth { get; set; }
        public double Elevation { get; set; }
        public double Distance { get; set; }
        public CelestialBody Body { get; set; }

        public override string ToString()
        {
            return $"{Body.Name} at {Units.JulianDateAsString(JulianDate)} RA:{RightAscension:N3}"
                + $" Dec:{Declination:N3} Az:{Azimuth:N3} El:{Elevation:N3} Dist:{Distance}AU";
        }
    }
}
