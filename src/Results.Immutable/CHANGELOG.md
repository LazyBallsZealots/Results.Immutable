# Change Log

All notable changes to this project will be documented in this file.
See [Conventional Commits](https://conventionalcommits.org) for commit guidelines.

# [1.0.0-alpha.1](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@1.0.0-alpha.0...Results.Immutable@1.0.0-alpha.1) (2024-04-28)

### Features

- add OkIfNotNull for Result ([6635b2e](https://github.com/LazyBallsZealots/Results.Immutable/commit/6635b2efb877f36fc465d46d5ea8ead7d28baf93))
- add SelectManyAsync for option ([da63c35](https://github.com/LazyBallsZealots/Results.Immutable/commit/da63c35573f7b118a6ece33ec7bf7f072bbf266e))
- add transpose for `Option<Result<T>>` to `Result<Option<T>>` and otherwise ([bd33198](https://github.com/LazyBallsZealots/Results.Immutable/commit/bd3319859de897e01a482029794361910716d531))
- added sync- and async projections to Option ([315b36c](https://github.com/LazyBallsZealots/Results.Immutable/commit/315b36ca6bfb8b3100a84ce313755237317eed87))

# [1.0.0-alpha.0](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@0.1.0-alpha.5...Results.Immutable@1.0.0-alpha.0) (2024-04-09)

### Features

- implement synchronous projections for tasks of generic Results ([92aa606](https://github.com/LazyBallsZealots/Results.Immutable/commit/92aa6069222e440be044ca75506a1436c9969595))

### BREAKING CHANGES

- renames and changes signatures
  of existing ValueTask extensions.

# [0.1.0-alpha.5](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@0.1.0-alpha.4...Results.Immutable@0.1.0-alpha.5) (2024-04-01)

### Bug Fixes

- adjust versioning scripts to fail if the csproj file cannot be modified ([a2127f5](https://github.com/LazyBallsZealots/Results.Immutable/commit/a2127f52bed88864c44b2d2903bce8a7980a30b5))

# [0.1.0-alpha.4](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@0.1.0-alpha.3...Results.Immutable@0.1.0-alpha.4) (2024-02-27)

### Features

- add missing merge overload ([44ee1be](https://github.com/LazyBallsZealots/Results.Immutable/commit/44ee1be468117e5bdb5f969f2fef42154f675384))

# [0.1.0-alpha.3](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@0.1.0-alpha.2...Results.Immutable@0.1.0-alpha.3) (2024-02-05)

### Features

- add .NET 8 to the list of supported frameworks ([97c989c](https://github.com/LazyBallsZealots/Results.Immutable/commit/97c989c4a31a8df403bc1b2ceef6f57166ee7a7b))
- add sync-to-async projections for generic ValueTasks ([f4dd6b8](https://github.com/LazyBallsZealots/Results.Immutable/commit/f4dd6b8987aa3e1ba5b3abe962bfa2e47a43be96))

# [0.1.0-alpha.2](https://github.com/LazyBallsZealots/Results.Immutable/compare/Results.Immutable@0.1.0-alpha.1...Results.Immutable@0.1.0-alpha.2) (2024-01-30)

### Bug Fixes

- add license expression to main .csproj file ([02c260f](https://github.com/LazyBallsZealots/Results.Immutable/commit/02c260fbc62cf3dc01518e068867defb4a1837e1))

### Features

- add WithErrors and IResult interface ([0093056](https://github.com/LazyBallsZealots/Results.Immutable/commit/0093056f7630d0375be23a679e17d45ca019b57b))
- add ZipWith/ZipWithAsync and convert Zip to extension methods ([8ca923c](https://github.com/LazyBallsZealots/Results.Immutable/commit/8ca923c8466fa98705a2985677a83398c2ccaa1b))
- convert between result and option ([1c73084](https://github.com/LazyBallsZealots/Results.Immutable/commit/1c730846cf1603e19fd52c5dec463a5537395d98))

# 0.1.0-alpha.1 (2023-11-14)

### Features

- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) implement missing operators for Unit ([9059152](https://github.com/LazyBallsZealots/Results.Immutable/commit/9059152836a65f656c7772267de612c63760234e))
- [#29](https://github.com/LazyBallsZealots/Results.Immutable/issues/29) refactor Zip and implement missing tests ([ddc31c6](https://github.com/LazyBallsZealots/Results.Immutable/commit/ddc31c61ced97e28d898a3029721ab4e5e709657))
- add an interface IResult to allow boxing the Result ([79d43fb](https://github.com/LazyBallsZealots/Results.Immutable/commit/79d43fbba3eed1b9a2e54eefc95d5dd2961828e6))
- add Select, SelectMany and Where to Option in a standard way ([ef65a59](https://github.com/LazyBallsZealots/Results.Immutable/commit/ef65a59f0732b3d39cf5cc53bc2b8127b3ad1e04))
- add Some struct ([05cb7f9](https://github.com/LazyBallsZealots/Results.Immutable/commit/05cb7f9f079ed76c884e3739b6633114e71ea13c))
- add TryAsync and asynchronous selectors for Results ([630864b](https://github.com/LazyBallsZealots/Results.Immutable/commit/630864bdb32f4256016aa1cc893a9f2f08881539))

### Performance Improvements

- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) reduce amount of heap allocations for Option<T> ([cc3ab2f](https://github.com/LazyBallsZealots/Results.Immutable/commit/cc3ab2ffcdfadbb8d4b94c2f0caca9960cf11967))
- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) reimplement Result record struct to minimize heap allocations ([0253210](https://github.com/LazyBallsZealots/Results.Immutable/commit/0253210acf1cc1ead5318053ffaad46bd851d4b4))

### Reverts

- Revert "chore: #1 move proposed implementation from FluentResults repo" ([77578ed](https://github.com/LazyBallsZealots/Results.Immutable/commit/77578edb7a97be5c12535fab8d7efcf01e48ae71)), closes [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1)
- Revert "chore: #2 run code cleanup on the solution" ([0e3fa7a](https://github.com/LazyBallsZealots/Results.Immutable/commit/0e3fa7a0932efacdb0637cfa96d6fefd4bf5c915)), closes [#2](https://github.com/LazyBallsZealots/Results.Immutable/issues/2)

# 0.1.0-alpha.0 (2023-11-02)

### Features

- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) implement missing operators for Unit ([9059152](https://github.com/LazyBallsZealots/Results.Immutable/commit/9059152836a65f656c7772267de612c63760234e))
- [#29](https://github.com/LazyBallsZealots/Results.Immutable/issues/29) refactor Zip and implement missing tests ([ddc31c6](https://github.com/LazyBallsZealots/Results.Immutable/commit/ddc31c61ced97e28d898a3029721ab4e5e709657))
- add Select, SelectMany and Where to Option in a standard way ([ef65a59](https://github.com/LazyBallsZealots/Results.Immutable/commit/ef65a59f0732b3d39cf5cc53bc2b8127b3ad1e04))
- add Some struct ([05cb7f9](https://github.com/LazyBallsZealots/Results.Immutable/commit/05cb7f9f079ed76c884e3739b6633114e71ea13c))

### Performance Improvements

- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) reduce amount of heap allocations for Option<T> ([cc3ab2f](https://github.com/LazyBallsZealots/Results.Immutable/commit/cc3ab2ffcdfadbb8d4b94c2f0caca9960cf11967))
- [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1) reimplement Result record struct to minimize heap allocations ([0253210](https://github.com/LazyBallsZealots/Results.Immutable/commit/0253210acf1cc1ead5318053ffaad46bd851d4b4))

### Reverts

- Revert "chore: #1 move proposed implementation from FluentResults repo" ([77578ed](https://github.com/LazyBallsZealots/Results.Immutable/commit/77578edb7a97be5c12535fab8d7efcf01e48ae71)), closes [#1](https://github.com/LazyBallsZealots/Results.Immutable/issues/1)
- Revert "chore: #2 run code cleanup on the solution" ([0e3fa7a](https://github.com/LazyBallsZealots/Results.Immutable/commit/0e3fa7a0932efacdb0637cfa96d6fefd4bf5c915)), closes [#2](https://github.com/LazyBallsZealots/Results.Immutable/issues/2)
