/*Alert messages*/
$(document).ready(function () {
    initMessages();
});
function initMessages() {
    if (!$("body").find(".alert-messages").length) {
        $("body").append("<div class=\"alert-messages\"></div>");
    }
}
function showMessage(text, type) {
    initMessages();
    var alertClass = "info";
    var iconClass = "info";
    if (type == 1 || type == "Success") {
        alertClass = "success";
        iconClass = "check";
    } else if (type == -1 || type == "Danger") {
        alertClass = "danger";
        iconClass = "exclamation";
    } else if (type == -2 || type == "Warning") {
        alertClass = "warning";
        iconClass = "exclamation";
    }

    const textSpan = document.createElement("span");
    const $textSpan = $(textSpan);

    $textSpan.text(text);
    $textSpan.prepend(`<i class="text-${alertClass} fas fa-${iconClass} mr-2"></i>`);

    const alertRow = document.createElement("DIV");
    const $alertRow = $(alertRow);
    $alertRow
        .addClass("row m-0")
        .on("click", function () {
            hideMessage($(this));
        });
    const alertDiv = document.createElement("DIV");
    const $alert = $(alertDiv);
    $alert
        .addClass(`bg-white cursor-pointer border-${alertClass} border rounded p-2 mb-2`)
        .attr("role", "alert")
        .append($textSpan);
    $alertRow.append($alert);
    $(".alert-messages").append($alertRow);

    setTimeout(hideMessage, 5000, $alertRow);
}
function hideMessage($element) {
    $element.addClass("hidding");
    $element.on("animationend", removeMessage);
    $element.on("webkitAnimationEnd", removeMessage);
    $element.on("mozAnimationEnd", removeMessage);
    setTimeout(removeMessage, 300, $element);
}
function removeMessage(element) {
    if ($(element).length)
        $(element).remove();
}
/*Alert messages*/