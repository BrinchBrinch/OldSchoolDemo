using System.Drawing;

namespace OldSchoolDemo.Utils
{
    public static class C64Colors
    {
        public static readonly Color[] Palette = new[]
        {
            Color.FromArgb(255, 255, 255),     // White
            Color.FromArgb(136, 0, 0),         // Red
            Color.FromArgb(170, 255, 238),     // Cyan
            Color.FromArgb(204, 68, 204),      // Purple
            Color.FromArgb(0, 204, 85),        // Green
            Color.FromArgb(0, 0, 170),         // Blue
            Color.FromArgb(238, 238, 119),     // Yellow
            Color.FromArgb(221, 136, 85),      // Orange
            Color.FromArgb(102, 68, 0),        // Brown
            Color.FromArgb(255, 119, 119),     // Light red
            Color.FromArgb(51, 51, 51),        // Dark gray
            Color.FromArgb(119, 119, 119),     // Medium gray
            Color.FromArgb(170, 255, 102),     // Light green
            Color.FromArgb(0, 136, 255),       // Light blue
            Color.FromArgb(187, 187, 187)      // Light gray
        };
    }
}
