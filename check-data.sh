#!/usr/bin/env bash

dotnet paket restore
dotnet paket generate-load-scripts
dotnet fsi ./check-data.fsx
