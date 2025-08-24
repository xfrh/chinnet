
$(document).ready(function () {
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
        uploadUrl: "/CRProject/UploadFile", //上传的地址jpg，jpeg，gif，bmp，png，txt，doc，xdoc，xls，xlsx
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
            console.log(obj);
            return obj;
        },
    }).on("filebatchselected", function (event, files) {
        $(this).fileinput("upload");
    }).on('filecleared', function (event) {
        $("#UploadFilePath").val("");
    }).on("fileuploaded", function (e, data) {

        if (data.response.status == "true") {
            //文件上传完成，自动提交数据
            UploadFileCallback(data.response);
            return;
        } else {
            MessageWindow(data.response.ErrorMessage);
            return;
        }
    });
}

//上传图片成功以后调用
function UploadFileCallback(item) {
    
    var value = item.message.split('|');
    var filePath = value[0];
    var fileName = value[1];

    if (filePath == "" || fileName == "") {
        MessageWindow("上传文件失败，请重新选择文件");
        return;
    }

    $.ajax({
        url: "/CRProject/Upload",
        type: "Post",
        dataType: "json",
        data: {
            FilePath: filePath,
            FileName: fileName,
        },
        success: function (json) {

            if (json === null || json === "") {
                MessageWindow("操作失败，请刷新重试");
                return;
            }

            if (json.status == "true") {
                MessageWindow("上传成功！");
            } else {
                ErrorWindow(json.message);
            }
        }
    });
}
