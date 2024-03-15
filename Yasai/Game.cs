using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Yasai.Graphics;
using Yasai.Resources;

namespace Yasai;

public class Game : GameWindow
{
    public static float WindowWidth = 1366;
    public static float WindowHeight = 768;
    
    protected Renderer Renderer = new();
    protected CacheContainer CacheContainer;
    
    public Game()
        : base(GameWindowSettings.Default, 
            new NativeWindowSettings
            {
                ClientSize = new Vector2i(1366, 768), 
                WindowBorder = WindowBorder.Resizable,
                Title = "Yasai"
            })
    {
        // enable stuff
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        CacheContainer = new CacheContainer();
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color.CornflowerBlue);
        Renderer.Load(CacheContainer);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);

        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        Renderer.Render();
        
        SwapBuffers();
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Size.X, e.Size.Y);
        WindowWidth = e.Size.X;
        WindowHeight = e.Size.Y;
    }
    
    protected override void OnUnload()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        base.OnUnload();
    }
}

