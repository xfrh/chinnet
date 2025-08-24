Vue.use(VeeValidate, { locale: 'zh_CN' });
app = new Vue({
    el: '#app',
    data: {
        username: null,
        password: null,
        dialogContent: null
    },
    methods: {
        /* 账号密码登录 */
        onLogin() {
            this.$validator.validateAll().then((result) => {
                if (result) {
                    var ladda = Ladda.create(app.$refs.btnLogin);

                    if (!ladda.isLoading()) {
                        $.ajax({
                            url: '/CreUser/Login',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                account: app.$data.username,
                                password: app.$data.password
                            },
                            cache: false,
                            timeout: 0,
                            beforeSend: function (xhr, settings) { ladda.start(); },
                            error: function (xhr, errorType, error) {
                                alert('网络请求失败.');
                                ladda.stop();
                            },
                            success: function (res) {
                                if (res.status) {
                                    try {
                                        localStorage.setItem('token', res.data);
                                        location.href = res.url;
                                    }
                                    catch (e) {
                                        ladda.stop();
                                    }
                                }
                                else {
                                    app.$data.dialogContent = res.message;
                                    $(app.$refs.iosDialog).fadeIn(200);
                                    ladda.stop();
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
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        }
    }
});