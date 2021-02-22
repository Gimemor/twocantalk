    var enPhrasebook = [
        {
            text: "Common Phrases",
            selectable: false,
            selectedIcon: "glyphicon glyphicon-stop",
            nodes: [
                {
                    selectable: false,
                    selectedIcon: "glyphicon glyphicon-stop",
                    text: "How are you?"
                    //,
                    //nodes: [
                    //    {
                    //        text: "Grandchild 1"
                    //    },
                    //    {
                    //        text: "Grandchild 2"
                    //    }
                    //]
                },
                {
                    text: "How are you doing?"
                }
                ,
                {
                    text: "Good morning!"
                }
                ,
                {
                    text: "Good evening!"
                },
                {
                    text: "Where you live?"
                }
            ]
        },
        {
            text: "Shopping"
        },
        {
            text: "New"
        },
        {
            text: "Lessons"
        }
    ];
    var phrasebooks = {
        'en': enPhrasebook
}

class PhrasebookController {
    constructor(phrasebookDefinition) {
        this.phrasebookDefinition = phrasebookDefinition;
    }
    get phrasebookTreeId() { return '#' + this.phrasebookDefinition.phrasebookTreeId; }
    get phrasebookModalId() { return '#' + this.phrasebookDefinition.phrasebookModalId; }

    getTree = (languageId)  => {
        const from = languages.find(x => x.id == languageId);
        if (phrasebooks[from.language]) {
            return phrasebooks[from.language];
        }
        return [];
    }

    showPhrasebook = (languageId) => {
        // Do something to each element here.
        const tree = this.getTree(languageId);
        if (tree.length == 0) {
            alert('Phrasebook is empty');
            return;
        }
        $(this.phrasebookTreeId).treeview({
            data: tree,
            collapseIcon: 'fa fa-minus',
            checkedIcon: 'fa fa-check',
            expandIcon: 'fa fa-plus'
        });
        $('.bstreeview .list-group-item').on('click', (event) => {
        })
        $(this.phrasebookTreeId).on('nodeSelected', (event, data) => {
            // close modal
            $(this.phrasebookModalId).modal('hide');
            this.phraseSelected.next(data.text);
        });
        $(this.phrasebookModalId).modal({});
    }
    phraseSelected = new rxjs.Subject();
}

function initPhrasebook(phrasebookDefinition) {
    return new PhrasebookController(phrasebookDefinition);
}