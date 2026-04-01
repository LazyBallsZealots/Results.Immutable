# Result.Immutable

This library proposes an explicit alternative to the exception handling paradigm of .NET.
We believe that computations that can fail due to non-exceptional circumstances should not throw exceptions, but rather return a result of a value which may or may not be successful.

The goal of the library is not to replace exceptions, since they are an integral part of .NET, but to provide a more explicit alternative to handle expected failures.

## Main features

- **Immutable**: A result is a value and its content is set in stone.
- **Thread-safe**: A natural consequence of immutable values.
- **Informative**: Be honest with your fellow coders about your methods.
- **Safe**: No possibility to misuse the content of a result without proper handling.
- **Efficient**: No memory allocations on the successful path. Low memory allocations on the failed path.
- **Awesome Developer Experience**: Comes with a rich set of methods for taming and combining errors.
- **LINQ style**: Query syntax is supported.

## Result vs Exception

Exceptions as their name suggests are exceptional. They are always exceptional. They are circumstances that are not expected to happen in the regular execution of a program in a healthy environment. On the other hand, results are always expected to happen in the execution of a program. A user introducing a wrong input or a missing row in the database are not exceptional, rather expected situations.

We recommend to use `Result.Immutable` for expected cases that must be handled correctly. And `Exception` for panicking situations that are rarely recoverable.

Example of circumstances where `Result` is appropriate:

- User errors.
- Contextual errors.
- Validation errors.
- Partial functions (functions that cannot map their domain to their range).
- etc.

Examples of circumstances where `Exception`s are appropriate:

- Network errors.
- File system errors.
- Segmentation faults.
- Device errors.
- etc.

Exceptions as seen in the real world are misused by the programmer. Exceptions are commonly used for control flow, not for error handling. One may think of exceptions as the counterpart of `goto`, as the `come from`. While `goto` is bounded in the function scope and its deterministic where it goes, exceptions make long jumps to an non-deterministic place anywhere in the call stack (or even crashing the application). It is common to see programmers eager to throw exceptions, but seldomly willing to catch and handle them.

## Result vs Validation

Validation is a check and forget process, libraries like FluentValidation or Data Annotations are good examples of this. For example, validating a `String` to be a valid email address, still leaves you with a `String` that is a valid email, but neither the type system or your fellow programmers (including you in the future) will know that.

`Result` mixes well with parsing and converting values to other types, keeping the meaning of the verification in the type system where it belongs. Types are documentation of the intention and the meaning of a value. Instead of writing a method that takes a `String` and returns nothing (but cries when the value is wrong), it is preferable to write a method that takes a `String` and returns a `Result` of a `Email`.

It is worth to mention that this library is not a parsing combinator library, it is a general purpose library.

## Design

This library attempts to deliver high performance by allocating as little memory as possible. If a result is successful, all memory is allocated on the stack. If it is failed, the errors are allocated on the heap.

`Result.Immutable` intentionally enforces error handling by the consumer of the object. The consumer is forced to handle the error by either dealing with the error or providing a default value.

An equivalent to the unsafe functions such as `.Value` of other libraries which trigger exceptions, or Rust's `.unwrap()` or Haskell's `fromJust` is intentionally left out by design. The intention for this is to keep honesty in code. `Exception`s are the ⊥ bottom type of C# (among others), which is a logical contradiction. Think about a function such as a `Int32.Parse` as a liar, since it promises an `Int32` for every input but it fails to deliver. A traditional way to cover it in C# is by using `TryParse`, which uses out arguments, breaking the whole concept of input/output of a function.

Instead of the old-fashioned `Try...` methods, `Result.Immutable` opts for the usage of modern techniques and language features. This library is designed to be used with pattern matching.

The `Result` and `Option` designed in this library is immutable, as its name indicates. Every method returns a new value object which is a modified version of the original one. This is due to the fact that C# heavily relies on references to shared objects owned by the garbage collector. It avoids spooky actions at a distance made by a different object holding a reference to the same object, specially in multi-threaded scenarios.

This library takes inspiration from several sources, specially from [F#](https://github.com/dotnet/fsharp/), [Haskell](https://www.haskell.org/) and [Rust](https://www.rust-lang.org/). Additionally, from C# libraries like [language-ext](https://github.com/louthy/language-ext), [FluentResult](https://github.com/altmann/FluentResults) and [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions).

The reason to create this library instead of simply using one of the other tools:

- `language-ext` is heavy and requires knowledge of category theory, even in deeper extend than idiomatic Haskell.
- `FluentResult` seems to be abandoned. The authors of `Result.Immutable` contributed to it until the author disappeared, additionally, it allocates a lot of memory in the heap, so it is not recommended for heavy use.
- `CSharpFunctionalExtensions` is if-oriented, which defeats the purpose of the pattern. `Result.Immutable `promotes the usage of safety and type correctness.

## Contributions

Check out [contribution guidelines](./CONTRIBUTING.md) before getting started.
