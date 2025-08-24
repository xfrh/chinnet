Vue.use(VeeValidate, { locale: 'zh_CN' });
var app = new Vue({
    el: '#app',
    data: {
        userphoto: '/Content/Images/u_default.png',
        username: null,
        realname: null,
        gender: null,
        genderText: null,
        email: null,
        mobile: null,
        province: null,
        hospital: null,
        postFile: null,
        provinceList: [],
        dialogContent: null,
        resFailMessage: null,
        resSuccessMessage: null,
        loadingText: '数据提交中',
        ladda: null,
        IsCheckEmail: false
    },
    created: function () {
        this.$nextTick(() => {
            var _this = this,
                $loadingToast = $(_this.$refs.loadingToast);
            $.ajax({
                url: '/User/ProfileData',
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
                    if (res.status) {
                        _this.userphoto = res.data.userphoto;
                        _this.username = res.data.username;
                        _this.realname = res.data.realname;
                        _this.IsCheckEmail = res.data.IsCheckEmail;
                        _this.gender = res.data.gender;
                        _this.genderText = res.data.genderText;
                        _this.email = res.data.email;
                        _this.mobile = res.data.mobile;
                        _this.province = res.data.province;
                        _this.hospital = res.data.hospital;
                        _this.provinceList = res.data.provinceList;
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
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        openGender() {
            $('#genderActionsheet').addClass('weui-actionsheet_toggle');
            $('#iosMask').fadeIn(200);
        },
        closeGender() {
            $('#genderActionsheet').removeClass('weui-actionsheet_toggle');
            $('#iosMask').fadeOut(200);
        },
        setGender(value, key) {
            app.$data.genderText = value;
            app.$data.gender = key;
            $('#genderActionsheet').removeClass('weui-actionsheet_toggle');
            $('#iosMask').fadeOut(200);
        },
        closeDialog() {
            $(this.$refs.iosDialog).fadeOut(200);
            this.dialogContent = null;
        },
        defaultHeadImage() {
            this.userphoto = '/Content/Images/u_default.png';
            this.$refs.imgHeadImage.onerror = null;
        },
        browsePhoto() {
            this.$refs.postFile.click();
        },
        previewPhoto() {
            var _this = this;
            if (_this.$refs.postFile.files.length === 1) {
                _this.postFile = _this.$refs.postFile.files[0];
                if (window.createObjectURL !== undefined) { // basic
                    _this.userphoto = window.createObjectURL(_this.postFile);
                }
                else if (window.URL !== undefined) { // mozilla(firefox)
                    _this.userphoto = window.URL.createObjectURL(_this.postFile);
                }
                else if (window.webkitURL !== undefined) { // webkit or chrome
                    _this.userphoto = window.webkitURL.createObjectURL(_this.postFile);
                }
            }
        },
        onSubmit() {
            _this = this;
            this.$validator.validateAll().then((result) => {
                if (result) {
                    if (_this.ladda) { _this.ladda.remove(); }
                    _this.ladda = Ladda.create(_this.$refs.btnSubmit);
                    if (!_this.ladda.isLoading()) {
                        var formdata = new FormData(),
                            $loadingToast = $(_this.$refs.loadingToast);
                        formdata.append('token', localStorage.getItem('token'));
                        if (!_this.postFile || _this.postFile === null) {
                            formdata.append('postFile', null);
                        }
                        else {
                            formdata.append('postFile', _this.postFile);
                        }

                        formdata.append('realname', _this.realname);
                        formdata.append('gender', _this.gender);
                        formdata.append('email', _this.email);
                        formdata.append('mobile', _this.mobile);
                        formdata.append('province', _this.province);
                        formdata.append('hospital', _this.hospital);

                        $.ajax({
                            url: '/User/Profile',
                            type: 'POST',
                            dataType: 'json',
                            data: formdata,
                            cache: false,
                            timeout: 0,
                            processData: false,
                            contentType: false,
                            beforeSend: function (xhr, settings) {
                                _this.ladda.start();
                                _this.loadingText = '数据提交中';
                                $loadingToast.fadeIn(100);
                            },
                            error: function (xhr, errorType, error) {
                                _this.resFailMessage = '网络请求失败.';
                                $loadingToast.fadeOut(50);
                                $failToast = $(_this.$refs.failToast);
                                $failToast.fadeIn(100);
                                setTimeout(function () {
                                    $failToast.fadeOut(200);
                                    _this.resFailMessage = null;
                                }, 1800);
                                _this.ladda.stop();
                            },
                            success: function (res) {
                                if (res.status) {
                                    _this.ladda.stop();
                                    try {
                                        setTimeout(function () {
                                            _this.loadingText = res.message;
                                            setTimeout(() => {
                                                location.href = res.url;
                                            }, 400);
                                        }, 500);
                                    }
                                    catch (e) {
                                        $loadingToast.fadeOut(200);
                                    }
                                }
                                else {
                                    _this.dialogContent = res.message;
                                    $loadingToast.fadeOut(20);
                                    $(_this.$refs.iosDialog).fadeIn(200);
                                    _this.ladda.stop();
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