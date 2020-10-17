//#Active buttons
$(document).ready(function () {
    CreateActiveAction();
});

function CreateActiveAction(element) {
    var elements = [];
    if (element === undefined || element === null || element.length <= 0) {
        $("body").find("a[data-model].activable").each(function() {
            elements.push($(this));
        });
    } else {
        elements.push(element);
    }
    for (let i = 0; i < elements.length; i++) {
        const $a = elements[i];
        $a.on("click", function (e) {
            e.preventDefault();
            const $this = $(this);
            const modelName = $this.attr("data-model");
            const key = $this.attr("data-key");
            if (modelName === undefined || key === undefined) return;

            $.post("/api/activable/index", { modelName, key }, function (data) {
                if (data === true) {
                    showMessage("Registo ativado com sucesso.", 1);
                    $this
                        .removeClass("deactivated")
                        .addClass("activated")
                        .find(".fa-toggle-on").removeClass("fa-rotate-180");
                } else if (data === false) {
                    showMessage("Registo inativado com sucesso.", 1);
                    $this
                        .removeClass("activated")
                        .addClass("deactivated")
                        .find(".fa-toggle-on").removeClass("fa-rotate-180").addClass("fa-rotate-180");
                } else {
                    location.href = location.href;
                }
            });
        });
    }
}

//#Active buttons