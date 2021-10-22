using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orrery
{
    /// <summary>
    /// Implement a coordinate in three dimensions
    /// </summary>
    
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Coordinate(Coordinate other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        public Coordinate()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public void ScaleBy(double scale)
        {
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        /// <summary>
        /// Magnitude of the coordinate in
        /// spherical polar coordinates
        /// </summary>
        
        public double R =>
            Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Angle anticlockwise about Z axis from
        /// positive X axis. Measured in degrees
        /// </summary>
        
        public double RA
        {
            get
            {
                return Units.RadToDeg(Math.Atan2(Y, X));
            }
            set
            {
                double xym = XYMagnitude;
                double angle = Units.DegToRad(value);
                X = xym * Math.Cos(angle);
                Y = xym * Math.Sin(angle);
            }
        }

        /// <summary>
        /// Angle upwards from the X-Y plane (negative
        /// if below the XY plane). Measured in degrees
        /// </summary>
        
        public double Dec
        {
            get
            {
                return Units.RadToDeg(Math.Atan2(Z, XYMagnitude));
            }
            set
            {
                double r = R;
                double angle = Units.DegToRad(value);
                Z = r * Math.Sin(angle);
                double xyScale = r * Math.Cos(angle) / XYMagnitude;
                X *= xyScale;
                Y *= xyScale;
            }
        }

        public void Add(Coordinate offset)
        {
            X += offset.X;
            Y += offset.Y;
            Z += offset.Z;
        }

        public void Subtract(Coordinate offset)
        {
            X -= offset.X;
            Y -= offset.Y;
            Z -= offset.Z;
        }

        private double XYMagnitude
            => Math.Sqrt(X * X + Y * Y);

        public override string ToString()
        {
            return $"({X},{Y},{Z}) R={R}, RA={RA}, Dec={Dec}";
        }
    }
}
