using Yasai.Allocation;

namespace Yasai.Resources.Caches;

public abstract class ResourceCache<TK, TV> : Cache<TK, CacheMetadata, TV> 
    where TV : IDisposable 
    where TK : notnull
{
    protected abstract TV GetResource(TK path);
    protected abstract TK ResolvePath(TK path);
    
    protected override (CacheMetadata, TV) ColdAcquire(TK index)
        => (new CacheMetadata(), GetResource(ResolvePath(index)));

    public string Root { get; set; } = @"res"; // TODO: move "res" elsewhere

    protected string AddRoot(string path) => Path.Combine(Root, path);

    protected override void Deallocate(TV item) => item.Dispose();
}

public abstract class StringResourceCache<T> : ResourceCache<string, T>
    where T : IDisposable
{
    protected override string ResolvePath(string path) => AddRoot(path);
}