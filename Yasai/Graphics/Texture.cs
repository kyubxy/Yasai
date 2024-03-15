using OpenTK.Graphics.OpenGL4;

namespace Yasai.Graphics;

public sealed class Texture : IDisposable
{
    public readonly int Handle;

    public Texture(int glHandle)
    {
        Handle = glHandle;
    }

    // Activate texture
    // Multiple textures can be bound, if your shader needs more than just one.
    // If you want to do that, use GL.ActiveTexture to set which slot GL.BindTexture binds to.
    // The OpenGL standard requires that there be at least 16, but there can be more depending on your graphics card.
    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    private void ReleaseUnmanagedResources()
    {
        GL.DeleteTexture(Handle);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~Texture()
    {
        ReleaseUnmanagedResources();
    }
}