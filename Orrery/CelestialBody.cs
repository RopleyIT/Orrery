using System;

namespace Orrery
{
    /// <summary>
    /// Class to track the details and positions in
    /// the sky of a Heliocentric orbiting body
    /// such as a planet, planetoid, or asteroid.
    /// </summary>
    
    public class CelestialBody
    {
        /// <summary>
        /// Name of the celestial object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mass of object in kilograms
        /// </summary>
        public double Mass { get; set; }

        /// <summary>
        /// Orbital period in seconds
        /// </summary>
        public double Period { get; set; }

        /// <summary>
        /// Semi major axis of orbital ellipse, in
        /// Astronomical Units (AU), and rate of
        /// change in AU/Julian day
        /// </summary>
        public Polynomial MeanDistance { get; set; }

        /// <summary>
        ///  Elliptical eccentricity of orbit,
        ///  and rate of change per day
        /// </summary>
        public Polynomial Eccentricity { get; set; }

        /// <summary>
        /// Inclination of orbit to the ecliptic
        /// Degrees for coeff 0, degrees/day for coeff 1
        /// </summary>
        public Polynomial Inclination { get; set; }

        /// <summary>
        /// Mean longitude of object at a given time
        /// Degrees for coeff 0, degrees/day for coeff 1
        /// </summary>
        public Polynomial MeanLongitude { get; set; }

        /// <summary>
        /// Longitude of closest point to body around which
        /// we are orbiting.
        /// Degrees for coeff 0, degrees/day for coeff 1
        /// </summary>
        public Polynomial LongitudeOfPerihelion { get; set; }

        /// <summary>
        /// Longitude at which orbit crosses the plane of the
        /// ecliptic in the 'upward' direction
        /// Degrees for coeff 0, degrees/day for coeff 1
        /// </summary>
        public Polynomial LongitudeOfAscendingNode { get; set; }

        /// <summary>
        /// Earth radius in astronomical units
        /// </summary>
        
        public const double EarthRadius =  6.371E6/Units.MetresPerAU;

        // Finding the position of a planet on its orbit at a given time
        // is not straightforward with a single equation, due to problems
        // making position a function of time. An iterative algorithm is used
        // to locate a planet in its orbit relative to perihelion.
        // The `eccentric anomaly' of a planet is the angle at the CENTRE of
        // its elliptical orbit, between the planet position and a line drawn
        // to the perihelion point. N.B. This is NOT the angle formed at the sun.
        // 
        // The Newton-Raphson iterative root finder is used to obtain a new
        // eccentric anomaly, E. Assuming a known eccentric anomaly in the
        // orbit was E0 at time t0, the new angle at time t will be
        // E = dE + E0, where the relationship between the two is given by:-
        // 
        // dE - e*cos(E0)*sin(dE) + (1 - cos(dE))*e*sin(E0) - 2*PI*(t-t0)/T == 0
        // 
        // The value 2*PI*(t-t0)/T is the value L-w with t0 coinciding with
        // perihelion. At perihelion E0 is zero, simplifying the above equation
        // to dE - e*sin(dE) - L + w == 0
        // 
        // Newton-Raphson states that if dE1 is a good guess for a root of f(dE)
        // then a better approximation will be given by:-
        // 
        // dE2 = dE1 - f(dE1) / ( d(f(dE1))/dx )
        // 
        // which involves the derivative of f(dE) being:-
        // 
        // d(f(dE))/dx = 1 - e*cos(E0)*cos(dE) + e*sin(E0)*sin(dE)
        // 
        // or with E0 being zero simplifies to 1 - e*cos(dE).
        // 
        // This function takes as its input a value of L and w for a chosen time,
        // the eccentricity `e' of the orbit of the planet,
        // and returns a value for E at the chosen time. A guess at dE1 is
        // made using the first-order estimate of:-
        // 
        // dE1 = 0
        // 
        // (which can be improved considerably by using a trigonometric function)
        // 
        // The value DELTA gives the precision at which the N-R algorithm ends.
        // WARNING! Calculations in calculus obviously use radians not degrees.

        private const double DELTA = 1.0E-14;

        // Calculate the eccentric anomaly for a body in an elliptical orbit
        // <param name="L">Longitude of body</param>
        // <param name="w">Longitude of perihelion</param>
        // <param name="e">Eccentricity of orbit</param>
        // <returns>The eccentric anomaly, being the angle at the CENTRE of
        // its elliptical orbit, between the planet position and a line drawn
        // to the perihelion point. N.B. This is NOT the angle formed at the 
        // sun nor is it the angle formed at the body-sun barycentre.</returns>
        
