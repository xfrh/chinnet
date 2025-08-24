
$(document).ready(function () {
    LayPageData(0);
})

function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
var pag = 0;
//加载数据验证的列表数据
function LayPageData(curr) {
    var Median = $("#Median").val();
    var Id = getQueryString("Id");

    var createdId = getQueryString("createdId");
    console.log("creatid:" + createdId + "id:" + Id);

    var rd = document.getElementsByName('rdCheck');
    var check = "";
    if (rd[0].checked) {
        check = "1";
    }
    if (rd[1].checked) {
        check = "0";
    }

    $.ajax({
        type: 'POST',
        url: "/DataInput/OLDataInput/GetDataList",
        dataType: 'json',
        data: {
            number: $('#search_txt').val(),
            ischeck: check,
            projectid: Id,
            createdid: '5505190829689418611',
            Page: curr || 1,  //向服务端传的参数，此处只是演示
            PageSize: 15//每页10行
        },
        success: function (result) {
            $("#DataCount").val(result.rows.length);
            //绑定页面的数据
            if (result.rows != null && result.rows.length > 0) {
                var html = "";
                for (var i = 0; i < result.rows.length; i++) {
                    var item = result.rows[i];
                    var id = result.ids[i];
                    var value = item.datevalue.split(',');
                    var td = "";
                    if (value.length > 0) {
                        for (var j = 0; j < value.length; j++) {
                            /*if (value[j] != "") {*/
                                td += " <td style='text-align:center;'>" + value[j] + "</td>"
                            //}
                        }
                    }
                    if (item.auditor == "") {
                        console.log(1);
                        html += "<tr style='text-align: center; background-color: rgb(255, 255, 255); height: 38px; color: rgb(0, 0, 0);'>" +
                            "<td><label for=data-item-" + item.Id + "><input class='' id=data-item-" + id + " type='checkbox' onclick='monitorCheck()' name='data-item' value='" + id + "'/><span class='text'></span></label></td>" +
                            //"<td style='text-align:center;'>" + item.project_name + "</td>" +
                            //"<td style='text-align:center;'>" + item.experimenter + "</td>" +
                            //"<td style='text-align:center;'>" + item.experimenttime + "</td>" +
                            "<td style='text-align:center;'>" + item.jobnumber + "</td>" +
                            // "<td style='text-align:center;'>" + item.germnumber + "</td>" +
                            "<td style='text-align:center;'>" + item.germname + "</td>" +
                            td + "<td style='text-align:center;'>" + item.experimenter + "</td>" +
                            "<td style='text-align:center;'>" + item.experimenttime + "</td>" + "<td style='text-align:center;'><a  class='opera_btn' onclick=examines('" + id + "')>审核</a> <a  class='opera_btn' href='/DataInput/OLDataInput/UpdateOLDataInput?Id=" + id + "&&proId=" + Id + "' >编辑</a> <a  class='opera_btn' onclick=deldata('" + id + "')>删除</a></td>" +
                            "</tr>";
                    } else {
                        console.log(2);
                        if (Median != "" && Median != null && Median != "True") {
                            html += "  <tr style='text-align: center; background-color: rgb(255, 255, 255); height: 38px; color: rgb(0, 0, 0);'>" +
                                "<td><label for=data-item-" + item.Id + "><input class='' id=data-item-" + id + " type='checkbox' onclick='monitorCheck()' name='data-item' value='" + id + "'/><span class='text'></span></label></td>" +
                                //"<td style='text-align:center;'>" + item.project_name + "</td>" +
                                //"<td style='text-align:center;'>" + item.experimenter + "</td>" +
                                //"<td style='text-align:center;'>" + item.experimenttime + "</td>" +
                                "<td style='text-align:center;'>" + item.jobnumber + "</td>" +
                                //"<td style='text-align:center;'>" + item.germnumber + "</td>" +
                                "<td style='text-align:center;'>" + item.germname + "</td>" +
                                td + "<td style='text-align:center;'>" + item.experimenter + "</td>" +
                                "<td style='text-align:center;'>" + item.experimenttime + "</td>" + "<td style='text-align:center;'>已审核</td>" +
                                "</tr>";
                        } else {
                            console.log(3);
                            html += "  <tr style='text-align: center; background-color: rgb(255, 255, 255); height: 38px; color: rgb(0, 0, 0);'>" +
                                "<td><label for=data-item-" + item.Id + "><input class='' id=data-item-" + id + " type='checkbox' onclick='monitorCheck()' name='data-item' value='" + id + "'/><span class='text'></span></label></td>" +
                                //"<td style='text-align:center;'>" + item.project_name + "</td>" +
                                //"<td style='text-align:center;'>" + item.experimenter + "</td>" +
                                //"<td style='text-align:center;'>" + item.experimenttime + "</td>" +
                                "<td style='text-align:center;'>" + item.jobnumber + "</td>" +
                                //"<td style='text-align:center;'>" + item.germnumber + "</td>" +
                                "<td style='text-align:center;'>" + item.germname + "</td>" +
                                td + "<td style='text-align:center;'>" + item.experimenter + "</td>" +
                                "<td style='text-align:center;'>" + item.experimenttime + "</td>" + "<td style='text-align:center;'>已审核 <a  class='opera_btn' href='/DataInput/OLDataInput/UpdateOLDataInput?Id=" + id + "&&proId=" + Id + "' >编辑</a> <a  class='opera_btn' onclick=deldata('" + id + "')>删除</a></td>" +
                                "</tr>";
                        }
                    }
                }
                $("#detailDataBody").html(html);
                //$("#spn").html("项目名称：" + item.project_name + "");
            } else {
                $("#detailDataBody").html("");
            }

            if (plsh == 1)
            {
                $("[name='data-items']").prop("checked", true);
                $("[name='data-item']").prop("checked", true);
                plsh = 0;
            }

            //显示分页
            laypage({
                cont: "detailDataPager", //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                pages: result.total, //通过后台拿到的总页数
                curr: curr || 1, //当前页
                skip: true, //是否开启跳页
                //skin: '#AF0000',
                skin: '#337ab7',
                groups: 7, //连续显示分页数
                jump: function (obj, first) { //触发分页后的回调
                    if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                        pag = obj.curr;
                        LayPageData(obj.curr);
                    }
                }
            });
        },
    });
}

