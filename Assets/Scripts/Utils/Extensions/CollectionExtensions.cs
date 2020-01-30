
using System;
using System.Collections.Generic;

namespace Core.Extensions {
    public static class CollectionExtensions {
        public static void DictionaryForEach<TKey, TValue>(this Dictionary<TKey, TValue> source, Action<KeyValuePair<TKey, TValue>> action) {
            foreach ( var pair in source ) {
                action.Invoke(pair);
            }
        }

        public static bool Exists<TKey, TValue>(this Dictionary<TKey, TValue> source, Predicate<KeyValuePair<TKey, TValue>> match) {
            foreach ( var pair in source ) {
                if ( !match(pair) ) {
                    continue;
                }

                return true;
            }

            return false;
        }
    }
}