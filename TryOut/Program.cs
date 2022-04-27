using OrreryPlotter;
using Orrery;

using StreamWriter sw = new("tryout.svg", false);
SkyChart sc = new();
sc.Start = DateTimeOffset.Now;
sc.End = sc.Start.AddDays(365);
//sc.Interval = Units.SiderealDay;
sc.Interval = TimeSpan.FromDays(1);
sc.LongLat = new((float)-3.0307, (float)56.3773); // Rathillet, Fife
sc.BodyNames.Add("Sun");
sc.BodyNames.Add("Mercury");
sc.BodyNames.Add("Venus");
sc.BodyNames.Add("Mars");
/*sc.BodyNames.Add("Jupiter");
sc.BodyNames.Add("Saturn");
sc.BodyNames.Add("Uranus");
sc.BodyNames.Add("Neptune");
sc.BodyNames.Add("Pluto");
sc.BodyNames.Add("Ceres");
sc.BodyNames.Add("Makemake");
sc.BodyNames.Add("Haumea");
sc.BodyNames.Add("Eris");*/
sw.WriteLine(sc.PlotSvg());
