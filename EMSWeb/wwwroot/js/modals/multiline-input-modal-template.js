(function () {
let chatTemplate = '\
<div class="modal fade" id="<%=modalId%>" tabIndex="-1" role="dialog" aria-labelledby="myModalLabel">\
    <div class="modal-dialog" role="document">\
        <div class="modal-content">\
            <div class="modal-header">\
                <h5 class="modal-title">\
                    <%=modalTitle%></h5>\
            </div>\
            <div class="modal-body" style="max-height: 60vh">\
                <div>\
                    <div class="row" style="height: 250px; margin: 8px">\
                        <div class="col-md-12"><textarea id="<%=modalTextareaId%>"\
                                                             style="width: 100%; height: 100%; padding: 8px;"></textarea>\
                        </div>\
                    </div>\
                </div>\
            </div>\
            <div class="modal-footer">\
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>\
                <button type="button" class="btn btn-primary" id="<%=modalSaveId%>">Save changes</button>\
            </div>\
        </div>\
    </div>\
</div>\
'
    $.fn.multilineModal = function (chatDefinition) {
        return this.each(function () {
            $(this).append(
                _.template(chatTemplate)(chatDefinition)
            );
        });
    }
})($)