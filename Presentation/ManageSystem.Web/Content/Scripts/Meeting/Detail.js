
$(document).ready(function () {

    //加载用户报名信息
    GetApplyMemberData();

    //设置信息动态的查看记录
    SetMettingView();

    //加载会议评论
    GetComment();

});


// ********************************  评论相关   ********************************


//加载信息动态的评论
function GetComment() {

    $("#review_loading").css("display", "block");
    $(".comment-ul").html("");

    $.ajax({
        async: false,
        type: "POST",
        url: "/Meeting/GetMeetingComment",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
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

//新增一个评论
function AddComment() {
    var content = $(".dt_review_form_content").val();
    SubmitComment(content, 0);
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

    $(".submit-btn").css("display", "none");
    //  $("#review_loading").css("display", "block");

    $.ajax({
        async: false,
        type: "POST",
        url: "/Meeting/AddComment",
        data: {
            Content: content,
            ParentId: parent,
            MeetingId: '' + $("#MeetingIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {

            $(".submit-btn").css("display", "block");
            //  $("#review_loading").css("display", "none");

            if (json == null || json == "") {
                ErrorWindow("提交评论失败，请重试！", true);
                return;
            }

            if (json.status == "true") {
                //评论成功
                MessageWindow("评论成功");

                GetComment();

                //var data = JSON.parse(json.message);
                //var html = "  <li class='comment-item'> " +
                //                "        <div class='content' >" +
                //                "                <div class='h-image' ><img style='width:60px; height:60px;' src='" + data.Image + "'> </div>" +
                //                "                <div class='m-detail' >   <p class='m-user' >" + data.User + "</p> <div class='m-text'>" + comment + "</div></div>" +
                //                "        </div>" +
                //                "        <div class='time'> " + data.Time + " </div>" +
                //                "  </li>";

                //$(".comment-ul").prepend(html);
            }
            else {
                ErrorWindow(json.message, true);
            }
        }
    });

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


// ********************************  报名相关   ********************************


//加载用户报名信息
function GetApplyMemberData() {
    $("#apply_list_main").html("");

    $.ajax({
        type: "POST",
        url: "/Meeting/GetMettingApplyMember",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {
            if (json == null || json == "")
                return;

            var html = "";
            for (var i = 0; i < json.length; i++) {
                html += "<li  class='dt_review_main'>";
                html += "    <div class='dt_review_K'>";
                html += "          <div class='dt_guess_item_icon'>";
                html += "                 <a target='_blank' rel='nofollow' href='#'><img class='default_img' src='" + json[i].HeadImage + "'></a>";
                html += "          </div>";
                html += "          <div class='dt_guess_item_title dt_loadNew'>";
                html += "                <a target='_blank' rel='nofollow' href='#'>" + json[i].MemberNickName + "</a>";
                html += "          </div>";
                html += "          <p class='dt_guess_item_time' style='text-align:center;'>" + json[i].TimeValue + "</p>";
                html += "    </div>";
                html += "</li>";
            }

            $("#apply_list_main").html(html);
        }

    });
}

//打开申请报名的窗口
function OpenApplyWindow() {
    //页面层-佟丽娅
    layer.open({
        type: 1,
        title: false,
        closeBtn: 1,
        area: '730px',
        skin: 'layui-layer-nobg', //没有背景色
        shadeClose: true,
        content: $('.apply-window')
    });
}


//保存报名申请
function SaveApplyMeeting() {
    var name = $(".apply_name").val();
    var nameReg = /\S{2,}/;
    if (name == "" || !nameReg.test(name)) {
        alert("姓名格式不正确");
        return;
    }

    var phone = $(".apply_phone").val();
    var phoneReg = /^[1]{1}[0-9]{10}$/;
    if (phone == "" || !phoneReg.test(phone)) {
        alert("手机号码格式不正确");
        return;
    }

    var email = $(".apply_email").val();
    var emailReg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3}){1,2})$/;
    if (email == "" || !emailReg.test(email)) {
        alert("邮箱格式不正确");
        return;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: "/Meeting/MemberApply",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
            Name: '' + name + '',
            Phone: '' + phone + '',
            Email: '' + email + ''
        },
        dataType: "json",
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("申请错误，请重试！", true);
                return;
            }

            if (json.status == "true") {
                if (json.message != "") {
                    location.href = json.message;
                    return;
                } else {
                    SuccessWindow("报名成功！", true);
                }
            } else {
                ErrorWindow(json.message, true);
            }
        }

    });
}



// ********************************  其他   ********************************

//设置信息动态的查看记录
function SetMettingView() {
    $.ajax({
        type: "POST",
        url: "/Meeting/AddMettingView",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {
            if (json == null || json == "")
                return;
        }
    });
}

//收藏信息动态的查看记录
function SetMettingCollect() {

    $.ajax({
        type: "POST",
        url: "/Meeting/AddMettingCollect",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {
            if (json === null || json === "")
                return;
            if (json.status || json.status === "true") {

                MessageWindow("收藏成功！");

                //var count = $("#dt_like_count").html();
                //count = (count == null || count == "") ? 1 : parseInt(count) + 1;
                $("#dt_like_count").html(JSON.parse(json.message).count);
                $('#like_p_i_text').text('已收藏');
                $('a#a_mettingcollect_btn').attr('href', 'javascript: CancelMettingCollect();');

            } else {
                MessageWindow(json.message);
            }
        }
    });
}

function CancelMettingCollect() {
    $.ajax({
        type: "POST",
        url: "/Meeting/CancelMettingCollect",
        data: {
            MeetingId: '' + $("#MeetingIndex").val() + '',
        },
        dataType: "json",
        success: function (json) {
            if (json === null || json === "")
                return;
            if (json.status || json.status === "true") {

                MessageWindow("已取消收藏!");
                $("#dt_like_count").html(JSON.parse(json.message).count);

                //var count = $("#dt_like_count").html();
                //count = (count == null || count == "") ? 1 : parseInt(count) - 1;
                //count = count > 1 ? count : 1;
                //$("#dt_like_count").html(count);
                $('#like_p_i_text').text('收藏');
                $('a#a_mettingcollect_btn').attr('href', 'javascript: SetMettingCollect();');
            } else {
                MessageWindow(json.message);
            }
        }
    });
}