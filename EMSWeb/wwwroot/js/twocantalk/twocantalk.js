
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
// get the tee for a language from serverside
function getTree(languageId) {
    const from = languages.find(x => x.id == languageId);
    if (phrasebooks[from.language]) {
        return phrasebooks[from.language];
    }
    return [];
}


function vh_sceneLoaded(sceneIndex, sceneRef) {
    loadedScenes.push(sceneRef);
    $(window).off('resize');
    contexts.filter(ref => loadedScenes.includes(ref.sceneId)).forEach(context => {
        $(window).on('resize',  function() {
            const ratio =  $(window).height() / $(window).width();
            const width =  $(context.placeholderSelector).find('.avatar').width();
            const height = $(context.placeholderSelector).find('.avatar').height();
            
            selectScene(context.sceneRef);
            dynamicResize(width, height);
        });
    });
    $(window).trigger('resize');
}

function initChat(
    sceneId,
    sceneRef,
    genderId,
    placeholderSelector,
    inputSelector,
    keyboardContainer,
    chatHistoryId,
    toggleVoiceId,
    languageSelectorId,
    repeatTranslatedButtonId,
    repeatLastSentenceButtonId,
    toggleTranslationButtonId,
    exportToPdfButtonId,
    openPharsebookButtonId,
    phrasebookModalId,
    phrasebookTreeId,
    toggleKeyboardId) {
    let context = {};
    context.inputSelector = inputSelector;
    context.placeholderSelector = placeholderSelector;
    context.keyboardContainer = keyboardContainer;
    context.chatHistoryId = chatHistoryId;
    context.genderId = genderId;
    context.toggleVoiceId = toggleVoiceId;
    context.languageSelectorId = languageSelectorId;
    context.repeatTranslatedButtonId = repeatTranslatedButtonId;
    context.repeatLastSentenceButtonId = repeatLastSentenceButtonId;
    context.toggleTranslationButtonId = toggleTranslationButtonId;
    context.exportToPdfButtonId = exportToPdfButtonId;
    context.openPharsebookButtonId = openPharsebookButtonId;
    context.phrasebookTreeId = phrasebookTreeId;
    context.toggleKeyboardId = toggleKeyboardId;
    context.sceneId = sceneId;
    context.sceneRef = sceneRef;
    context.sceneLoaded = false;
    context.messages = [];
    context.isVoiceEnabled = true;
    context.phrasebookModalId = phrasebookModalId;

    $(context.toggleVoiceId).on('click', function () {
        context.isVoiceEnabled = !context.isVoiceEnabled;
    });

    context.oldInputValue = null;
    context.inputHandler = function (event) {
        let value = $(inputSelector).val()
        if (!value || value.length <= 0) {
            return;
        }
        $(inputSelector).val('');
        context.oldInputValue = value;
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: context.languageId };
        const from = languages.find(x => x.id == context.connectedContext.languageId);
        const to = languages.find(x => x.id == context.languageId);
        if (from.language === to.language) {
            context.addMessage(message, false, false, message.text, message.text, context.languageId, context.languageId);
        }

        if (context.sendToConnected) {
            context.sendToConnected({
                type: 'message',
                value: Object.assign({}, message)
            })
        }
    };
    $(inputSelector).on('keyup', function (event) {
        if (event.key != 'Enter') {
            return;
        }
        context.inputHandler(event);
    });
    $(inputSelector).on('accepted', context.inputHandler);
    $(inputSelector).on('change', function (event) {
        //context.inputHandler(event); 
        console.log(event);
    });

    context.addMessage = function (message, wasSpoken, wasTranslated, sourceText, translatedText, sourceLanguageId, translatedLanguageId) {
        message.wasSpoken = wasSpoken;
        message.wasTranslated = wasTranslated;
        message.sourceText = sourceText;
        message.translatedText = translatedText;
        message.sourceLanguageId = sourceLanguageId;
        message.translatedLanguageId = translatedLanguageId;

        context.messages.push(message);
        context.messages.sort(function (a, b) { return a.time > b.time ? 1 : -1 });
        context.updateTextarea();
    }
    context.sayText = function (text, languageId) {
        selectScene(context.sceneRef);
        const langInfo = languages.find(x => x.id == languageId);
        if (context.genderId == GENDER_ID.male) {
            sayText(text, 2, langInfo.voiceId ? langInfo.voiceId : langInfo.id, 2);
        } else {
            sayText(text, 1, langInfo.voiceId ? langInfo.voiceId : langInfo.id, 3);
        }
    }
    context.handleConnection = function (event) {
        if (event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent) ? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            const from = languages.find(x => x.id == event.value.languageId);
            const to = languages.find(x => x.id == context.languageId);
            const shouldTranslate = from.language != to.language;
            let sourceText = event.value.text;
            let translatedText = event.value.text;
            let sourceLanguageId = event.value.languageId;
            let translatedLanguageId = context.languageId;
            if (shouldTranslate) {
                translate(event.value.text, event.value.languageId, context.languageId, function (data, status) {
                    translatedText = data.data.translations[0].translatedText;
                    if (context.isVoiceEnabled) {
                        context.sayText(translatedText, context.languageId);
                    }
                    context.addMessage(event.value, context.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
                    if (context.sendToConnected) {
                        context.sendToConnected({
                            type: 'reverse-translation',
                            value: Object.assign({}, event.value)
                        });
                    }
                });
            } else {
                if (context.isVoiceEnabled) {
                    selectScene(context.sceneRef);
                    context.sayText(event.value.text, context.languageId);
                }
                context.addMessage(event.value, context.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            }
        } else if (event.type == 'reverse-translation') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent) ? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            translate(event.value.translatedText, event.value.translatedLanguageId, context.languageId, function (data, status) {
                translatedText = data.data.translations[0].translatedText;
                context.addMessage(
                    event.value,
                    context.isVoiceEnabled,
                    true,
                    event.value.sourceText,
                    translatedText,
                    event.value.sourceLanguageId,
                    event.value.translatedLanguageId);
            });
        }
    };
    context.showTranslatedText = false;
    context.updateTextarea = function () {
        console.log('updating chat')
        const el = $(context.chatHistoryId);
        let str = '';
        for (let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i]
            let messageKey = (message.type == MESSAGE_TYPE.recieved) ? 'translatedText' : 'sourceText';
            let hiddenTextKey = (message.type == MESSAGE_TYPE.recieved) ? 'sourceText' : 'translatedText';
            if (context.showTranslatedText) {
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
    $(languageSelectorId).select2({
        data: languages
    });
    context.updateKeyboardHideFlag = function () {
        $(context.toggleKeyboardId).hasClass('active') ?
            $(context.keyboard).getkeyboard().$keyboard.addClass('english-hidden') :
            $(context.keyboard).getkeyboard().$keyboard.removeClass('english-hidden')
    }
    context.languageId = 1;
    const languageInfo = languages.find(x => x.id == context.languageId);
    context.keyboard = initKeyboard(languageInfo, inputSelector, keyboardContainer)
    $.keyboard.keyaction.enter = function (kb) {
        // same as $.keyboard.keyaction.accept();
        kb.close(true);
        return false;     // return false prevents further processing
    };

    $(languageSelectorId).val(context.languageId);
    $(languageSelectorId).on('change', function (event) {
        context.languageId = $(languageSelectorId).val();
        const languageInfo = languages.find(x => x.id == context.languageId);
        $(context.keyboard).getkeyboard().destroy();
        context.keyboard = initKeyboard(languageInfo, context.inputSelector, context.keyboardContainer);
        context.updateKeyboardHideFlag();
    })
    $(languageSelectorId).trigger('change');
    $(repeatLastSentenceButtonId).on('click', function () {
        let last = context.messages.length - 1;
        while (last >= 0 && !(context.messages[last].type == MESSAGE_TYPE.sent)) {
            last--;
        }
        if (last >= 0) {
            const message = context.messages[last];
            $(inputSelector).val(message.text);
        }
    });
    $(repeatTranslatedButtonId).on('click', function () {
        let last = context.messages.length - 1;
        while (last >= 0 && !(context.messages[last].wasTranslated && context.messages[last].type == MESSAGE_TYPE.recieved)) {
            last--;
        }
        if (last >= 0) {
            const message = context.messages[last];
            context.sayText(message.translatedText, message.translatedLanguageId);
        }
    });
    $(toggleTranslationButtonId).on('click', function () {
        context.showTranslatedText = !context.showTranslatedText;
        context.updateTextarea();
    })
    $(exportToPdfButtonId).on('click', function () {
        const firstName = window.prompt('Please enter that Teacher name:');
        const secondName = window.prompt('Please enter the Student name:');
        // playground requires you to assign document definition to a variable called dd

        let str = '';
        const messageKey = 'translatedText';
        const hiddenTextKey = 'sourceText';
        lines = [];
        lines = lines.concat([
            {
                alignment: 'justify',
                columns: [
                    `${firstName}`,
                    `${secondName}`,
                ]
            },
            '\n']);
        for (let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i];
            const connectedMessage = context.connectedContext.messages[i];
            let isSent = message.type == MESSAGE_TYPE.sent;
            const fromLangInfo = languages.find(x => x.id == message.sourceLanguageId);
            const toLangInfo = languages.find(x => x.id == connectedMessage.translatedLanguageId);
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

    })
    $(openPharsebookButtonId).on("click", function () {
        const tree = getTree(context.languageId);
        if (tree.length == 0) {
            alert('Phrasebook is empty');
            return;
        }
        $(context.phrasebookTreeId).treeview({
            data: tree,
            collapseIcon: 'fa fa-minus',
            checkedIcon: 'fa fa-check',
            expandIcon: 'fa fa-plus'
        });
        $('.bstreeview .list-group-item').on('click', function (event) {
        })
        $(context.phrasebookTreeId).on('nodeSelected', function (event, data) {
            // close modal
            $(context.phrasebookModalId).modal('hide');
            // put selected into input
            $(inputSelector).val(data.text);
            $(inputSelector).trigger('accepted');
        });


        $(context.phrasebookModalId).modal({});
    });
    $(context.toggleKeyboardId).on('click', function () {
        $(context.keyboard).getkeyboard().$keyboard.toggleClass('english-hidden');
    });

    $(context.toggleKeyboardId).click();
    $(context.inputSelector).parent().click(function () { $(context.inputSelector).focus(); });
    return context;
}

function connectChats(firstContext, secondContext) {
    firstContext.sendToConnected = function(event) {
        secondContext.handleConnection(event);
    }
    firstContext.connectedContext = secondContext;
    secondContext.sendToConnected = function(event) {
        firstContext.handleConnection(event);
    }
    secondContext.connectedContext = firstContext;
}

function initTct(chatDefinitions) {
    contexts = []
    index = 0;
    chatDefinitions.forEach(element => {
        contexts.push(
                initChat(index, 
            element.scene, 
            element.genderId,
            element.placeholderSelector,
            '#' + element.userInputId,
            '#' + element.keyboardContainerId, 
            '#' + element.chatHistoryId, 
            '#' + element.toggleVoiceId,
            '#' + element.languageSelectorId,
            '#' + element.repeatTranslatedButtonId, 
            '#' + element.repeatLastSentenceButtonId,
            '#' + element.toggleTranslationButtonId,
            '#' + element.exportToPdfButtonId,
            '#' + element.openPharsebookButtonId,
            '#' + element.phrasebookModalId,
            '#' + element.phrasebookTreeId,
            '#' + element.toggleKeyboardId
            )
        );
        index++;
    })
    for(let i = 1; i < contexts.length; i++) {
        connectChats(contexts[i], contexts[i - 1]);
    }
    return contexts;
    
}