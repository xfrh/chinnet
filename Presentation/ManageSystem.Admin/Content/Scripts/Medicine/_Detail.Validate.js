
$(document).ready(function () {
    LayPageValidate(0);
})


//加载数据验证的列表数据
function LayPageValidate(curr) {

    $.ajax({
        type: 'POST',
        url: "/Medicine/GetValidateList",
        dataType: 'json',
        data: {
            DataId: $("#DataId").val(),
            Level: $("#ValidateLevelValue").val(),
            Page: curr || 1,  //向服务端传的参数，此处只是演示
            PageSize: 100//每页10行
        },
        success: function (result) {

            //绑定页面的数据
            if (result.rows != null && result.rows.length > 0) {
                var html = "";
                var index = 0;
                for (var i = 0; i < result.rows.length; i++) {
                    var item = result.rows[i];

                    var className = "";
                    index++;

                    switch (item.Level) {
                        case 1:
                            className = "error-data-info  info";
                            break;
                        case 2:
                            className = "error-data-warning  warning";
                            break;
                        case 3:
                            className = "error-data-danger  danger";
                            break;
                    }

                    html += "    <tr class='error-data-item " + className + "'>" +
                        "<td style='text-align:center;'>" + index + "</td>" +
                        "<td style='text-align:center;'>" + item.UploadRowIndex + "</td>" +
                        "<td>" + item.LevelName + "</td>" +
                        "<td  class='left'>" + item.KeyName + "</td>" +
                        "<td  class='left'>" + item.Content + "</td>" +
                        "</tr>";
                }
                $("#validateDataTbody").html(html);
            } else {
                $("#validateDataTbody").html("");
            }

            //显示分页
            laypage({
                cont: "divIncomeIntegral", //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                pages: result.total, //通过后台拿到的总页数
                curr: curr || 1, //当前页
                skip: true, //是否开启跳页
                skin: '#AF0000',
                groups: 7, //连续显示分页数
                jump: function (obj, first) { //触发分页后的回调
                    if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                        LayPageValidate(obj.curr);
                    }
                }
            });
        },
    });
}

//显示全部的错误提示
function ShowErrorAll() {
    $(".error-data-item").css("display", "table-row");
    $("#ValidateLevelValue").val("0");
    LayPageValidate(0);
}

//仅显示提醒
function ShowErrorInfo() {
    $(".error-data-item").css("display", "none");
    $(".error-data-info").css("display", "table-row");
    $("#ValidateLevelValue").val("1");
    LayPageValidate(0);
}

//仅显示警告
function ShowErrorWarning() {
    $(".error-data-item").css("display", "none");
    $(".error-data-warning").css("display", "table-row");
    $("#ValidateLevelValue").val("2");
    LayPageValidate(0);
}

//仅显示错误
function ShowErrorDanger() {
    $(".error-data-item").css("display", "none");
    $(".error-data-danger").css("display", "table-row");
    $("#ValidateLevelValue").val("3");
    LayPageValidate(0);
}
