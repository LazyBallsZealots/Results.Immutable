﻿using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Results.Immutable;

BenchmarkRunner.Run<ResultBenchmarks>();

[MemoryDiagnoser]
public class ResultBenchmarks
{
    private const string value = "A string, ahoy!";

    private const string aNewValue = "A new string";

    private static readonly Error Error = new("Error message");

    [Benchmark]
    public Result<Unit> ResultOfUnitWhichShouldNotAllocate() => Result.Ok();

    [Benchmark]
    public Result<string> ResultOfStringWhichShouldNotAllocate() => Result.Ok(value);

    [Benchmark]
    public Option<string> SomeStringWhichShouldNotAllocate() => Option.Some(value);

    [Benchmark]
    public Option<string> NoneStringWhichShouldNotAllocate() => Option.None<string>();

    [Benchmark]
    public Result<string> SelectOnSuccessWithoutReasonsWhichShouldNotAllocate() =>
        Result.Ok(value)
            .Select(static _ => Result.Ok(aNewValue));

    [Benchmark]
    public ImmutableList<Error> ErrorsForFailedResult() => ImmutableList.Create(Error);

    [Benchmark]
    public Result<string> FailedResultOfStringWhichShouldAllocateJustTheErrorsList() => Result.Fail<string>(Error);

    [Benchmark]
    public Result<string> SelectOnFailureWhichShouldAllocateJustTheErrorsList() =>
        Result.Fail(Error)
            .Select(static _ => Result.Ok(value));
}