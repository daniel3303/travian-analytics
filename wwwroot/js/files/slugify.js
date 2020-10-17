//# Generate Slugs
function slugify(text) {
    return removeAccents(text.toString().toLowerCase())
        .replace(/\s+/g, "-")           // Replace spaces with -
        .replace(/[^\w\-]+/g, "")       // Remove all non-word chars
        .replace(/\-\-+/g, "-")         // Replace multiple - with single -
        .replace(/^-+/, "")             // Trim - from start of text
        .replace(/-+$/, "");            // Trim - from end of text
}

$(document).ready(function () {
    $(".slugify-btn").on("click", function () {
        const $target = $(this).siblings("input");
        const $source = $(`#${$(this).data("source").replace("[", "\\[").replace("]", "\\]")}`);
        $target.val(slugify($source.val()));
    });

    $(".slugify-source").on("blur", function () {
        const $source = $(this);
        const $target = $(`#${$(this).data("target").replace("[", "\\[").replace("]", "\\]")}`);
        if ($target.val() === "")
            $target.val(slugify($source.val()));
    });

    //$("[name^=Slug], [name^=slug], .slug").each(function (index, el) {
    //const $el = $(el);

    //$el.on("blur", (e) => e.target.value = slugify(e.target.value));

    //const source = $el.attr("data-source");
    //if (source !== undefined) {
    //	let $source = $(`[name=${source}]`);
    //	if ($source.length === 0) {
    //		const lang = $el.attr("name")
    //			.slice($el.attr("name").lastIndexOf('['), $el.attr("name").lastIndexOf(']') + 1)
    //			.replace("[", "\\[").replace("]", "\\]");
    //		$source = $(`[name=${source + lang}]`);
    //	}
    //	$source.on("blur", (e) => {
    //		if ($el.val() === null || $el.val().length <= 3) {
    //			$el.val(slugify(e.target.value));
    //		}
    //	});
    //}
    //});
});
//# Generate Slugs