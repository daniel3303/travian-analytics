$(document).ready(function () {
    $(".checkbox-all").on("change", function () {
        var dataCheckbox = $(this).data("checkbox");
        var checked = $(this).prop("checked")
        if (dataCheckbox != undefined && dataCheckbox != "") {
            $(`input.checkbox-all-target[data-checkbox="${dataCheckbox}"]`).each(function () {
                $(this).prop("checked", checked).trigger("change");
            })
        }
    });
    $("input.checkbox-all-target").on("change", function () {
        var dataCheckbox = $(this).data("checkbox");
        if (dataCheckbox != undefined && dataCheckbox != "") {
            var allChecked = true;
            $(`input.checkbox-all-target[data-checkbox="${dataCheckbox}"]`).each(function () {
                if (!$(this).prop("checked")) {
                    allChecked = false;
                    return false;
                }
            });
            $(`.checkbox-all[data-checkbox="${dataCheckbox}"]`).prop("checked", allChecked);
        }
    });

    $(".checkbox-all").each(function () {
        var dataCheckbox = $(this).data("checkbox");
        if (dataCheckbox != undefined && dataCheckbox != "") {
            var allChecked = true;
            $(`input.checkbox-all-target[data-checkbox="${dataCheckbox}"]`).each(function () {
                if (!$(this).prop("checked")) {
                    allChecked = false;
                    return false;
                }
            });
            $(`.checkbox-all[data-checkbox="${dataCheckbox}"]`).prop("checked", allChecked);
        }
    });

});