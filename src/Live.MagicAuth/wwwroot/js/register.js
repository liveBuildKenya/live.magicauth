async function fetchCredentialOptions() {
    let response = await fetch('/attestation/options?username=example@example.com&displayName=Display name', {
        method: 'GET',
        headers: {
            'Accept': 'application/json'
        }
    });

    let data = await response.json();

    return data;
}

async function makeCredentials() {
    const credentialCreationOptions = await fetchCredentialOptions();

    console.log(credentialCreationOptions);

    credentialCreationOptions.challenge = coerceToArrayBuffer(credentialCreationOptions.challenge);

    credentialCreationOptions.user.id = coerceToArrayBuffer(credentialCreationOptions.user.id);

    if (credentialCreationOptions.excludedCredentials) {
        credentialCreationOptions.excludedCredentials = credentialCreationOptions.excludedCredentials.map((credential) => {
            credential.id = coerceToArrayBuffer(credential.id);

            return credential;
        });
    }

    let credentials;

    try {
        credentials = await navigator.credentials.create({
            publicKey: credentialCreationOptions
        });
    }
    catch (error) {
        console.log(error)
    }

    try {
        registerNewCredentials(credentials);

    }
    catch(error) {
        console.log(error);
    }
}

async function registerNewCredentials(newCredentials) {
    let attestationObject = new Uint8Array(newCredentials.response.attestationObject);
    let clientDataJSON = new Uint8Array(newCredentials.response.clientDataJSON);
    let rawId = new Uint8Array(newCredentials.rawId);

    const data = {
        id: newCredentials.id,
        rawId: coerceToBase64Url(rawId),
        type: newCredentials.type,
        extensions: newCredentials.getClientExtensionResults(),
        response: {
            attestationObject: coerceToBase64Url(attestationObject),
            clientDataJson: coerceToBase64Url(clientDataJSON)
        }
    };

    let response;

    try {
        response = await fetch('/attestation', {
            method: 'POST',
            body: JSON.stringify(data),
            headers: {
                'Accepts': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        response = response.json();
    }
    catch (error) {
        console.log(error);
    }

    // redirect to dashboard to show keys
    window.location.href = "/"
    //+ value("#login-username");
}

makeCredentials();