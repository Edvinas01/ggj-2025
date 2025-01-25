using System;
using System.Collections.Generic;
using System.Linq;

namespace UABPetelnia.GGJ2025.Runtime.Utilities
{
    internal static class CollectionUtilities
    {
        public static T GetRandom<T>(this IReadOnlyCollection<T> collection)
        {
            // Check if the collection is empty
            if (collection == null || collection.Count == 0)
            {
                throw new InvalidOperationException("The collection is empty.");
            }

            var randomIndex = UnityEngine.Random.Range(0, collection.Count);

            return collection.ElementAt(randomIndex);
        }
    }
}
