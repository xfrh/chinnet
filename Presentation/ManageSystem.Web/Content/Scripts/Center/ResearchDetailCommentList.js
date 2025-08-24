
$(document).ready(function () {
    //加载会议评论
    GetComment();

});

//加载信息动态的评论
function GetComment() {

    $("#review_loading").css("display", "block");
    $(".comment-ul").html("");

    $.ajax({
        async: false,
        type: "POST",
        url: "/Research/GetResearchComment",
        data: {
            researchId: '' + $("#ResearchIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {

            $(".submit-btn").css("display", "block");

            if (json == null || json == "")
                return;

            if (json.status == "true") {
                //评论成功
                var data = JSON.parse(json.message);
                SetCommentItem(data);

                $("#review_loading").css("display", "none");
            }
            else {
                ErrorWindow(json.message, true);
            }
        }
    });
}

//加载一个个评论项
function SetCommentItem(data) {

    for (var i = 0; i < data.length; i++) {

        var html = "  <li class='comment-item  item-'" + data[i].IdString + "'' Comment='" + data[i].IdString + "' Nickname='" + data[i].MemberNickName + "'  style='padding-left:" + (data[i].Level * 50) + "px;' > " +
                           "        <div class='content' >" +
                           "                <div class='h-image' ><img style='width:60px; height:60px;' src='" + data[i].MemberHeadImage + "'> </div>" +
                           "                <div class='m-detail' > " +
                          "                       <p class='m-user' >" + data[i].MemberNickName + "</p> <div class='m-text'>" + data[i].Content + "</div>" +
                          "                        <div class='time'> " + data[i].InsertTimeString + " | <span onclick=\"OpenReplyItem(this)\">回复</span></div>" +
                          "                   </div>" +
                         "        </div>" +
                           "  </li>";

        $(".comment-ul").append(html);

        if (data[i].ItemCommentList != null && data[i].ItemCommentList != "") {
            SetCommentItem(data[i].ItemCommentList);
        }

    }
}

//打开回复评论窗口
function OpenReplyItem(obj) {
    $(".comment-reply").remove();

    var item = $(obj).closest(".comment-item");
    var box = $(obj).closest(".m-detail");
    var comment = $(item).attr("Comment");
    var nickname = $(item).attr("Nickname");

    var html = "  <div class='comment-reply' >" +
                    "  <div class='con-box'><textarea class='txt-" + comment + "' placeholder='回复：" + nickname + "'></textarea></div>" +
                    "    <div class='btn-box'><input Comment='" + comment + "'   type='button' class='btn' onclick=\"ReplyComment('" + comment + "')\"  value='回复评论'/>  </div>" +
                    " </div>";

    $(box).append(html);
}

//回复一个评论
function ReplyComment(parent) {
    var content = $(".txt-" + parent).val();
    SubmitComment(content, parent);
}

//提交评论,content 评论内容，parent 上级评论
function SubmitComment(content, parent) {

    var nameReg = /\S{3,}/;
    if (content == "" || !nameReg.test(content)) {
        MessageWindow("评论长度必须大于2位！");
        return;
    }

    var isHaveLogin = $("#IsHavelogin").val();
    if (isHaveLogin == null || isHaveLogin != "1") {
        ErrorWindow("请登录！", false, "/Home/login");
        return;
    }

    $(".submit-btn").css("display", "none");

    $.ajax({
        async: false,
        type: "POST",
        url: "/Research/AddComment",
        data: {
            Content: content,
            ParentId: parent,
            researchId: '' + $("#ResearchIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {

            $(".submit-btn").css("display", "block");

            if (json == null || json == "") {
                ErrorWindow("提交评论失败，请重试！", true);
                return;
            }

            if (json.status == "true") {
                //评论成功
                MessageWindow("评论成功");

                GetComment();
            }
            else {
                ErrorWindow(json.message, true);
            }
        }
    });

}
