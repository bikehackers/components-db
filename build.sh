#!/usr/bin/env bash

dotnet tool restore
dotnet paket restore
dotnet paket generate-load-scripts
dotnet build
dotnet test
