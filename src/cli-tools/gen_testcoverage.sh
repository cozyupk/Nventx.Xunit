#!/bin/sh

DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )
SOLDIR="${DIR}/../"

cd "${SOLDIR}" && dotnet test "NventX.xProof.sln" --no-build \
  -p:CollectCoverage=true \
  -p:CoverletOutput=./coverage.cobertura.xml \
  -p:CoverletOutputFormat=cobertura

files=""

cd "${SOLDIR}" && \
for PKG in "xProof"; do
  for f in `find "${SOLDIR}" -name '*coverage.cobertura.xml' | sed 's@^/\\([a-zA-z]\\)@\\U\\1:@' | sed 's@/@\\\\@g' | grep -i "$PKG" | grep -v _ComponentRoots`; do 
    if [ -n "${files}" ]; then
      files="${files};"
    fi
    files="${files}$f"
  done
done

reportgenerator -reports:$files -targetdir:Documents/utest_coverages/ -reporttypes:Html
