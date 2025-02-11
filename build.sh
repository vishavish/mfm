#!/bin/bash

set -e

echo "Building project..."

dotnet publish mfm -c Release -r linux-x64 --output build/linux 
# dotnet publish mfm -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true --output build/linux
# dotnet publish mfm -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained true --output build/linux

echo "Linking executable..."

ln -s $PWD/build/linux/mfm ~/.local/bin/mfm

# cp -r ./build/linux/mfm ~/.local/bin

echo "Successfully installed..."
