//# Select2
$(document).ready(function () {
    CreateSelect2();
});
function CreateSelect2($element) {
    if ($element === undefined || $element === null)
        $element = $("form");
    if ($element.find("select.select2-element").length) {
        $element.find("select.select2-element").each(function () {
            var options = {
                language: "pt",
                placeholder: "",
                width: "resolve",
                allowClear: true,
                matcher: matchAllWords,
                dropdownParent: null
            };

            var $this = $(this);
            options.dropdownParent = $this.parent();

            if ($this.data("search") == false) {
                options.minimumResultsForSearch = -1;
            }
            if ($this.data("clear") == false)
                options.allowClear = false;

            if ($this.data("placeholder") != undefined && $this.data("placeholder") != "")
                options.placeholder = $this.data("placeholder");

            $this
                .select2(options)
                .on("select2:open", function (e) {
                    $(e.target).siblings(".select2-container").find(".select2-search__field").addClass("form-control");
                    if ($(e.target).data("search") == false) {
                        $(e.target).siblings(".select2-container").find(".select2-search").remove();
                    }
                }).on("select2:select", function (e) {
                    $(this).closest("form").validate().element(e.target);
                });

            $this.siblings(".select2").addClass("form-control");
        });
    }
}
//# Select2