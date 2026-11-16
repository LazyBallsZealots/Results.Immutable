using System.Collections.Immutable;
using static Results.Immutable.Result;

namespace Results.Immutable;

public static class ZipExtensions
{
    extension<T1>(Result<T1> first)
    {
        public Result<(T1, T2)> Zip<T2>(Result<T2> second) =>
            (first, second) switch
            {
                (T1 firstUnwrapped, T2 secondUnwrapped) => Ok((firstUnwrapped, secondUnwrapped)),
                _ => Fail<(T1, T2)>(ImmutableList.Concat(first.Errors, second.Errors)),
            };

        public Result<(T1, T2, T3)> Zip<T2, T3>(
            Result<T2> second,
            Result<T3> third) =>
            (first, second, third) switch
            {
                (T1 firstValue, T2 secondValue, T3 thirdValue) => Ok((firstValue, secondValue, thirdValue)),
                _ => Fail<(T1, T2, T3)>(
                    ImmutableList.Concat(
                        first.Errors,
                        second.Errors,
                        third.Errors)),
            };

        public Result<(T1, T2, T3, T4)> Zip<T2, T3, T4>(
            Result<T2> second,
            Result<T3> third,
            Result<T4> fourth) =>
            (first, second, third, fourth) switch
            {
                (T1 firstValue, T2 secondValue, T3 thirdValue, T4 fourthValue) => Ok(
                    (firstValue, secondValue, thirdValue, fourthValue)),
                _ => Fail<(T1, T2, T3, T4)>(
                    ImmutableList.Concat(
                        first.Errors,
                        second.Errors,
                        third.Errors,
                        fourth.Errors)),
            };

        public Result<(T1, T2, T3, T4, T5)> Zip<T2, T3, T4, T5>(
            Result<T2> second,
            Result<T3> third,
            Result<T4> fourth,
            Result<T5> fifth) =>
            (first, second, third, fourth, first) switch
            {
                (T1 firstValue, T2 secondValue, T3 thirdValue, T4 fourthValue, T5 fifthValue) =>
                    Ok((firstValue, secondValue, thirdValue, fourthValue, fifthValue)),
                _ => Fail<(T1, T2, T3, T4, T5)>(
                    ImmutableList.Concat(
                        first.Errors,
                        second.Errors,
                        third.Errors,
                        fourth.Errors,
                        fifth.Errors)),
            };
    }
}