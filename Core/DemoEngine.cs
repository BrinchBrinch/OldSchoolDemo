using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OldSchoolDemo.Effects;

namespace OldSchoolDemo.Core
{
    public class DemoEngine : IDisposable
    {
        private readonly List<IEffect> _effects = new();

        public void AddEffect(IEffect effect)
        {
            _effects.Add(effect);
        }

        public void Update(float deltaTime)
        {
            foreach (var effect in _effects)
            {
                effect.Update(deltaTime);
            }
        }

        public void Render(Graphics graphics, int width, int height)
        {
            graphics.Clear(Color.Black);

            foreach (var effect in _effects)
            {
                effect.Render(graphics, width, height);
            }
        }

        public void Dispose()
        {
            foreach (var effect in _effects.OfType<IDisposable>())
            {
                effect.Dispose();
            }
            _effects.Clear();
        }
    }
}
