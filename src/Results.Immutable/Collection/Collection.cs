namespace Results.Immutable.Collection;

internal static class Collection
{
    internal static ImmutableList<TItem> ConcatLists<TItem>(params ImmutableList<TItem>?[] lists)
    {
        var builder = ImmutableList.CreateBuilder<TItem>();
        foreach (var list in lists)
        {
            if (list is { })
            {
                builder.AddRange(list);
            }
        }

        return builder.ToImmutable();
    }
}