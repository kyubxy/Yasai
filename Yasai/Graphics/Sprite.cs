using Yasai.Graphics.Shaders;
using Yasai.Resources;

namespace Yasai.Graphics;

public class Sprite : BoxedDrawable
{
    public Texture Texture { get; set; }
    private string? texpath;
    
    public Sprite(Texture tex)
    {
        Texture = tex;
    }

    public Sprite(string tex)
    {
        texpath = tex;
    }

    public override Shader GetShader(CacheContainer c)
    {
        if (texpath is not null)
            Texture = c.TextureCache.Get(texpath);
        if (Texture is null)
            throw new Exception("no texture assigned");
        return new TextureShader(c, Texture, Geometry);
    }
}