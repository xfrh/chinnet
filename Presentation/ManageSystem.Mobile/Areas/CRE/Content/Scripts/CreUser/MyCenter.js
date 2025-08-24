var app = new Vue({
    el: '#app',
    data: {
        user: {
            photo: '/Content/Images/u_default.png',
            username: null,
            timetext: null
        },
        resFailMessage: null,
        loadingText: '数据提交中'
    },
    created: function () {
        var _this = this;
        _this.$nextTick(() => {
            var $loadingToast = $(_this.$refs.loadingToast);
            $.ajax({
                url: '/CreUser/MyCenter',
                type: 'POST',
                data: { token: localStorage.getItem('token') },
                dataType: 'json',
                cache: false,
                timeout: 0,
                beforeSend: function () {
                    _this.loadingText = '加载中';
                    $loadingToast.fadeIn(200);
                },
                error: function () {
                    _this.resFailMessage = '网络请求失败.';
                    $loadingToast.fadeOut(50);
                    $(_this.$refs.failToast).fadeIn(100);
                    setTimeout(function () {
                        $(_this.$refs.failToast).fadeOut(200);
                        _this.resFailMessage = null;
                    }, 1800);
                },
                success: function (res) {
                    console.log(res);
                    if (res.status) {
                        _this.user = res.user;
                        setTimeout(function () {
                            $loadingToast.fadeOut(200);
                        }, 500);
                    }
                    else {
                        _this.dialogContent = res.message;
                        $loadingToast.fadeOut(50);
                        $(_this.$refs.iosDialog).fadeIn(200);
                    }
                }
            });
        });
    },
    methods: {
        defaultHeadImage() {
            this.user.photo = '/Content/Images/u_default.png';
            this.$refs.imgHeadImage.onerror = null;
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        }
    }
});