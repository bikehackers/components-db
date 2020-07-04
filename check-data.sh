#!/usr/bin/env bash

dotnet paket install
dotnet paket generate-load-scripts
dotnet fsi ./check-data.fsx
