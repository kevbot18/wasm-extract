SHELL=/bin/bash

.PHONY: all clean web netExtract html2md compress

all: netExtract html2md web compress

netExtract:
	rm -rf NetExtract/bin/Release/net9.0/publish
	cd NetExtract && dotnet publish -c Release

html2md:
	cd html2md && GOOS=js GOARCH=wasm go build  -ldflags="-w -s" -o main.wasm

build: netExtract html2md

web:
	mkdir -p dist
	cp -r NetExtract/bin/Release/net9.0/publish/wwwroot/* dist/
	cp html2md/main.wasm dist/
	cp html2md/wasm_exec.js dist/

compress: web
	find dist -type f ! -name "*.br" ! -name "*.gz" -print0 | while IFS= read -r -d '' f; do	\
		test -f "$$f.br" || brotli -k "$$f"													;	\
		test -f "$$f.gz" || zopfli "$$f"													;	\
	done

clean:
	rm -rf dist/{*,.*}
