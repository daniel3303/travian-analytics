function Confirm(text, functionYes) {
    if (text === undefined || text === null || text === "")
        text = "Deseja continuar?";
    $("body").find("#modalConfirm").find(".modal-text").text(text);
    $("body").find("#modalConfirm").modal({ backdrop: "static" });
    $(document).unbind("keyup").keyup(function (e) {
        const code = e.which;
        if (code === 13 || code === 32) {
            $("body").find("#modalConfirm").find(".btn-primary").trigger("click");
            $(document).unbind("keyup");
        } else if (code === 27) {
            $("body").find("#modalConfirm").find(".btn-outline-primary").trigger("click");
            $(document).unbind("keyup");
        }
    });
    $("body")
        .find("#modalConfirm")
        .find(".btn-primary")
        .unbind("click")
        .on("click", functionYes)
        .on("click", function () { $("body").find("#modalConfirm").modal("hide") })
        .trigger("focus");
}

$(document).ready(function () {
    $(".form-confirm").on("submit", function (event) {
        if ($(this).hasClass("form-confirm")) {
            event.preventDefault();
            event.stopPropagation();
            const $element = $(this);

            Confirm($element.data("message"), function () {
                $element.removeClass("form-confirm");
                $element.submit();
            });
            return false;
        }
    });

    $(".btn-confirm").on("submit", function (event) {
        if ($(this).hasClass("btn-confirm")) {
            event.preventDefault();
            event.stopPropagation();
            const $element = $(this);

            Confirm($element.data("message"), function () {
                $element.removeClass("btn-confirm");
                $element.trigger("click");
            });
            return false;
        }
    });
});