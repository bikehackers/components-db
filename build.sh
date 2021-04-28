#!/usr/bin/env bash

set -e

dotnet tool restore
dotnet paket restore
dotnet paket generate-load-scripts
dotnet build
dotnet test

dotnet fable ./components
