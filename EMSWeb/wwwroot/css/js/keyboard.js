let languages = [
    {
        "text": "English",
        "id": 42,
        "layout": "qwerty",
        "language": "en",
        "oddcastLanguageId": 1,
        "oddcastMaleVoice": [2, 1],
        "oddcastFemaleVoice": [1, 1],
        "isSpoken": 1,
    },
    {
        "text": "English (United Kingdom)",
        "id": 1,
        "layout": "ms-United Kingdom",
        "language": "en",
        "isSpoken": 1,

    },
    {
        "text": "Spanish (español)",
        "id": 2,
        "layout": "ms-Spanish",
        "language": "es",
        "isSpoken": 1,

    },
    {
        "text": "German (Deutsch)",
        "id": 3,
        "layout": "ms-German",
        "language": "de",
        "isSpoken": 1,

    },
    {
        "text": "French",
        "id": 4,
        "layout": "french-azerty-1",
        "language": "fr",
        "isSpoken": 1,
    },
    {
        "text": "Portuguese (português)",
        "id": 6,
        "layout": "portuguese-qwerty",
        "language": "pt",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 3]
    },
    {
        "text": "Italian (italiano)",
        "id": 7,
        "layout": "ms-Italian",
        "language": "it",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 3]
    },
    {
        "text": "Greek (Ελληνικά)",
        "id": 8,
        "layout": "ms-Greek",
        "language": "el",
        "isSpoken": 1,
        "oddcastMaleVoice": [3, 2],
        "oddcastFemaleVoice": [2, 2]
    },
    {
        "text": "Swedish (svenska)",
        "id": 9,
        "layout": "ms-Swedish",
        "language": "sv",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]

    },
    {
        "text": "Chinese Bopomofo IME",
        "id": 10,
        "layout": "ms-Chinese Bopomofo IME",
        "language": "zh",
        "isSpoken": 1,
        "oddcastMaleVoice": [4, 3],
        "oddcastFemaleVoice": [1, 3]
    },
    {
        "text": "Japanese Hiragana (平仮名)",
        "id": 12,
        "layout": "ms-Japanese Hiragana",
        "language": "ja",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 3],
        "oddcastFemaleVoice": [3, 3]
    },
    {
        "text": "Korean (韓國語)",
        "id": 13,
        "layout": "ms-Korean",
        "language": "ko",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 3],
        "oddcastFemaleVoice": [1, 3]
    },
    {
        "text": "Polish (214)",
        "id": 14,
        "layout": "ms-Polish (214)",
        "language": "pl",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Turkish F",
        "id": 16,
        "layout": "ms-Turkish F",
        "language": "tr",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 2],
        "oddcastFemaleVoice": [2, 2]
    },
    {
        "text": "Czech (český)",
        "id": 18,
        "layout": "ms-Czech",
        "language": "cs",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 7], // ONLY ONE VOICE AVAILABLE
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Danish (Dansk)",
        "id": 19,
        "layout": "ms-Danish",
        "language": "da",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Norwegian (norsk)",
        "id": 20,
        "layout": "ms-Norwegian",
        "language": "no",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Russian (русский)",
        "id": 21,
        "layout": "ms-Russian",
        "language": "ru",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Finnish",
        "id": 23,
        "layout": "ms-Finnish",
        "language": "fi",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 2],
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Hindi (हिन्दी)",
        "id": 24,
        "layout": "ms-Hindi Traditional",
        "language": "hi",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 7],
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Thai Kedmanee",
        "id": 26,
        "layout": "ms-Thai Kedmanee",
        "language": "th",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 3],
        "oddcastFemaleVoice": [2, 3]
    },
    {
        "text": "Arabic",
        "id": 27,
        "layout": "arabic-azerty",
        "language": "ar",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 7],
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Hungarian (Magyar)",
        "id": 29,
        "layout": "ms-Hungarian",
        "language": "hu",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 7],
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Romanian (Română)",
        "id": 30,
        "layout": "ms-Romanian (Legacy)",
        "language": "ro",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 2], // ONLY ONE VOICE AVAILABLE
        "oddcastFemaleVoice": [1, 2]
    },
    {
        "text": "Slovak (slovenčina)",
        "id": 37,
        "layout": "ms-Slovak",
        "language": "sk",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 7],  // ONLY ONE VOICE AVAILABLE
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Ukrainian",
        "id": 40,
        "layout": "ms-Ukrainian",
        "language": "uk",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 7], // ONLY ONE VOICE AVAILABLE
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Vietnamese",
        "id": 41,
        "layout": "ms-Vietnamese",
        "language": "vi",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 7],
        "oddcastFemaleVoice": [1, 7]
    },
    {
        "text": "Thai Pattachote",
        "id": 43,
        "oddcastLanguageId": 26,
        "layout": "thai-qwerty",
        "language": "th",
        "isSpoken": 1,
        "oddcastMaleVoice": [1, 3],
        "oddcastFemaleVoice": [2, 3]
    },
    {
        "text": "Japanese Katakana (片仮名)",
        "id": 47,
        "oddcastLanguageId": 12,
        "layout": "japanese-kana",
        "language": "ja",
        "isSpoken": 1,
        "oddcastMaleVoice": [2, 3],
        "oddcastFemaleVoice": [3, 3]
    },
    {
        "text": "Turkish Q",
        "id": 44,
        "layout": "turkish-q",
        "language": "tr",
        "isSpoken": 1,
        "oddcastLanguageId": 16,
        "oddcastMaleVoice": [1, 2],
        "oddcastFemaleVoice": [2, 2]
    },
    {
        "text": "Urdu",
        "id": 45,
        "layout": "ms-Urdu",
        "language": "ur",
        "isSpoken": 0
    },
    {
        "text": "Chinese",
        "id": 46,
        "layout": "chinese",
        "language": "zh",
        "isSpoken": 1,
        "oddcastLanguageId": 10,
        "oddcastMaleVoice": [4, 3],
        "oddcastFemaleVoice": [1, 3]
    }
]

function initKeyboard(languageInfo, inputSelector, keyboardContainer) {

    return $(inputSelector).keyboard({
        position: {
            // null = attach to input/textarea;
            // use $(sel) to attach elsewhere
            of: $(keyboardContainer),
            my: 'center top',
            at: 'center top',
            // used when "usePreview" is false
            at2: 'center bottom'
        },
        language: [languageInfo.language],
        layout: languageInfo.layout,
        alwaysOpen: true,
        // Set this to append the keyboard immediately after the
        // input/textarea it is attached to. This option works
        // best when the input container doesn't have a set width
        // and when the "tabNavigation" option is true
        appendLocally: false,
        usePreview: false,
        autoAccept: true,
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
}

