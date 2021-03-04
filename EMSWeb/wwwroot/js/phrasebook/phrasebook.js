class PhrasebookController {
    constructor(phrasebookDefinition) {
        this.phrasebookDefinition = phrasebookDefinition;

        // todo move modal control out of this controller
        this.isModal = !!phrasebookDefinition.phrasebookModalId
    }
    get phrasebookTreeId() { return '#' + this.phrasebookDefinition.phrasebookTreeId; }
    get phrasebookModalId() { return '#' + this.phrasebookDefinition.phrasebookModalId; }
    get deleteButtonId() { return '#' + this.phrasebookDefinition.deleteButtonId; }
    get addPhraseButtonId() { return '#' + this.phrasebookDefinition.addPhraseButtonId; }
    get addCategoryButtonId() { return '#' + this.phrasebookDefinition.addCategoryButtonId; }
    get modifyButtonId() { return '#' + this.phrasebookDefinition.modifyButtonId; }
    get clearSelectButtonId() { return '#' + this.phrasebookDefinition.clearSelectButtonId; }

    parseTree(rawTree, parentId) {
        if (!rawTree) {
            return;
        }
        const root = {
            selectedIcon: "glyphicon glyphicon-stop",
            text: rawTree.name,
            selectable: false,
            list: true,
            id: rawTree.id,
            state: {
                expanded: !!parentId && (rawTree.id == parentId),
                checked: !!parentId && (rawTree.id == parentId),
            },
        };
        root.nodes = []
        if (rawTree.childLists.length > 0) {
            root.nodes = root.nodes.concat(rawTree.childLists.map(child => this.parseTree(child, parentId)));
        }
        if (rawTree.phrases.length > 0) {
            root.nodes = root.nodes.concat(rawTree.phrases.map(phrase => {
                return {
                    text: phrase.content,
                    id: phrase.id
                }
            }));
        }
        if (root.nodes.length == 0) {
            root.nodes = undefined;
            root.icon = 'fa fa-folder-open-o';
        }

        return root;
    }


    setTree = (rawTree, parentId) => {
        if (rawTree.length == 0) {
            alert('Phrasebook is empty');
            return;
        }

        this.tree = this.parseTree(rawTree, parentId);
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
            if (this.isModal) {
                $(this.phrasebookModalId).modal('hide');
            }
            this.phraseSelected.next(data.text);
        });
        let handleButtonState = (event, data) => {
            let selectedData = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)
            $(this.deleteButtonId).prop('disabled', selectedData.length === 0);
            $(this.modifyButtonId).prop('disabled', selectedData.length === 0 || selectedData.length > 1);
            $(this.clearSelectButtonId).prop('disabled', selectedData.length === 0);
        }
        $(this.phrasebookTreeId).on('nodeChecked', handleButtonState);
        $(this.phrasebookTreeId).on('nodeUnchecked', handleButtonState);
        if (this.isModal) {
            $(this.phrasebookModalId).modal({});
        }
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
        $(this.clearSelectButtonId).on('click', () => {
            $(this.phrasebookTreeId).treeview('uncheckAll', { silent: true });
        })
        $(this.addPhraseButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked && x.list)
                .map(x => { return { id: x.id, isList: !!x.list, text: x.text } });
            let parentId;
            let oldtext = '';
            if (arrayIds.length == 0) {
                parentId = 0;
            } else if (arrayIds.length > 0) {
                parentId = arrayIds[0].id;
            } else {
                alert('Please, select the only category to be parent for a new phrase');
                return;
            }

            DayPilot.Modal.prompt('Enter the text:', oldtext, { theme: "modal_rounded" }).then((result) => {
                var text = result.result;
                if (!text) {
                    return;
                }

                createPhrase(text, parentId).then(id => {
                    getTree(languageId).done((rawTree) => {
                        this.setTree(rawTree, parentId);
                        const selection = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.id  == id)[0]
                        $(this.phrasebookTreeId).treeview('selectNode', [selection, { silent: true }]);
                        $(this.phrasebookTreeId).animate({
                            scrollTop: $(this.phrasebookTreeId).find(".node-selected").offset().top - 500
                        }, 500);
                    });
                });
            });
        });


        $(this.addCategoryButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked && x.list)
                .map(x => { return { id: x.id, isList: !!x.list } });
            let parentId = 0;

            DayPilot.Modal.prompt('Enter the name:', { theme: "modal_rounded" }).then((result) => {
                var text = result.result;
                if (!text) {
                    return;
                }
                createCategory(text, parentId).then(id => {
                    getTree(languageId).done((rawTree) => {
                        this.setTree(rawTree, parentId);
                        const selection = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.id == id)[0]
                        $(this.phrasebookTreeId).treeview('selectNode', [selection, { silent: true }]);
                        $(this.phrasebookTreeId).animate({
                            scrollTop: $(this.phrasebookTreeId).find(".node-selected").offset().top - 500
                        }, 500);
                    });
                });
            });
        });


        $(this.modifyButtonId).on('click', (evt) => {
            evt.preventDefault();
            const arrayIds = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)
                .map(x => { return { id: x.id, isList: !!x.list, text: x.text } });
            let id = 0;
            let oldtext = '';
            if (arrayIds.length == 1) {
                id = arrayIds[0].id;
                oldtext = arrayIds[0].text;
            } else {
                alert('Please, select the only node to be edited');
                return;
            }

            DayPilot.Modal.prompt('Enter the name:', oldtext, { theme: "modal_rounded" }).then((result) => { 
                var text = result.result;
                if (!text) {
                    return;
                }
                const selection = $(this.phrasebookTreeId).treeview('getEnabled').filter(x => x.state.checked)[0]
                selection.text = text;
                selection.parentId = (!!selection.parentId) ? selection.parentId : 0;
                modifyNode(text, id, arrayIds[0].isList);
                $(this.phrasebookTreeId).treeview('revealNode', [selection, { silent: true }]);
            });
        });
    }
    phraseSelected = new rxjs.Subject();
}

function initPhrasebook(phrasebookDefinition) {
    return new PhrasebookController(phrasebookDefinition);
}