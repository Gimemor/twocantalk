
let MESSAGE_TYPE = {
    sent: 0,
    recieved: 1
}
let GENDER_ID = {
    male: 0,
    female: 1
}
let contexts = [];
let loadedScenes = [];

// Callback for ODDCAST API loaded on a scene initialization finish. 
// Here we setup a resize for an avatar .
function vh_sceneLoaded(sceneIndex, sceneRef) {
    loadedScenes.push(sceneRef);
    $(window).off('resize');
    contexts.filter(ref => loadedScenes.includes(ref.chatDefinition.sceneId)).forEach(context => {
        $(window).on('resize', function () {
            const ratio = $(window).height() / $(window).width();
            const width = $(context.placeholderSelector).find('.avatar').width();
            const height = $(context.placeholderSelector).find('.avatar').height();

            selectScene(context.chatDefinition.sceneRef);
            dynamicResize(width, height);
        });
    });
    $(window).trigger('resize');
}

class ChatContext {
    that = this;

    constructor(chatDefinition) {
        this.chatDefinition = chatDefinition;
        this.sceneLoaded = false;
        this.isVoiceEnabled = true;
        this.messages = [];
        this.languageId = 1;
        this.showTranslatedText = false;


        this.initLanguageSelector();
        this.initKeyboard();
        $(this.toggleVoiceId).on('click', this.toggleVoice);
        $(this.inputSelector).on('accepted', (evt) => this.inputHandler(evt));
        $(this.languageSelectorId).on('change', this.handleLanguageSelect)
        $(this.repeatLastSentenceButtonId).on('click', this.repeatLastSentence);
        $(this.repeatTranslatedButtonId).on('click', this.repeatTranslated);
        $(this.toggleTranslationButtonId).on('click', this.handleToggleTranslationClick);
        $(this.exportToPdfButtonId).on('click', this.handleCreatePdfClick);
        $(this.toggleKeyboardId).on('click', this.handleToggleKeyboardClick);
        $(this.openPhrasebookButtonId).on("click", this.handleOpenPhrasebookClick);


        $(this.languageSelectorId).trigger('change');
        $(this.toggleKeyboardId).click();
        $(this.inputSelector).parent().click(() => { $(this.inputSelector).focus(); });
        $(this.languageSelectorId).val(this.languageId);


    }

    get toggleVoiceId() { return '#' + this.chatDefinition.toggleVoiceId; }
    get inputSelector() { return '#' + this.chatDefinition.userInputId; }
    get languageSelectorId() { return '#' + this.chatDefinition.languageSelectorId; }
    get chaselftoryId() { return '#' + this.chatDefinition.chatHistoryId; }
    get sceneRef() { return this.chatDefinition.sceneRef; }
    get keyboardContainer() { return '#' + this.chatDefinition.keyboardContainerId; }
    get toggleKeyboardId() { return '#' + this.chatDefinition.toggleKeyboardId; }
    get chatHistoryId() { return '#' + this.chatDefinition.chatHistoryId; }
    get repeatLastSentenceButtonId() { return '#' + this.chatDefinition.repeatLastSentenceButtonId; }
    get repeatTranslatedButtonId() { return '#' + this.chatDefinition.repeatTranslatedButtonId; }
    get toggleTranslationButtonId() { return '#' + this.chatDefinition.toggleTranslationButtonId; }
    get exportToPdfButtonId() { return '#' + this.chatDefinition.exportToPdfButtonId; }
    get placeholderSelector() { return '#' + this.chatDefinition.placeholderSelector; }
    get phrasebookInfo() { return this.chatDefinition.phrasebook; }
    get openPhrasebookButtonId() { return (this.chatDefinition.phrasebook) ? '#' + this.chatDefinition.phrasebook.openPhrasebookButtonId : ''; }
    get genderId() { return this.chatDefinition.genderId; }

    initLanguageSelector = function () {
        $(this.languageSelectorId).select2({
            data: languages
        });
    }

    initKeyboard = function () {
        const languageInfo = languages.find(x => x.id == this.languageId);
        this.keyboard = initKeyboard(languageInfo, this.inputSelector, this.keyboardContainer)
        $.keyboard.keyaction.enter = (kb, val, evt) => {
            evt.preventDefault();

            if (evt.ctrlKey) {
                kb.$el.val(kb.$el.val() + '\n');
                return;
            }
            // same as $.keyboard.keyaction.accept();
            kb.close(true);
            return false;     // return false prevents further processing
        };
    }

