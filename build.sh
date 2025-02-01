#!/bin/bash

set -e

echo "Building project..."

cd ./mfm
dotnet publish --self-contained true -r linux-x64 -p:PublishSingleFile=true

echo "Copying executable..."

cp -r ./bin/Release/net9.0/linux-x64/publish/mfm ~/.local/bin

echo "Successfully installed..."
