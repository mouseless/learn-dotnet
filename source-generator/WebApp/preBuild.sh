#!/usr/bin/env bash

basedir=$(dirname "$0")
bin=$1
config=$2
netversion=$3
generatepath=$4

cli=$basedir/../Cli/bin/$config/$netversion/Cli

eval $cli webapp ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json ../WebApp/obj/$config/$netversion/Controller.schema.json
