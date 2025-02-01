#!/bin/bash
 
cd ./mfm
dotnet publish --self-contained true -r linux-x64 -p:PublishSingleFile=true
