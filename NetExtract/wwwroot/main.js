// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { dotnet } from './_framework/dotnet.js'

const { setModuleImports, getAssemblyExports, getConfig, runMain } = await dotnet
    .create();

setModuleImports('main.js', {
    dom: {
        setStatus: (status) => document.querySelector("#status").innerText = status
    }
});

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);

const extractText = (filename, buffer) => exports.ExtractUtil.ExtractText(filename, buffer);

document.getElementById('extract').addEventListener('click', e => {
    let fileInput = document.getElementById('doc');
    let name = fileInput.files[0].name;
    let reader = new FileReader();
    reader.onloadend = (e) => {
        let array = new Uint8Array(e.target.result);
        let result = extractText(name, array);
        console.log(result);
        document.getElementById('extracted').innerText = result;
    };
    reader.readAsArrayBuffer(fileInput.files[0]);
    
    e.preventDefault();
});

// run the C# Main() method and keep the runtime process running and executing further API calls
await runMain();