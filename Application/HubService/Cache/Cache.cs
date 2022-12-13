using System.Collections.Concurrent;

namespace HubService.Cache
{
    public static class Cache
    {
        public static ConcurrentDictionary<string, HashSet<string>> Groups = new ConcurrentDictionary<string, HashSet<string>>();
    }
}
