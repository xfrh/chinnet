//注册
function Regist() {
    var nickName = $("#NickName").val();
    if (nickName === "") {
        MessageWindow("请输入您的姓名");
        return;
    }
    var loginId = $('#id_name').val();
    if (loginId === '') {
        MessageWindow("请输入用户名");
        return;
    }
    if (loginId.length < 6) {
        MessageWindow("请输入用户名至少需要6位");
        return;
    }
    var phone = $("#Phone").val();
    if (!IsPhoneNumber(phone)) {
        MessageWindow("请输入手机号码");
        return;
    }

    var password = $("#Password").val();
    if (password === "") {
        MessageWindow("请输入密码");
        return;
    }
    if ($.trim(password).length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    var password2 = $("#Password2").val();
    if (password2 === "") {
        MessageWindow("请输入确认密码");
        return;
    }
    if ($.trim(password2).length < 6) {
        MessageWindow("确认密码长度必须大于等于6位");
        return;
    }

    if (password !== password2) {
        MessageWindow("2次密码不一致");
        return;
    }



    $(".btn-login").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/Regist/PostRegist",
        data: {
            LoginId: loginId,
            Phone: phone,
            Password: password,
            Password2: password2,
            MemberName: nickName,
            Source: 1
        },
        dataType: "json",
        success: function (result) {

            $(".btn-login").removeAttr("disabled");

            if (result === null || result === "") {
                ErrorWindow("注册失败，请重试！");
                return;
            }

            if (result.status === "true") {
                layer.msg('注册成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    parent.OpenLoginCallback();
                });
            } else {
                ErrorWindow(result.message);
            }
        }
    });
}
