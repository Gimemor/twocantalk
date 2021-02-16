
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
    toLanguageSelectorId) {

    let context = {};
    context.inputSelector = inputSelector;
    context.outputSelector = outputSelector;
    context.placeholderSelector = placeholderSelector;
    context.keyboardContainer = keyboardContainer;
    context.fromLanguageSelectorId = fromLanguageSelectorId;
    context.toLanguageSelectorId = toLanguageSelectorId;

    context.messages = [];
    $.keyboard.keyaction.enter = function( kb ) {
        // same as $.keyboard.keyaction.accept();
        
        return false;     // return false prevents further processing
    };
    context.inputHandler = function(event) {
        let value = $(inputSelector).val()
        if(!value || value.length <= 0) {
            return;
        }
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: context.fromLanguageId };
        context.handleConnection({
            type: 'message',
            value: Object.assign({}, message)
        })
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
    });
    
    context.addMessage = function(message, sourceText, translatedText, sourceLanguageId, translatedLanguageId) {
        message.sourceMessage = sourceText;
        message.translatedMessage = translatedText;
        message.sourceLanguageId = sourceLanguageId;
        message.translatedLanguageId = translatedLanguageId;
        
        context.messages.push(message);
        context.messages.sort(function (a, b) {return a.time > b.time? 1 : -1});
        context.updateTextarea();
    }
    context.handleConnection = function(event) {
        if(event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent)? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent;
            let sourceText = event.value.text;
            let translatedText = event.value.text;
            let sourceLanguageId = event.value.languageId;
            let translatedLanguageId = context.toLanguageId;
            if(sourceLanguageId !== translatedLanguageId) { 
                translate(event.value.text, context.fromLanguageId, context.toLanguageId, function (data, status) {
                    translatedText = event.value.text = data.data.translations[0].translatedText;
                    context.addMessage(event.value, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
                });
            } else {
                context.addMessage(event.value, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            }
        }
    };
    context.showTranslatedText = true;
    context.updateTextarea = function () {
        //$(context.inputSelector).val(context.messages[context.messages.length - 1].sourceMessage);
        $(context.outputSelector).val(context.messages[context.messages.length - 1].text);
    }

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
            '#' + element.toLanguageSelectorId
        ));
        index++;
    })
    return contexts;
}