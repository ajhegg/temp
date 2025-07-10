// ChatInput.speech.js: Handles speech-to-text for ChatInput

export function startRecognition(textArea, dotNetHelper) {
    if (!('webkitSpeechRecognition' in window)) {
        alert('Speech recognition is not supported in this browser.');
        return;
    }
    const recognition = new window.webkitSpeechRecognition();
    recognition.lang = 'en-US';
    recognition.interimResults = false;
    recognition.maxAlternatives = 1;
    recognition.onresult = function(event) {
        const transcript = event.results[0][0].transcript;
        dotNetHelper.invokeMethodAsync('SetMessageText', transcript);
    };
    recognition.onerror = function(event) {
        alert('Speech recognition error: ' + event.error);
    };
    recognition.start();
}
