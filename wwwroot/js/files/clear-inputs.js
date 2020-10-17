$(document).ready(function() {
    $(".clear-input").on("click", function() {
        $(this).siblings("input").val("");
    });
    $(".clear-select").on("click", function () {
        $(this).parent().find("select").val("").trigger("change");
    });
});