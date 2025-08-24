
$(function () {

    /*tab标签切换*/
    function tabs(tabTit, on, tabCon) {
        $(tabCon).each(function () {
            $(this).children().eq(0).show();

        });
        $(tabTit).each(function () {
            $(this).children().eq(0).addClass(on);
        });
        $(tabTit).children().click(function () {
            $(this).addClass(on).siblings().removeClass(on);
            var index = $(tabTit).children().index(this);
            $(tabCon).children().eq(index).show().siblings().hide();
        });
    }
    tabs(".aui-form-header", "on", ".aui-form-content");

})

//帐号和密码登录
function AccountLogin() {

    var account = $("#LoginId").val();
    if (IsEmpty(account)) {
        MessageWindow("请输入登录帐号");
        return;
    }

    if (account.length < 3) {
        MessageWindow("登录帐号格式不正确");
        return;
    }

    var password = $("#Password").val();
    if (IsEmpty(password)) {
        MessageWindow("请输入密码");
        return;
    }

    if (password.length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/AccountLogin",
        data: {
            Account: account,
            Password: password,
            ReturnUrl: ""
        },
        dataType: "json",
        success: function (result) {

            if (result == null || result == "") {
                MessageWindow("登录失败，请重试");
                return;
            }

            if (result.status == "true") {
                layer.msg('登录成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    parent.OpenLoginCallback();
                });
            } else {
                MessageWindow(result.message);
            }

        }
    });
}

//手机号码登录
function PhoneLogin() {
    var phone = $("#Phone").val();
    if (!IsPhoneNumber(phone)) {
        MessageWindow("请输入手机号码");
        return;
    }

    var phoneCode = $("#PhoneCode").val();
    if (!IsNumber(phoneCode) || phoneCode.length !== 6) {
        ErrorWindow("验证码格式不正确");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/PhoneLogin",
        data: {
            Phone: phone,
            ValidateCode: phoneCode,
            ReturnUrl: ""
        },
        dataType: "json",
        success: function (result) {

            if (result === null || result === "") {
                MessageWindow("登录失败，请重试");
                return;
            }

            if (result.status == "true") {
                layer.msg('登录成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    parent.OpenLoginCallback();
                });
            } else {
                MessageWindow(result.message);
            }

        }
    });

}

//发送短信验证码
function SendValidateCode() {

    var loginId = $("#Phone").val();
    if (loginId === null || loginId === "") {
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
        url: "/Login/SendValidateCode",
        data: "phone=" + loginId,
        dataType: "json",
        success: function (result) {

            if (result === null || result === "") {
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

var timeIndex = 60; //180秒
function SetValidateTime() {

    if (timeIndex <= 0) {
        $(".yzm").removeAttr("disabled");
        $(".yzm").val("获取验证码");
        return;
    }

    $(".yzm").val(timeIndex + " 秒");

    timeIndex--;
}