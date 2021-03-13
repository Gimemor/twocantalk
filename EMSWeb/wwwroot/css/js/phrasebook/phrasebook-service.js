

function getTree() {
    const url = '/api/Phrasebook';
    return $.ajax({
        url: url,
        context: document.body
    })
}


function deletePhrases(ids) {
    const url = '/api/Phrasebook/delete';
    return $.ajax({
        type: 'POST',
        url: url,
        async: false,
        context: document.body,
        data: JSON.stringify(ids),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        dataType: 'json',
    });
}

function createPhrase(text, listId) {
    const url = '/api/Phrasebook';
    return $.ajax({
        type: 'POST',
        url: url,
        async: false,
        context: document.body,
        data: JSON.stringify({
            text: text,
            listId: listId
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        dataType: 'json',
    })
}

function createCategory(text, listId) {
    const url = '/api/Phrasebook/Category';
    return $.ajax({
        type: 'POST',
        url: url,
        async: false,
        context: document.body,
        data: JSON.stringify({
            name: text,
            parentId: listId
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        dataType: 'json',
    })
}

function modifyNode(text, id, isList) {
    const url = '/api/Phrasebook/modify';
    return $.ajax({
        type: 'POST',
        url: url,
        async: false,
        context: document.body,
        data: JSON.stringify({
            name: text,
            id: id,
            isList: isList
        }),
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        dataType: 'json',
    })
}
