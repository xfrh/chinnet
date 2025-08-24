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

    $('#LoginId').on('input propertychange', function () {
        var _value = this.value,
            _layerLoading;
        if (IsMobileNumber(_value)) {
            $.ajax({
                url: '/Login/CheckMobile',
                method: 'POST',
                data: { mobile: _value },
                dataType: 'JSON',
                timeout: 0,
                cache: false,
                beforeSend: function () { _layerLoading = layer.load(2, { offset: ['', '20%'] }); },
                complete: function () { layer.close(_layerLoading); },
                error: function () {
                    $('#cfmPasswordCtrl,#btnLogin2').hide();
                    $('#PasswordCtrl,#btnLogin1,#FindPasswordCtrl').show();
                },
                success: function (res) {
                    if (res.status && res.setting) {
                        // 需要设置密码
                        layer.msg('请设置登录密码', { time: 1500 });
                        $('#cfmPasswordCtrl,#btnLogin2').show();
                        $('#PasswordCtrl,#btnLogin1,#FindPasswordCtrl').hide();
                    }
                    else {
                        $('#cfmPasswordCtrl,#btnLogin2').hide();
                        $('#PasswordCtrl,#btnLogin1,#FindPasswordCtrl').show();
                    }
                }
            });
        }
    });

    $("#LoginId").val(sessionStorage.getItem('login_mobile') || '');
});

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

    var validationCode = $('#ValidationCode').val(),
        uniqueId = $('#_mvcCaptchaGuid').val();

    if (IsEmpty(validationCode)) {
        MessageWindow("请输入验证码");
        return;
    }

    if (validationCode.length !== 4) {
        MessageWindow("验证码输入不正确");
        return;
    }
    var _layerLoading;

    $.ajax({
        type: "POST",
        url: "/Login/AccountLogin",
        data: {
            Account: account,
            Password: password,
            ValidationCode: validationCode,
            MvcCaptchaGuid: uniqueId,
            ReturnUrl: $('#HiddenReturnUrl').val()
        },
        dataType: "json",
        beforeSend: function () { sessionStorage.setItem('login_mobile', account); _layerLoading = layer.load(2, { offset: ['', '20%'] }); },
        complete: function () { layer.close(_layerLoading); },
        success: function (result) {
            if (result === null || result === "") { MessageWindow("登录失败，请重试"); return; }

            if (result.status === true || result.status === "true") {
                sessionStorage.removeItem('login_mobile');
                layer.msg('登录成功', { time: 1500 }, function () {
                    parent.OpenLoginCallback(result.message);
                });
            } else {
                layer.msg(result.message, function () {
                    location.reload();
                });
            }
        }
    });
}

// 账号密码登录
function SettingPassword() {
    var LoginId = $('#LoginId').val(),
        newPassword = $('#newPassword').val(),
        cfmPassword = $('#cfmPassword').val(),
        validationCode = $('#ValidationCode').val(),
        _mvcCaptchaGuid = $('#_mvcCaptchaGuid').val(),
        _layerLoading;

    if (IsEmpty(LoginId)) {
        MessageWindow("请输入手机号");
        $('#LoginId').focus();
        return;
    }

    if (!IsMobileNumber(LoginId)) {
        MessageWindow("输入手机号不正确");
        $('#LoginId').focus();
        return;
    }

    if (IsEmpty(newPassword)) {
        MessageWindow("请设置登录密码");
        $('#newPassword').focus();
        return;
    }

    if (newPassword.length < 6) {
        MessageWindow("登录密码长度必须大于等于6位");
        $('#newPassword').focus();
        return;
    }

    if (IsEmpty(cfmPassword)) {
        MessageWindow("请再次输入登录密码");
        $('#cfmPassword').focus();
        return;
    }

    if (cfmPassword.length < 6) {
        MessageWindow("登录密码长度必须大于等于6位");
        $('#cfmPassword').focus();
        return;
    }

    if (newPassword !== cfmPassword) {
        MessageWindow("两次输入密码不一致");
        $('#cfmPassword').focus();
        return;
    }

    if (IsEmpty(validationCode)) {
        MessageWindow("请输入验证码");
        $('#ValidationCode').focus();
        return;
    }

    if (validationCode.length !== 4) {
        MessageWindow("验证码输入不正确");
        $('#ValidationCode').focus();
        return;
    }

    $.ajax({
        type: "POST",
        url: "/Login/SatelliteWithPassword",
        data: {
            mobile: LoginId,
            new_Password: newPassword,
            cfm_Password: cfmPassword,
            validationCode: validationCode,
            mvcCaptchaGuid: _mvcCaptchaGuid,
            ReturnUrl: $('#HiddenReturnUrl').val()
        },
        dataType: "json",
        beforeSend: function () { sessionStorage.setItem('login_mobile', account); _layerLoading = layer.load(2, { offset: ['', '20%'] }); },
        complete: function () { layer.close(_layerLoading); },
        success: function (res) {
            if (res.status) {
                sessionStorage.removeItem('login_mobile');
                layer.msg(res.message, { time: 1500 }, function () {
                    parent.OpenLoginCallback(res.returnUrl);
                });
            }
            else {
                layer.msg(result.message, function () {
                    location.reload();
                });
                //MessageWindow(res.message);
                //_reloadMvcCaptchaImage();
            }
        }
    });
}