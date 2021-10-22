using Orrery;
using SvgPlotter;
using System.Drawing;

namespace OrreryPlotter
{
    public class SkyChart
    {
        SVGCreator svg = new SVGCreator();

        public PointF LongLat { get; set; }
        public List<string> BodyNames { get; init; } = new();
        public DateTimeOffset Start { get; set; } = DateTimeOffset.Now;
        public TimeSpan Interval { get; set; } = TimeSpan.FromDays(7);
        public DateTimeOffset End { get; set; } = DateTimeOffset.Now.AddYears(1);

        public string PlotSvg()
        {
            // First plot the axes

            svg.AddLine(new PointF(-900, 0), new PointF(900, 0), "gray", 2);
            svg.AddLine(new PointF(0, -450), new PointF(0, 450), "gray", 2);
            for(int i = -450; i < 450; i += 50)
            {
                if (i == 0) 
                    continue;
                var element = svg.AddLine(new PointF(-900, i), new PointF(900, i), "gray", 1);
                element.SetDashes(5, 10);
                svg.AddText($"{-i / 5}°", new PointF(10, i+10));
            }
            for(int j = -900; j <= 900; j += 75)
            {
                if (j == 0)
                    continue;
                var element = svg.AddLine(new PointF(j, -450), new PointF(j, 450), "gray", 1);
                element.SetDashes(5, 10);
                svg.AddText($"{j / 5}°", new PointF(j - 15, -5));
            }
            svg.AddText("N", new PointF(5, 25));
            svg.AddText("NE", new PointF(230, 25));
            svg.AddText("E", new PointF(455, 25));
            svg.AddText("SE", new PointF(680, 25));
            svg.AddText("S", new PointF(-895, 25));
            svg.AddText("SW", new PointF(-670, 25));
            svg.AddText("W", new PointF(-445, 25));
            svg.AddText("NW", new PointF(-220, 25));

            for (int i = 0; i < BodyNames.Count; i++)
                PlotKey(i, BodyNames[i], new PointF(-900 + 175 * i, 425));

            // Now plot each planet's position

            List<BodyLocation> locations = new();
            DateTimeOffset dt = Start;
            while (dt < End)
            {
                for (int i = 0; i < BodyNames.Count; i++)
                    locations.Add(CelestialBody.Find
                        (BodyNames[i], Units.JulianDate(dt), LongLat.X, LongLat.Y));
                dt = dt.Add(Interval);
            }
            locations.Sort(FarthestLast);
            for (int i = 0; i < locations.Count; i++)
                PlotBody(svg, locations[i], BodyNames.IndexOf(locations[i].Body.Name));

            svg.CalculateViewBox(new SizeF(20, 20));
            return svg.ToString();
        }

        private int FarthestLast(BodyLocation near, BodyLocation far)
        {
            if (far.Distance < near.Distance)
                return -1;
            if (far.Distance > near.Distance)
                return 1;
            return 0;
        }

        private string[] bodyColours = new string[]
        {
            "gold",
            "darkgreen",
            "darkred",
            "cornflowerblue",
            "darkorange",
            "green",
            "red",
            "cadetblue",
            "yellow",
            "greenyellow",
            "tomato",
            "blue",
            "coral",
            "olive",
            "salmon",
            "darkblue",
            "darkmagenta"
        };

        void PlotKey(int itemNumber, string text, PointF location)
        {
            svg.AddCircle(location, 10, "black", 1, bodyColours[itemNumber]);
            svg.AddText(text, new PointF(location.X + 25, location.Y+10));
        }
        void PlotBody(SVGCreator svg, BodyLocation loc, int colour)
        {
            PointF centre = new PointF((float)loc.Azimuth * 5, (float)loc.Elevation * -5);
            svg.AddCircle(centre, 10, "black", 1, bodyColours[colour]);
        }
    }
}