        static double CalcE(double L, double w, double e)
        {
            double dE1, dE2;          // Estimates for eccentric anomaly 
            double delta;             // Difference in successive approximations
            double ma = Units.DegToRad(L - w) % (2 * Math.PI); // Mean anomaly in radians

            dE2 = ma + e * Math.Sin(ma); // Initial approximation for eccentric anomaly
            do
            {
                dE1 = dE2;              // Use next approximation value
                double cdE = Math.Cos(dE1);
                double sdE = Math.Sin(dE1);
                delta = dE1 - e * sdE - ma;
                delta /= 1 - e * cdE;
                dE2 -= delta;
            } while (Math.Abs(delta) > DELTA);
            return Units.RadToDeg(dE2);            // Angle required in degrees 
        }

        // Given an eccentric anomaly E, the semi-major axis of the orbit `a',
        // and the eccentricity of the orbit `e', we can calculate the cartesian
        // coordinates of the planet relative to the sun. Note that these are
        // coordinates in the plane of the planet's own orbit, with the X-axis
        // pointing from the sun towards perihelion, and Y-axis 90 degrees further
        // round the orbit, i.e. as viewed from above the north poles.
        // The equation which provides radial distance is: r = a*(1 - e*cos(E)),
        // from which distance at perihelion and aphelion are obviously given by
        // a*(1-e) and a*(1+e) respectively. E is measured here in radians.
        // 
        // The equations for X and Y also include translation of the origin to the
        // centre of the sun, even though E is an angle at the centre of the
        // elliptical orbit.

        static double CalcX(double a, double E, double e) 
            => a * (Math.Cos(Units.DegToRad(E)) - e);

        static double CalcY(double a, double E, double e) 
            => a * Math.Sqrt(1 - e * e) * Math.Sin(Units.DegToRad(E));

        // Velocity components of the planets parallel to the above x and y
        // axes are necessary to be able to calculate the apparent shift of
        // a planet due to aberration, i.e. finite speed of light combined
        // with the relative speed of the earth and the observed planet.

        static double CalcU(double a, double E, double r) 
            => -0.0172021 * Math.Sqrt(a) * Math.Sin(Units.DegToRad(E)) / r;

        static double CalcV(double a, double E, double r, double e) 
            => 0.0172021 * Math.Sqrt(a * (1 - e * e)) * Math.Cos(Units.DegToRad(E)) / r;

        // The standard cartesian coordinate system for space allows a planet
        // to be located at some point (X,Y,Z) in space relative to the sun.
        // The orientation of the X,Y,Z axes is determined as follows.
        // The X,Y plane is the plane in which the earth rotates about the sun,
        // and is known as the `plane of the ecliptic'.
        // 
        // The direction of the X axis is pointing towards a place on the ecliptic
        // known as the `First point of Aries'. Roughly speaking, this is the
        // position the sun appears in the star sphere at the Vernal (spring)
        // Equinox, i.e. directly overhead on the equator at noon on March 21st.
        // It is the line along which the earth's equatorial plane intersects
        // the ecliptic plane (plane of earth's orbit round the sun).
        // 
        // Planetary orbits (other than the earth) are inclined to the plane of
        // the ecliptic by a small angle `i'. Their orbital plane cuts the
        // plane of the ecliptic along the `line of nodes' which obviously
        // passes through the sun. The line from the sun to the point where the
        // planet's orbit passes upwards (northward) through the ecliptic is known
        // as the `ascending node', and makes an angle `W' with the X axis.
        // 
        // The point of perihelion is at a further `w' radians from the ascending
        // node around the plane of the planet's orbit. This is not the same as
        // the `w' parameter passed to the following routine, which should really
        // be called `w-bar', the longitude of perihelion relative to 1 pt. Aries.
        // 
        // From this information, we can obtain the absolute position of the
        // planet in heliocentric coordinates, if we know the coordinates of the
        // planet within its own ellipse. We can also use exactly the same
        // function to translate the velocity components to heliocentric coords.
        // Just substitute u for x and v for y.

        static Coordinate ToHelioCoord(double x, double y, double W, double w, double i)
        {

            double cw = Math.Cos(Units.DegToRad(w - W)), sw = Math.Sin(Units.DegToRad(w - W)),
                    cW = Math.Cos(Units.DegToRad(W)), sW = Math.Sin(Units.DegToRad(W)),
                    ci = Math.Cos(Units.DegToRad(i)), si = Math.Sin(Units.DegToRad(i));
            return new()
            {
                X = (x * cw - y * sw) * cW - (x * sw + y * cw) * sW * ci,
                Y = (x * cw - y * sw) * sW + (x * sw + y * cw) * cW * ci,
                Z = (x * sw + y * cw) * si
            };
        }

