Vue.use(VeeValidate, { locale: 'zh_CN' });
app = new Vue({
    el: '#app',
    data: {
        orgPassword: null,
        newPassword: null,
        cfmPassword: null,
        resSuccessMessage: null,
        resFailMessage: null
    },
    created: function () {
        this.$nextTick(() => {
        });
    },
    methods: {
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        onSubmit() {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    var ladda = Ladda.create(app.$refs.btnSubmit);

                    if (!ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/Cipher',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                token: localStorage.getItem('token'),
                                orgPassword: app.$data.orgPassword,
                                newPassword: app.$data.newPassword,
                                cfmPassword: app.$data.cfmPassword
                            },
                            cache: false,
                            timeout: 0,
                            beforeSend: function (xhr, settings) {
                                ladda.start();
                                $(app.$refs.submitToast).fadeIn(100);
                            },
                            error: function (xhr, errorType, error) {
                                app.$data.resFailMessage = '网络请求失败.';
                                $(app.$refs.submitToast).fadeOut(50);
                                $(app.$refs.failToast).fadeIn(100);
                                setTimeout(function () {
                                    $(app.$refs.failToast).fadeOut(200);
                                    app.$data.resFailMessage = null;
                                }, 1800);
                                ladda.stop();
                            },
                            success: function (res) {
                                if (res.status) {
                                    try {
                                        app.$data.resSuccessMessage = res.message;
                                        $(app.$refs.submitToast).fadeOut(50);
                                        $(app.$refs.successToast).fadeIn(100);
                                        setTimeout(function () {
                                            location.href = res.url;
                                        }, 800);
                                    }
                                    catch (e) {
                                        ladda.stop();
                                    }
                                }
                                else {
                                    app.$data.resFailMessage = res.message;
                                    $(app.$refs.submitToast).fadeOut(50);
                                    $(app.$refs.failToast).fadeIn(100);
                                    ladda.stop();
                                    setTimeout(function () {
                                        $(app.$refs.failToast).fadeOut(200);
                                        app.$data.resFailMessage = null;
                                    }, 1800);
                                }
                            }
                        });
                    }
                }
            });
            return false;
        }
    }
});