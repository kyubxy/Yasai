using Yasai.Allocation;
using Yasai.Graphics.Shaders;

namespace Yasai.Resources.Caches;

public record ShaderPath(string VertexShaderPath, string FragmentShaderPath);

public class ShaderProgramCache : ResourceCache<ShaderPath, ShaderProgram>
{
    protected override ShaderProgram GetResource(ShaderPath paths)
    {
        return new ShaderProgram(paths.VertexShaderPath, paths.FragmentShaderPath);
    }

    protected override ShaderPath ResolvePath(ShaderPath path)
    {
        return new ShaderPath(AddRoot(path.VertexShaderPath), AddRoot(path.FragmentShaderPath));
    }
}