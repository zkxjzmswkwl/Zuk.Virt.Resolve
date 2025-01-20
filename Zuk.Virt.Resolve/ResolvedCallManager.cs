public sealed class ResolvedCallManager
{
    private static readonly Lazy<ResolvedCallManager> _instance = new Lazy<ResolvedCallManager>(() => new ResolvedCallManager());

    private readonly Dictionary<IntPtr, List<ulong>> resolvedCalls = new Dictionary<IntPtr, List<ulong>>();
    private readonly object lockObject = new object();

    private ResolvedCallManager() { }

    public static ResolvedCallManager Instance => _instance.Value;

    public void AddResolvedCall(IntPtr key, ulong value)
    {
        lock (lockObject)
        {
            if (!resolvedCalls.TryGetValue(key, out var list))
            {
                list = new List<ulong>();
                resolvedCalls[key] = list;
            }
            if (!list.Contains(value)) list.Add(value);
        }
    }

    public List<ulong> GetResolvedCalls(IntPtr key)
    {
        lock (lockObject)
        {
            return resolvedCalls.TryGetValue(key, out var list) ? new List<ulong>(list) : new List<ulong>();
        }
    }

    public Dictionary<IntPtr, List<ulong>> GetAllResolvedCalls()
    {
        lock (lockObject)
        {
            return new Dictionary<IntPtr, List<ulong>>(resolvedCalls);
        }
    }
}
