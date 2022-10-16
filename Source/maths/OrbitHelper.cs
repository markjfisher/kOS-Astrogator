using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kOS.AddOns.kOSAstrogator.orbit
{
    /// <summary>
    /// Orbital functions
    /// </summary>
    public static class OrbitHelper
    {
        /// <summary>
        /// Returns the next time at which a will cross its ascending node with b.
        /// For elliptical orbits this is a time between UT and UT + a.period.
        /// For hyperbolic orbits this can be any time, including a time in the past if the ascending node is in the past.
        /// NOTE: this function will throw an ArgumentException if a is a hyperbolic orbit and the "ascending node" occurs at a true anomaly that a does not actually ever attain.
        /// Taken from Astrogator PhysicsTools, which in turn was borrowed from KAC's borrowing from r4m0n's MJ plugin.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="UT"></param>
        /// <returns></returns>
        public static double TimeOfAscendingNode(this Orbit a, Orbit b, double UT)
        {
            return a.TimeOfTrueAnomaly(a.AscendingNodeTrueAnomaly(b), UT);
        }

        /// <summary>
		/// Returns the next time at which a will cross its descending node with b.
		/// For elliptical orbits this is a time between UT and UT + a.period.
		/// For hyperbolic orbits this can be any time, including a time in the past if
		/// the descending node is in the past.
		/// NOTE: this function will throw an ArgumentException if a is a hyperbolic orbit and the "descending node"
		/// occurs at a true anomaly that a does not actually ever attain
        /// Taken from Astrogator PhysicsTools, which in turn was borrowed from KAC's borrowing from r4m0n's MJ plugin.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="UT"></param>
        /// <returns></returns>
        public static double TimeOfDescendingNode(this Orbit a, Orbit b, double UT)
        {
            return a.TimeOfTrueAnomaly(a.DescendingNodeTrueAnomaly(b), UT);
        }

        private static double AscendingNodeTrueAnomaly(this Orbit a, Orbit b)
        {
            Vector3d vectorToAN = Vector3d.Cross(a.SwappedOrbitNormal(), b.SwappedOrbitNormal());
            return a.TrueAnomalyFromVector(vectorToAN);
        }

        ///Gives the true anomaly (in a's orbit) at which a crosses its descending node
        ///with b's orbit.
        ///The returned value is always between 0 and 360.
        private static double DescendingNodeTrueAnomaly(this Orbit a, Orbit b)
        {
            Vector3d vectorToDN = Vector3d.Cross(b.SwappedOrbitNormal(), a.SwappedOrbitNormal());
            return a.TrueAnomalyFromVector(vectorToDN);
        }

        private static Vector3d SwappedOrbitNormal(this Orbit o)
        {
            return -o.GetOrbitNormal().SwapYZ().normalized;
        }

        ///For hyperbolic orbits, the true anomaly only takes on values in the range
        /// (-M, M) for some M. This function computes M.
        private static double MaximumTrueAnomaly(this Orbit o)
        {
            if (o.eccentricity < 1) return 180;
            else return 180 / Math.PI * Math.Acos(-1 / o.eccentricity);
        }

    }

    internal static class VectorExtensions
    {

        /// <summary>
        /// Return the vector with the Y and Z components exchanged
        /// Borrowed from MechJeb
        /// </summary>
        /// <param name="v">Input vector</param>
        /// <returns>
        /// Vector equivalent to (v.x, v.z, v.y)
        /// </returns>
        public static Vector3d SwapYZ(this Vector3d v)
        {
            return v.Reorder(132);
        }

        internal static Vector3d Reorder(this Vector3d vector, int order)
        {
            switch (order)
            {
                case 123:
                    return new Vector3d(vector.x, vector.y, vector.z);
                case 132:
                    return new Vector3d(vector.x, vector.z, vector.y);
                case 213:
                    return new Vector3d(vector.y, vector.x, vector.z);
                case 231:
                    return new Vector3d(vector.y, vector.z, vector.x);
                case 312:
                    return new Vector3d(vector.z, vector.x, vector.y);
                case 321:
                    return new Vector3d(vector.z, vector.y, vector.x);
            }
            throw new ArgumentException("Invalid order", "order");
        }
    }

}
