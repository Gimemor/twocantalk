
var scene1; 
var chatTemplate = '\
   <div class="p-8 pt-0 flex-column d-flex" style="width:100%">\
         <div class="row pb-8">\
            <div class="col-lg-12 d-flex flex-wrap" style="justify-content: space-between">\
                \
                <div class="fromLanguageSelectorContainer">\
                    <label for="<%=fromLanguageSelectorId%>">From: </label>\
                    <select class="languageSelector" id="<%=fromLanguageSelectorId%>">\
                    </select>\
                </div>\
                <div class="toLanguageSelectorContainer">\
                    <label for="<%=toLanguageSelectorId%>">To: </label>\
                    <select class="languageSelector" id="<%=toLanguageSelectorId%>">\
                    </select>\
                </div>\
            </div>\
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12 d-flex flex-wrap">\
                  <div><button title="Clear" type="button" class="btn btn-light" id="<%=clearButtonId%>"><i class="fa fa-ban" aria-hidden="true"></i> Clear </button></div>\
                  <div><button title="Translate" type="button" class="btn btn-primary" id="<%=translateButtonId%>"><i class="fa fa-play" aria-hidden="true"></i> Translate </button></div>\
            </div> \
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12">\
                <textarea id="<%=userInputId%>" class="chat-input textarea"></textarea>\
            </div> \
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12 chat-history-container">\
                <table class="chat-history table" id="<%=userOutputId%>">\
                    <tbody>\
                        <tr></tr>\
                    </tbody>\
                </table>\
            </div>\
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12 keyboard-container" id="<%=keyboardContainerId%>">\
            </div>\
        </div>\
    </div>\
';



function createChat(chatDefinitions) {
    chatDefinitions.forEach(chatDefinition => {
        $(chatDefinition.placeholderSelector).append(
            _.template(chatTemplate)(chatDefinition)
        );
    });
}