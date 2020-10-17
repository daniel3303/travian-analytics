//# Remove accents
function removeAccents(str) {
    const accents = "ÀÁÂÃÄÅàáâãäåÒÓÔÕÕÖØòóôõöøÈÉÊËèéêëðÇçÐÌÍÎÏìíîïÙÚÛÜùúûüÑñŠšŸÿýŽž";
    const accentsOut = "AAAAAAaaaaaaOOOOOOOooooooEEEEeeeeeCcDIIIIiiiiUUUUuuuuNnSsYyyZz";
    str = str.split("");
    const strLen = str.length;
    var i, x;
    for (i = 0; i < strLen; i++) {
        if ((x = accents.indexOf(str[i])) !== -1) {
            str[i] = accentsOut[x];
        }
    }
    return str.join("");
}
//# Remove accents

//# Search all words
function matchAllWords(params, data) {
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === "") {
        return data;
    }
    // Do not display the item if there is no 'text' property
    if (typeof data.text === "undefined") {
        return null;
    }
    const textSplitted = params.term.split(" ");
    for (j = 0; j < textSplitted.length; j++) {
        textSplitted[j] = removeAccents(textSplitted[j]).trim();
    }
    var matchesAll = true;
    for (let i = 0; i < textSplitted.length; i++) {
        const termo = textSplitted[i];
        if (termo !== "") {
            if (removeAccents(data.text.toUpperCase()).indexOf(termo.toUpperCase()) < 0) {
                matchesAll = false;
            }
        }
    }
    if (matchesAll) {
        const modifiedData = $.extend({}, data, true);
        return modifiedData;
    }
    // Return `null` if the term should not be displayed
    return null;
}
//# Search all words

//# Pad numbers
function pad(str, max, char) {
    str = str.toString();
    return str.length < max ? pad(`${char}${str}`, max) : str;
}
//# Pad numbers

//# Format date
function formatDate(date) {
    return pad(date.getDate(), 2, "0") + "/" + pad((date.getMonth() + 1), 2, "0") + "/" + date.getFullYear();
}
//# Format date

//# Format date
function formatCurrency(double) {
    return double.toFixed(2).replace(",", ".") + "€";
}
//# Format date

