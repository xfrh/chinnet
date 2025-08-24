
$(document).ready(function () {

    $(".post_title_Party").attr("disabled", "disabled");

    //加载日志数据
    LogList();

    //加载明细数据
    LayPageItem(0);

});


//加载日志数据
function LogList() {

    var id = $("#ProjectId").val();

    $.ajax({
        type: 'POST',
        url: "/CRProject/LogList",
        dataType: 'json',
        data: {
            projectId: id
        },
        success: function (result) {

            //绑定页面的数据
            if (result != null && result.length > 0) {
                var html = "";
                var index = 0;
                for (var i = 0; i < result.length; i++) {
                    var item = result[i];
                    html += "<tr class='error-data-item'>" +
                        "<td>" + item.InsertTime + "</td>" +
                        "<td>" + item.UserinfoName + "</td>" +
                        "<td>" + item.Content + "</td>" +
                        "</tr>";
                }
                $("#LogContet").html(html);
            } else {
                $("#LogContet").html("");
            }
        },
    });
}

//加载明细数据
function LayPageItem(curr) {
    var id = $("#ProjectId").val();

    $.ajax({
        type: 'POST',
        url: "/CRProject/ItemList",
        dataType: 'json',
        data: {
            ProjectId: id,
            Page: curr || 1,  //向服务端传的参数，此处只是演示
            PageSize: 20//每页10行
        },
        success: function (result) {

            //绑定页面的数据
            if (result.rows != null && result.rows.length > 0) {
                var html = "";
                var index = 0;
                for (var i = 0; i < result.rows.length; i++) {
                    var item = result.rows[i];

                    html += "<tr class='error-data-item'>" +
                        "<td>" + item.Number + "</td>" +
                        "<td>" + item.Name + "</td>" +
                        "<td>" + item.MIC_Imipenem + "</td>" +
                        "<td>" + item.ImineNumber + "</td>" +
                        "<td>" + item.TegacyclineNumber + "</td>" +
                        "<td>" + item.BacteriostasisNumber + "</td>" +
                        "<td>" + item.RecheckNumber + "</td>" +
                        "</tr>";
                }
                $("#ItemContent").html(html);
            } else {
                $("#ItemContent").html("");
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
                        LayPageItem(obj.curr);
                    }
                }
            });
        },
    });
}