#!/usr/bin/env bash

basedir=$(dirname "$0")
bin=$1
config=$2
netversion=$3
generatepath=$4

generatepath=$(echo $generatepath | sed -e 's/\r//g')

cli=$basedir/../Cli/bin/$config/$netversion/Cli

cd ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/
echo $(ls)

eval $cli domain ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.generated.cs ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json
