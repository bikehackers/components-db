#!/usr/bin/env bash

dotnet tool restore
dotnet paket install
dotnet paket generate-load-scripts
dotnet build
dotnet test