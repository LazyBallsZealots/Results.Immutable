namespace Results.Immutable.Tests.OptionFactoriesTests;

public sealed class ZipTests
{
    public static TheoryData<Option<int>, Option<int>, Option<(int, int)>> Zips2ValuesIntoTupleData =>
        new()
        {
            {Option.Some(1), Option.Some(2), Option.Some((1, 2))},
            {Option.None<int>(), Option.Some(2), Option.None<(int, int)>()},
            {Option.Some(1), Option.None<int>(), Option.None<(int, int)>()},
            {Option.None<int>(), Option.None<int>(), Option.None<(int, int)>()},
        };

    [Theory(DisplayName = "Zips 2 values into a tuple")]
    [MemberData(nameof(Zips2ValuesIntoTupleData))]
    public void Zips2ValuesIntoTuple(
        Option<int> a,
        Option<int> b,
        Option<(int, int)> expected) =>
        Option.Zip(a, b)
            .Should()
            .Be(expected);

    public static TheoryData<Option<int>, Option<int>, Option<int>, Option<(int, int, int)>> Zips3ValuesIntoTupleData =>
        new()
        {
            {Option.Some(1), Option.Some(2), Option.Some(3), Option.Some((1, 2, 3))},
            {Option.None<int>(), Option.Some(2), Option.Some(3), Option.None<(int, int, int)>()},
            {Option.Some(1), Option.None<int>(), Option.Some(3), Option.None<(int, int, int)>()},
            {Option.Some(1), Option.Some(2), Option.None<int>(), Option.None<(int, int, int)>()},
            {Option.None<int>(), Option.None<int>(), Option.None<int>(), Option.None<(int, int, int)>()},
        };

    [Theory(DisplayName = "Zips 3 values into a tuple")]
    [MemberData(nameof(Zips3ValuesIntoTupleData))]
    public void Zips3ValuesIntoTuple(
        Option<int> a,
        Option<int> b,
        Option<int> c,
        Option<(int, int, int)> expected) =>
        Option.Zip(
                a,
                b,
                c)
            .Should()
            .Be(expected);

    public static TheoryData<Option<int>, Option<int>, Option<int>, Option<int>, Option<(int, int, int, int)>>
        Zips4ValuesIntoTupleData =>
        new()
        {
            {Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4), Option.Some((1, 2, 3, 4))},
            {Option.None<int>(), Option.Some(2), Option.Some(3), Option.Some(4), Option.None<(int, int, int, int)>()},
            {Option.Some(1), Option.None<int>(), Option.Some(3), Option.Some(4), Option.None<(int, int, int, int)>()},
            {Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4), Option.None<(int, int, int, int)>()},
            {Option.Some(1), Option.Some(2), Option.Some(3), Option.None<int>(), Option.None<(int, int, int, int)>()},
            {
                Option.None<int>(), Option.None<int>(), Option.None<int>(), Option.None<int>(),
                Option.None<(int, int, int, int)>()
            },
        };

    [Theory(DisplayName = "Zips 4 values into a tuple")]
    [MemberData(nameof(Zips4ValuesIntoTupleData))]
    public void Zips4ValuesIntoTuple(
        Option<int> a,
        Option<int> b,
        Option<int> c,
        Option<int> d,
        Option<(int, int, int, int)> expected) =>
        Option.Zip(
                a,
                b,
                c,
                d)
            .Should()
            .Be(expected);

    public static TheoryData<Option<int>, Option<int>, Option<int>, Option<int>, Option<int>,
            Option<(int, int, int, int, int)>>
        Zips5ValuesIntoTupleData =>
        new()
        {
            {
                Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4), Option.Some(5),
                Option.Some((1, 2, 3, 4, 5))
            },
            {
                Option.None<int>(), Option.Some(2), Option.Some(3), Option.Some(4), Option.Some(5),
                Option.None<(int, int, int, int, int)>()
            },
            {
                Option.Some(1), Option.None<int>(), Option.Some(3), Option.Some(4), Option.Some(5),
                Option.None<(int, int, int, int, int)>()
            },
            {
                Option.Some(1), Option.Some(2), Option.None<int>(), Option.Some(4), Option.Some(5),
                Option.None<(int, int, int, int, int)>()
            },
            {
                Option.Some(1), Option.Some(2), Option.Some(3), Option.None<int>(), Option.Some(5),
                Option.None<(int, int, int, int, int)>()
            },
            {
                Option.Some(1), Option.Some(2), Option.Some(3), Option.Some(4), Option.None<int>(),
                Option.None<(int, int, int, int, int)>()
            },
            {
                Option.None<int>(), Option.None<int>(), Option.None<int>(), Option.None<int>(), Option.None<int>(),
                Option.None<(int, int, int, int, int)>()
            },
        };

    [Theory(DisplayName = "Zips 5 values into a tuple")]
    [MemberData(nameof(Zips5ValuesIntoTupleData))]
    public void Zips5ValuesIntoTuple(
        Option<int> a,
        Option<int> b,
        Option<int> c,
        Option<int> d,
        Option<int> e,
        Option<(int, int, int, int, int)> expected) =>
        Option.Zip(
                a,
                b,
                c,
                d,
                e)
            .Should()
            .Be(expected);

    public static TheoryData<Option<int>[], Option<ImmutableList<int>>> TransposesListOfOptionsToOptionOfListData =>
        new()
        {
            {Array.Empty<Option<int>>(), Option.Some(ImmutableList<int>.Empty)},
            {
                new[]
                {
                    Option.Some(1),
                },
                Option.Some(ImmutableList.Create(1))
            },
            {
                new[]
                {
                    Option.Some(1),
                    Option.None<int>(),
                },
                Option.None<ImmutableList<int>>()
            }
        };

    [Theory(DisplayName = "Transposes a list of options to an option of list")]
    [MemberData(nameof(TransposesListOfOptionsToOptionOfListData))]
    public void TransposesListOfOptionsToOptionOfList(
        Option<int>[] options,
        Option<ImmutableList<int>> expected)
    {
        var actual = Option.Transpose(options);
        actual.IsSome.Should()
            .Be(expected.IsSome);

        if ((actual.Some, expected.Some) is var ((a), (b)))
        {
            a.Should()
                .BeEquivalentTo(b);
        }
    }
}