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
    get deleteButtonId() { return '#' + this.phrasebookDefinition.deleteButtonId; }
    get addPhraseButtonId() { return '#' + this.phrasebookDefinition.addPhraseButtonId; }
    get addCategoryButtonId() { return '#' + this.phrasebookDefinition.addCategoryButtonId; }
    get modifyButtonId() { return '#' + this.phrasebookDefinition.modifyButtonId; }
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
            list: true,
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
    setTree = (rawTree) => {
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
        let handleButtonState = (event, data) => {
            let selectedData = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)
            $(this.deleteButtonId).prop('disabled', selectedData.length === 0);
            $(this.modifyButtonId).prop('disabled', selectedData.length === 0 || selectedData.length > 1);
        }
        $(this.phrasebookTreeId).on('nodeChecked', handleButtonState);
        $(this.phrasebookTreeId).on('nodeUnchecked', handleButtonState);
        $(this.phrasebookModalId).modal({});
        $(this.phrasebookTreeId).trigger('nodeChecked');
    }
    showPhrasebook = (languageId) => {
        // Do something to each element here.
        getTree(languageId).done((rawTree) => this.setTree(rawTree));
        $(this.deleteButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)
                .map(x => { return { id: x.id, isList: !!x.list } });
            deletePhrases(arrayIds);
            getTree(languageId).done((rawTree) => this.setTree(rawTree));
        });

        $(this.addPhraseButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked && x.list)
                .map(x => { return { id: x.id, isList: !!x.list } });
            let parentId;
            if (arrayIds.length == 0) {
                parentId = 0;
            } else if (arrayIds.length > 0) {
                parentId = arrayIds[0].id;
            } else {
                alert('Please, select the only category to be parent for a new phrase');
                return;
            }

            const text = prompt('Enter the text:');
            if (!text) {
                return;
            }

            createPhrase(text, parentId);
            getTree(languageId).done((rawTree) => this.setTree(rawTree));
        });
        $(this.addCategoryButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked && x.list)
                .map(x => { return { id: x.id, isList: !!x.list } });
            let parentId = 0;

            const text = prompt('Enter the name:');
            if (!text) {
                return;
            }

            createCategory(text, parentId);
            getTree(languageId).done((rawTree) => this.setTree(rawTree));
        });
        $(this.modifyButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)
                .map(x => { return { id: x.id, isList: !!x.list } });
            let id = 0;
            if (arrayIds.length == 1) {
                id = arrayIds[0].id;
            } else {
                alert('Please, select the only node to be edited');
                return;
            }

            const text = prompt('Enter the name:');
            if (!text) {
                return;
            }
            modifyNode(text, id, arrayIds[0].isList);
            getTree(languageId).done((rawTree) => this.setTree(rawTree));
        });
    }
    phraseSelected = new rxjs.Subject();
}

function initPhrasebook(phrasebookDefinition) {
    return new PhrasebookController(phrasebookDefinition);
}