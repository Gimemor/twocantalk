(function ($) {
    var chatTemplate = '\
<div class="modal fade" id="<%= phrasebookModalId %>" tabindex="-1" role="dialog" aria-labelledby="<%= phrasebookLabel %>" aria-hidden="true">\
    <div class="modal-dialog modal-lg" role = "document" >\
        <div class="modal-content">\
            <div class="modal-header">\
                <h5 class="modal-title" id="<%= phrasebookLabel %>">Phrasebook</h5>\
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">\
                    <span aria-hidden="true">&times;</span>\
                </button>\
            </div>\
            <div class="modal-body">\
            <% if (showControlButton) {%>\
                <div class="row pb-8 pt-8">\
            <% } else { %>\
                <div class="row pb-8 pt-8" style="display: none">\
            <% } %>\
                    <div class="col-lg-12 d-flex flex-wrap">\
                        <div><button title="Add a category" type="button" class="btn btn-light" id="<%=addCategoryButtonId%>"><i class="fa fa-plus-square-o" aria-hidden="true"></i> Add Category</button></div>\
                        <div><button title="Add a new phrase" type="button" class="btn btn-light" id="<%=addPhraseButtonId%>"><i class="fa fa-folder-open" aria-hidden="true"></i> Add Phrase</button></div>\
                        <div><button title="Modify" type="button" class="btn btn-light" id="<%=modifyButtonId%>"><i class="fa fa-pencil-square-o"></i> Modify</button></div>\
                        <div><button title="Delete" type="button" class="btn btn-light" id="<%=deleteButtonId%>"><i class="fa fa-trash-o" aria-hidden="true"></i> Delete </button></div>\
                    </div>\
                </div>\
                <div class="row pb-8 pt-8" style="display: contents">\
                    <div class="list-container"><div id="<%= phrasebookTreeId %>"></div></div>\
                </div>\
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
