test:
	dotnet run --project src/App.fsproj --framework netcoreapp3.1

jsdbg:
	dotnet fable src -o build/es6 --runScript --define=DEBUG

build/es6/: $(wildcard src/*.fs)
	dotnet fable src -o build/es6 --optimize	

build/dist/: build/es6/
	ncc -m build build/es6/App.js -o build/dist

build/result-mid.js: build/dist/
	uglifyjs build/dist/index.js -m -c -b -o build/result-mid.js --ecma 5

build/result.js: build/result-mid.js src/App.js
	cat build/result-mid.js > build/result.js
	echo "" >> build/result.js
	cat src/App.js >> build/result.js

all: build/result.js
	node build/result.js

submit: build/result.js
	cat build/result.js | clip
