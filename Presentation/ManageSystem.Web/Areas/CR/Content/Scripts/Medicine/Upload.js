var intervalResult;

$(document).ready(function () {
    if ($('#HiddenFaultiness').val() === '1') {
        layer.alert('请先完善个人信息', { icon: 0, title: '提示', closeBtn: 0 }, function () {
            window.location.href = '/Center';
        });
        return;
    }
    initFileInput("FilePath");

    $(".file-drop-zone-title").html("请选择文件");
    $(".hidden-xs").html("选择文件");

    $(".kv-upload-progress").css("display", "none");
});

//初始化fileinput控件（第一次初始化）
function initFileInput(ctrlName) {
    var control = $('#' + ctrlName);
    var projectType = "";

    control.fileinput({
        language: 'zh', //设置语言
        uploadUrl: "/Medicine/UploadFile", //上传的地址jpg，jpeg，gif，bmp，png，txt，doc，xdoc，xls，xlsx
        allowedFileExtensions: ['xlsx', 'xls', 'dbf'],//接收的文件后缀
        showUpload: false, //是否显示上传按钮
        showCaption: true,//是否显示标题
        uploadAsync: true,
        maxFileCount: 10,
        multiple: false,
        maxFileSize: 1024 * 70,//单位为kb，如果为0表示不限制文件大小，70M
        enctype: 'multipart/form-data',
        browseClass: "btn btn-primary", //按钮样式
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        uploadExtraData: function (previewId, index) {   //额外参数的关键点
            var obj = {};
            obj.projectType = $("#ProjectType").val();
            return obj;
        },
    }).on("filebatchselected", function (event, files) {
        $(this).fileinput("upload");
    }).on('filecleared', function (event) {
        $("#UploadFilePath").val("");
    }).on("fileuploaded", function (e, data) {
        UploadFileCallback(data.response);
    });
}

//上传图片成功以后调用
function UploadFileCallback(item) {

    $("#UploadFilePath").val("");
    $("#OldeFileName").val("");
    $("#OldeFileSize").val("");

    $(".file-upload-message").html("");

    if (!item.IsSuccess && item.ErrorMessage !== "") {
        MessageWindow(item.ErrorMessage);
        return;
    }

    var html = "数据文件已加载，请点击“开始上传”，<a href='" + item.FilePath + "' target='_blank' style='color: #428bca;'>" + item.FileName + "</a>";
    $("#UploadFilePath").val(item.FilePath);
    $("#OldeFileName").val(item.FileName);
    $("#OldeFileSize").val(item.FileSize);
    $(".file-upload-message").html(html);
}

