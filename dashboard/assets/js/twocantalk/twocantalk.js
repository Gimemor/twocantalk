
let MESSAGE_TYPE = {
    sent: 0,
    recieved: 1
}

let contexts = [];
let loadedScenes = [];


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
    placeholderSelector,
    inputSelector, 
    keyboardContainer, 
    chatHistoryId, 
    toggleVoiceId, 
    languageSelectorId, 
    repeatTranslatedButtonId, 
    repeatVoicedButtonId,
    toggleTranslationButtonId,
    exportToPdfButtonId) {
    let context = {};
    context.inputSelector = inputSelector;
    context.placeholderSelector = placeholderSelector;
    context.keyboardContainer = keyboardContainer;
    context.chatHistoryId = chatHistoryId;
    context.toggleVoiceId = toggleVoiceId;
    context.languageSelectorId = languageSelectorId;
    context.repeatTranslatedButtonId = repeatTranslatedButtonId;
    context.repeatVoicedButtonId = repeatVoicedButtonId;
    context.toggleTranslationButtonId = toggleTranslationButtonId;
    context.exportToPdfButtonId = exportToPdfButtonId;
    context.sceneId = sceneId;
    context.sceneRef = sceneRef;
    context.sceneLoaded = false;
    context.messages = [];
    context.isVoiceEnabled = true;
    $(context.toggleVoiceId).on('click', function() {
        context.isVoiceEnabled = !context.isVoiceEnabled;
    });
    $.keyboard.keyaction.enter = function( kb ) {
        // same as $.keyboard.keyaction.accept();
        kb.close( true );
        return false;     // return false prevents further processing
    };
    context.oldInputValue = null;
    context.inputHandler = function(event) {
        let value = $(inputSelector).val()
        if(!value || value.length <= 0) {
            return;
        }
        $(inputSelector).val('');
        context.oldInputValue = value;
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: context.languageId };
        
        if(context.languageId === context.connectedContext.languageId) {
            context.addMessage(message, false, false, message.text, message.text, context.languageId, context.languageId);
        }
        
        if(context.sendToConnected) {
            context.sendToConnected({
                type: 'message',
                value: Object.assign({}, message)
            })
        }
    };
    $(inputSelector).on('keyup', function(event) { 
        if(event.key != 'Enter') {
            return;
        }
        context.inputHandler(event); 
    });
    $(inputSelector).on('accepted', context.inputHandler);
    $(inputSelector).on('change', function(event) { 
        //context.inputHandler(event); 
        console.log(event);
    });
    
    context.addMessage = function(message, wasSpoken, wasTranslated, sourceText, translatedText, sourceLanguageId, translatedLanguageId) {
        message.wasSpoken = wasSpoken;
        message.wasTranslated = wasTranslated;
        message.sourceText = sourceText;
        message.translatedText = translatedText;
        message.sourceLanguageId = sourceLanguageId;
        message.translatedLanguageId = translatedLanguageId;
        
        context.messages.push(message);
        context.messages.sort(function (a, b) {return a.time > b.time? 1 : -1});
        context.updateTextarea();
    }
    context.sayText = function(text, languageId) {
        selectScene(context.sceneRef);
        sayText(text, 2, languageId, 2);
    }
    context.handleConnection = function(event) {
        if(event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent)? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            const shouldTranslate = context.languageId !== event.value.languageId;
            let sourceText = event.value.text;
            let translatedText = event.value.text;
            let sourceLanguageId = event.value.languageId;
            let translatedLanguageId = context.languageId;
            if(shouldTranslate) {
                translate(event.value.text, event.value.languageId, context.languageId, function (data, status) {
                    translatedText = event.value.text = data.data.translations[0].translatedText;
                    if(context.isVoiceEnabled) {
                        context.sayText(event.value.text, context.languageId);
                    }
                    context.addMessage(event.value, context.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
                    if(context.sendToConnected) {
                        context.sendToConnected({
                            type: 'reverse-translation',
                            value: Object.assign({}, event.value)
                        });
                    }
                });
            } else {
                if(context.isVoiceEnabled) {
                    selectScene(context.sceneRef);
                    context.sayText(event.value.text, context.languageId);
                }
                context.addMessage(event.value, context.isVoiceEnabled, shouldTranslate, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            }
        } else if(event.type == 'reverse-translation') {
                event.value.type = (event.value.type == MESSAGE_TYPE.sent)? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
                translate(event.value.translatedText, event.value.translatedLanguageId, context.languageId, function (data, status) {
                    translatedText = event.value.text = data.data.translations[0].translatedText;
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
    context.showTranslatedText = true;
    context.updateTextarea = function () {
        console.log('updating chat')
        const el = $(context.chatHistoryId);
        let str = '';
        const messageKey = (context.showTranslatedText)? 'text' : 'sourceText';
        const hiddenTextKey = (context.showTranslatedText)? 'sourceText' : 'text';
        for(let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i]
            
            if(message.wasTranslated && message.type == MESSAGE_TYPE.sent) {
                str = `<tr class="row chat-line flex-nowrap">\
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
    context.languageId = 1;
    const languageInfo =  languages.find(x => x.id == context.languageId);
    context.keyboard = initKeyboard(languageInfo, inputSelector, keyboardContainer)
    $(languageSelectorId).val(context.languageId);
    $(languageSelectorId).on('change', function(event) {
        context.languageId = $(languageSelectorId).val();
        const languageInfo =  languages.find(x => x.id == context.languageId);
        $(context.keyboard).getkeyboard().destroy();
        context.keyboard =  initKeyboard(languageInfo, context.inputSelector, context.keyboardContainer);
    })
    $(languageSelectorId).trigger('change');
    $(repeatVoicedButtonId).on('click', function () {
        let last = context.messages.length - 1;
        while(last >= 0 && !(context.messages[last].wasSpoken && context.messages[last].type == MESSAGE_TYPE.recieved)) {
            last--;
        }
        if(last >= 0) {
            const message = context.messages[last];
            context.sayText(message.text, message.translatedLanguageId);
        }
    });
    $(repeatTranslatedButtonId).on('click', function() {
        let last = context.messages.length - 1;
        while(last >= 0 && !(context.messages[last].wasTranslated && context.messages[last].type == MESSAGE_TYPE.recieved)) {
            last--;
        }
        if(last >= 0) {
            const message = context.messages[last];
            context.sayText(message.text, message.translatedLanguageId);
        }
    });
    $(toggleTranslationButtonId).on('click', function() {
        context.showTranslatedText = !context.showTranslatedText;
        context.updateTextarea();
    })
    $(exportToPdfButtonId).on('click', function() {
        
        // playground requires you to assign document definition to a variable called dd
        
        var dd = {
            content: [
            ],
            styles: {
                header: {
                    fontSize: 18,
                    bold: true,
                    alignment: 'justify'
                }
            }
            
        }
        let str = '';
        const messageKey = 'text';
        const hiddenTextKey =  'sourceText';
        dd.content.push('My side:')
        for(let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i];
            str = message['time'].toLocaleTimeString() + ' > ' + message[messageKey] + ' ' + (message.wasTranslated? `(${message[hiddenTextKey]})` : '');
            dd.content.push(str.trim());
        }
        dd.content.push('The other side:')
        for(let i = 0; i < context.connectedContext.messages.length; i++) {
            const message = context.connectedContext.messages[i];
            str = message['time'].toLocaleTimeString() + ' > ' + message[messageKey] + ' ' + (message.wasTranslated? `(${message[hiddenTextKey]})` : '');
            dd.content.push(str.trim());
        }
        pdfMake.createPdf(dd).download();
        
        // html2canvas($(context.chatHistoryId)[0]).then(img => {
        //     doc.addImage(img.toDataURL(), 'png', 0, 0, 300, 300);
        //     doc.save("transcription.pdf");
        // });
        
    })
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
            element.placeholderSelector,
            '#' + element.userInputId,
            '#' + element.keyboardContainerId, 
            '#' + element.chatHistoryId, 
            '#' + element.toggleVoiceId,
            '#' + element.languageSelectorId,
            '#' + element.repeatTranslatedButtonId, 
            '#' + element.repeatVoicedButtonId,
            '#' + element.toggleTranslationButtonId,
            '#' + element.exportToPdfButtonId)
        );
        index++;
    })
    for(let i = 1; i < contexts.length; i++) {
        connectChats(contexts[i], contexts[i - 1]);
    }
    return contexts;
    
}