$(document).ready(function () {
    Backbone.TemplateManager.baseUrl = '{name}';
    var upManager = new Backbone.UploadManager({
        uploadUrl: url,
        autoUpload: true,
        templates: {
            main: urlTemplates + "main/",
            file: {
                name: urlTemplates + "files/",
                onUploadDone: function (data, el) {
                    $('div.fileLink', el).removeClass("hidden");
                    $('input.fileUrl', el).val(data.Url);
                    $("a.btn", el).zclip({
                        path: urlZClip,
                        afterCopy: function () {
                        },
                        copy: function () {
                            return $(this).prev().val();
                        }
                    });
                },
            }
        }
    });
    upManager.renderTo($('div#uploadSection'));
});

function clickIe(elem) {
    if (ie < 9) {
        $("#" + $(elem).attr("for")).click();
    }
}

