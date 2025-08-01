import json
import hashlib
import os
import sys

if len(sys.argv) == 2:
    os.chdir(sys.argv[1])


with open('blazor.boot.json', 'r') as f:
    data = json.load(f)

for file in data['resources']['coreAssembly']:
    if os.path.exists(file):
        with open(file, 'rb') as f:
            data[file] = hashlib.sha256(f.read()).hexdigest()
    else:
        print(f"Warning: File {file} not found, skipping.")

with open('blazor.boot.json', 'w') as f:
    json.dump(data, f, indent=2)
