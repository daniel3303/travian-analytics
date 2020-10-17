$(document).ready(function () {
    const options = {
        language: "pt",
        templates: {
            leftArrow: "<i class=\"fas fa-chevron-left\"></i>",
            rightArrow: "<i class=\"fas fa-chevron-right\"></i>"
        },
        format: "dd/mm/yyyy",
        startView: 0,
        startDate: "-99y",
        endDate: "+99y",
        clearBtn: true,
        autoclose: true
    };
    $(".datepicker-div").each(function () {
        const $this = $(this);
        if ($this.data("start") !== undefined) {
            options.startDate = $this.data("start");
        }
        if ($this.data("end") !== undefined) {
            options.endDate = $this.data("end");
        }
        if ($this.data("view") !== undefined) {
            options.startView = $this.data("view");
        }
        $this.datepicker(options).on("changeDate", function (e) {
            $(e.target).trigger("blur");
        });
    });
});