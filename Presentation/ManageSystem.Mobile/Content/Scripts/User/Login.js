////const { json } = require("node:stream/consumers");

Vue.use(VeeValidate, { locale: 'zh_CN' });

var app = new Vue({
    el: '#app',
    data: {
        mobile: null,
        password: null,
        new_password: null,
        cfm_password: null,
        validationCode: null,
        dialogContent: null,
        resFailMessage: null,
        resSuccessMessage: null,
        ladda: null,
        setting_password: false,
        loginCtrl: false
    },
    mounted: function () {
        var that = this;
        that.$validator.extend('mobile', {
            validate: (value, ref) => {
                return /^[1](([3|5|8][\d])|([4][1,4,5,6,7,8,9])|([6][5,6])|([7][3,4,5,6,7,8])|([9][8,9]))[\d]{8}$/.test(value);
            },
            messages: (field) => {
                zh_CN: '请输入正确的手机号码';
            }
        });
        if (sessionStorage.getItem('loginform')) {
            var cacheJson = JSON.parse(sessionStorage.getItem('loginform'));
            that.$data.mobile = cacheJson.mobile;
            that.$data.password = cacheJson.password;
            that.$data.new_password = cacheJson.new_password;
            that.$data.cfm_password = cacheJson.cfm_password;
            that.$data.setting_password = cacheJson.setting_password;
            sessionStorage.removeItem('loginform');
        }
    },
    methods: {
        /* 账号密码登录 */
        onLogin() {
            var that = this;
            if (that.$data.setting_password) {
                that.$validator.validateAll({
                    mobile: that.$data.mobile,
                    new_password: that.$data.new_password,
                    cfm_password: that.$data.cfm_password,
                    validationCode: ""
                }).then((result) => {
                    if (result) {
                        if (app.ladda) { app.ladda.remove(); }
                        app.ladda = Ladda.create(app.$refs.btnLogin);
                        if (!app.ladda.isLoading()) {
                            sessionStorage.setItem('loginform', JSON.stringify({ mobile: that.$data.mobile, password: null, new_password: that.$data.new_password, cfm_password: that.$data.cfm_password, setting_password: that.$data.setting_password }));
                            that.postSettingPassword();
                        }
                    }
                });
            }
            else {
                that.$validator.validateAll({
                    mobile: that.$data.mobile,
                    password: that.$data.password,
                    validationCode: ""
                }).then((result) => {
                    if (result) {
                        if (app.ladda) { app.ladda.remove(); }
                        app.ladda = Ladda.create(app.$refs.btnLogin);
                        if (!app.ladda.isLoading()) {
                            sessionStorage.setItem('loginform', JSON.stringify({ mobile: that.$data.mobile, password: that.$data.password, new_password: null, cfm_password: null, setting_password: that.$data.setting_password }));
                            that.postLogin();
                        }
                    }
                });
            }

            return false;
        },
        postLogin: function () {
            $.ajax({
                url: '/User/Login',
                type: 'POST',
                dataType: 'json',
                data: {
                    mobile: app.mobile,
                    password: app.password,
                    validationCode: app.validationCode,
                    mvcCaptchaGuid: ""
                },
                cache: false,
                timeout: 0,
                beforeSend: function (xhr, settings) {
                    app.loginCtrl = true;
                    app.ladda.start();
                    $(app.$refs.submitToast).fadeIn(100);
                },
                error: function (xhr, errorType, error) {
                    app.resFailMessage = '网络请求失败.';
                    $(app.$refs.submitToast).fadeOut(50);
                    $(app.$refs.failToast).fadeIn(100);
                    setTimeout(function () {
                        $(app.$refs.failToast).fadeOut(200);
                        app.resFailMessage = null;
                    }, 1800);
                    app.ladda.stop();
                },
                success: function (res) {
                    if (res.status) {
                        try {
                            localStorage.setItem('token', res.data);
                            if (app.$refs.hidden_ret_url.value && app.$refs.hidden_ret_url.value.length > 0) {
                                console.log(app.$refs.hidden_ret_url.value);
                                location.href = app.$refs.hidden_ret_url.value;
                            }
                            else {
                                location.href = res.url;
                            }
                            location.href = "/Home/List";
                            app.ladda.stop();
                        }
                        catch (e) {
                            app.ladda.stop();
                        }
                    }
                    else {
                        app.dialogContent = res.message;
                        $(app.$refs.submitToast).fadeOut(50);
                        $(app.$refs.iosDialog).fadeIn(200);
                        app.ladda.stop();
                    }
                }
            });
        },
        postSettingPassword: function () {
            $.ajax({
                url: '/User/SettingPasswordAndLogin',
                type: 'POST',
                dataType: 'json',
                data: {
                    mobile: app.mobile,
                    new_password: app.new_password,
                    cfm_password: app.cfm_password,
                    validationCode: app.validationCode,
                    mvcCaptchaGuid: document.getElementById('_mvcCaptchaGuid').value
                },
                cache: false,
                timeout: 0,
                beforeSend: function (xhr, settings) {
                    app.loginCtrl = true;
                    app.ladda.start();
                    $(app.$refs.submitToast).fadeIn(100);
                },
                error: function (xhr, errorType, error) {
                    app.resFailMessage = '网络请求失败.';
                    $(app.$refs.submitToast).fadeOut(50);
                    $(app.$refs.failToast).fadeIn(100);
                    setTimeout(function () {
                        $(app.$refs.failToast).fadeOut(200);
                        app.resFailMessage = null;
                    }, 1800);
                    app.ladda.stop();
                },
                success: function (res) {
                    if (res.status) {
                        try {
                            localStorage.setItem('token', res.data);
                            location.href = res.url;
                            app.ladda.stop();
                        }
                        catch (e) {
                            app.ladda.stop();
                        }
                    }
                    else {
                        app.dialogContent = res.message;
                        $(app.$refs.submitToast).fadeOut(50);
                        $(app.$refs.iosDialog).fadeIn(200);
                        app.ladda.stop();
                    }
                }
            });
        },
        closeDialog() {
            $(app.$refs.iosDialog).fadeOut(200);
            app.dialogContent = null;
            if (app.loginCtrl) {
                app.loginCtrl = false;
                window.location.reload();
            }
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        inputFunc: function () {
            var that = this;
            that.$validator.validate('mobile', that.$data.mobile).then((result) => {
                if (result) {
                    var loading;
                    $.ajax({
                        url: '/User/CheckLoginMobile',
                        type: 'POST',
                        data: { mobile: that.$data.mobile },
                        dataType: 'json',
                        timeout: 0,
                        cache: false,
                        beforeSend: function () {
                            loading = weui.loading('请稍等', { className: 'chinet-weui-toast fon-32' });
                        },
                        complete: function () {
                            loading.hide();
                            that.$data.password = null;
                            that.$data.new_password = null;
                            that.$data.cfm_password = null;
                        },
                        error: function () {
                            that.$data.setting_password = false;
                        },
                        success: function (res) {
                            if (res.status && res.setting) {
                                // 需要设置密码
                                weui.alert('请设置登录密码', { className: 'chinet-weui-dialog fon-32' });
                                that.$data.setting_password = true;
                            }
                            else {
                                that.$data.setting_password = false;
                            }
                        }
                    });
                }
            });
            return false;
        }
    }
});


var oHeight = $(document).height(); //浏览器当前的高度
$(window).resize(function () {

    if ($(document).height() < oHeight) {

        $(".Lo-footer").css("position", "static");
    } else {

        $(".Lo-footer").css("position", "absolute");
    }
});