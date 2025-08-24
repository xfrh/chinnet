
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

});

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
        url: "/FindPassword/SendValidateCode",
        data: "phone=" + loginId,
        dataType: "json",
        success: function (result) {

            if (result === null || result === "") {
                MessageWindow("发送验证码失败，请重试");
                return;
            }

            if (result.status === true || result.status === "true") {
                //成功
                var $form_content = $('#Phone_PhoneCode.aui-input').closest('div.aui-form-content-item');
                var $yzm = $form_content.find('.yzm');
                $yzm.attr("disabled", "disabled");
                SetValidateTime(180, $yzm);
                //$(".yzm").attr("disabled", "disabled");
                //setInterval(SetValidateTime, 1000);
                return;
            }
            else {
                MessageWindow(result.message);
            }
        }
    });
}

var timeIndex = 180; //180秒
var smsTimer;
// 倒计时
function SetValidateTime(time, target) {
    smsTimer = setTimeout(function () {

        if (time < 1) {
            clearTimeout(smsTimer);
            $(target).removeAttr("disabled");
            $(target).val("获取验证码");
            return;
        }
        else {
            if (time) {
                $(target).val(time + " 秒");
            }
            CountdownTime(--time, target);
        }
    }, 1000);

    //var $form_content = $('#Phone_PhoneCode').closest('div.aui-form-content-item');
    //if (timeIndex <= 0) {
    //    $form_content.find('.yzm').removeAttr("disabled");
    //    $form_content.find('.yzm').val("获取验证码");
    //    return;
    //}

    //$form_content.find('.yzm').val(timeIndex + " 秒");

    //timeIndex--;
}

var emailTimer;
function CountdownTime(time, target) {
    emailTimer = setTimeout(function () {

        if (time < 1) {
            clearTimeout(emailTimer);
            $(target).removeAttr("disabled");
            $(target).val("获取验证码");
            return;
        }
        else {
            if (time) {
                $(target).val(time + " 秒");
            }
            CountdownTime(--time, target);
        }
    }, 1000);
}

// 发送验证码
function SendEmailValidateCode() {
    var email = $("#Email").val();
    if (email === null || email === "" || email.length === 0) {
        $("#Email").focus();
        MessageWindow("请输入您的邮箱");
        return;
    }

    if (!IsEmail(email)) {
        $("#Email").focus();
        MessageWindow("请输入正确的邮箱");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/FindPassword/SendEmailValidateCode",
        data: { email: email },
        dataType: "json",
        success: function (result) {
            if (result === null || result === "") {
                MessageWindow("发送验证码失败，请重试");
                return;
            }

            if (result.status === true || result.status === "true") {
                //成功
                var $form_content = $('#Email.aui-input').closest('div.aui-form-content-item');
                var $yzm = $form_content.find('.yzm');
                $yzm.attr("disabled", "disabled");
                CountdownTime(180, $yzm);
                MessageWindow(result.message);
                //$(".email-yzm").attr("disabled", "disabled");
                //CountdownTime(180, '.email-yzm');
                return;
            }
            else {
                MessageWindow(result.message);
            }
        }
    });
}

// 找回密码
OnPhoneSave = function () {

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

    var phoneCode = $("#Phone_PhoneCode").val();
    if (!IsNumber(phoneCode) || phoneCode.length !== 6) {
        MessageWindow("验证码格式不正确");
        return;
    }

    var password = $("#Phone_Password").val();
    if (password === "") {
        MessageWindow("请输入密码");
        return;
    }

    if ($.trim(password).length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    var password2 = $("#Phone_Password2").val();
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

            if (result === null || result === "") {
                ErrorWindow("找回密码失败，请重试！");
                return;
            }

            if (result.status === true || result.status === "true") {
                layer.msg('找回密码成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    location.href = "/Login/OpenIndex";
                });
            } else {
                ErrorWindow(result.message);
            }
        }
    });
};

// 找回密码
OnEmailSave = function () {
    var email = $("#Email").val();
    if (email === null || email === "" || email.length === 0) {
        $("#Email").focus();
        MessageWindow("请输入您的邮箱");
        return;
    }

    if (!IsEmail(email)) {
        $("#Email").focus();
        MessageWindow("请输入正确的邮箱");
        return;
    }

    var phoneCode = $("#Email_PhoneCode").val();
    if (phoneCode.length !== 6) {
        MessageWindow("验证码格式不正确");
        return;
    }

    var password = $("#Email_Password").val();
    if (password === "") {
        MessageWindow("请输入密码");
        return;
    }

    if ($.trim(password).length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    var password2 = $("#Email_Password2").val();
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
        url: "/FindPassword/EmailSubmit",
        data: {
            Phone: email,
            ValidateCode: phoneCode,
            Password: password,
            Password2: password2
        },
        dataType: "json",
        success: function (result) {

            $(".btn-login").removeAttr("disabled");

            if (result === null || result === "") {
                ErrorWindow("找回密码失败，请重试！");
                return;
            }

            if (result.status === true || result.status === "true") {
                layer.msg('找回密码成功', {
                    time: 1500 //2秒关闭（如果不配置，默认是3秒）
                }, function () {
                    location.href = "/Login/OpenIndex";
                });
            } else {
                ErrorWindow(result.message);
            }
        }
    });
};