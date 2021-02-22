    var enPhrasebook = [
        {

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

    getTree2= (languageId)  => {
        const from = languages.find(x => x.id == languageId);
        if (phrasebooks[from.language]) {
            return phrasebooks[from.language];
        }
        return [];
    }
    parseTree(rawTree) {
        if (!rawTree) {
            return;
        }
        const root = {
            selectedIcon: "glyphicon glyphicon-stop",
            text: rawTree.name,
            selectable: false,
            id: rawTree.id
        };
        root.nodes = []
        if (rawTree.childLists.length > 0) {
            root.nodes = root.nodes.concat(rawTree.childLists.map(child => this.parseTree(child)));
        }
        if (rawTree.phrases.length > 0) {
            root.nodes = root.nodes.concat(rawTree.phrases.map(phrase => {
                return {
                    text: phrase.content,
                    id: phrase.id
                }
            }));
        }
        return root;
    }

    showPhrasebook = (languageId) => {
        // Do something to each element here.
        getTree(languageId).done((rawTree) => {
            if (rawTree.length == 0) {
                alert('Phrasebook is empty');
                return;
            }

            this.tree = this.parseTree(rawTree);
            $(this.phrasebookTreeId).treeview({
                data: this.tree.nodes,
                collapseIcon: 'fa fa-minus',
                checkedIcon: 'fa fa-check-square-o',
                uncheckedIcon: 'fa fa-square-o',
                expandIcon: 'fa fa-plus',
                showCheckbox: true
            });
            $('.bstreeview .list-group-item').on('click', (event) => {
            })
            $(this.phrasebookTreeId).on('nodeSelected', (event, data) => {
                // close modal
                $(this.phrasebookModalId).modal('hide');
                this.phraseSelected.next(data.text);
            });
            $(this.phrasebookModalId).modal({});
        })
        
    }
    phraseSelected = new rxjs.Subject();
}

function initPhrasebook(phrasebookDefinition) {
    return new PhrasebookController(phrasebookDefinition);
}