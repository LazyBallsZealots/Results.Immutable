# Change Log

All notable changes to this project will be documented in this file.
See [Conventional Commits](https://conventionalcommits.org) for commit guidelines.

# 0.1.0 (2023-10-04)

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
