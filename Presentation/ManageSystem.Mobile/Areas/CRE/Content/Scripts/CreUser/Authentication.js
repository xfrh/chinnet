Vue.use(VeeValidate, { locale: 'zh_CN' });
app = new Vue({
    el: '#app',
    data: {
        Attestation: {
            Date: null,
            Status: null,
            AreaName: null,
            HospitalName: null,
            HospitalDepartmentName: null,
            DoctorTitleName: null
        },
        CreateModel: {
        },
        SubmitModel: {
            Province: '0',
            City: '0',
            Hospital: '0',
            HospitalDepartment: '0',
            DoctorTitle: '0'
        },
        CreateAttestation: false,
        resSuccessMessage: null,
        resFailMessage: null
    },
    created: function () {
        this.$nextTick(() => {
        });
    },
    mounted: function () {
        this.init();
    },
    methods: {
        init() {
            var _this = this;
            $.ajax({
                url: '/CreUser/Authentication',
                type: 'POST',
                data: { token: localStorage.getItem('token') },
                dataType: 'json',
                beforeSend: function () {
                    _this.$data.CreateAttestation = false;
                    $(_this.$refs.loadingToast).fadeIn(100);
                },
                complete: function () {

                },
                error: function () {
                    app.$data.resFailMessage = '网络请求失败.';
                    $(_this.$refs.loadingToast).fadeOut(20);
                    $(app.$refs.failToast).fadeIn(100);
                    setTimeout(function () {
                        $(app.$refs.failToast).fadeOut(200);
                        app.$data.resFailMessage = null;
                    }, 1800);
                },
                success: function (res) {
                    if (res.status && res.attestation) {
                        _this.$data.Attestation = res.data;
                        setTimeout(function () {
                            $(_this.$refs.loadingToast).fadeOut(20);
                        }, 500);
                    }
                    else if (res.status && !res.attestation) {
                        _this.$data.Attestation = res.data;
                        _this.$data.CreateModel = res.FormInitData;
                        _this.$data.CreateAttestation = true;
                        setTimeout(function () {
                            $(_this.$refs.loadingToast).fadeOut(20);
                        }, 200);
                    }
                    else if (!res.status) {
                        app.$data.resFailMessage = res.message;
                        $(_this.$refs.loadingToast).fadeOut(20);
                        $(app.$refs.failToast).fadeIn(100);
                        setTimeout(function () {
                            $(app.$refs.failToast).fadeOut(200);
                            app.$data.resFailMessage = null;
                        }, 1800);
                    }
                    else {
                        $(_this.$refs.loadingToast).fadeOut(20);
                    }
                }
            });
        },
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
                            url: '/CreUser/SubmitAuthentication',
                            type: 'POST',
                            dataType: 'json',
                            data: {
                                token: localStorage.getItem('token'),
                                areaId: app.$data.SubmitModel.City,
                                hospitalId: app.$data.SubmitModel.Hospital,
                                departmentId: app.$data.SubmitModel.HospitalDepartment,
                                doctorTitleId: app.$data.SubmitModel.DoctorTitle
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
        },
        loadCity(value) {
            if (value) {
                if (value !== '0') {
                    $.ajax({
                        url: '/CreUser/DistrictList',
                        type: 'POST',
                        data: { parentId: value },
                        dataType: 'json',
                        cache: false,
                        timeout: 0,
                        beforeSend: function () { $(app.$refs.loadingToast).fadeIn(100); },
                        complete: function () {
                            $(app.$refs.loadingToast).fadeOut(200);
                        },
                        success: function (res) {
                            app.$data.SubmitModel.City = '0';
                            app.$data.CreateModel.CityList = res;
                        }
                    });
                }
                else {
                    app.$data.SubmitModel.City = '0';
                    app.$data.CreateModel.CityList = [{ Value: '0', Text: '-- 请选择 --' }];
                }
            }
        }
    }
});