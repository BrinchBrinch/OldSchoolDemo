using System.Drawing;

namespace OldSchoolDemo.Effects
{
    public interface IEffect
    {
        void Update(float deltaTime);
        void Render(Graphics graphics, int width, int height);
    }
}
