Vue.use(VeeValidate, { locale: 'zh_CN' });
app = new Vue({
    el: '#app',
    data: {
        email: null,
        smscode: null,
        password: null,
        password2: null,
        dialogContent: null,
        resFailMessage: null,
        resSuccessMessage: null,
        timeaTotal: 180,
        timeCount: 180,
        timer: null,
        smsText: '发送验证码',
        ladda: null,
        isAndroid: navigator.userAgent.match(/(Android);?[\s/]+([\d.]+)?/),
        showSubmit: true,
        originHeight: 0
    },
    mounted: function () {
        var that = this;
        window.onresize = function () {
            if (that.isAndroid) {
                let resizeHeight = document.documentElement.clientHeight || document.body.clientHeight;
                that.showSubmit = that.originHeight < resizeHeight;
                that.originHeight = resizeHeight;
            }
        };
    },
    methods: {
        /* 发送验证码 */
        onSendCode() {
            this.$validator.validate('email').then((result) => {
                if (result) {
                    if (app.ladda) { app.ladda.remove(); }
                    app.ladda = Ladda.create(app.$refs.btnSendCode);
                    if (!app.ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/ForgetPasswordEmailCode',
                            type: 'POST',
                            data: { email: app.email },
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
            });
            return false;
        },
        /* 确认修改密码 */
        onSubmit() {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    if (app.ladda) { app.ladda.remove(); }
                    app.ladda = Ladda.create(app.$refs.btnSubmit);
                    if (!app.ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/EmailForgetPassword',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                email: app.email,
                                smsCode: app.smscode,
                                newPassword: app.password,
                                cfmPassword: app.password2
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
                                        app.resSuccessMessage = res.message;
                                        $(app.$refs.submitToast).fadeOut(50);
                                        $(app.$refs.successToast).fadeIn(100);
                                        setTimeout(function () {
                                            location.href = res.url;
                                        }, 800);
                                    }
                                    catch (e) {
                                        app.ladda.stop();
                                    }
                                }
                                else {
                                    app.resFailMessage = res.message;
                                    $(app.$refs.submitToast).fadeOut(50);
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
            });
            return false;
        },
        countDown() {
            if (!this.timer) {
                if (this.ladda && this.ladda.isLoading()) { this.ladda.stop(); }
                this.timeCount = this.timeaTotal;
                this.timer = setInterval(() => {
                    if (this.timeCount > 0 && this.timeCount <= this.timeaTotal) {
                        this.timeCount--;
                        app.$data.smsText = this.timeCount + 's';
                    }
                    else {

                        clearInterval(this.timer);
                        this.timer = null;
                        app.$data.smsText = '发送验证码';
                    }
                }, 1000);
            }
        }
    }
});