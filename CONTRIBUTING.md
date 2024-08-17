# Contributing

**Notes:**

This repository uses js tooling for general scripting.

## Prerequisites

Ensure .net 6.0, 7.0 and 8.0 are installed, additionally [yarn](https://yarnpkg.com/) should be installed.

Required tools:

- `node`

  - _In Windows_: installed in [git bash](https://git-scm.com/download/win).
  - must be installed via [nvm](https://github.com/nvm-sh/nvm) for Windows (git bash), Mac, Linux.
  - install node by running `nvm install` and `nvm use` in the root of the repository
    the version will be taken from `.nvmrc`

- `yarn` can preferably be installed via `corepack enable`.

Alternatively, [dev container](https://containers.dev/) specified in this repo can be used;
said container comes bundled with additional utilities like
[autojump](https://github.com/wting/autojump), [fzf](https://github.com/junegunn/fzf)
and all supported .NET versions.

## Getting started

Install the required packages, this action also enables git hooks.

```bash
yarn --immutable
```

When using dev containers, this should be taken care of automatically,
in the `postCreateCommand`.