    inputHandler = function (event) {
        event.preventDefault();
        let value = $(this.inputSelector).val().trim();
        if (!value || value.length <= 0) {
            return;
        }
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: this.languageId };
        const from = languages.find(x => x.id == this.connectedContext.languageId);
        const to = languages.find(x => x.id == this.languageId);
        if (from.language === to.language) {
            this.addMessage(message, false, false, message.text, message.text, this.languageId, this.languageId);
        }
        if (this.sendToConnected) {
            this.sendToConnected({
                type: 'message',
                value: Object.assign({}, message)
            })
        }
        $(this.inputSelector).val('');
        $(this.inputSelector).prop('selectionStart', 0); $(this.inputSelector).prop('selectionEnd', 0);
    };

    addMessage = function (message, wasSpoken, wasTranslated, sourceText, translatedText, sourceLanguageId, translatedLanguageId) {
        message.wasSpoken = wasSpoken;
        message.wasTranslated = wasTranslated;
        message.sourceText = sourceText;
        message.translatedText = translatedText;
        message.sourceLanguageId = sourceLanguageId;
        message.translatedLanguageId = translatedLanguageId;

        this.messages.push(message);
        this.messages.sort(function (a, b) { return a.time > b.time ? 1 : -1 });
        this.updateTextarea();
    }

    sayText = function (text, languageId) {
        window.selectScene(this.sceneRef);
        const langInfo = languages.find(x => x.id == languageId);
        if (this.genderId == GENDER_ID.male) {
            window.sayText(text, 2, langInfo.voiceId ? langInfo.voiceId : langInfo.id, 2);
        } else {
            window.sayText(text, 1, langInfo.voiceId ? langInfo.voiceId : langInfo.id, 3);
        }
    }


    handleConnection = (function (event) {
        let translatedText = '';
        if (event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent) ? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            const from = languages.find(x => x.id == event.value.languageId);
            const to = languages.find(x => x.id == this.languageId);
            const shouldTranslate = from.language != to.language;
            let sourceText = event.value.text;
            let sourceLanguageId = event.value.languageId;
            let translatedLanguageId = this.languageId;
            translatedText = event.value.text;
            if (shouldTranslate) {
                translate(event.value.text, event.value.languageId, this.languageId, (data, status) => {
                    translatedText = data.data.translations[0].translatedText;
                    if (this.isVoiceEnabled) {
                        this.sayText(translatedText, this.languageId);
                    }
                    this.addMessage(event.value, this.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
                    if (this.sendToConnected) {
                        this.sendToConnected({
                            type: 'reverse-translation',
                            value: Object.assign({}, event.value)
                        });
                    }
                });
            } else {
                if (this.isVoiceEnabled) {
                    selectScene(this.sceneRef);
                    this.sayText(event.value.text, this.languageId);
                }
                this.addMessage(event.value, this.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            }
        } else if (event.type == 'reverse-translation') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent) ? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            translate(event.value.translatedText, event.value.translatedLanguageId, this.languageId, (data, status) => {
                translatedText = data.data.translations[0].translatedText;
                this.addMessage(
                    event.value,
                    this.isVoiceEnabled,
                    true,
                    event.value.sourceText,
                    translatedText,
                    event.value.sourceLanguageId,
                    event.value.translatedLanguageId);
            });
        }
    }).bind(this)

    updateTextarea = function () {
        const el = $(this.chatHistoryId);
        let str = '';
        for (let i = 0; i < this.messages.length; i++) {
            const message = this.messages[i]
            let messageKey = (message.type == MESSAGE_TYPE.recieved) ? 'translatedText' : 'sourceText';
            let hiddenTextKey = (message.type == MESSAGE_TYPE.recieved) ? 'sourceText' : 'translatedText';
            if (this.showTranslatedText) {
                const temp = hiddenTextKey;
                hiddenTextKey = messageKey;
                messageKey = temp;
            }
            if (message.wasTranslated && message.type == MESSAGE_TYPE.sent) {
                str = `<tr class="row chat-line flex-nowrap grayed">\
                    <td class="col-md-1 col-xs-1 toggle-row-container">\
                    <button onClick='toggleRow(this)' title="Toggle reverse translation" type="button" class="btn btn-light toggle-row" data-toggle="button" aria-pressed="false" autocomplete="off"></button>\
                    </td>\
                    <td class="col-md-2 col-xs-2">${message['time'].toLocaleTimeString()}</td>\
                    <td class="col-md-9 col-xs-9">${message[messageKey]}</td>\
                
                    </tr><tr class='expanded-row-content hide-row'><td colspan="4">${message[hiddenTextKey]}</td>\</tr>\n` + str;
            } else {
                str = `<tr class="row chat-line flex-nowrap">\
                    <td class="col-md-1 col-xs-1"></td>\
                    <td class="col-md-2 col-xs-2">${message['time'].toLocaleTimeString()}</td>\
                    <td class="col-md-9 col-xs-8">${message[messageKey]}</td>\
                    </tr>\n` + str;
            }
        }

        el.html(str);
    }

    updateKeyboardHideFlag = () => {
        $(this.toggleKeyboardId).hasClass('active') ?
            $(this.keyboard).getkeyboard().$keyboard.addClass('english-hidden') :
            $(this.keyboard).getkeyboard().$keyboard.removeClass('english-hidden')
    }

    toggleVoice = () => {
        this.isVoiceEnabled = !this.isVoiceEnabled;
    }


    recreateKeyboard = (languageInfo) => {
        $(this.keyboard).getkeyboard().destroy();
        this.keyboard = initKeyboard(languageInfo, this.inputSelector, this.keyboardContainer);
        this.updateKeyboardHideFlag();
    }

    handleLanguageSelect = (event) => {
        this.languageId = $(this.languageSelectorId).val();
        const languageInfo = languages.find(x => x.id == this.languageId);
        this.recreateKeyboard(languageInfo);
    }

    repeatLastSentence = () => {
        let last = this.messages.length - 1;
        while (last >= 0 && !(this.messages[last].type == MESSAGE_TYPE.sent)) {
            last--;
        }
        if (last >= 0) {
            const message = this.messages[last];
            $(this.inputSelector).val(message.text);
        }
    }

    repeatTranslated = () => {
        let last = this.messages.length - 1;
        while (last >= 0 && !(this.messages[last].wasTranslated && this.messages[last].type == MESSAGE_TYPE.recieved)) {
            last--;
        }
        if (last >= 0) {
            const message = this.messages[last];
            this.sayText(message.translatedText, message.translatedLanguageId);
        }
    }

    handleToggleTranslationClick = () => {
        this.showTranslatedText = !this.showTranslatedText;
        $(this.toggleTranslationButtonId).toggleClass('active', this.showTranslatedText);
        this.updateTextarea();
    }

    handleCreatePdfClick = () => {
        DayPilot.Modal.prompt('Please enter that Teacher name:', { theme: "modal_rounded" }).then((firstNameResult) => {
            DayPilot.Modal.prompt('Please enter the Student name:', { theme: "modal_rounded" }).then((secondNameResult) => {
                const firstName = firstNameResult.result;
                const secondName = secondNameResult.result;
                const messageKey = 'translatedText';
                let lines = [];
                lines = lines.concat([
                    {
                        alignment: 'justify',
                        columns: [
                            `${firstName}`,
                            `${secondName}`,
                        ]
                    },
                    '\n']);
                for (let i = 0; i < this.messages.length; i++) {
                    const message = this.messages[i];
                    const connectedMessage = this.connectedContext.messages[i];
                    lines = lines.concat([
                        //{
                        //    alignment: 'justify',
                        //    columns: [
                        //        `from ${(isSent)? firstName : secondName}`,
                        //        `to ${(isSent)? secondName : firstName}`,
                        //    ]
                        //},
                        //{
                        //    alignment: 'justify',
                        //    columns: [
                        //        `${(isSent) ? firstName : secondName}` + fromLangInfo.text,
                        //        `${(isSent) ? secondName : firstName}` +toLangInfo.text
                        //    ]
                        //},
                        {
                            alignment: 'justify',
                            columns: [
                                {
                                    text: message['time'].toLocaleTimeString() + ' > ' + message[messageKey] + ' ' + (message.wasTranslated ? `` : ''),
                                    style: [message.type == MESSAGE_TYPE.recieved ? 'bold' : '']
                                },
                                {
                                    text: connectedMessage['time'].toLocaleTimeString() + ' > ' + connectedMessage[messageKey] + ' ' + (connectedMessage.wasTranslated ? `` : ''),
                                    style: [connectedMessage.type == MESSAGE_TYPE.recieved ? 'bold' : '']
                                }
                            ]
                        },
                        '\n'
                    ])
                };
                var dd = {
                    content: [
                        'The two can talk transcription between ' + firstName + ' & ' + secondName + ' on ' + (new Date()).toLocaleDateString() + '\n\n',
                        'Bold is for recipient.\n\n'
                    ].concat(lines),
                    styles: {
                        header: {
                            fontSize: 18,
                            bold: true
                        },
                        bold: {
                            bold: true
                        }
                    },
                    defaultStyle: {
                        columnGap: 20
                    }
                };

                pdfMake.createPdf(dd).download();

                // html2canvas($(context.chatHistoryId)[0]).then(img => {
                //     doc.addImage(img.toDataURL(), 'png', 0, 0, 300, 300);
                //     doc.save("transcription.pdf");
                // });
            });
        });
    }

    handleToggleKeyboardClick = () => {
        $(this.toggleKeyboardId).toggleClass('active');
        const val = $(this.toggleKeyboardId).hasClass('active');
        $(this.keyboard).getkeyboard().$keyboard.toggleClass('english-hidden', val);
    }

    handleOpenPhrasebookClick = () => {
        this.openPhrasebookClicked.next(this.languageId);
    }
    setPhrase = (text) => {
        $(this.inputSelector).val(text);
        $(this.inputSelector).trigger('accepted');
    }

    // output events
    openPhrasebookClicked = new rxjs.Subject();
}

function initTctController(chatDefinition) {
    let context = new ChatContext(chatDefinition);
    contexts.push(context);
    return context;
}



function connectChats(firstContext, secondContext) {
    firstContext.sendToConnected = function (event) {
        secondContext.handleConnection(event);
    }
    firstContext.connectedContext = secondContext;
    secondContext.sendToConnected = function (event) {
        firstContext.handleConnection(event);
    }
    secondContext.connectedContext = firstContext;
}
