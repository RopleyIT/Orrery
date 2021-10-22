using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orrery
{
    public class Planets
    {
        public Planets()
        {
            AddPlanets();
        }

        public List<CelestialBody> Bodies = new();

        public CelestialBody FindBody(string name) 
            => Bodies.FirstOrDefault(b => string.Equals(b.Name, name, StringComparison.CurrentCultureIgnoreCase));

        public void AddPlanets()
        {
            Bodies.Add(new()
            {
                Name = "Sun",
                Mass = 1.98847E30
            });

            Bodies.Add(new()
            {
                Name = "Mercury",
                MeanDistance = new(0.38709893, 0.00000066 / Units.DaysPerCentury),
                Eccentricity = new(0.20563069, 0.00002527 / Units.DaysPerCentury),
                Inclination = new(7.00487, -23.51 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(48.33167, -446.30 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(77.45645, 573.57 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(252.25084, 538101628.29 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 3.301E23,
                Period = 7.6005216E6
            });

            Bodies.Add(new()
            {
                Name = "Venus",
                MeanDistance = new(0.72333199, 0.00000092 / Units.DaysPerCentury),
                Eccentricity = new(0.00677323, -0.00004938 / Units.DaysPerCentury),
                Inclination = new(3.39471, -2.86 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(76.68069, -996.89 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(131.53298, -108.80 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(181.97973, 210664136.06 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 4.869E24,
                Period = 19.4141664E6
            });

            Bodies.Add(new()
            {
                Name = "Earth",
                MeanDistance = new(1.00000011, -0.00000005 / Units.DaysPerCentury),
                Eccentricity = new(0.01671022, -0.00003804 / Units.DaysPerCentury),
                Inclination = new(0.00005, -46.94 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(-11.26064, -18228.25 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(102.94719, 1198.28 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(100.46435, 129597740.63 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 5.978E24,
                Period = 31.5581184E6
            });

            Bodies.Add(new()
            {
                Name = "Mars",
                MeanDistance = new(1.52366231, -0.00007221 / Units.DaysPerCentury),
                Eccentricity = new(0.09341233, 0.00011902 / Units.DaysPerCentury),
                Inclination = new(1.85061, -25.47 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(49.57854, -1020.19 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(336.04084, 1560.78 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(355.45332, 68905103.78 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 6.420E23,
                Period = 59.355072E6
            });

            Bodies.Add(new()
            {
                Name = "Jupiter",
                MeanDistance = new(5.20336301, 0.00060737 / Units.DaysPerCentury),
                Eccentricity = new(0.04839266, -0.00012880 / Units.DaysPerCentury),
                Inclination = new(1.30530, -4.15 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(100.55615, 1217.17 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(14.75385, 839.93 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(34.40438, 10925078.35 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 1.899E27,
                Period = 374.335776E6
            });

            Bodies.Add(new()
            {
                Name = "Saturn",
                MeanDistance = new(9.53707032, -0.00301530 / Units.DaysPerCentury),
                Eccentricity = new(0.05415060, -0.00036762 / Units.DaysPerCentury),
                Inclination = new(2.48446, 6.11 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(113.71504, -1591.05 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(92.43194, -1948.89 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(49.94432, 4401052.95 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 5.685E26,
                Period = 929.59488E6
            });

            Bodies.Add(new()
            {
                Name = "Uranus",
                MeanDistance = new(19.19126393, 0.00152025 / Units.DaysPerCentury),
                Eccentricity = new(0.04716771, -0.00019150 / Units.DaysPerCentury),
                Inclination = new(0.76986, -2.09 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(74.22988, -1681.40 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(170.96424, 1312.56 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(313.23218, 1542547.79 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 8.686E25,
                Period = 2.651184E9
            });

            Bodies.Add(new()
            {
                Name = "Neptune",
                MeanDistance = new(30.06896348, -0.00125196 / Units.DaysPerCentury),
                Eccentricity = new(0.00858587, 0.0000251 / Units.DaysPerCentury),
                Inclination = new(1.76917, -3.64 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(131.72169, -151.25 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(44.97135, -844.43 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(304.88003, 786449.21 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 1.025E26,
                Period = 5.20043328E9
            });

            Bodies.Add(new()
            {
                Name = "Pluto",
                MeanDistance = new(39.48168677, -0.00076912 / Units.DaysPerCentury),
                Eccentricity = new(0.24880766, 0.00006465 / Units.DaysPerCentury),
                Inclination = new(17.14175, 11.07 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfAscendingNode = new(110.30347, -37.33 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                LongitudeOfPerihelion = new(224.06676, -132.25 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                MeanLongitude = new(238.92881, 522747.90 / (Units.ArcSecondsPerDegree * Units.DaysPerCentury)),
                Mass = 5.0E23,
                Period = 7.81619328E9
            });
        }
    }
}
