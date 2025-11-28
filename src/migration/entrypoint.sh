#!/bin/bash

# This needs to be in a shell script so that it evaluates the environment
# variables.

if [ -z "$VERSION" ]; then
    echo -e "\e[31mMissing 'VERSION' environment variable.\e[0m"
    exit 1
fi

if [ -z "$CONNECTION_STRING" ]; then
    echo -e "\e[31mMissing 'CONNECTION_STRING' environment variable.\e[0m"
    exit 1
fi

# echo \
./grate \
    --silent \
    --sqlfilesdirectory=/db \
    --outputPath=/output \
    --version=${VERSION} \
    --databasetype=postgresql \
    --connstring=${CONNECTION_STRING} \
    --create=${CREATE_DATABASE:-true} \
    --drop=${DROP_DATABASE:-false} \
    --transaction=${TRANSACTION:-false} \
    --environment=${ENVIRONMENT:-LOCAL}
