using OpenTK.Graphics.OpenGL4;
using Yasai.Graphics.Shaders;
using Yasai.Resources;

namespace Yasai.Graphics;

public class Renderer : ILoad
{
    // Collection of internally generated data
    // required for rendering. created and managed by the renderer
    record struct Node(IDrawable Drawable, Shader Shader, int Vao);
    
    private List<Node> scenegraph = new();

    // before the renderer load, drawables are buffered
    // into an intermediate data structure and the load process is deferred
    // to the actual load function
    private List<IDrawable> intmd = new();
    
    private CacheContainer? container;

    public void Add(IDrawable d)
    {
        if (container is null)
            intmd.Add(d);         // not loaded yet
        else
            append(d, container); // already loaded
    }
    
    public void Load(CacheContainer c)
    {
        foreach (var d in intmd)
            append(d, c);
        
        // realistically speaking, the intermediate list should cease to exist by this point
        // we clear it so subsequent load calls don't generate more nodes
        intmd.Clear();
        container = c;
    }
    
    // load the drawable, generate a node and add it to the scenegraph
    void append(IDrawable d, CacheContainer c)
    {
        int vbo, vao, ebo;
        
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, d.Geometry.Vertices.Length * sizeof(float), 
            d.Geometry.Vertices, BufferUsageHint.StaticDraw);
        
        ebo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, d.Geometry.Indices.Length * sizeof(uint), 
            d.Geometry.Indices, BufferUsageHint.StaticDraw);

        Shader sh = d.GetShader(c); // creates a new shader program
        sh.Initialise();

        scenegraph.Add(new Node(d, sh, vao));
    }

    public bool Remove(IDrawable d)
    {
        var node = scenegraph.Find(n => n.Drawable == d);
        if (node == default) return false;
        GL.DeleteBuffer(node.Vao); // should delete vao but idk
        scenegraph.Remove(node);
        return true;
    }

    public void Render()
    {
        foreach (Node n in scenegraph)
        {
            GL.BindVertexArray(n.Vao);
            n.Shader.Use();
            GL.DrawElements(PrimitiveType.Triangles, n.Drawable.Geometry.Indices.Length, DrawElementsType.UnsignedInt,
                0);
        }
    }
}