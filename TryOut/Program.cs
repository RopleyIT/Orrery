using Orrery;
using OrreryPlotter;
using SvgPlotter;
using System.Drawing;
using System.IO;

using StreamWriter sw = new("tryout.svg", false);
SkyChart sc = new();
sc.Start = DateTimeOffset.Now;
//sc.Start = new DateTimeOffset(2021, 10, 20, 12, 0, 0, TimeSpan.Zero);
sc.End = sc.Start.AddDays(365);
sc.Interval = TimeSpan.FromDays(1);
sc.LongLat = new((float)-0.3735825, (float)52.8400336); // Rippingale
sc.BodyNames.Add("Sun");
sc.BodyNames.Add("Mercury");
sc.BodyNames.Add("Venus");
sc.BodyNames.Add("Mars");
sc.BodyNames.Add("Jupiter");
sc.BodyNames.Add("Saturn");
sc.BodyNames.Add("Uranus");
sc.BodyNames.Add("Neptune");
sc.BodyNames.Add("Pluto");
sw.WriteLine(sc.PlotSvg());
