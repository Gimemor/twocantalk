
let MESSAGE_TYPE = {
    sent: 0,
    recieved: 1
}

let contexts = [];
let loadedScenes = [];


function initTranslator(
    placeholderSelector,
    inputSelector,
    outputSelector, 
    keyboardContainer, 
    fromLanguageSelectorId, 
    toLanguageSelectorId,
    clearButtonId,
    translateButtonId) {

    let context = {};
    context.inputSelector = inputSelector;
    context.outputSelector = outputSelector;
    context.placeholderSelector = placeholderSelector;
    context.keyboardContainer = keyboardContainer;
    context.fromLanguageSelectorId = fromLanguageSelectorId;
    context.toLanguageSelectorId = toLanguageSelectorId;
    context.clearButtonId = clearButtonId;
    context.translateButtonId = translateButtonId;

    context.messages = [];
    $.keyboard.keyaction.enter = function( kb ) {
        // same as $.keyboard.keyaction.accept();
        return false;     // return false prevents further processing
    };
    context.inputHandler = function(event) {
        let value = $(inputSelector).val().trim();
        $(inputSelector).val('');
        if(!value || value.length <= 0) {
            return;
        }
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: context.fromLanguageId };
        context.handleConnection({
            type: 'message',
            value: Object.assign({}, message)
        })
    };

    $(window).on('keyup', function(event) { 
        if(event.key != 'Enter') {
            return;
        }
        context.inputHandler(event); 
    });
    $(clearButtonId).on('click', () => {
        $(inputSelector).val('');
        $(outputSelector).val('');
        context.messages = [];
        context.updateTextarea();
    });
    $(translateButtonId).on('click', context.inputHandler);
    $(inputSelector).on('accepted', context.inputHandler);
    $(inputSelector).on('change', function(event) { 
        //context.inputHandler(event); 
    });
    
    context.addMessage = function (message, sourceMessage, translatedMessage, sourceLanguageId, translatedLanguageId) {
        message.sourceMessage = sourceMessage;
        message.translatedMessage = translatedMessage;
        message.sourceLanguageId = sourceLanguageId;
        message.translatedLanguageId = translatedLanguageId;
        
        context.messages.push(message);
        context.messages.sort(function (a, b) {return a.time > b.time? 1 : -1});
        context.updateTextarea();
    }
    context.handleConnection = function(event) {
        if(event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent)? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            let sourceMessage = event.value.text;
            let translatedMessage = event.value.text;
            let sourceLanguageId = event.value.languageId;
            let translatedLanguageId = context.toLanguageId;
            if(sourceLanguageId !== translatedLanguageId) { 
                translate(event.value.text, context.fromLanguageId, context.toLanguageId, function (data, status) {
                    translatedMessage = event.value.text = data.data.translations[0].translatedText;
                    context.addMessage(event.value, sourceMessage, translatedMessage, sourceLanguageId, translatedLanguageId);
                });
            } else {
                context.addMessage(event.value, sourceMessage, translatedMessage, sourceLanguageId, translatedLanguageId);
            }
        }
    };
    context.showTranslatedText = false;
    context.updateTextarea = function () {
        const el = $(context.outputSelector);
        let str = '';
        for (let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i]
            let messageKey = (message.type == MESSAGE_TYPE.recieved) ? 'translatedMessage' : 'sourceMessage';
            let hiddenTextKey = (message.type == MESSAGE_TYPE.recieved) ? 'sourceMessage' : 'translatedMessage';
            if (context.showTranslatedText) {
                const temp = hiddenTextKey;
                hiddenTextKey = messageKey;
                messageKey = temp;
            }
            if (message.wasTranslated && message.type == MESSAGE_TYPE.sent) {
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

        el.html(str);    }

    $(fromLanguageSelectorId).select2({
        data: languages
    });
    context.fromLanguageId = 1;
    let languageInfo =  languages.find(x => x.id == context.fromLanguageId);
    context.keyboard = initKeyboard(languageInfo, inputSelector, keyboardContainer)
    $(fromLanguageSelectorId).val(context.fromLanguageId);
    $(fromLanguageSelectorId).on('change', function(event) {
        context.fromLanguageId = $(fromLanguageSelectorId).val();
        languageInfo =  languages.find(x => x.id == context.fromLanguageId);
        $(context.keyboard).getkeyboard().destroy();
        context.keyboard =  initKeyboard(languageInfo, context.inputSelector, context.keyboardContainer);
    })
    $(fromLanguageSelectorId).trigger('change');


     $(toLanguageSelectorId).select2({
        data: languages
    });
    context.toLanguageId = 1;
    languageInfo =  languages.find(x => x.id == context.toLanguageId);
    $(toLanguageSelectorId).val(context.toLanguageId);
    $(toLanguageSelectorId).on('change', function(event) {
        context.toLanguageId = $(toLanguageSelectorId).val();
    })
    $(toLanguageSelectorId).trigger('change');
}


function initTextTutor(chatDefinitions) {
    contexts = []
    index = 0;
    chatDefinitions.forEach(element => {
        contexts.push(
        initTranslator(
            element.placeholderSelector,
            '#' + element.userInputId,
            '#' + element.userOutputId,
            '#' + element.keyboardContainerId, 
            '#' + element.fromLanguageSelectorId,
            '#' + element.toLanguageSelectorId,
            '#' + element.clearButtonId,
            '#' + element.translateButtonId
        ));
        index++;
    })
    return contexts;
}