        // The Earth's coordinates as derived from the preceding parts of
        // this program are actually for the earth-moon `barycentre', i.e.
        // the centre of gravity about which they rotate. These have to
        // be translated back to the position of the earth for the earth-bound
        // observer. The equations for translation are:-
        //    X' = X - 0.0000312*cos(H); Y' = Y - 0.0000312*sin(H); Z' = Z
        // where the angle H in degrees is given by:-
        //    H = 218 + 481268*T,  T being in Julian centuries (36525 days)
        // relative to the 2000.0 epoch, JD2451545.0

        static void BaryTranslate(Coordinate emb, double jd)
        {
            double H = Units.DegToRad(218.0 + 481268.0 * (jd - Units.JulianDate2000) / 36525.0);

            emb.X -= 0.0000312 * Math.Cos(H);
            emb.Y -= 0.0000312 * Math.Sin(H);
        }

        // Given a particular planet in the sky, and a Julian date
        // calculate the ecliptic (heliocentric) coordinates of that planet.

        static (Coordinate Position, Coordinate Velocity) GetHeliocentric
        (
                CelestialBody plan, // Body to find coordinates for
                double absJD        // Time for which planet location required 
        ) 
        {
            Coordinate pos = new();
            Coordinate vel = new();
            double a, e, i, L, w, W, E, r, x, y, u, v;

            if(string.Compare(plan.Name, "Sun", true ) == 0) // If looking for the sun
            { 
                pos.X = pos.Y = pos.Z = 0.0;    // Sun at origin, as heliocentric
                vel.X = vel.Y = vel.Z = 0.0;    // Sun stationary, as heliocentric
                return (pos, vel);
            }

            double jd = absJD - Units.JulianDate2000;
            a = plan.MeanDistance.At(jd);  
            e = plan.Eccentricity.At(jd); 
            i = plan.Inclination.At(jd); 
            L = plan.MeanLongitude.At(jd);
            w = plan.LongitudeOfPerihelion.At(jd);
            W = plan.LongitudeOfAscendingNode.At(jd);

            E = CalcE(L, w, e );           // Iterative search for eccentric anomaly

            x = CalcX(a, E, e );           //  x,y are coords within plane of orbit
            y = CalcY(a, E, e );
            r = Math.Sqrt(x*x + y*y);      // Distance of planet from sun

            u = CalcU(a, E, r );           // Velocity components in orbital plane
            v = CalcV(a, E, r, e );        // used to calculate relativistic aberration 

            pos = ToHelioCoord(x, y, W, w, i); // Convert to heliocentric coordinates
            if(plan.Name == "Earth" )          // For earth, just move from barycentre
                    BaryTranslate(pos, absJD );
            vel = ToHelioCoord(u, v, W, w, i); // Heliocentric velocities
            return (pos, vel);
        }

        // Given a planet to search for, and a time, find that planet in
        // Geocentric ecliptic coordinates, i.e. the XYZ axes centred on the
        // earth rather than the sun, but still parallel to their previous
        // directions. No earth tilt included just yet.
        // 
        // In this routine, the effects of light aberration are removed. In the
        // time it takes for light to travel from a planet to the earth, both
        // bodies have moved! This means the planet will appear somewhere
        // different to its true location. The planetary velocity components
        // are used to remove this effect, if the flag `doAberr' is TRUE.

        static Coordinate GeoEcliptic(CelestialBody earth, CelestialBody plan, double jd, bool doAberr)
        {
            var pv = GetHeliocentric(plan, jd);  /* Find planet itself    */
            var ev = GetHeliocentric(earth, jd); /* Needed for rel coords */
            Coordinate ppos = pv.Position;
            Coordinate pvel = pv.Velocity;
            Coordinate epos = ev.Position;
            Coordinate evel = ev.Velocity;

            ppos.Subtract(epos);
            pvel.Subtract(evel);

            // Apply aberration adjustment based on time taken for
            // light to travel to Earth while Earth is rotating

            double tau = doAberr ? ppos.R / 173.1446 : 0.0;
            Coordinate p = new(ppos);
            pvel.ScaleBy(tau);
            p.Subtract(pvel);
            return p;
        }

