using Yasai.Resources.Caches;

namespace Yasai.Resources;

public class CacheContainer
{
    public readonly ShaderProgramCache ShaderProgramCache = new(); //{ Root = @"res/shaders"};
    public readonly TextureCache TextureCache = new();
}