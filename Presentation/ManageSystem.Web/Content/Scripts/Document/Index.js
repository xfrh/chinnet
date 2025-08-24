
function Download(obj) {

    var id = $(obj).attr("data-id");
    var name = $(obj).attr("data-name");
    var url = $(obj).attr("data-url");

    $.ajax({
        type: "POST",
        url: "/Document/Download",
        data: {
            Id: id
        },
        dataType: "json",
        success: function (result) {

        }
    });

    location.href = url;

}
