
$(document).ready(function () {


    //加载上传控件
    ReloadUpload();

});



//选择省市区以后的回调函数
function SelectAreaCallBack(areaId) {
    $("#AreaId").val("" + areaId + "");
}

//设置Ueditor
function setUeditor() {
    var myEditor = document.getElementById("Content");
    myEditor.value = editor.getContent();

    alert(myEditor.value);
}

//选择一级分类，加载二级分类
function SelectMettingType() {
    var html = "<option selected=\"selected\" value=\"0\">请选择</option>";
    $("#MeetingTypeId").html(html);

    var parentId = $("#MeetingTypeParentId").val();
    if (parentId == null || parentId == "" || parentId == "0")
        return;

    $.ajax({
        async: false,
        type: "POST",
        url: "/Meeting/GetMettingTypeList",
        data: "parentId=" + parentId,
        dataType: "json",
        success: function (result) {
            if (result == null || result == "")
                return;

            for (var i = 0; i < result.length; i++) {
                html += "<option  value=\"" + result[i].Value + "\">" + result[i].Text + "</option>";
            }

            $("#MeetingTypeId").html(html);
        }
    });
}

function SelectPrice(type) {
    $(".price-item").removeClass("active");
    $("." + type).addClass("active");

    if (type == "price-item-yes") {
        $(".price-item-price").css("display", "block");
        $("#Type").val("1");
    } else {
        $(".price-item-price").css("display", "none");
        $("#Price").val("0");
        $("#Type").val("2");
    }
}

//加载上传控件
function ReloadUpload() {
    $("#uploadify").uploadify({
        swf: '/Content/Scripts/uploadify/uploadify.swf',//上传的Flash，不用管，路径对就行
        uploader: '/Meeting/UploadFile',//Post文件到指定的处理文件
        auto: true,
        buttonClass: 'posterimg_add_btn',//浏览按钮的class
        buttonText: '添加海报',//浏览按钮的Text
        cancelImage: '/Content/Scripts/uploadify/uploadify-cancel.png',//取消按钮的图片地址
        fileTypeDesc: '*.jpg;*.jpeg;*.png;*.bmp',//需过滤文件类型
        fileTypeExts: '*.jpg;*.jpeg;*.png;*.bmp',//需过滤文件类型的提示
        fileSizeLimit: 2048, //2M
        //		    height: 28,//浏览按钮高
        //		    width:52,//浏览按钮宽
        multi: false,//是否允许多文件上传
        uploadLimit: 999,//同时上传多小个文件
        queueSizeLimit: 999,//队列允许的文件总数
        removeCompleted: true,//当上传成功后是否将该Item删除
        onSelect: function (file) { },//选择文件时触发事件
        onSelectError: function (file, errorCode, errorMsg) { },//选择文件有误触发事件
        onUploadComplete: function (file) { },//上传成功触发事件
        onUploadError: function (file, errorCode, errorMsg) { },//上传失败触发事件
        onUploadProgress: function (file, fileBytesLoaded, fileTotalBytes) { },//上传中触发事件
        onUploadStart: function (file) { },//上传开始触发事件
        onUploadSuccess: function (event, response, status) { AddDocItem($.parseJSON(response)); }  //当单个文件上传成功后激发的事件

    });
}

//上传图片成功以后调用
function AddDocItem(item) {

    if (!item.IsSuccess && item.ErrorMessage != "") {
        MessageWindow(item.ErrorMessage);
        return;
    }

    $("#CoverImage").val(item.FilePath);
    var html = "<a href='" + item.FilePath + "' target='_blank'><img class='posterImg_pic image-div' src='" + item.FilePath + "' ></a>";
    $(".meeting-cover-image").html(html);
}

