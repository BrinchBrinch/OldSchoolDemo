using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using OldSchoolDemo.Utils;

namespace OldSchoolDemo.Effects
{
    public class RasterBarsEffect : IEffect
    {
        private const int BarCount = 10;
        private const int BarHeight = 30;
        private const int ColorOffset = 4;
        private const double AnimationSpeed = 200.0;
        private const double WaveAmplitude = 40.0;
        private const double WaveBaseY = 200.0;

        public void Update(float deltaTime)
        {
        }

        public void Render(Graphics graphics, int width, int height)
        {
            for (int i = 0; i < BarCount; i++)
            {
                int y = CalculateBarY(i);
                var (startColor, endColor) = GetBarColors(i);

                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, y, width, BarHeight),
                    startColor,
                    endColor,
                    0f))
                {
                    graphics.FillRectangle(brush, 0, y, width, BarHeight);
                }
            }
        }

        private static int CalculateBarY(int barIndex)
        {
            double time = Environment.TickCount / AnimationSpeed;
            return (int)(Math.Sin(time + barIndex) * WaveAmplitude + WaveBaseY + (double)barIndex * BarHeight);
        }

        private static (Color start, Color end) GetBarColors(int barIndex)
        {
            int paletteLen = C64Colors.Palette.Length;
            Color c1 = C64Colors.Palette[barIndex % paletteLen];
            Color c2 = C64Colors.Palette[(barIndex + ColorOffset) % paletteLen];

            return (
                Color.FromArgb(255, c1.R, c1.G, c1.B),
                Color.FromArgb(255, c2.R, c2.G, c2.B)
            );
        }
    }
}
