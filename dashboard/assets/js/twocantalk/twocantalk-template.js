
var scene1; 
var chatTemplate = '\
   <div class="p-8 pt-0">\
        <div class="row logo-content">\
            <div class="col-lg-6">\
            </div>\
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12">\
                <script type="text/javascript"><%=embedAvatarFunction()%></script>\
            </div>\
        </div>\
         <div class="row pb-8">\
            <div class="col-lg-12 d-flex flex-wrap">\
                <div><button title="Repeat last translated phrase" type="button" class="btn btn-light" id="<%=repeatTranslatedButtonId%>"><i class="fa fa-play-circle-o" aria-hidden="true"></i></button></div>\
                <div><button title="Repeat last voiced phrase" type="button" class="btn btn-light" id="<%=repeatVoicedButtonId%>"><i class="fa fa-step-backward" aria-hidden="true"></i></button></div>\
                <div><button title="Export to PDF" type="button" class="btn btn-light" id="<%=exportToPdfButtonId%>"> <i class="fa fa-file-pdf-o" aria-hidden="true"></i></button></div>\
                <div><button title="Toggle reverse translation" type="button" class="btn btn-light" data-toggle="button" aria-pressed="false" autocomplete="off"  id="<%=toggleTranslationButtonId%>"><i class="fa fa-repeat" aria-hidden="true"></i></button></div>\
                \
                <div class="languageSelectorContainer">\
                    <select class="languageSelector" id="<%=languageSelectorId%>">\
                    </select>\
                </div>\
            </div>\
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12 chat-history-container">\
                <table class="chat-history table" id="<%=chatHistoryId%>">\
                    <tbody>\
                        <tr></tr>\
                    </tbody>\
                </table>\
            </div>\
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12">\
                <input id="<%=userInputId%>" class="chat-input">\
            </div> \
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