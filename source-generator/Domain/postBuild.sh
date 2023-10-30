#!/usr/bin/env bash

basedir=$(dirname "$0")
bin=$1
config=$2
netversion=$3
generatepath=$4

cli=$basedir/../Cli/bin/$config/$netversion/Cli
pdb=$basedir/../Cli/bin/$config/$netversion/Cli.pdb
dll=$basedir/../Cli/bin/$config/$netversion/Cli.dll

eval $pdb domain ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.generated.cs ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json
eval $dll domain ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.generated.cs ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json
eval $cli domain ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.generated.cs ../Domain/$generatepath/CodeGen/CodeGen.JsonSchemaGenerator/Domain.schema.json