function Search() {
    LayPageData(0);
}

function deldata(id) {
    if (id) {

        //询问框
        layer.confirm('您确认要删除该数据吗？', {
            title: '系统提示',
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                async: false,
                type: "POST",
                url: "/DataInput/OLDataInput/deleteData",
                data: { Id: id },
                dataType: "json",
                success: function (json) {
                    if (json == null || json == "") {
                        return false;
                    }

                    if (json.status == "true") {
                        MessageWindow("删除成功");
                        LayPageData(pag);
                    }
                    else {
                        ErrorWindow(json.message, true);
                    }
                }
            });
        });
    }
    return false;
}

function examines(id) {
    $.ajax({
        async: false,
        type: "POST",
        url: "/DataInput/OLDataInput/Examine",
        data: { Id: id },
        dataType: "json",
        success: function (json) {
            if (json == null || json == "") {
                return false;
            }

            if (json.status == "true") {
                MessageWindow("审核成功！");
                LayPageData(pag);
            }
            else {
                ErrorWindow(json.message, true);
            }
        }
    });
}

var plsh = 0;

function examine() {
    var ids = "";
    $("input[name='data-item']:checked").each(function () {
        ids += $(this).val() + ","
    });
    if (ids == "" || ids.length <= 0) {
        MessageWindow("请选择要审核的数据");
    }
    else {
        $.ajax({
            async: false,
            type: "POST",
            url: "/DataInput/OLDataInput/Examine",
            data: { Id: ids },
            dataType: "json",
            success: function (json) {
                if (json == null || json == "") {
                    return false;
                }

                if (json.status == "true") {
                    MessageWindow("审核成功");
                    var stringArr = ids.split(",");
                    console.log(stringArr);
                    if (stringArr.length == 16) {
                        if (pag == 0)
                            pag = 2;
                        else
                            pag++;
                        LayPageData(pag);
                        plsh = 1;
                    }
                    else {
                        LayPageData(pag);
                        $("[name='data-items']").prop("checked", false)
                    }
                }
                else {
                    ErrorWindow(json.message, true);
                }
            }
        });
    }
}
function deldatas() {
    var ids = "";
    $("input[name='data-item']:checked").each(function () {
        ids += $(this).val() + ","
    });
    if (ids == "" || ids.length <= 0) {
        MessageWindow("请选择要删除的数据");
    }

    if (ids) {
        $.ajax({
            async: false,
            type: "POST",
            url: "/DataInput/OLDataInput/deleteCheck",
            data: { Id: ids },
            dataType: "json",
            success: function (json) {
                if (json == null || json == "") {
                    return false;
                }

                if (json.status == "true") {
                    //询问框
                    layer.confirm('您确认要删除所选数据吗？', {
                        title: '系统提示',
                        btn: ['确认', '取消'] //按钮
                    }, function () {
                        $.ajax({
                            async: false,
                            type: "POST",
                            url: "/DataInput/OLDataInput/deleteDatainput",
                            data: { Id: ids },
                            dataType: "json",
                            success: function (json) {
                                if (json == null || json == "") {
                                    return false;
                                }

                                if (json.status == "true") {
                                    MessageWindow("删除成功");
                                    LayPageData(pag);
                                    $("[name='data-items']").prop("checked", false)
                                }
                                else {
                                    ErrorWindow(json.message, true);
                                }
                            }
                        });
                    });
                }
                else {
                    ErrorWindow(json.message, true);
                }
            }
        });
    }
    return false;
}

//监听单个checkbox选择时，全选框
function monitorCheck() {
    var DataCount = $("#DataCount").val();
    var i = 0;
    $("input[name='data-item']:checked").each(function () {
        i++;
    });
    if (i == DataCount) {
        $("[name='data-items']").prop("checked", true)
    } else {
        $("[name='data-items']").prop("checked", false)
    }
}