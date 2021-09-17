using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelProgrammingSamples
{
    public abstract class BasicCacheTester
    {
        public async Task<int[]> CacheTester1(int[] toCache)
        {
            try
            {
                var tasks = toCache.Select(futureKey => Task.Run(() => AddToCache(futureKey))).ToList();
                await Task.WhenAll(tasks);
                return GetCachedKeys();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public int[] CacheTester2(int[] toCache)
        {
            try
            {
                var tasks = toCache.Select(futureKey => new Action(() => AddToCache(futureKey))).ToArray();
                Parallel.Invoke(tasks);
                return GetCachedKeys();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected virtual void AddToCache(int i)
        {
        }

        protected abstract int[] GetCachedKeys();
    }

    public class Cache : BasicCacheTester
    {
        private readonly Dictionary<int, int> _cache = new();

        protected override void AddToCache(int key)
        {
            try
            {
                if (_cache.ContainsKey(key) == false)
                    _cache.Add(key, key * key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override int[] GetCachedKeys() =>
            _cache.Keys.ToArray();
    }

    public class CacheWithLocking : BasicCacheTester
    {
        private readonly Dictionary<int, int> _cache = new();
        private readonly object _locker = new();

        protected override void AddToCache(int key)
        {
            try
            {
                lock (_locker)
                    if (_cache.ContainsKey(key) == false)
                        _cache.Add(key, key * key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override int[] GetCachedKeys()
        {
            lock (_locker)
                return _cache.Keys.ToArray();
        }
    }
    
    public class CacheWithConcurrency : BasicCacheTester
    {
        private readonly ConcurrentDictionary<int, int> _cache = new();

        protected override void AddToCache(int key)
        {
            try
            {
                    if (_cache.ContainsKey(key) == false)
                        _cache.AddOrUpdate(key, key * key, (origKey, origValue) => origKey*origKey);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override int[] GetCachedKeys() => 
            _cache.Keys.ToArray();
    }
}