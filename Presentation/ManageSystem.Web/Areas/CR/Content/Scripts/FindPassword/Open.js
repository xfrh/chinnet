
//发送短信验证码
function SendValidateCode() {

    var loginId = $("#Phone").val();
    if (loginId == null || loginId == "") {
        $("#Phone").focus();
        MessageWindow("请输入手机号码");
        return;
    }

    var loginIdRe = /^[1-9]{1}[0-9]{10}$/;
    if (!loginIdRe.test(loginId)) {
        $("#Phone").focus();
        MessageWindow("手机号码格式不正确");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/FindPassword/SendValidateCode",
        data: "phone=" + loginId,
        dataType: "json",
        success: function (result) {

            if (result == null || result == "") {
                MessageWindow("发送验证码失败，请重试");
                return;
            }

            if (result.status == "true") {
                //成功
                $(".yzm").attr("disabled", "disabled");
                setInterval(SetValidateTime, 1000);

                return;
            } else {
                MessageWindow(result.message);
            }
        }
    });
}

var timeIndex = 180; //180秒
function SetValidateTime() {

    if (timeIndex <= 0) {
        $(".yzm").removeAttr("disabled");
        $(".yzm").val("获取验证码");
        return;
    }

    $(".yzm").val(timeIndex + " 秒");

    timeIndex--;
}

//找回密码
function Save() {

    var phone = $("#Phone").val();
    if (!IsPhoneNumber(phone)) {
        MessageWindow("请输入手机号码");
        return;
    }

    var loginIdRe = /^[1-9]{1}[0-9]{10}$/;
    if (!loginIdRe.test(phone)) {
        $("#Phone").focus();
        MessageWindow("手机号码格式不正确");
        return;
    }

    var phoneCode = $("#PhoneCode").val();
    if (!IsNumber(phoneCode) || phoneCode.length != 6) {
        MessageWindow("验证码格式不正确");
        return;
    }

    var password = $("#Password").val();
    if (password == "") {
        MessageWindow("请输入密码");
        return;
    }

    if ($.trim(password).length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    var password2 = $("#Password2").val();
    if (password2 == "") {
        MessageWindow("请输入确认密码");
        return;
    }
    if ($.trim(password2).length < 6) {
        MessageWindow("确认密码长度必须大于等于6位");
        return;
    }

    if (password != password2) {
        MessageWindow("2次密码不一致");
        return;
    }
    

    $(".btn-login").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/FindPassword/Submit",
        data: {
            Phone: phone,
            ValidateCode: phoneCode,
            Password: password,
            Password2: password2
        },
        dataType: "json",
        success: function (result) {

            $(".btn-login").removeAttr("disabled");

            if (result == null || result == "") {
                ErrorWindow("找回密码失败，请重试！");
                return;
            }

            if (result.status == "true") {
                layer.msg('找回密码成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    location.href = "/Login/Open";
                });
            } else {
                ErrorWindow(result.message);
            }
        }
    });
}
