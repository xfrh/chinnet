Vue.use(VeeValidate, { locale: 'zh_CN' });
var app = new Vue({
    el: '#app',
    data: {
        membername: null,
        loginid: null,
        username: null,
        password: null,
        password2: null,
        province: '-1',
        hospital: null,
        dialogContent: null,
        resFailMessage: null,
        resSuccessMessage: null,
        ladda: null,
        loadingText: '数据提交中'
    },
    methods: {
        /* 去注册 */
        onRegister() {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    if (app.ladda) { app.ladda.remove(); }
                    app.ladda = Ladda.create(app.$refs.btnRegister);
                    if (!app.ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/Register',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                loginid: app.loginid,
                                name: app.membername,
                                mobile: app.username,
                                newPassword: app.password,
                                cfmPassword: app.password2,
                                //provinceId: app.province,
                                //hospital: app.hospital
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
                                    app.ladda.stop();
                                    try {
                                        app.loadingText = res.message;
                                        localStorage.setItem('token', res.token);
                                        location.href = res.url;
                                    }
                                    catch (e) {
                                        app.ladda.stop();
                                    }
                                }
                                else {
                                    app.dialogContent = res.message;
                                    $(app.$refs.submitToast).fadeOut(20);
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