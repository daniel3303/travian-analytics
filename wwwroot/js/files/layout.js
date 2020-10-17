$(document).ready(function () {
    //#Hide ScrollBar Width
    const scrollbarWith = getScrollBarWidth();
    $(".sidebar-container").css("padding-right", scrollbarWith + "px");
    $(".sidebar").css("right", -scrollbarWith + "px");
    //#Hide ScrollBar Width

    //#Sidebar Toggle
    $(".sidebar-toggle").on("click",
        function () {
            $("body").toggleClass("sidebar-toggle");
        });
    //#Sidebar Toggle

    //#Sidebar Toggle
    $(".sidebar-overlay").on("click",
        function () {
            $("body").toggleClass("sidebar-toggle");
        });
    //#Sidebar Toggle
});

//#Get ScrollBar Width
function getScrollBarWidth() {
    const inner = document.createElement("p");
    inner.style.width = "100%";
    inner.style.height = "200px";

    const outer = document.createElement("div");
    outer.style.position = "absolute";
    outer.style.top = "0px";
    outer.style.left = "0px";
    outer.style.visibility = "hidden";
    outer.style.width = "200px";
    outer.style.height = "150px";
    outer.style.overflow = "hidden";
    outer.appendChild(inner);

    document.body.appendChild(outer);
    const w1 = inner.offsetWidth;
    outer.style.overflow = "scroll";
    var w2 = inner.offsetWidth;
    if (w1 == w2) w2 = outer.clientWidth;

    document.body.removeChild(outer);

    return (w1 - w2);
}
//#Get ScrollBar Width