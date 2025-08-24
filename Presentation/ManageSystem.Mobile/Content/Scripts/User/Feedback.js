Vue.use(VeeValidate, { locale: 'zh_CN' });
app = new Vue({
    el: '#app',
    data: {
        title: null,
        mobile: null,
        email: null,
        content: null,
        resSuccessMessage: null,
        resFailMessage: null
    },
    created: function () {
        var _that = this;
        this.$nextTick(() => {
            $.ajax({
                url: '/User/ProfileData',
                type: 'POST',
                data: { token: localStorage.getItem('token') },
                dataType: 'json',
                success: function (res) {
                    if (res.status) {
                        _that.mobile = res.data.mobile;
                        _that.email = res.data.email;
                    }
                }
            });
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
                            url: '/User/Feedback',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                token: localStorage.getItem('token'),
                                title: app.$data.title,
                                mobile: app.$data.mobile,
                                email: app.$data.email,
                                content: app.$data.content
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