
$(document).ready(function () {
    initFileInput("FilePath");

    //$(".file-drop-zone-title").html("请选择文件");
    //$(".hidden-xs").html("选择文件");

    $(".kv-upload-progress").css("display", "none");
});

//初始化fileinput控件（第一次初始化）
function initFileInput(ctrlName) {
    var control = $('#' + ctrlName);
    var projectType = "";
    control.fileinput({
        language: 'zh', //设置语言
        uploadUrl: "/CRUpload/UploadFile", //上传的地址jpg，jpeg，gif，bmp，png，txt，doc，xdoc，xls，xlsx
        allowedFileExtensions: ['xlsx', 'xls'],//接收的文件后缀
        showUpload: false, //是否显示上传按钮
        showCaption: true,//是否显示标题
        uploadAsync: true,
        maxFileCount: 10,
        multiple: false,
        maxFileSize: 1024 * 50,//单位为kb，如果为0表示不限制文件大小，50M
        enctype: 'multipart/form-data',
        browseClass: "btn btn-primary", //按钮样式
        previewFileIcon: "<i class='glyphicon glyphicon-king'></i>",
        uploadExtraData: function (previewId, index) {   //额外参数的关键点          
            var obj = {};
            obj.projectId = parent.$("#ProjectId").val();
            return obj;
        },
    }).on("filebatchselected", function (event, files) {
        $("#btnBEPost").css({ 'background-color': 'gray' });
        $("#btnBEPost").attr("disabled", true);
        $(this).fileinput("upload");
    }).on('filecleared', function (event, data) {
        $("#UploadFilePath").val("");
    }).on("fileuploaded", function (e, data) {
        if (data.response.status == "true") {
            $("#btnBEPost").css({ 'background-color': '#0099e9' });
            $("#btnBEPost").attr("disabled", false);
            var value = data.response.message.split('|');
            //文件上传完成，自动提交数据
            $("#UploadFilePath").val(value[0]);
            $("#OldeFileName").val(value[1]);
            $("#UploadFilename").html(value[1]);
            return;
        } else {
            MessageWindow(data.response.ErrorMessage);
            return;
        }
    });
}

//上传图片成功以后调用
function UploadFileCallback() {

    var filePath = $("#UploadFilePath").val();
    if (filePath == "")
    {
        MessageWindow("请选择上传文件");
        return;
    }
    var fileName = $("#OldeFileName").val();
    var year1 = $("#Year").val();
    if (year1 == "") {
        MessageWindow("请选择年份");
        return;
    }
    var year = year1 - 1;
    var quarter = $("#Quarter").val();
    if (quarter == "")
    {
        MessageWindow("请选择季度");
        return;
    }
    var credetectionRate = $("#CREDetectionRate").val();
    if (isNaN(credetectionRate))
    {
        MessageWindow("CRE平均检出率必须是数字类型");
        return;
    }
    var credrugRate = $("#CREDrugRate").val();
    if (isNaN(credrugRate)) {
        MessageWindow("CRAB平均检出率必须是数字类型");
        return;
    }

    if (filePath == "" || fileName == "") {
        MessageWindow("上传文件失败，请重新选择文件");
        return;
    }

    $.ajax({
        url: "/CRUpload/Upload",
        type: "Post",
        dataType: "json",
        data: {
            FilePath: filePath,
            FileName: fileName,
            Year: year,
            Quarter: quarter,
            CREDetectionRate: credetectionRate,
            CREDrugRate: credrugRate,
        },
        success: function (json) {

            if (json === null || json === "") {
                MessageWindow("操作失败，请刷新重试");
                return;
            }

            if (json.status == "true") {
                SuccessWindow("上传成功", true);
            } else {
                ErrorWindow(json.message);
            }
        }
    });
}
