using System;
using System.Drawing;
using System.Windows.Forms;
using OldSchoolDemo.Core;
using OldSchoolDemo.Effects;

namespace OldSchoolDemo
{
    public class MainForm : Form
    {
        private readonly Timer _timer;
        private readonly DemoEngine _engine;
        private const int TargetFps = 60;
        private const int TimerInterval = 1000 / TargetFps;

        public MainForm()
        {
            InitializeForm();
            _engine = new DemoEngine();
            InitializeEffects();
            _timer = CreateTimer();
            _timer.Start();
        }

        private void InitializeForm()
        {
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            KeyPreview = true;
            KeyDown += OnFormKeyDown;
        }

        private void InitializeEffects()
        {
            _engine.AddEffect(new StarfieldEffect());
            _engine.AddEffect(new RasterBarsEffect());
            _engine.AddEffect(new CubeEffect());
            _engine.AddEffect(new ScrollTextEffect());
        }

        private Timer CreateTimer()
        {
            var timer = new Timer { Interval = TimerInterval };
            timer.Tick += OnTimerTick;
            return timer;
        }

        private void OnFormKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            float deltaTime = TimerInterval / 1000f;
            _engine.Update(deltaTime);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _engine.Render(e.Graphics, Width, Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer?.Dispose();
                _engine?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
