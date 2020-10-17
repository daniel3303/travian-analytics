function dynamicModal(html, functionYes, functionNo, textYes, textNo) {
    if (textYes === undefined || textYes === null || textYes === "")
        textYes = "Sim";
    if (textNo === undefined || textNo === null || textNo === "")
        textNo = "Não";

    $("body").find("#modalDynamic").find(".modal-html").html(html);
    $("body").find("#modalDynamic").modal({ backdrop: "static" });
    $("body")
        .find("#modalDynamic")
        .find(".btn-primary")
        .text(textYes)
        .unbind("click")
        .on("click", functionYes)
        .on("click", function () { $("body").find("#modalDynamic").modal("hide") });

    $("body")
        .find("#modalDynamic")
        .find(".btn-outline-primary")
        .text(textNo)
        .unbind("click")
        .on("click", functionNo)
        .on("click", function () { $("body").find("#modalDynamic").modal("hide") });
}