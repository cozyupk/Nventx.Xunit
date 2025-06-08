#!/bin/sh

# You can install coverlet.console by command below:
#  dotnet tool install --global coverlet.console

DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )
SOLDIR="${DIR}/../"
TARGET_BINDIR="${SOLDIR}/NventX.xProofPkg/ComponentRoots/E2ETests/ForXunitV2/bin/Debug/net8.0/"
OUTDIR="${SOLDIR}/Documents/e2etest_coverages/"

coverlet "${TARGET_BINDIR}NventX.xProof.ForXunitV2.E2ETests.ForXunitV2.dll" \
    --target "${TARGET_BINDIR}NventX.xProof.ForXunitV2.E2ETests.ForXunitV2.exe" \
    --targetargs "" \
    --format cobertura \
    --output ${OUTDIR}/coverage.cobertura.xml

reportgenerator -reports:"${OUTDIR}/coverage.cobertura.xml" -targetdir:"$OUTDIR" -reporttypes:Html
