namespace Yasai.Allocation;

public class CacheMetadata
{
    public int AcquisitionFrequency { get; set; }
}

public abstract class Cache<TI, TM, TP>
    where TI : notnull 
    where TM: CacheMetadata
{
    protected delegate TI? Locator(List<(TI, TM)> records);

    // default to using LFU
    protected virtual Locator EvictionPolicy => rs => rs.MinBy(r => r.Item2.AcquisitionFrequency).Item1;

    private InternalCache internalCache = new();
    private int? maxSize;

    protected Cache(int? maxSize = null)
    {
        this.maxSize = maxSize;
    }

    public TP Get(TI index)
    {
        // determine cache hit
        if (!internalCache.ContainsIndex(index)) 
            cacheMiss(index); 
        var (meta, payload) = internalCache.Get(index); // cache hit
        UpdateMetadata(meta);
        return payload;
    }

    protected virtual void UpdateMetadata(TM metadata)
    {
        metadata.AcquisitionFrequency++;
    }

    void cacheMiss(TI index)
    {
        // cache eviction
        if (maxSize != null && internalCache.Count > maxSize)
        {
            var candidate = EvictionPolicy.Invoke(internalCache.Records);
            if (candidate is not null)
            {
                var (_, p) = internalCache.Get(candidate);
                Deallocate(p);                      // free the resource
                internalCache.Remove(candidate);    // remove entry from cache
            }
        }

        // load resource
        var (m, v) = ColdAcquire(index);
        internalCache.Add(index, m, v); 
    }
    
    protected virtual void Deallocate(TP item)
    { }
    
    protected abstract (TM, TP) ColdAcquire(TI index);
    
    private class InternalCache
    {
        private Dictionary<TI, TP> payloadCache = new ();
        private Dictionary<TI, TM> metaCache = new ();

        private int count;

        public void Add(TI index, TM metadata, TP payload)
        {
            payloadCache[index] = payload;
            metaCache[index] = metadata;
            count++;
        }
        
        public void Remove(TI index)
        {
            payloadCache.Remove(index);
            metaCache.Remove(index);
            count--;
        }

        public (TM, TP) Get(TI index) => (metaCache[index], payloadCache[index]);

        public List<(TI, TM)> Records => metaCache.Select(kv => (kv.Key, kv.Value)).ToList();

        public bool ContainsIndex(TI ind) => payloadCache.ContainsKey(ind) && metaCache.ContainsKey(ind);

        public int Count => count;
    }
}