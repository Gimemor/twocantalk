
(function ($) {
    var chatTemplate = '\
   <div class="p-8 pt-0">\
        <div class="row logo-content">\
            <div class="col-lg-6">\
            </div>\
        </div>\
         <div class="row pb-8 pt-8">\
            <div class="col-lg-12 d-flex flex-wrap">\
                <div><button title="Repeat last translated phrase" type="button" class="btn btn-light" id="<%=repeatTranslatedButtonId%>"><i class="fa fa-play-circle-o" aria-hidden="true"></i></button></div>\
                <div><button title="Edit last phrase" type="button" class="btn btn-light" id="<%=repeatLastSentenceButtonId%>"><i class="fa fa-pencil-square-o" aria-hidden="true"></i></button></div>\
                <div><button title="Export to PDF" type="button" class="btn btn-light" id="<%=exportToPdfButtonId%>"> <i class="fa fa-file-pdf-o" aria-hidden="true"></i></button></div>\
                <div><button title="Toggle reverse translation" type="button" class="btn btn-light" data-toggle="button" aria-pressed="false" autocomplete="off"  id="<%=toggleTranslationButtonId%>"><i class="fa fa-repeat" aria-hidden="true"></i></button></div>\
                <div><button title="Hide english keyboard" type="button" class="btn btn-light" data-toggle="button" aria-pressed="false" autocomplete="off"  id="<%=toggleKeyboardId%>"><i class="fa fa-keyboard-o" aria-hidden="true"></i></button></div>\
                <% if (!!phrasebook && !phrasebook.hidePhrasebook) { %>\
                    <div class="pl-8">\
                        <button title = "Open pharsebook" type = "button" class="btn btn-light" id="<%=phrasebook.openPhrasebookButtonId%>" >\
                            <i class="fa fa-book" aria-hidden="true"></i> Phrasebook\
                        </button >\
                    </div >\
                <% } %>\
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
                 <textarea id="<%=userInputId%>" class="chat-input"></textarea>\
            </div> \
        </div>\
        <div class="row pb-8">\
            <div class="col-lg-12 keyboard-container" id="<%=keyboardContainerId%>">\
            </div>\
        </div>\
    </div>\
';



    $.fn.twocantalk = function (chatDefinition) {
        return this.each(function() {
            $(this).append(
                _.template(chatTemplate)(chatDefinition)
            );
        });
    }

})(jQuery);