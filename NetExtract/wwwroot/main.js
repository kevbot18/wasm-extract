// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
    .create();

setModuleImports('main.js', {
    dom: {}
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

const extractText = (filename, buffer) => exports.ExtractUtil.ExtractText(filename, buffer);

const btn = document.getElementById('extract');

btn.addEventListener('click', e => {
    let statusElem = document.getElementById('status');
    let start = performance.now();
    let fileInput = document.getElementById('doc');
    if (fileInput.files.length == 0) {
        statusElem.innerText = 'Missing File';
        return;
    }
    let file = fileInput.files[0];
    let reader = new FileReader();
    reader.onloadend = (load) => {
        let array = new Uint8Array(load.target.result);
        let status = "Unknown";
        try {
            let result = extractText(file.name, array);
            document.getElementById('extracted').value = result;
            status = "Success";
        } catch (error) {
            console.error(error);
            status = "Error";
        } finally {
            let end = performance.now();
            statusElem.innerText = `${status} (${end - start}ms)`;
            btn.disabled = false;
        }
    };
    statusElem.innerText = 'Running';
    btn.disabled = true;
    reader.readAsArrayBuffer(file);

    e.preventDefault();
});

dispatchEvent(new Event("wasm-load"));

// run the C# Main() method and keep the runtime process running and executing further API calls
await runMain();