        // Routines for converting Geocentric ecliptic coordinates to
        // geocentric equatorial. Several awkward things need to be done here.
        // 
        // (i)     The line through the poles of the earth is inclined to the
        //         ecliptic poles by about 23.5 degrees. This polar line is
        //         in a plane at right angles to the line to the first point
        //         of Aries. The earth's poles rotate about the ecliptic polar
        //         direction at a rate of 50.2" per year, drawing out a cone
        //         once every 25,800 years (which is why the Ancient
        //         astronomers' first point of Aries is now in Pisces!). Thus
        //         the ecliptic axes must be rotated by +t*(50.2") if t is in
        //         years since the 2000.0 epoch.
        // 
        // (ii)    Due to the earth's equatorial bulge and the motion of the
        //         moon and sun relative to the earth, the earth nods or suffers
        //         `nutation', which must also be accommodated when translating
        //         to equatorial coordinates.
        // 
        // (iii)   The whole celestial sphere needs to be rotated about the line
        //         to the first point of Aries by approx. 23.5 degrees to make the
        //         planetary positions geocentric equatorial. This angle of
        //         obliquity is reducing by 0.013 of a degree per century at the
        //         moment, which must be accommodated.

        static void Precess( Coordinate p, double jd )
        {
            double a, b, c;                 /* Used for precession and nutation */

            jd = (jd - Units.JulianDate2000)/36525;    /* Centuries relative to 2000.0     */
            a = 1.397*jd;                   /* Setup parameters for precession  */
            b = 0.0131*jd;
            c = 5.1236 + 0.2416*jd;
            var ra = Units.DegToRad(p.RA + c);
            p.Dec += b * Math.Sin(ra);
            p.RA += a - b * Math.Cos(ra)*Math.Tan(Units.DegToRad(p.Dec));
        }

        static double Nutate( Coordinate p, double jd )
        {
            double dp, de;       /* Rotary and declination nutation components   */

            jd = jd - Units.JulianDate2000;                  /* Days relative to 2000.0     */
            double a1 = Units.DegToRad(125.0 - 0.05295 * jd);
            double a2 = Units.DegToRad(200.9 + 1.97129 * jd);
            dp = -0.0048* Math.Sin(a1);
            dp -= 0.0004* Math.Sin(a2);
            de = 0.0026* Math.Cos(a1);
            de += 0.0002* Math.Cos(a2);

            p.RA += dp;          /* Add rotary nutation component straight in */
            return de;           /* Return nodding component to caller        */
        }

        static void GeoEquatorial( Coordinate p, double jd, double de )
        {
            double ra = Units.DegToRad(p.RA);
            double dec = Units.DegToRad(p.Dec);
            double cra = Math.Cos(ra);
            double sra = Math.Sin(ra);
            double cdec = Math.Cos(dec);
            double sdec = Math.Sin(dec);
            double o, so, co;       /* Obliquity of earth's equator to ecliptic  */

            jd = (jd - Units.JulianDate2000) / 36525;    /* Centuries relative to 2000.0     */

            /* Now find the earth's obliquity, and then rotate the coordinates   */
            o = Units.DegToRad(23.4393 - 0.0130 * jd + de); /* Nutation component */
            so = Math.Sin(o);
            co = Math.Cos(o);

            p.Dec = Units.RadToDeg(Math.Asin(cdec * sra * so + sdec * co));
            p.RA = Units.RadToDeg(Math.Atan2(cdec * sra * co - sdec * so, cdec * cra));
        }

        //      Routines for translating from geoequatorial RA and dec to Greenwich
        //      This is the point on the earth's surface for which the planet
        //      in question is directly overhead.
        //      The first routine calculates the Right Ascension of the Greenwich
        //      meridian at a given time and date, in degrees.
        //      The second routine translates coordinates to Greenwich-relative.
        //      N.B. The planet longitude is in astronomical longitude,
        //      i.e. degrees East of Greenwich, rather than degrees West.

        static double GetMeridianRA(double jd)
        {
            double ra;
            double tc = (Math.Floor(jd) - 2415020.0 + 0.5) / 36525.0;

            // Firstly get RA at 0h U.T. on chosen day for the meridian

            ra = 99.6909833 + 36000.7689 * tc + 0.00038708 * tc * tc;

            // Now add in the part of a day component

            ra += 0.25068447 * 60 * 24 * (jd - Math.Floor(jd) + 0.5);
            ra = ra % 360.0; // Round down to one revolution 
            return ra;
        }

        static void ToGreenwichCoords(Coordinate pc, double jd) 
            => pc.RA -= GetMeridianRA(jd);

        // For telescope-steering purposes, we need to be able to calculate
        // the azimuth and elevation of a planet from the ground station.
        // The following routines do this, returning in a pcoord structure
        // the azimuth and elevation, together with the distance of the
        // planet from the observer, rather than the earth centre.
        // The parameters y and z are the longitude and latitude of the ground
        // station in degrees, while p is the long/lat/earth-centre distance
        // of the planet, in Greenwich-relative coordinates.
        // Note that the observer's azimuth is changed in this function to
        // degrees east(Clockwise from North).
        // To revert to degrees west, reverse the subtraction
        // that produces pa near the top of this routine.

