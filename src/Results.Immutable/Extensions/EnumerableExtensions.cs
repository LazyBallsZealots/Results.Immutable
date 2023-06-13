namespace Results.Immutable.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> Flatten<T>(
        this IEnumerable<T> reasons,
        Func<T, IEnumerable<T>> selector)
    {
        var queue = new Queue<T>(reasons);

        while (queue.TryDequeue(out var reason))
        {
            yield return reason;

            selector(reason)
                .ForEach(queue.Enqueue);
        }
    }

    public static void ForEach<T>(
        this IEnumerable<T> enumerable,
        Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action(item);
        }
    }
}