

function getTree() {
    const url = '/api/Phrasebook';
    return $.ajax({
        url: url,
        context: document.body
    })
}


