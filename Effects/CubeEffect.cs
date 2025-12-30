using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using OldSchoolDemo.Utils;

namespace OldSchoolDemo.Effects
{
    public class CubeEffect : IEffect
    {
        private float _angleX;
        private float _angleY;
        private float _angleZ;
        private float _zoom = 1f;

        private const float RotationSpeedX = 1.2f;
        private const float RotationSpeedY = 0.8f;
        private const float RotationSpeedZ = 2f;
        private const float ZoomAmplitude = 0.20f;
        private const float ZoomFrequency = 0.6f;
        private const int ColorOffset = 4;

        private static readonly (int a, int b)[] CubeEdges = new[]
        {
            (0,1),(1,2),(2,3),(3,0), // near face
            (4,5),(5,6),(6,7),(7,4), // far face
            (0,4),(1,5),(2,6),(3,7)  // connectors
        };

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

            var vertices = CreateCubeVertices(half);
            var projected = ProjectVertices(vertices, side, cx, cy);

            DrawCubeEdges(graphics, projected);
            DrawCubeFace(graphics, projected);
        }

        private static Vector3D[] CreateCubeVertices(float half)
        {
            return new[]
            {
                new Vector3D(-half, -half, -half), // 0: top-left-near
                new Vector3D( half, -half, -half), // 1: top-right-near
                new Vector3D( half,  half, -half), // 2: bottom-right-near
                new Vector3D(-half,  half, -half), // 3: bottom-left-near
                new Vector3D(-half, -half,  half), // 4: top-left-far
                new Vector3D( half, -half,  half), // 5: top-right-far
                new Vector3D( half,  half,  half), // 6: bottom-right-far
                new Vector3D(-half,  half,  half), // 7: bottom-left-far
            };
        }

        private PointF[] ProjectVertices(Vector3D[] vertices, float side, float cx, float cy)
        {
            float ax = Projection3D.ToRadians(_angleX);
            float ay = Projection3D.ToRadians(_angleY);
            float az = Projection3D.ToRadians(_angleZ);

            float perspective = Math.Max(300f, side * 2f);
            float zOffset = 800f;

            var projected = new PointF[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                var rotated = Projection3D.Rotate(vertices[i], ax, ay, az);
                projected[i] = Projection3D.Project(rotated, perspective, zOffset, cx, cy);
            }

            return projected;
        }

        private void DrawCubeEdges(Graphics graphics, PointF[] projected)
        {
            double time = Environment.TickCount / 800.0;
            int paletteLen = C64Colors.Palette.Length;

            for (int i = 0; i < CubeEdges.Length; i++)
            {
                Color color = CalculateEdgeColor(i, time, paletteLen);

                using (var pen = new Pen(color, 2) { LineJoin = LineJoin.Round })
                {
                    graphics.DrawLine(pen, projected[CubeEdges[i].a], projected[CubeEdges[i].b]);
                }
            }
        }

        private static Color CalculateEdgeColor(int edgeIndex, double time, int paletteLen)
        {
            int idx1 = (edgeIndex + ((int)time % paletteLen)) % paletteLen;
            int idx2 = (idx1 + ColorOffset) % paletteLen;

            Color c1 = C64Colors.Palette[idx1];
            Color c2 = C64Colors.Palette[idx2];

            float fade = (float)((Math.Sin(time + edgeIndex) + 1.0) * 0.5);

            return Color.FromArgb(
                255,
                (int)(c1.R * (1 - fade) + c2.R * fade),
                (int)(c1.G * (1 - fade) + c2.G * fade),
                (int)(c1.B * (1 - fade) + c2.B * fade)
            );
        }

        private static void DrawCubeFace(Graphics graphics, PointF[] projected)
        {
            try
            {
                using (var brush = new SolidBrush(Color.FromArgb(40, 0, 255, 0)))
                using (var pen = new Pen(Color.FromArgb(120, Color.Black), 1))
                using (var path = new GraphicsPath())
                {
                    path.AddPolygon(new[] { projected[4], projected[5], projected[6], projected[7] });
                    graphics.FillPath(brush, path);
                    graphics.DrawPath(pen, path);
                }
            }
            catch
            {
            }
        }
    }
}
