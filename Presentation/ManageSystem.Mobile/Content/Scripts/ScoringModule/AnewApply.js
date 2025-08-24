Vue.use(VeeValidate, { locale: 'zh_CN' });
var mySwiper,
    app = new Vue({
        el: '#app',
        data: {
            dataUrl: '/ScoringModule/HospitalList',
            items: [],
            ajaxLoading: false,
            resFailMessage: null,
            dialogContent: null,
            loadingText: '加载中',
            reason: null
        },
        created: function () {
            var that = this;
            that.$nextTick(() => {
                that.initSwiper();
            });
        },
        methods: {
            initSwiper() {
                setTimeout(() => {
                    mySwiper = new Swiper('.swiper-container', {
                        direction: 'vertical',
                        loop: false,
                        observer: true,
                        observeParents: false,
                        watchSlidesProgress: true,
                        freeMode: true,
                        slidesPerView: 'auto'
                    });
                });
            },
            closeDialog() {
                $(this.$refs.iosDialog).fadeOut(200);
                this.dialogContent = null;
            },
            onSubmit() {
                var that = this;
                that.$validator.validateAll({
                    reason: that.$data.reason
                }).then(result => {
                    if (result) {
                        var l = Ladda.create(that.$refs.btnSubmit);
                        if (l.isLoading()) { return false; }
                        $.ajax({
                            url: '',
                            type: 'POST',
                            data: {
                                token: localStorage.getItem('token'),
                                reason: that.reason
                            },
                            dataType: 'JSON',
                            beforeSend: function () {
                                l.start();
                            },
                            complete: function () {
                                setTimeout(() => { l.stop(); }, 800);
                            },
                            success: function (res) {
                                res = JSON.parse(res);
                                console.log(res);
                                if (res.status) {
                                    location.href = res.ret_url;
                                }
                                else {
                                    that.dialogContent = res.message;
                                    $(that.$refs.iosDialog).fadeIn(200);
                                }
                            },
                            error: function () {
                                that.dialogContent = '提交请求出现错误';
                                $(that.$refs.iosDialog).fadeIn(200);
                            }
                        });
                    }
                });
                return false;
            }
        },
        watch: {
            source_input_1(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.source_input_1 === curVal) {
                        that.source_input_1_score = 0;
                        var val = parseFloat(that.source_input_1);
                        if (val <= 5.0) {
                            that.source_input_1_score = 0;
                        }
                        else if (val <= 10.0) {
                            that.source_input_1_score = 1;
                        }
                        else if (val <= 15) {
                            that.source_input_1_score = 3;
                        }
                        else if (val > 15) {
                            that.source_input_1_score = 5;
                        }
                        that.source_score = that.source_input_1_score + that.source_input_2_score;
                        if (that.source_score > 10) {
                            that.source_score = 10;
                        }
                    }
                }, 800);
            },
            source_input_2(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.source_input_2 === curVal) {
                        that.source_input_2_score = 0;
                        var val = parseFloat(that.source_input_2);
                        if (val <= 5.0) {
                            that.source_input_2_score = 0;
                        }
                        else if (val <= 10.0) {
                            that.source_input_2_score = 1;
                        }
                        else if (val <= 15) {
                            that.source_input_2_score = 2;
                        }
                        else if (val <= 20) {
                            that.source_input_2_score = 3;
                        }
                        else if (val > 20) {
                            that.source_input_2_score = 5;
                        }
                        that.source_score = that.source_input_1_score + that.source_input_2_score;
                        if (that.source_score > 10) {
                            that.source_score = 10;
                        }
                    }
                }, 800);
            },
            strain_input_1(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.strain_input_1 === curVal) {
                        that.strain_input_1_score = 0;
                        var val = parseInt(that.strain_input_1);
                        if (val < 300) {
                            that.strain_input_1_score = 0;
                        }
                        else if (val < 1000) {
                            that.strain_input_1_score = 3;
                        }
                        else if (val < 2000) {
                            that.strain_input_1_score = 5;
                        }
                        else if (val < 4000) {
                            that.strain_input_1_score = 7;
                        }
                        else if (val >= 4000) {
                            that.strain_input_1_score = 10;
                        }
                        that.strain_score = that.strain_input_1_score + that.strain_input_2_score;
                        if (that.strain_score > 20) {
                            that.strain_score = 20;
                        }
                    }
                }, 800);
            },
            strain_input_2(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.strain_input_2 === curVal) {
                        that.strain_input_2_score = 0;
                        var val = parseInt(that.strain_input_2);
                        if (val < 100) {
                            that.strain_input_2_score = 0;
                        }
                        else if (val < 300) {
                            that.strain_input_2_score = 1;
                        }
                        else if (val < 800) {
                            that.strain_input_2_score = 5;
                        }
                        else if (val < 1500) {
                            that.strain_input_2_score = 7;
                        }
                        else if (val >= 1500) {
                            that.strain_input_2_score = 10;
                        }
                        that.strain_score = that.strain_input_1_score + that.strain_input_2_score;
                        if (that.strain_score > 20) {
                            that.strain_score = 20;
                        }
                    }
                }, 800);
            }
        }
    });

sessionStorage.setItem('htmlFontSize', $('html').css('font-size'));
window.addEventListener('pageshow', function (e) {
    if (e.persisted) {
        $('html').css('font-size', this.sessionStorage.getItem('htmlFontSize'));
    }
});