let languages = [
   {
      "text":"English",
      "id": 42,
      "voiceId": 1,
      "layout":"qwerty",
      "language":"en"
   },
   {
      "text":"English (United Kingdom)",
      "id":1,
      "layout":"ms-United Kingdom",
      "language":"en"
   },
   {
      "text":"Spanish (español)",
      "id":2,
      "layout":"ms-Spanish",
      "language":"es"
   },
   {
      "text":"German (Deutsch)",
      "id":3,
      "layout":"ms-German",
      "language":"de"
   },
   {
      "text":"French",
      "id":4,
      "layout":"french-azerty-1",
      "language":"fr"
   },
   {
      "text":"Portuguese (português)",
      "id":6,
      "layout":"portuguese-qwerty",
      "language":"pt"
   },
   {
      "text":"Italian (italiano)",
      "id":7,
      "layout":"ms-Italian",
      "language":"it"
   },
   {
      "text":"Greek (Ελληνικά)",
      "id":8,
      "layout":"ms-Greek",
      "language":"el"
   },
   {
      "text":"Swedish (svenska)",
      "id":9,
      "layout":"ms-Swedish",
      "language":"sv"
   },
   {
      "text":"Chinese Bopomofo IME",
      "id":10,
      "layout":"ms-Chinese Bopomofo IME",
      "language":"zh"
   },
   {
      "text":"Japanese Hiragana (平仮名)",
      "id":12,
      "layout":"ms-Japanese Hiragana",
      "language":"ja"
   },
   {
      "text":"Korean (韓國語)",
      "id":13,
      "layout":"ms-Korean",
      "language":"ko"
   },
   {
      "text":"Polish (214)",
      "id":14,
      "layout":"ms-Polish (214)",
      "language":"pl"
   },
   {
      "text":"Turkish F",
      "id":16,
      "layout":"ms-Turkish F",
      "language":"tr"
   },
   {
      "text":"Czech (český)",
      "id":18,
      "layout":"ms-Czech",
      "language":"cs"
   },
   {
      "text":"Danish (Dansk)",
      "id":19,
      "layout":"ms-Danish",
      "language":"da"
   },
   {
      "text":"Norwegian (norsk)",
      "id":20,
      "layout":"ms-Norwegian",
      "language":"no"
   },
   {
      "text":"Russian (русский)",
      "id":21,
      "layout":"ms-Russian",
      "language":"ru"
   },
   {
      "text":"Finnish",
      "id":23,
      "layout":"ms-Finnish",
      "language":"fi"
   },
   {
      "text":"Hindi (हिन्दी)",
      "id":24,
      "layout":"ms-Hindi Traditional",
      "language":"hi"
   },
   {
      "text":"Thai Kedmanee",
      "id":26,
      "layout":"ms-Thai Kedmanee",
      "language":"th"
   },
   {
      "text":"Arabic",
      "id":27,
      "layout":"arabic-azerty",
      "language":"ar"
   },
   {
      "text":"Hungarian (Magyar)",
      "id":29,
      "layout":"ms-Hungarian",
      "language":"hu"
   },
   {
      "text":"Romanian (Română)",
      "id":30,
      "layout":"ms-Romanian (Legacy)",
      "language":"ro"
   },
   {
      "text":"Slovak (slovenčina)",
      "id":37,
      "layout":"ms-Slovak",
      "language":"sk"
   },
   {
      "text":"Ukrainian",
      "id":40,
      "layout":"ms-Ukrainian",
      "language":"uk"
   },
   {
      "text":"Vietnamese",
      "id":41,
      "layout":"ms-Vietnamese",
      "language":"vi"
   },
   {
      "text":"Thai Pattachote",
      "id":43,
      "layout":"thai-qwerty",
      "language":"th"
   },
   {
      "text":"Japanese Katakana (片仮名)",
      "id":43,
      "layout":"japanese-kana",
      "language":"ja"
   },
   {
      "text":"Turkish Q",
      "id":44,
      "layout":"turkish-q",
      "language":"tr"
   },
   {
      "text":"Urdu",
      "id":45,
      "layout":"ms-Urdu",
      "language":"ur"
   },
   {
      "text":"Chinese",
      "id":46,
      "layout":"chinese",
      "language":"zh"
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

