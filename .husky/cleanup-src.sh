#!/usr/bin/env bash

dotnet tool restore && dotnet restore src/Results.Immutable.sln
dotnet build src/Results.Immutable.sln --no-restore
printf -v joined '%s;' "${@#src/}"
dotnet jb cleanupcode --profile=FullCleanup --settings=./src/Results.Immutable.sln.DotSettings ./src/Results.Immutable.sln "--include=${joined%;}" --no-build