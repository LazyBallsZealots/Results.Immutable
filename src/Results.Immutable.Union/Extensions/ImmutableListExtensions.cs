using System.Collections.Immutable;

namespace Results.Immutable;

internal static class ImmutableListExtensions
{
    extension<T>(ImmutableList)
    {
        public static ImmutableList<T> Concat(params IEnumerable<ImmutableList<T>> lists)
        {
            var builder = ImmutableList.CreateBuilder<T>();
            foreach (var list in lists)
            {
                builder.AddRange(list);
            }

            return builder.ToImmutable();
        }
    }
}