window.chatInputHandleTab = function (element, dotNetHelper) {
    element.addEventListener('keydown', function (e) {
        if (e.key === 'Tab' && element.dataset.autocomplete === 'true') {
            e.preventDefault();
            if (dotNetHelper) {
                dotNetHelper.invokeMethodAsync('AcceptAutocomplete');
            }
        }
    });
};
