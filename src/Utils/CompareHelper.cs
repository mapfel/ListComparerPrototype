using System.Collections.Generic;

namespace Utils
{
    public delegate bool Predicate<T>(T oldValue, T newValue);
    public static class CompareHelper<T>
    {
        public static bool DeepComparer<T>(T oldValue, T newValue)
        {
            var propertyInfos = oldValue.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var oldPropertyValue = oldValue.GetType().GetProperty(propertyInfo.Name).GetValue(oldValue, null);
                var newPropertyValue = newValue.GetType().GetProperty(propertyInfo.Name).GetValue(newValue, null);

                if (!oldPropertyValue.Equals(newPropertyValue))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool SimpleComparer<T>(T oldValue, T newValue)
        {
            return newValue.Equals(oldValue);
        }

        public static Predicate<T> GetComparerForType(IEnumerable<T> oldValues)
        {
            Predicate<T> compare;
            var type = oldValues.GetItemType();
            if (type.IsValueType)
            {
                compare = SimpleComparer;
            }
            else if (true /*type.IsReferenceType*/)
            {
                compare = DeepComparer;
            }

            return compare;
        }


    }
}