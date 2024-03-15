using OpenTK.Mathematics;

namespace Yasai.Graphics;

public struct Geometry
{
    public readonly float[] Vertices;
    public readonly uint[] Indices;
    
    public Matrix4 Model = Matrix4.Identity;
    public Matrix4 Projection = Matrix4.Identity;

    public Geometry(float[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;
    }
}
