

function startTwoCanTalk() {
    // init view
    $('#' + chatsDefinitions[0].placeholderSelector).twocantalk(chatsDefinitions[0]);
    $('#' + chatsDefinitions[1].placeholderSelector).twocantalk(chatsDefinitions[1]);
    // init phrasebook
    $('#' + chatsDefinitions[0].placeholderSelector).phrasebook(chatsDefinitions[0].phrasebook);
    // init controller
    const firstController = initTctController(chatsDefinitions[0]);
    const secondController = initTctController(chatsDefinitions[1]);
    const firstPhrasebook = initPhrasebook(chatsDefinitions[0].phrasebook);

    // connect controller
    connectChats(firstController, secondController);

    firstPhrasebook.phraseSelected.subscribe(x => firstController.setPhrase(x));
    firstController.openPhrasebookClicked.subscribe(x => firstPhrasebook.showPhrasebook(x));
}