#!/usr/bin/env bash

bin=$1
config=$2
netversion=$4
basedir=$(dirname "$0")

cli=$basedir/../Cli/bin/$config/$netversion/Cli

eval $cli ../Domain/Generated/CodeGen/CodeGen.CodeToJsonSchemaGenerator/Scheduler.generated.cs ../Domain/bin/$config/$netversion/Scheduler.schema.json ../WebApp/bin/$config/$netversion/Scheduler.schema.json
