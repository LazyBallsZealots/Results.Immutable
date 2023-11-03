#!/usr/bin/env bash

dotnet tool restore
printf -v joined '%s;' "${@#src/}"
dotnet jb cleanupcode --profile=FullCleanup --settings=./src/Results.Immutable.sln.DotSettings ./src/Results.Immutable.sln "--include=${joined%;}"