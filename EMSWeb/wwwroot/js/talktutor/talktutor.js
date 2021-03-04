
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

function initTranslator(
    sceneId,
    placeholderSelector,
    sceneRef,
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
    context.sceneRef = sceneRef;
    context.sceneId = sceneId;
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
        $(inputSelector).val('');
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent, languageId: context.languageId };
        context.handleConnection({
            type: 'message',
            value: Object.assign({}, message)
        })
    };
    $(clearButtonId).on('click', () => {
        $(inputSelector).val('');
        $(outputSelector).val('');
    });
    $(translateButtonId).on('click', context.inputHandler);

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
    context.sayText = function(text, languageId) {
        selectScene(context.sceneRef);
        sayText(text, 1, languageId, 2, 'D', 1);
    }
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
            let sourceLanguageId = context.fromLanguageId;
            let translatedLanguageId = context.toLanguageId;
            translate(event.value.text, context.fromLanguageId, context.toLanguageId, function (data, status) {
                translatedText = event.value.text = data.data.translations[0].translatedText;
                context.addMessage(event.value, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            });
            if(sourceLanguageId !== translatedLanguageId) { 
                translate(event.value.text, context.fromLanguageId, context.toLanguageId, function (data, status) {
                    translatedText = event.value.text = data.data.translations[0].translatedText;
                    context.sayText(translatedText, translatedLanguageId);
                    context.addMessage(event.value, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
                });
            } else {
                context.sayText(translatedText, translatedLanguageId);
                context.addMessage(event.value, sourceText, translatedText, sourceLanguageId, translatedLanguageId);
            }
            
        }
    };
    context.showTranslatedText = true;
    context.updateTextarea = function () {
        $(context.inputSelector).val(context.messages[context.messages.length - 1].sourceMessage);
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
    return context;
}


function initTalkTutor(chatDefinitions) {
    contexts = []
    index = 0;
    chatDefinitions.forEach(element => {
        contexts.push(
        initTranslator(index,
            element.placeholderSelector,
            element.scene,
            '#' + element.userInputId,
            '#' + element.userOutputId,
            '#' + element.keyboardContainerId, 
            '#' + element.fromLanguageSelectorId,
            '#' + element.toLanguageSelectorId,
            '#' + element.clearButtonId,
            '#' + element.translateButtonId,
        ));
        index++;
    })
    return contexts;
}