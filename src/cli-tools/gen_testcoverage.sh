#!/bin/sh

DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )
SOLDIR="${DIR}/../"

cd "${SOLDIR}" && dotnet test "HelloShadowDI.sln" --no-build \
  -p:CollectCoverage=true \
  -p:CoverletOutput=./coverage.cobertura.xml \
  -p:CoverletOutputFormat=cobertura

files=""

cd "${SOLDIR}" && \
for PKG in "NventX.FoundationPkg"; do
  for f in `find "${SOLDIR}" -name '*coverage.cobertura.xml' | sed 's@^/\\([a-zA-z]\\)@\\U\\1:@' | sed 's@/@\\\\@g' | grep "$PKG" | grep -v ComponentRoots`; do 
    if [ -n "${files}" ]; then
      files="${files};"
    fi
    files="${files}$f"
  done
done

reportgenerator -reports:$files -targetdir:Documents/utest_coverages/ -reporttypes:Html
