#!/usr/bin/env bash
# Runs every input/output test in tests/ against a Release build of the game.
# Each tests/NN-name.in is fed to the program and its output is compared,
# byte by byte, with tests/NN-name.out.
set -u
cd "$(dirname "$0")"

dotnet build src/BatalhaNaval/BatalhaNaval.csproj -c Release -nologo -v q > /dev/null || {
    echo "Build failed"; exit 1;
}
app="src/BatalhaNaval/bin/Release/net8.0/BatalhaNaval.dll"

passed=0; failed=0
for input in tests/*.in; do
    expected="${input%.in}.out"
    actual="$(mktemp)"
    dotnet "$app" < "$input" > "$actual"
    if cmp -s "$expected" "$actual"; then
        echo "PASS  $(basename "${input%.in}")"
        passed=$((passed + 1))
    else
        echo "FAIL  $(basename "${input%.in}")"
        diff "$expected" "$actual" | head -20
        failed=$((failed + 1))
    fi
    rm -f "$actual"
done

echo "$passed passed, $failed failed"
[ "$failed" -eq 0 ]
