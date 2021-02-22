(function ($) {
    var chatTemplate = '\
<div class="modal fade" id="<%= phrasebookModalId %>" tabindex="-1" role="dialog" aria-labelledby="<%= phrasebookLabel %>" aria-hidden="true">\
    <div class="modal-dialog modal-xs" role = "document" >\
        <div class="modal-content">\
            <div class="modal-header">\
                <h5 class="modal-title" id="<%= phrasebookLabel %>">Phrasebook</h5>\
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                    <span aria-hidden="true">&times;</span>\
                </button>\
            </div>\
            <div class="modal-body">\
                <div id="<%= phrasebookTreeId %>"></div>\
            </div>\
            <div class="modal-footer">\
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>\
            </div>\
        </div>\
    </div >\
</div >\
';

    $.fn.phrasebook = function(chatDefinition) {
         return this.each(function() {
            $(this).append(
                _.template(chatTemplate)(chatDefinition)
            );
        });
    }
})($)
