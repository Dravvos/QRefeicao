using MongoDB.Driver;
using System.Linq.Expressions;

namespace QRefeicao.Data.NoSQL
{
    public static class FilterHelper<T>
    {
        private static readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;
        private static readonly SortDefinitionBuilder<T> _sortBuilder = Builders<T>.Sort;

        public static FilterDefinition<T> BuildFilter(Expression<Func<T, bool>> predicate)
        {
            return _filterBuilder.Where(predicate);
        }

        public static FilterDefinition<T> BuildEqualityFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Eq(propertyName, value);
            return filter;
        }
        public static FilterDefinition<T> BuildGreaterThanOrEqualsFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Gte(propertyName, value);
            return filter;
        }

        public static FilterDefinition<T> BuildLessThanOrEqualsFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Lte(propertyName, value);
            return filter;
        }

        public static FilterDefinition<T> BuildGreaterThanFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Gt(propertyName, value);
            return filter;
        }

        public static FilterDefinition<T> BuildLessThanFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Lt(propertyName, value);
            return filter;
        }
        public static FilterDefinition<T> BuildNotEqualsFilter(string propertyName, object value)
        {
            var filter = _filterBuilder.Ne(propertyName, value);
            return filter;
        }

        public static FilterDefinition<T> BuildCombinedFilter(Dictionary<string, object> filters)
        {
            var filterDefinitions = filters
                .Select(kvp => BuildEqualityFilter(kvp.Key, kvp.Value))
                .ToList();

            return _filterBuilder.And(filterDefinitions);
        }

        // Build a sort definition from a property name and direction
        public static SortDefinition<T> BuildSort(string propertyName, bool isAscending = true)
        {
            return isAscending
                ? _sortBuilder.Ascending(propertyName)
                : _sortBuilder.Descending(propertyName);
        }

        public static SortDefinition<T> BuildCombinedSort(params (string propertyName, bool isAscending)[] sortCriteria)
        {
            var sortDefinitions = new SortDefinition<T>[sortCriteria.Length];
            for (int i = 0; i < sortCriteria.Length; i++)
            {
                sortDefinitions[i] = BuildSort(sortCriteria[i].propertyName, sortCriteria[i].isAscending);
            }
            return _sortBuilder.Combine(sortDefinitions);
        }

        public static FilterDefinition<T> BuildRangeFilter<TValue>(
       Expression<Func<T, TValue>> property,
       TValue min,
       TValue max) where TValue : IComparable<TValue>
        {
            return _filterBuilder.And(
                _filterBuilder.Gt(property, min),
                _filterBuilder.Lt(property, max)
            );
        }
    }
}
