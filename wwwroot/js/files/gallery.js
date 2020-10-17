//#Gallery
$(document).ready(function () {
    CreateGallery();
});
function CreateGallery(element) {
    var elements = [];
    if (element === undefined || element === null || element.length <= 0) {
        $("body").find(".gallery").each(function () {
            elements.push($(this));
        });
    } else {
        elements.push(element);
    }

    for (let i = 0; i < elements.length; i++) {
        const $gallery = elements[i];

        sortable($gallery.find(".galleryImages"),
            {
                forcePlaceholderSize: true,
                placeholder: "<div class=\"col-12 col-sm-4 col-md-3 col-xl-2 mb-3 mb-3\"><div class=\"w-100 h-100 bg-grey\"></div></div>",
                itemSerializer: (serializedItem) => {
                    return $(serializedItem.node).data("id");
                }
            }
        )[0].addEventListener('sortupdate', function (e) {
            const items = sortable($(e.target), "serialize")[0].items;
            $.ajax({
                url: "/admin/api/sortable",
                method: "POST",
                data: { dbset: "Images", items: items }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR);
            });
        });

        $gallery.find("button.galleryDeleteButton").each(function (el) { $(el).unbind("click") });
        $gallery.find("button.galleryEditButton").each(function (el) { $(el).unbind("click") });

        $gallery.find("input.galleryUploadInput").on("change", function (e) {
            if (this.files !== undefined && this.files !== null) {
                const uploadFiles = [];
                const files = this.files;
                const arrayLength = this.files.length;
                for (let j = 0; j < arrayLength; j++) {
                    uploadFiles.push(files[j]);
                }
                GalleryUploadFiles($(e.target).parent(), uploadFiles);
                $(e.target).val("");
            }
        });

        $gallery.find("button.galleryUploadButton").on("click", function (e) {
            e.preventDefault();
            $(e.target).siblings("input.galleryUploadInput").trigger("click");
            return false;
        });

        $gallery.on("click", "button.galleryDeleteButton", function (e) {
            e.stopPropagation();
            e.preventDefault();
            const $image = $(e.target).parentsUntil(".galleryImageContainer").parent();
            const id = $image.data("id");
            Confirm("Deseja apagar a imagem?",
                function () {
                    $.ajax({
                        method: "POST",
                        url: "/admin/api/images/gallerydelete",
                        data: { imageid: id }
                    }).done(function (data) {
                        $image.remove();
                        showMessage("Imagem apagada com sucesso.", 1);
                    }).fail(function (data) {
                        $image.remove();
                        showMessage("Não foi possível apagar a imagem!", -1);
                    });
                }
            );

            return false;
        });

        $gallery.on("click", "button.galleryEditButton", function (e) {
            e.stopPropagation();
            e.preventDefault();
            const $image = $(e.target).parentsUntil(".galleryImageContainer").parent();
            const id = $image.data("id");
            $.ajax({
                method: "GET",
                url: `/admin/api/images/galleryedit/${id}`
            }).done(function (data) {
                dynamicModal(data,
                    function () {
                        const formValues = $("body").find("#GalleryEdit").serializeArray();
                        formValues.push({ name: "id", value: id });
                        $.ajax({
                            method: "POST",
                            contentType: "application/x-www-form-urlencoded",
                            url: "/admin/api/images/galleryedit",
                            data: formValues
                        }).done(function () {
                            showMessage("Dados de imagem atualizados com sucesso", 1);
                        }).fail(function () {
                            showMessage("Não foi possível atualizar os dados da imagem", -1);
                        });
                    },
                    undefined,
                    "Gravar",
                    "Cancelar");
                CreateLocalizableInputTab();
            });
            return false;
        });
    }
}

function GalleryUploadFiles($gallery, files) {
    const galleryId = $gallery.data("gallery");

    const progressBarTemplate = $gallery.find(".progress-template").clone().html();

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const formdata = new FormData();
        formdata.append("galleryId", galleryId);
        formdata.append("image", file);

        const ajax = new XMLHttpRequest();
        ajax.upload.index = i;

        const $progressBar = $(progressBarTemplate);
        $($progressBar).insertAfter($gallery.find(".galleryUploadInput"));

        ajax.upload.progressBar = $progressBar;
        ajax.upload.gallery = $gallery;
        ajax.upload.addEventListener("progress", progressHandler, false);
        ajax.addEventListener("load", completeHandler, false);
        ajax.addEventListener("error", errorHandler);
        ajax.open("POST", "/admin/api/images/galleryupload");
        ajax.send(formdata);
    }
    return false;
}

function progressHandler(event) {
    const $progressBar = $(event.currentTarget.progressBar).find(".progress-bar");
    var percent = event.loaded / event.total * 100;
    if (percent >= 100) {
        percent = 99;
    }
    const percentText = Math.round(percent) + "%";
    $progressBar.text(percentText);
    $progressBar.css("width", percent * 100);
}

function completeHandler(event) {
    const $progressBar = event.currentTarget.upload.progressBar;
    const $gallery = event.currentTarget.upload.gallery;
    const status = event.currentTarget.status;
    if (status >= 400) {
        errorHandler(event);
    } else {
        const resposta = event.currentTarget.response;
        $gallery.find(".galleryImages").prepend(resposta);
        $progressBar.remove();
    }
}
function errorHandler(event) {
    showMessage("Não foi possível inserir a imagem", -1);
    $(event.currentTarget.upload.progressBar).remove();
}
//#Gallery