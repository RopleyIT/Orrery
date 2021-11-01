using System;

namespace Orrery
{
    public static class Units
    {
        public const int SecondsPerDay = 86400;
        public const int DaysPerCentury = 36525;
        public const int ArcSecondsPerDegree = 3600;
        public const int ArcMinutesPerDegree = 60;
        public const long MetresPerAU = 149597870700L;
        //public static readonly DateTimeOffset Jan1st1970
        //    = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        //public const double JulianDateOfJan1st1970 = 2440587.5;
        public const double JulianDate2000 = 2451545.0;
        public static readonly DateTimeOffset Date2000
            = new (2000, 1, 1, 12, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Conversion of a DateTime into Julian Date.
        /// Julian Date as defined by JF Herschel is measured in whole earth-days since
        /// noon on Jan 1st, 4713 BC. UNIX or 'C' time is measured in seconds
        /// since midnight preceding Jan 1st. 1970. The Julian Date of that
        /// time instant is 2440587.5 days.
        /// </summary>
        /// <param name="t">A .NET date and time expressed as a DateTimeOffset</param>
        /// <returns>The equivalent Julian date</returns>

        public static double JulianDate(DateTimeOffset t)
            => JulianDate2000 + (t - Date2000).TotalDays;

        /// <summary>
        /// COnvert a Julian date into its equivalent .NET time
        /// </summary>
        /// <param name="jd">The Julian date to convert</param>
        /// <returns>The equivalent .NET time</returns>

        public static DateTimeOffset AsDateTime(double jd)
            => Date2000.AddDays(jd - JulianDate2000);

        /// <summary>
        /// Express a Julian date as a printable string
        /// </summary>
        /// <param name="jd">The Julian date to render</param>
        /// <returns>The Julian date in string form</returns>

        public static string JulianDateAsString(double jd)
        {
            return $"JD {jd} = {AsDateTime(jd):G}";
        }

        /// <summary>
        /// Convert angle in degrees to radians
        /// </summary>
        /// <param name="v">Angle in degrees</param>
        /// <returns>Angle in radians</returns>

        public static double DegToRad(double v)
            => v * Math.PI / 180;

        /// <summary>
        /// Convert angle in radians to degrees
        /// </summary>
        /// <param name="v">Angle in radians</param>
        /// <returns>Angle in degrees</returns>

        public static double RadToDeg(double v)
            => v * 180 / Math.PI;

        /// <summary>
        /// Duration of a sidereal day, time taken
        /// for the firmament to rotate to same
        /// longitude in the sky.
        /// </summary>

        public readonly static TimeSpan SiderealDay
            = new (0, 23, 56, 4, 100);
    }
}
