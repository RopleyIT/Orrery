using Orrery;
using SvgPlotter;
using System.Drawing;

namespace OrreryPlotter;

public class SkyChart
{
    readonly SVGCreator svg = new();

    public PointF LongLat { get; set; }
    public List<string> BodyNames { get; init; } = new();
    public DateTimeOffset Start { get; set; } = DateTimeOffset.Now;
    public TimeSpan Interval { get; set; } = TimeSpan.FromDays(7);
    public DateTimeOffset End { get; set; } = DateTimeOffset.Now.AddYears(1);

    public string PlotSvg()
    {
        // Turn off header, width and height

        svg.HasXmlHeader = false;
        svg.HasWidthAndHeight = false;

        // First plot the axes

        svg.AddLine(new PointF(-900, 0), new PointF(900, 0), "gray", 2);
        svg.AddLine(new PointF(0, -450), new PointF(0, 450), "gray", 2);
        for (int i = -450; i < 450; i += 50)
        {
            if (i == 0)
                continue;
            var element = svg.AddLine(new PointF(-900, i), new PointF(900, i), "gray", 1);
            element.SetDashes(5, 10);
            svg.AddText($"{-i / 5}°", new PointF(10, i + 10));
        }
        for (int j = -900; j <= 900; j += 75)
        {
            if (j == 0)
                continue;
            var element = svg.AddLine(new PointF(j, -450), new PointF(j, 450), "gray", 1);
            element.SetDashes(5, 10);
            svg.AddText($"{j / 5}°", new PointF(j - 15, -5));
        }

        // Now plot the times of day the plot took place

        string times = $"From: {Start:dd MMM yy HH:mm:ss} UTC";
        svg.AddText(times, new PointF(-350, -425));
        times = $"To: {End:dd MMM yy HH:mm:ss} UTC";
        svg.AddText(times, new PointF(-350, -400));
        times = $"Interval: {TimeSpanStr(Interval)}";
        svg.AddText(times, new PointF(-350, -375));

        // Plot the compass points

        svg.AddText("N", new PointF(5, 25));
        svg.AddText("NE", new PointF(230, 25));
        svg.AddText("E", new PointF(455, 25));
        svg.AddText("SE", new PointF(680, 25));
        svg.AddText("S", new PointF(-895, 25));
        svg.AddText("SW", new PointF(-670, 25));
        svg.AddText("W", new PointF(-445, 25));
        svg.AddText("NW", new PointF(-220, 25));

        for (int i = 0; i < BodyNames.Count; i++)
        {
            int colourIndex = CelestialBody.PlanetIndex(BodyNames[i]);
            PlotKey(BodyNames[i], 
                new PointF(-900 + 175 * (i % 10), 425 - (i / 10) * 50), 
                colourIndex);
        }

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
            PlotBody(svg, locations[i], CelestialBody.PlanetIndex(locations[i].Body.Name));

        svg.CalculateViewBox(new SizeF(20, 20));
        return svg.ToString();
    }

    private static string TimeSpanStr(TimeSpan ts)
    {
        string result = string.Empty;
        if (ts >= TimeSpan.FromDays(1))
        {
            result += $"{ts.Days}d ";
            ts -= TimeSpan.FromDays(ts.Days);
        }
        if (ts >= TimeSpan.FromHours(1))
        {
            result += $"{ts.Hours}h ";
            ts -= TimeSpan.FromHours(ts.Hours);
        }
        if (ts >= TimeSpan.FromMinutes(1))
        {
            result += $"{ts.Minutes}m ";
            ts -= TimeSpan.FromMinutes(ts.Minutes);
        }
        if(ts.Seconds > 0)
            result += $"{ts.Seconds}s";
        return result;
    }

    private int FarthestLast(BodyLocation near, BodyLocation far)
    {
        if (far.Distance < near.Distance)
            return -1;
        if (far.Distance > near.Distance)
            return 1;
        return 0;
    }

    private readonly string[] bodyColours = new string[]
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

    void PlotKey(string text, PointF location, int colour)
    {
        svg.AddCircle(location, 10, "black", 1, bodyColours[colour]);
        svg.AddText(text, new PointF(location.X + 25, location.Y + 10));
    }
    void PlotBody(SVGCreator svg, BodyLocation loc, int colour)
    {
        if (colour >= 0 && loc != null && svg != null)
        {
            PointF centre = new((float)loc.Azimuth * 5, (float)loc.Elevation * -5);
            svg.AddCircle(centre, 10, "black", 1, bodyColours[colour]);
        }
    }
}
