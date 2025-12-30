using System;
using System.Drawing;
using OldSchoolDemo.Utils;

namespace OldSchoolDemo.Effects
{
    public class StarfieldEffect : IEffect
    {
        private readonly PointF[] _stars;
        private readonly Random _random;
        private float _angleX;
        private float _angleY;
        private float _angleZ;
        private float _zoom = 1f;

        private const int StarCount = 50;
        private const float RotationSpeedX = 1.2f;
        private const float RotationSpeedY = 0.8f;
        private const float RotationSpeedZ = 2f;
        private const float ZoomAmplitude = 0.20f;
        private const float ZoomFrequency = 0.6f;

        public StarfieldEffect()
        {
            _random = new Random();
            _stars = new PointF[StarCount];
            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i] = new PointF((float)_random.NextDouble(), (float)_random.NextDouble());
            }
        }

        public void Update(float deltaTime)
        {
            _angleX += RotationSpeedX;
            _angleY += RotationSpeedY;
            _angleZ += RotationSpeedZ;
            _zoom = 1.0f + ZoomAmplitude * (float)Math.Sin(_angleZ * Math.PI / 180.0 * ZoomFrequency);
        }

        public void Render(Graphics graphics, int width, int height)
        {
            float side = 900f * _zoom;
            float half = side / 2f;
            float cx = width / 2f;
            float cy = height / 2f;

            float perspective = Math.Max(300f, side * 2f);
            float zOffset = 800f;

            float ax = Projection3D.ToRadians(_angleX);
            float ay = Projection3D.ToRadians(_angleY);
            float az = Projection3D.ToRadians(_angleZ);

            using (var brush = new SolidBrush(Color.White))
            {
                foreach (var star in _stars)
                {
                    float u = star.X, v = star.Y;
                    var point3D = new Vector3D(-half + u * side, -half + v * side, half);

                    var rotated = Projection3D.Rotate(point3D, ax, ay, az);
                    var projected = Projection3D.Project(rotated, perspective, zOffset, cx, cy);

                    if (IsOutOfBounds(projected, width, height))
                        continue;

                    graphics.FillRectangle(brush, projected.X - 1, projected.Y - 1, 2, 2);
                }
            }
        }

        private static bool IsOutOfBounds(PointF point, int width, int height)
        {
            return point.X < -100 || point.X > width + 100 || point.Y < -100 || point.Y > height + 100;
        }
    }
}
