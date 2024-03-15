using Yasai.Graphics.Shaders;
using Yasai.Resources;

namespace Yasai.Graphics;

public interface IDrawable
{
    Geometry Geometry { get; }
    Shader GetShader(CacheContainer c);
}