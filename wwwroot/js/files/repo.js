//#Repo
$(document).ready(function () {
    CreateRepo();
});
function CreateRepo(element) {
    var elements = [];
    if (element === undefined || element === null || element.length <= 0) {
        $("body").find(".repo").each(function () {
            elements.push($(this));
        });
    } else {
        elements.push(element);
    }

    for (let i = 0; i < elements.length; i++) {
        const $repo = elements[i];

        sortable($repo.find(".repoFiles"),
            {
                handle: ".sort-handle",
                forcePlaceholderSize: true,
                placeholder: "<div class=\"col-12 mb-3\"><div class=\"w-100 h-100 bg-grey\"></div></div>",
                itemSerializer: (serializedItem) => {
                    return $(serializedItem.node).data("id");
                }
            }
        )[0].addEventListener('sortupdate', function (e) {
            const items = sortable($(e.target), "serialize")[0].items;
            const model = $(e.target).find(".sort-handle").data("model");
            $.ajax({
                url: "/api/sortable",
                method: "POST",
                data: { modelName: model, items: items }
            }).fail(function (jqXHR, textStatus) {
                console.log(jqXHR);
            });
        });

        $repo.find("button.repoDeleteButton").each(function (el) { $(el).unbind("click") });
        $repo.find("button.repoEditButton").each(function (el) { $(el).unbind("click") });

        $repo.find("input.repoUploadInput").on("change", function (e) {
            if (this.files !== undefined && this.files !== null) {
                const uploadFiles = [];
                const files = this.files;
                const arrayLength = this.files.length;
                for (let j = 0; j < arrayLength; j++) {
                    uploadFiles.push(files[j]);
                }
                RepoUploadFiles($(e.target).parent(), uploadFiles);
                $(e.target).val("");
            }
        });

        $repo.find("button.repoUploadButton").on("click", function (e) {
            e.preventDefault();
            $(e.target).siblings("input.repoUploadInput").trigger("click");
            return false;
        });

        $repo.on("click", "button.repoDeleteButton", function (e) {
            e.stopPropagation();
            e.preventDefault();
            const $file = $(e.target).parentsUntil(".repoFileContainer").parent();
            const id = $file.data("id");
            Confirm("Deseja apagar o ficheiro?",
                function () {
                    $.ajax({
                        method: "POST",
                        url: "/api/files/filedelete",
                        data: { fileid: id }
                    }).done(function (data) {
                        $file.remove();
                        showMessage("Ficheiro eliminado com sucesso.", 1);
                    }).fail(function (data) {
                        $file.remove();
                        showMessage("Não foi possível apagar o ficheiro.", -1);
                    });
                }
            );

            return false;
        });

    }
}

function RepoUploadFiles($repo, files) {
    const repoId = $repo.data("repo");

    const progressBarTemplate = $repo.find(".progress-template").clone().html();

    for (let i = 0; i < files.length; i++) {
        const file = files[i];

        const formdata = new FormData();
        formdata.append("repoId", repoId);
        formdata.append("file", file);

        const ajax = new XMLHttpRequest();
        ajax.upload.index = i;

        const $progressBar = $(progressBarTemplate);
        $($progressBar).insertAfter($repo.find(".repoUploadInput"));

        ajax.upload.progressBar = $progressBar;
        ajax.upload.repo = $repo;
        ajax.upload.addEventListener("progress", progressHandler, false);
        ajax.addEventListener("load", completeHandler, false);
        ajax.addEventListener("error", errorHandler);
        ajax.open("POST", "/api/files/fileupload");
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
    const $repo = event.currentTarget.upload.repo;
    const status = event.currentTarget.status;
    if (status >= 400) {
        errorHandler(event);
    } else {
        const resposta = event.currentTarget.response;
        $repo.find(".repoFiles").prepend(resposta);
        $progressBar.remove();
    }
}
function errorHandler(event) {
    var message = "Não foi possível inserir o ficheiro.";
    if (event.currentTarget.status == 400) {
        message = event.currentTarget.response;
    }
    showMessage(message, -1);
    $(event.currentTarget.upload.progressBar).remove();
}
//#Repo