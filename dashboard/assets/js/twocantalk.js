
let MESSAGE_TYPE = {
    sent: 0,
    recieved: 1
}

let chatContexts = [];
let loadedScenes = []
function vh_sceneLoaded(sceneIndex, sceneRef) {
    loadedScenes.push(sceneIndex);
}

function initChat(sceneId, sceneRef, inputSelector, keyboardContainer, chatHistoryId, toggleVoiceId) {
    let context = {};
    context.inputSelector = inputSelector;
    context.keyboardContainer = keyboardContainer;
    context.chatHistoryId = chatHistoryId;
    context.toggleVoiceId = toggleVoiceId;
    context.sceneId = sceneId;
    context.sceneRef = sceneRef;
    context.sceneLoaded = false;
    context.messages = [];
    context.isVoiceEnabled = $(context.toggleVoiceId).is(":checked");
    $(context.toggleVoiceId).on('change', function() {
        context.isVoiceEnabled = $(context.toggleVoiceId).is(":checked");
    });
    context.keyboard = $(inputSelector).keyboard({
        change: (e, keyboard, el) => {
            // Dispatch input event to notify angular about changes
            const inputEvent = document.createEvent('CustomEvent');
            inputEvent.initEvent('input', true, true);
            el.dispatchEvent(inputEvent);
        },
        position: {
            // null = attach to input/textarea;
            // use $(sel) to attach elsewhere
            of: $(keyboardContainer),
            my: 'center top',
            at: 'center top',
            // used when "usePreview" is false
            at2: 'center bottom'
        },
        alwaysOpen: true,
        // Set this to append the keyboard immediately after the
        // input/textarea it is attached to. This option works
        // best when the input container doesn't have a set width
        // and when the "tabNavigation" option is true
        appendLocally: false,
        usePreview: false,
        // When appendLocally is false, the keyboard will be appended
        // to this object
        appendTo: $(keyboardContainer),
        css: {
            // input & preview
            input: 'ui-widget-content ui-corner-all',
            // keyboard container
            container: 'ui-widget-content ui-widget ui-corner-all ui-helper-clearfix dom',
            // keyboard container extra class (same as container, but separate)
            popup: '',
            // default state
            buttonDefault: 'ui-state-default ui-corner-all',
            // hovered button
            buttonHover: 'ui-state-hover',
            // Action keys (e.g. Accept, Cancel, Tab, etc);
            // this replaces "actionClass" option
            buttonAction: 'ui-state-active',
            // Active keys
            // (e.g. shift down, meta keyset active, combo keys active)
            buttonActive: 'ui-state-active',
            // used when disabling the decimal button {dec}
            // when a decimal exists in the input area
            buttonDisabled: 'ui-state-disabled',
            // {empty} button class name
            buttonEmpty: 'ui-keyboard-empty'
        }
    });
    context.oldInputValue = null;
    $(inputSelector).on('change', function(event) {
        
        let value = $(inputSelector).val()
        if(!value || value.length <= 0 || value == context.oldInputValue) {
            return;
        }
        context.oldInputValue = value;
        console.log(value);
        let message = { text: value, time: new Date(), type: MESSAGE_TYPE.sent };
        context.addMessage(message);
        if(context.sendToConnected) {
            context.sendToConnected({
                type: 'message',
                value: message
            })
        }
    });
    context.addMessage = function(message) {
        context.messages.push(message);
        context.messages.sort(function (a, b) {return a.time > b.time? 1 : -1});
        context.updateTextarea();
        
    }
    context.handleConnection = function(event) {
        if(event.type == 'message') {
            event.value.type = (event.value.type == MESSAGE_TYPE.sent)? MESSAGE_TYPE.recieved : MESSAGE_TYPE.sent
            context.addMessage(event.value);
            if(context.isVoiceEnabled) {
                selectScene(context.sceneRef);
                sayText(event.value.text, 1,1,1,'S',-2);
            }
        }
    };
    context.updateTextarea = function () {
        console.log('updating chat')
        const el = $(context.chatHistoryId);
        let str = '';
        for(let i = 0; i < context.messages.length; i++) {
            const message = context.messages[i]
            str += `${message['time'].toLocaleTimeString()} > ${message['text']}\n`;
        }
        el.val(str);
    }
    return context;
}

function connectChats(firstContext, secondContext) {
    firstContext.sendToConnected = function(event) {
        secondContext.handleConnection(event);
    }
    secondContext.sendToConnected = function(event) {
        firstContext.handleConnection(event);
    }
}

function initTct() {
    let firstContext = initChat(0, scene1, '#firstUserInput', '#firstKeyboardContainer', '#firstChatHistory', '#firstToggleVoice');
    let secondContext = initChat(1, scene2, '#secondUserInput', '#secondKeyboardContainer', '#secondChatHistory', '#secondToggleVoice');
    connectChats(firstContext, secondContext);
}