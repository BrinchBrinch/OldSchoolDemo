using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace OldSchoolDemo.Effects
{
    public class ScrollTextEffect : IEffect, IDisposable
    {
        private readonly Font _font;
        private float _scrollOffsetX;
        private float _scrollPhaseY;

        private const string DefaultText = "THIS IS A SIMPLE SCROLLER TEXT IN MY NEWEST DEMO TO SEE IF I CAN CREATE SOME OLDSCHOOL EFFECTS IN WINDOWS";
        private const float ScrollSpeedX = 320f;
        private const float ScrollSpeedY = 1.6f;
        private const int OutlineThickness = 6; // percentage of font size

        public string Text { get; set; } = DefaultText;

        public ScrollTextEffect()
        {
            _font = CreateScrollFont();
        }

        public void Update(float deltaTime)
        {
            _scrollOffsetX += ScrollSpeedX * deltaTime;
            _scrollPhaseY += ScrollSpeedY * deltaTime;

            if (_scrollOffsetX > 1e6f) _scrollOffsetX %= 10000;
            if (_scrollPhaseY > 1e6f) _scrollPhaseY %= (float)(2.0 * Math.PI);
        }

        public void Render(Graphics graphics, int width, int height)
        {
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var size = graphics.MeasureString(Text, _font);
            int textWidth = Math.Max(1, (int)Math.Ceiling(size.Width));
            int textHeight = Math.Max(1, (int)Math.Ceiling(size.Height));

            float x = CalculateScrollX(textWidth, width);
            float y = CalculateScrollY(textHeight, height);

            DrawScrollingText(graphics, x, y, textWidth);
        }

        private float CalculateScrollX(int textWidth, int width)
        {
            float cycle = textWidth + width;
            return width - (_scrollOffsetX % cycle);
        }

        private float CalculateScrollY(int textHeight, int height)
        {
            float topAnchor = (height / 4f) - (textHeight / 2f);
            float centerAnchor = (height / 2f) - (textHeight / 2f);
            float smoothness = (float)((Math.Sin(_scrollPhaseY) + 1.0) * 0.5);

            return centerAnchor + (topAnchor - centerAnchor) * smoothness;
        }

        private void DrawScrollingText(Graphics graphics, float x, float y, int textWidth)
        {
            float emSize = _font.Size * graphics.DpiY / 72f;
            var stringFormat = StringFormat.GenericTypographic;
            stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            using (var path = new GraphicsPath())
            {
                path.AddString(Text, _font.FontFamily, (int)_font.Style, emSize, new PointF(x, y), stringFormat);
                path.AddString(Text, _font.FontFamily, (int)_font.Style, emSize, new PointF(x + textWidth, y), stringFormat);

                using (var fillBrush = new SolidBrush(Color.FromArgb(51, 0, 255, 255)))
                {
                    graphics.FillPath(fillBrush, path);
                }

                using (var outlinePen = new Pen(Color.Black, Math.Max(1f, emSize * OutlineThickness / 100f))
                {
                    LineJoin = LineJoin.Round
                })
                {
                    graphics.DrawPath(outlinePen, path);
                }
            }
        }

        private static Font CreateScrollFont()
        {
            try
            {
                var preferredFonts = new[] { "PressStart2P", "Consolas" };
                var installedFonts = new InstalledFontCollection();
                var availableFonts = installedFonts.Families
                    .Select(f => f.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                string fontName = preferredFonts.FirstOrDefault(f => availableFonts.Contains(f)) ?? "Consolas";
                return new Font(fontName, 136f, FontStyle.Bold);
            }
            catch
            {
                return new Font("Consolas", 30f, FontStyle.Bold);
            }
        }

        public void Dispose()
        {
            _font?.Dispose();
        }
    }
}
