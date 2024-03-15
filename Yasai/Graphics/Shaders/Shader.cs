using OpenTK.Graphics.OpenGL4;
using Yasai.Resources;
using Yasai.Resources.Caches;

namespace Yasai.Graphics.Shaders;

public record struct VertexAttribute(string AttributeName, int Size, VertexAttribPointerType PointerType,
    bool Normalised, int Stride, int Offset);

public abstract class Shader 
{
    public abstract VertexAttribute[] VertexAttributes { get; }
    
    public ShaderProgram Program { get; }

    public Shader(CacheContainer container, string vsh, string fsh)
    {
        Program = container.ShaderProgramCache.Get(new ShaderPath(vsh, fsh));
    }

    public void Initialise()
    {
        foreach (var attr in VertexAttributes)
        {
            var vertexLocation = Program.GetAttribLocation(attr.AttributeName);
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, attr.Size, attr.PointerType, attr.Normalised, attr.Stride, attr.Offset);
        }
    }

    public virtual void Use()
    {
        Program.Use();
    }
}