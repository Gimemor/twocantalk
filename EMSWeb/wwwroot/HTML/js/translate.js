
function translate(text, fromIndex, toIndex, onSuccess) {
    const from = languages.find(x => x.id == fromIndex);
    const to = languages.find(x => x.id == toIndex);
    var url = "https://translation.googleapis.com/language/translate/v2?key=AIzaSyCgDWPzfAMHwS_Pt7GntBUeOCWIHptmOd4";
    url += "&source=" + from.language;
    url += "&target=" + to.language;
    url += "&format=text";
    url += "&q=" + encodeURIComponent(text);

    $.get(url, onSuccess);
}