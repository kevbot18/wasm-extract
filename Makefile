SHELL=/bin/bash

.PHONY: all clean web netExtract html2md compress

all: netExtract netClean html2md build web compress clean

netExtract:
	cd NetExtract && dotnet publish -c Release

netClean:
	rm -rf NetExtract/{bin,obj}

html2md:
	cd html2md && GOOS=js GOARCH=wasm go build  -ldflags="-w -s" -o main.wasm

web:
	mkdir -p dist
	rm -rf dist/{*,.*}
	cp -r NetExtract/bin/Release/net9.0/publish/wwwroot/_framework dist/
	cp NetExtract/wwwroot/* dist/
	cp html2md/main.wasm dist/
	cp html2md/wasm_exec.js dist/


compress:
	find dist -type f ! -name "*.br" ! -name "*.gz" -print0 | while IFS= read -r -d '' f; do	\
		echo "Compressing $$f";	\
		test -f "$$f.br" || brotli -k "$$f";	\
		test -f "$$f.gz" || zopfli "$$f";	\
	done

clean:
	rm -rf dist/{*,.*}

