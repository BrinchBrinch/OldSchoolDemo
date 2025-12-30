using System;
using System.Drawing;
using OldSchoolDemo.Effects;

namespace OldSchoolDemo.Utils
{
    public static class Projection3D
    {
        /// <summary>
        /// Rotates a 3D point around the X, Y, and Z axes.
        /// </summary>
        public static Vector3D Rotate(Vector3D point, float axRad, float ayRad, float azRad)
        {
            // Rotate around X axis
            float cosX = (float)Math.Cos(axRad), sinX = (float)Math.Sin(axRad);
            float y1 = point.Y * cosX - point.Z * sinX;
            float z1 = point.Y * sinX + point.Z * cosX;

            // Rotate around Y axis
            float cosY = (float)Math.Cos(ayRad), sinY = (float)Math.Sin(ayRad);
            float x2 = point.X * cosY + z1 * sinY;
            float z2 = -point.X * sinY + z1 * cosY;

            // Rotate around Z axis
            float cosZ = (float)Math.Cos(azRad), sinZ = (float)Math.Sin(azRad);
            float x3 = x2 * cosZ - y1 * sinZ;
            float y3 = x2 * sinZ + y1 * cosZ;

            return new Vector3D(x3, y3, z2);
        }

        /// <summary>
        /// Projects a 3D point onto a 2D screen using perspective projection.
        /// </summary>
        public static PointF Project(Vector3D point, float perspective, float zOffset, float centerX, float centerY)
        {
            float z = point.Z + zOffset;
            float inv = perspective / (perspective + z);
            float sx = centerX + point.X * inv;
            float sy = centerY + point.Y * inv;
            return new PointF(sx, sy);
        }

        public static float ToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180f;
        }
    }
}