        static Coordinate FindAzimElev( Coordinate p, double y, double z )
        {
            double pa;      // Difference in W. longitudes of SPP and observer
            double csg;     // Cosine of angle at earth centre between ground 
                            // station and sub-planetary point - to be found 

            z = Units.DegToRad(z);
            double cz = Math.Cos(z);
            double sz = Math.Sin(z);

            // Set up difference in W. longitudes of SPP and observer
            // NB This way round to swap E. longitudes to diff in W. longitudes 
            
            pa = Units.DegToRad(y - p.RA);
            while(pa < -Math.PI)       // Map into the range -PI to +PI
                    pa += 2*Math.PI;
            while(pa > Math.PI)
                    pa -= 2*Math.PI;

            // Calculate cosine of angle between SPP and observer
            double dec = Units.DegToRad(p.Dec);
            csg = cz*Math.Cos(dec) * Math.Cos(pa)
                    + sz* Math.Sin(dec);

            // Find distance from observer to planet 

            double r = EarthRadius / p.R;
            Coordinate ae = new()
            {
                X = p.R * Math.Sqrt(1 + r * r - 2 * r * csg),
                Y = 0,
                Z = 0
            };

            // Find elevation of planet relative to observer

            ae.Dec = 90.0 - Units.RadToDeg(Math.Acos((p.R * csg - EarthRadius) / ae.X ) );

            // Azimuth of planet relative to observer 

            double ra = Math.Sin(dec) - sz*csg;
            ra /= cz* Math.Sin(Math.Acos(csg));
            ra = Units.RadToDeg(Math.Acos(ra));
            if(pa > 0.0 )
                ae.RA = 360.0 - ra;
            else
                ae.RA = ra;
            return ae;
        }

        // Do all the work to find a planet's right ascension and declination
        // relative to the earth-equatorial plane and the first point of Aries

        static Coordinate FindRADec(CelestialBody earth, CelestialBody plan, double jd, bool doAberration)
        {
            double de = 0.0;                /* Trans-polar nutation component   */

            Coordinate p = GeoEcliptic(earth, plan, jd, doAberration);
            Precess( p, jd );      /* Accommodate precession           */
            de = Nutate( p, jd );  /* Correct for nutation             */
            GeoEquatorial( p, jd, de );    /* Translate to earth coordinates   */
            if(p.RA < 0.0 )                /* All right ascensions positive    */
                    p.RA += 360.0;
            return p;
        }

        // The set of planetary data held by the orrery

        private static Planets planets = null;


        /// <summary>
        /// The users' entry point to the library routines. This function
        /// returns a copy of a celestialObject structure, that gives distances,
        /// RA and declination in geo-equatorial coordinates, and distances
        /// azimuth and elevation as seen from the user's earth longitude and
        /// latitude.
        /// </summary>
        /// <param name="planetName">Name of the planet to be found, or
        /// the name "Sun" for finding the sun's position</param>
        /// <param name="jd">The astronomical Julian Date</param>
        /// <param name="obsLong">Observer's longitude in degrees</param>
        /// <param name="obsLat">Observer's latitude in degrees</param>
        /// <returns>A BodyLocation object, describing all positional data
        /// about the celestial body at the chosen date and time</returns>
        /// <exception cref="ArgumentException">Thrown if an invalid
        /// planet name is provided as an argument</exception>
        
        public static BodyLocation Find
            (
                string planetName,      /* Which celestial object we are seeking */
                double jd,              /* The time for which we seek it         */
                double obsLong,         /* Observer's long and lat in degrees    */
                double obsLat
            )
        {
            if(planets == null)
                planets = new Planets();
            CelestialBody plan = planets.FindBody(planetName);
            if (plan == null)
                throw new ArgumentException("Invalid planet name");
            CelestialBody earth = planets.FindBody("Earth");
            BodyLocation co = new();
            co.JulianDate = jd;
            co.Body = plan;
            Coordinate raDec = FindRADec(earth, plan, jd, true );
            co.RightAscension = raDec.RA;
            co.Declination = raDec.Dec;
            ToGreenwichCoords( raDec, jd );
            co.Longitude = raDec.RA;
            raDec = FindAzimElev(raDec, obsLong, obsLat );
            co.Azimuth = raDec.RA;
            co.Elevation = raDec.Dec;
            co.Distance = raDec.R;
            return co;
        }
    }
}
