using OpenTK.Graphics.OpenGL4;
using Yasai.Resources;

namespace Yasai.Graphics.Shaders;

public class TextureShader : Shader
{
    public override VertexAttribute[] VertexAttributes { get; } = 
    {
        new ("aPosition", 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0),
        new ("aTexCoord", 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float))
    }; 

    private Texture tex;
    private Geometry geometry;

    public TextureShader(CacheContainer c, Texture t, Geometry g)
        : base (c, @"shader.vert", @"tex.frag")
    {
        tex = t;
        geometry = g;
    }

    public override void Use()
    {
        base.Use();
        Program.SetMatrix4("model", geometry.Model);
        Program.SetMatrix4("proj", geometry.Projection);
        tex.Use(TextureUnit.Texture0);
    }
}