function Save() {
    var l = Ladda.create(document.getElementById('btnBEPost'));
    var year = $("#Year").val();
    if (year === null || year === "" || year === "0") {
        MessageWindow("请选择上报数据所属年度");
        return;
    }

    var quarter = $("#Quarter").val();
    if (quarter === null || quarter === "" || quarter === "0") {
        MessageWindow("请选择上报数据所属季度");
        return;
    }

    var email = $("#Email").val();
    if (email === null || email === "" || email === "0") {
        MessageWindow("请输入您的邮箱地址");
        return;
    }

    var projectType = $("#ProjectType").val();
    if (projectType === null || projectType === "" || projectType === "0") {
        MessageWindow("请选择数据所属项目");
        return;
    }

    var method = $("#BacteriaIds").val();
    //if (method == null || method == "" || method == "0") {
    //    MessageWindow("请选择药敏实验方法");
    //    return;
    //}

    var file = $("#UploadFilePath").val();
    if (file === null || file === "") {
        MessageWindow("请上传数据文件");
        return false;
    }

    //选择的所属医院
    var hospitalId = $("#HospitalId").val();
    var hospital = $("#Hospital").val();
    if (!hospital || hospital === "" || hospitalId <= 0)
        hospitalId = 0;

    if (l.isLoading()) { return false; }
    l.start();

    $.ajax({
        url: "/Medicine/Upload",
        type: "Post",
        dataType: "json",
        data: {
            Year: year,
            Quarter: quarter,
            BacteriaIds: method,
            FilePath: file,
            FileName: $("#OldeFileName").val(),
            FileSize: $("#OldeFileSize").val(),
            Email: email,
            ProjectType: projectType,
            HospitalId: hospitalId
        },
        complete: function () {
            l.stop();
        },
        success: function (json) {

            if (json === null || json === "") {
                MessageWindow("操作失败，请刷新重试");
                $(".btn-save").css("display", "inline-block");
                $(".btn-loading").css("display", "none");
                return;
            }

            if (json.Status) {
                $(".btn-save").css("display", "none");

                $("#UploadMedicineId").val(json.Message);

                layer.open({
                    closeBtn: 0, //不显示关闭按钮
                    type: 1,
                    title: '数据上传成功提醒',
                    area: ['620px', '230px'], //宽高
                    content: '<div><p  style="font-size: 14px; text-align: center;margin: 40px 0px;    margin-bottom: 50px;"> 数据已上传完毕，系统正在处理中，您可关闭当前页面，数据处理完之后会发送邮件通知您。 </p> <p style="text-align: center;">  <input type="button" id="MessageBtn" onclick="CloseMessageWindow()"    class="btn_Post btn-save" value="10秒后关闭" style="cursor:pointer; text-align: center;  font-size: 13px; cursor: pointer;margin: 0px;width: 120px; height: 35px;line-height: 35px; " /></p></div>'
                });

                //文件上传成功了，同时已通知接口处理，每隔1秒检查一次处理结果
                intervalResult = setInterval("UpdateMessageTime()", 1000);

            } else {
                ErrorWindow(json.Message);
                $(".btn-save").css("display", "inline-block");
                $(".btn-loading").css("display", "none");
            }
        },
        error: function (e) {
            ErrorWindow('数据处理请求超时');
            $(".btn-save").css("display", "inline-block");
            $(".btn-loading").css("display", "none");
        }
    });
}

//选择细菌类型
function SelectBacteriaType() {
    var li = $(".bacteria-type-item .chk-item ");
    if (li === null || li.length <= 0) return;

    var ids = new Array();
    for (var i = 0; i < li.length; i++) {

        if ($(li[i]).prop("checked") || $(li[i]).attr("checked") === "checked") {
            var itemId = $(li[i]).attr("ItemValue");

            if (itemId === null || itemId === "") continue;

            ids.push(itemId);
        }
    }

    var value = ids.join(",");
    $("#BacteriaIds").val(value);

    if (value === null || value === "")
        $(".bacteria-type-error").css("display", "block");
    else
        $(".bacteria-type-error").css("display", "none");
}

////开始自动化处理数据
//function StartAutoDispose() {

//    var id = $("#UploadMedicineId").val();
//    if (id == null || id == "") {
//        ErrorWindow("未获取到数据", true);
//        return;
//    }

//    $.ajax({
//        url: "/Medicine/AutoDispose",
//        type: "Post",
//        dataType: "json",
//        data: {
//            MedicalId: id,
//        },
//        success: function (json) {

//            if (json == null || json == "") {
//                MessageWindow("操作失败，请刷新重试");
//                return;
//            }
//        }
//    });

//}

////每隔20秒检查下数据的上传结果


//更新弹出的消息倒计时
var messageTime = 10;
function UpdateMessageTime() {
    if (messageTime <= 0) {
        //结束
        clearInterval(intervalResult);
        //location.href = "/";
        return;
    }

    $("#MessageBtn").val(messageTime + "秒后关闭");
    messageTime--;
}

//关闭弹窗
function CloseMessageWindow() {
    //location.href = "/";
}



$(function () {
    $("#Hospital").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Medicine/QueryHospital?query=" + request.term,
                dataType: "json",
                method: "get",
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 2,
        autoFocus: true,
        focus: function (event, ui) {

            return false;
        },
        select: function (event, ui) {
            $("#HospitalId").val(ui.item.value);
            $("#Hospital").val(ui.item.text);
            return false;
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li class='hos-li' data-id='" + item.value + "' data-text='" + item.text + "'> " + item.text + "</li>")
            .appendTo(ul);
    };
});