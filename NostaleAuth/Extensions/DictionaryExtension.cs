using System.Collections.Generic;

namespace NostaleAuth.Extensions
{
    public static class DictionaryExtension
    {
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dictionary, K key, V value) => dictionary.TryGetValue(key, out V result) ? result : value;
        public static V GetValueOrDefault<K, V>(this IDictionary<K, V> dictionary, K key) => dictionary.GetValueOrDefault(key, default);
    }
}