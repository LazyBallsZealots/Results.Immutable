#!/usr/bin/env bash

dotnet tool restore && dotnet restore src/Results.Immutable.slnx
dotnet build src/Results.Immutable.slnx --no-restore
printf -v joined '%s;' "${@#src/}"
dotnet jb cleanupcode --profile=FullCleanup --settings=./src/Results.Immutable.sln.DotSettings ./src/Results.Immutable.slnx "--include=${joined%;}" --no-build