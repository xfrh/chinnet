//选择登录方式   1、帐号密码登录   2、手机验证码登录
function SelectLoginMode() {
    var mode = $("#LoginMode").val();
    if (mode == "1") {
        //帐号密码登录
        $("#LoginId").attr("placeholder", "用户名/手机号码");
        $("#ValidateCode").attr("placeholder", "密码");
        $("#ValidateCode").attr("type", "password");
        $(".yzm").css("display", "none");
        $(".login-btn").css("width", "80%");


    } else {
        //手机验证码登录
        $("#LoginId").attr("placeholder", "手机号码");
        $("#ValidateCode").attr("placeholder", "短信验证码");
        $("#ValidateCode").attr("type", "text");
        $(".yzm").css("display", "block");
        $(".login-btn").css("width", "40%");
    }
}

//登录
function Login() {
    var mode = $("#LoginMode").val();
    if (mode == "1") {
        //帐号密码登录
        AccountLogin();
    } else {
        //手机验证码登录
        PhoneLogin();
    }
}

//帐号和密码登录
function AccountLogin() {

    //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
    //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
    //用户名长度为4～18个字符
 
    var account = $("#LoginId").val();
    if (account == "" || account.length < 3) {
        MessageWindow("用户名/手机号码格式不正确");
        return;
    }

    var password = $("#ValidateCode").val();
    if (password == "" || password.length < 6) {
        MessageWindow("密码长度必须大于等于6位");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/AccountLogin",
        data: {
            Account: account,
            Password: password,
            ReturnUrl: $("#returnUrl").val()
        },
        dataType: "json",
        success: function (result) {

            if (result == null || result == "") {
                MessageWindow("登录失败，请重试");
                return;
            }

            if (result.status == "true") {
                console.log(result.message);
                location.href = result.message;
            } else {
                MessageWindow(result.message);
            }

        }
    });
}

//手机号码登录
function PhoneLogin() {
    var phone = $("#LoginId").val();
    if (!IsPhoneNumber(phone)) {
        MessageWindow("请输入手机号码");
        return;
    }

    var phoneCode = $("#ValidateCode").val();
    if (!IsNumber(phoneCode) || phoneCode.length != 6) {
        ErrorWindow("短信验证或格式不正确");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/PhoneLogin",
        data: {
            Phone: phone,
            ValidateCode: phoneCode,
            ReturnUrl: $("#returnUrl").val()
        },
        dataType: "json",
        success: function (result) {

            if (result == null || result == "") {
                MessageWindow("登录失败，请重试");
                return;
            }

            if (result.status == "true") {
                location.href = result.message;
            } else {
                MessageWindow(result.message);
            }

        }
    });

}

//发送短信验证码
function SendValidateCode() {
    var loginId = $("#LoginId").val();
    if (loginId == null || loginId == "") {
        $("#LoginId").focus();
        MessageWindow("请输入手机号码");
        return;
    }

    //由字母a～z(不区分大小写)、数字0～9、点、减号或下划线组成
    //只能以字母开头，包含字符 数字 下划线，例如：beijing.2008
    //用户名长度为4～18个字符
    //var loginIdRe = /^[a-zA-Z][a-zA-Z0-9_]{3,15}$/;
    //if (!loginIdRe.test(loginId)) {
    //    $("#LoginId").focus();
    //    alert("用户名格式不正确");
    //    return;
    //}

    var loginIdRe = /^[1-9]{1}[0-9]{10}$/;
    if (!loginIdRe.test(loginId)) {
        $("#LoginId").focus();
        MessageWindow("手机号码格式不正确");
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/SendValidateCode",
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