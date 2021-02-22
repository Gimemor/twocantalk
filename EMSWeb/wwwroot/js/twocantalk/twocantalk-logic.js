

function startTwoCanTalk() {
    // init view
    $('#' + chatsDefinitions[0].placeholderSelector).twocantalk(chatsDefinitions[0]);
    $('#' + chatsDefinitions[1].placeholderSelector).twocantalk(chatsDefinitions[1]);
    // init phrasebook
    $('#' + chatsDefinitions[0].placeholderSelector).phrasebook(chatsDefinitions[0].phrasebook);

    // init controller
    const firstChatWindow = initTctController(chatsDefinitions[0]);
    const secondChatWindow = initTctController(chatsDefinitions[1]);
    const firstPhrasebook = initPhrasebook(chatsDefinitions[0].phrasebook);
    
    // connect controller
    connectChats(firstChatWindow, secondChatWindow);

    firstPhrasebook.phraseSelected.subscribe(x => firstChatWindow.setPhrase(x));
    firstChatWindow.openPhrasebookClicked.subscribe(x => firstPhrasebook.showPhrasebook(x));
}