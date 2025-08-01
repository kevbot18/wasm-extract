
.PHONY: all clean web netExtract html2md

all: netExtract html2md web

netExtract:
	cd NetExtract && dotnet publish -c Release

html2md:
	cd html2md && GOOS=js GOARCH=wasm go build  -ldflags="-w -s" -o main.wasm

web:
	mkdir -p dist
	cp -r NetExtract/bin/Release/net9.0/publish/wwwroot/_framework dist/
	cp NetExtract/wwwroot/* dist/
	cp html2md/main.wasm dist/
	cp html2md/wasm_exec.js dist/

clean:
	rm -rf dist/{*,.*}
