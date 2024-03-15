using OpenTK.Mathematics;
using Yasai.Graphics.Shaders;
using Yasai.Resources;

namespace Yasai.Graphics;

public abstract class BoxedDrawable : IDrawable
{
    // TODO: 
    public abstract Shader GetShader(CacheContainer c);
    
    private static readonly float[] Verts =
    {
        0.5f, 0.5f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        -0.5f, 0.5f, 0.0f, 0.0f, 1.0f
    };

    private static readonly uint[] Inds =
    {
        0, 1, 3,
        1, 2, 3
    };

    private Geometry geometry = new(Verts, Inds)
    {
        Projection = Matrix4.CreateOrthographicOffCenter(0f, Game.WindowWidth,
            Game.WindowHeight, 0f, -1f, 1f)
    };
    public Geometry Geometry => geometry;

    private Vector2 position;
    public Vector2 Position
    {
        get => position;
        set
        {
            if (value == position)
                return;
            position = value;
            updateModel();
        }
    }

    private Vector2 size;
    public Vector2 Size
    {
        get => size;
        set
        {
            if (value == size)
                return;
            size = value;
            updateModel();
        }
    }

    private void updateModel()
    {
        geometry.Model = Matrix4.CreateScale(new Vector3(Size / 2)) * Matrix4.CreateTranslation(new Vector3(Position));
    }
}