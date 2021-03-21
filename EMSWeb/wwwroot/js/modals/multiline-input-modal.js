class MultilineInputModal {
    constructor(chatDefinition) {
        this.viewDefinition = chatDefinition;

        $(this.modalSaveId).on('click', () => {
            this.submit.next($(this.modalTextareaId).val());
            $(this.modalId).modal('hide');
        });
            
    }
    get modalId() { return '#' + this.viewDefinition.modalId; }
    get modalSaveId() { return '#' + this.viewDefinition.modalSaveId; }
    get modalTextareaId() { return '#' + this.viewDefinition.modalTextareaId; }
    submit = new rxjs.Subject();
    
    showModal(text) {
        $(this.modalTextareaId).val(text);
        $(this.modalId).modal({});
        return this.submit;
    }
}

function initMultilineInput(chatDefinition) {
    return new MultilineInputModal(chatDefinition);
}