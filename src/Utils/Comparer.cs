using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Utils
{
    public static class Comparer<T> //where T : struct
    {
        public static IEnumerable<Tuple<T, T, ComparisonResult>> GetAllChanges(IEnumerable<T> oldValues, IEnumerable<T> newValues)
        {
            var deletedValues = GetDeletedValues(oldValues, newValues);
            foreach (var deletedValue in deletedValues)
            {
                yield return new Tuple<T, T, ComparisonResult>(deletedValue, default(T), ComparisonResult.Deleted);
            }

            var addedValues = GetAddedValues(oldValues, newValues);
            foreach (var addedValue in addedValues)
            {
                yield return new Tuple<T, T, ComparisonResult>(default(T), addedValue, ComparisonResult.Added);
            }

            var updatedValues = GetUpdatedValuesEx(oldValues, newValues, null);
            foreach (var updatedValue in updatedValues)
            {
                yield return new Tuple<T, T, ComparisonResult>(updatedValue.Item1, updatedValue.Item2, ComparisonResult.Updated);
            }
        }

        public static IEnumerable<T> GetAddedValues(IEnumerable<T> oldValues, IEnumerable<T> newValues)
        {
            var result = GetDeletedValues(newValues, oldValues);
            return result;
        }

        public static IEnumerable<T> GetUpdatedValues(IEnumerable<T> oldValues, IEnumerable<T> newValues, Predicate<T> compare)
        {
            var list = GetUpdatedValuesEx(oldValues, newValues, compare);
            foreach (var value in list)
            {
                yield return value.Item2;
            }
        }

        public static IEnumerable<T> GetDeletedValues(IEnumerable<T> oldValues, IEnumerable<T> newValues)
        {
            //var oldKeys = GetListOfKeys(oldValues);
            //var newKeys = GetListOfKeys(newValues);
            var (oldKeys, newKeys) = GetKeyLists(oldValues, newValues);

            var deletedKeys = GetOuterKeys(oldKeys, newKeys);

            var list = CreateKeyValuePairs(oldValues);

            var result = JoinLists(deletedKeys, list);
            return result;
        }

        private static IEnumerable<Tuple<T, T>> GetUpdatedValuesEx(IEnumerable<T> oldValues, IEnumerable<T> newValues, Predicate<T> compare)
        {
            if (null == compare)
            {
                compare = CompareHelper<T>.GetComparerForType(oldValues);
            }

            var (oldKeys, newKeys) = GetKeyLists(oldValues, newValues);

            var commonKeys = GetCommonKeys(oldKeys, newKeys);

            var oldKvpOfCommonKeys = CreateKeyValuePairs(oldValues);
            var newKvpOfCommonKeys = CreateKeyValuePairs(newValues);

            // faster with Dictionary
            IDictionary<int, T> oldDic = oldKvpOfCommonKeys.ToDictionary(x => x.Key, x => x.Value);
            IDictionary<int, T> newDic = newKvpOfCommonKeys.ToDictionary(x => x.Key, x => x.Value);

            foreach (var key in commonKeys)
            {
                var oldValue = oldDic[key];
                var newValue = newDic[key];

                if (!compare(oldValue, newValue))
                {
                    yield return new Tuple<T, T>(oldValue, newValue);
                }
            }
        }

        private static (IEnumerable<int>, IEnumerable<int>) GetKeyLists(IEnumerable<T> oldValues, IEnumerable<T> newValues)
        {
            var oldKeys = GetListOfKeys(oldValues);
            var newKeys = GetListOfKeys(newValues);
            return (oldKeys, newKeys);
        }

        private static IEnumerable<KeyValuePair<int, T>> CreateKeyValuePairs(IEnumerable<T> list)
        {
            return list.Select(item => new KeyValuePair<int, T>(item.KeyValue(), item));
        }

        private static IEnumerable<T> JoinLists(IEnumerable<int> keys, IEnumerable<KeyValuePair<int, T>> keyValuePairs)
        {
            var result = keys.Join(keyValuePairs, key => key, kvp => kvp.Key, (key, key2) => key2.Value);
            return result;
        }

        private static IEnumerable<int> GetOuterKeys(IEnumerable<int> left, IEnumerable<int> right)
        {
            var outerKeys = left.Except(right);
            return outerKeys;
        }

        private static IEnumerable<int> GetCommonKeys(IEnumerable<int> left, IEnumerable<int> right)
        {
            var commonKeys = left.Intersect(right);
            return commonKeys;
        }

        private static IEnumerable<int> GetListOfKeys(IEnumerable<T> list)
        {
            return list.Select(item => item.KeyValue());
        }
    }
}