Vue.use(VeeValidate, { locale: 'zh_CN' });
var app = new Vue({
    el: '#app',
    data: {
        mobile: null,
        smscode: null,
        dialogContent: null,
        resFailMessage: null,
        resSuccessMessage: null,
        timeaTotal: 60,
        timeCount: 60,
        timer: null,
        smsText: '发送验证码',
        ladda: null
    },
    methods: {
        /* 账号密码登录 */
        onLogin() {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    if (app.ladda) { app.ladda.remove(); }
                    app.ladda = Ladda.create(app.$refs.btnLogin);
                    if (!app.ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/SmsLogin',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                mobile: app.mobile,
                                smsCode: app.smscode
                            },
                            cache: false,
                            timeout: 0,
                            beforeSend: function (xhr, settings) {
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
                                        localStorage.setItem('token', res.token);
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
                    }
                }
            });
            return false;
        },
        closeDialog() {
            $(app.$refs.iosDialog).fadeOut(200);
            app.dialogContent = null;
        },
        sendSms() {
            this.$validator.validate('mobile').then((result) => {
                if (result) {
                    if (app.ladda) { app.ladda.remove(); }
                    app.ladda = Ladda.create(app.$refs.btnSendCode);
                    if (!app.ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/SmsLoginSmsCode',
                            type: 'POST',
                            data: { mobile: app.mobile },
                            dataType: 'json',
                            cache: false,
                            timeout: 0,
                            beforeSend: function () { app.ladda.start(); },
                            complete: function () { },
                            error: function () {
                                app.resFailMessage = '网络请求失败.';
                                $(app.$refs.failToast).fadeIn(100);
                                setTimeout(function () {
                                    $(app.$refs.failToast).fadeOut(200);
                                    app.resFailMessage = null;
                                }, 1800);
                                app.ladda.stop();
                            },
                            success: function (res) {
                                if (res.status) {
                                    app.countDown();
                                }
                                else {
                                    app.resFailMessage = res.message;
                                    $(app.$refs.failToast).fadeIn(100);
                                    app.ladda.stop();
                                    setTimeout(function () {
                                        $(app.$refs.failToast).fadeOut(200);
                                        app.resFailMessage = null;
                                    }, 1800);
                                }
                            }
                        });
                    }
                }
                else {
                    app.dialogContent = '手机号格式不正确';
                    $(app.$refs.iosDialog).fadeIn(200);
                }
            });
            return false;
        },
        countDown() {
            if (!app.timer) {
                if (app.ladda && app.ladda.isLoading()) { app.ladda.stop(); }
                app.timeCount = app.timeaTotal;
                app.timer = setInterval(() => {
                    if (app.timeCount > 0 && app.timeCount <= app.timeaTotal) {
                        app.timeCount--;
                        app.smsText = app.timeCount + 's';
                    }
                    else {

                        clearInterval(this.timer);
                        app.timer = null;
                        app.smsText = '发送验证码';
                    }
                }, 1000);
            }
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        }
    }
});