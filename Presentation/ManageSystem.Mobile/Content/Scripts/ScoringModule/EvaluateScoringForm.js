Vue.use(VeeValidate, { locale: 'zh_CN' });

var mySwiper,
    app = new Vue({
        el: '#app',
        data: {
            dataUrl: '/ScoringModule/HospitalList',
            items: [],
            pageConfig: {
                page: 1,
                pageSize: 15
            },
            nextPageLoading: false,
            noNetxPage: false,
            pageNull: false,
            ajaxLoading: false,
            resFailMessage: null,
            dialogContent: null,
            loadingText: '加载中',
            source_score: 0,
            source_input_1: null,
            source_input_1_score: 0,
            source_input_2: null,
            source_input_2_score: 0,
            strain_score: 0,
            strain_input_1: null,
            strain_input_1_score: 0,
            strain_input_2: null,
            strain_input_2_score: 0,
            drugSensitive: {
                total: 0,
                item1: 0,
                item2: 0,
                item3: 0,
                item4: 0,
                item5: 0,
                item6: 0,
                item7: 0
            },
            emp: {
                score: 0,
                total: 0,
                mrsa_score: 0,
                mrsa_score_value: 0,
                vrefm_score: 0,
                vrefm_score_value: 0,
                crkp_score: 0,
                crkp_score_value: 0,
                crpa_score: 0,
                crpa_score_value: 0,
                crab_score: 0,
                crab_score_value: 0,
                ctx_score: 0,
                ctx_score_value: 0
            }
        },
        created: function () {
            var that = this;
            that.$nextTick(() => {
                that.initSwiper();
            });
        },
        mounted: function () {
            var that = this;
            that.$nextTick(() => {
                that.drugSensitive = JSON.parse(that.$refs._drug_sensitive_config.value);
                that.emp = JSON.parse(that.$refs._emphasis_score_config.value);
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
                that.dialogContent = '';
                that.$validator.validateAll({
                    source_input_1: that.$data.source_input_1,
                    source_input_2: that.$data.source_input_2,
                    strain_input_1: that.$data.strain_input_1,
                    strain_input_2: that.$data.strain_input_2,
                    drugSensitive_item1: that.drugSensitive.item1,
                    drugSensitive_item2: that.drugSensitive.item2,
                    drugSensitive_item3: that.drugSensitive.item3,
                    drugSensitive_item4: that.drugSensitive.item4,
                    drugSensitive_item5: that.drugSensitive.item5,
                    drugSensitive_item6: that.drugSensitive.item6,
                    drugSensitive_item7: that.drugSensitive.item7,
                    emp_mrsa_score: that.emp.mrsa_score,
                    emp_vrefm_score: that.emp.vrefm_score,
                    emp_crkp_score: that.emp.crkp_score,
                    emp_crpa_score: that.emp.crpa_score,
                    emp_crab_score: that.emp.crab_score,
                    emp_ctx_score: that.emp.ctx_score
                }).then(result => {
                    if (result) {

                        var _total = that.totalDrugSensitiveScore();
                        if (_total > 30) {
                            that.dialogContent = '药敏品种合理性打分不正确<br />最高总分：30分';
                            $(that.$refs.iosDialog).fadeIn(200);
                            return false;
                        }

                        _total = parseFloat(that.emp.total) || 0;
                        if (_total <= 0 || _total > 50) {
                            that.dialogContent = '重点监测耐药菌<br />最高总分：50分';
                            $(that.$refs.iosDialog).fadeIn(200);
                            return false;
                        }


                        var l = Ladda.create(that.$refs.btnSubmit);
                        if (l.isLoading()) { return false; }
                        var scoredata = [
                            {
                                Key: '标本来源(10分)',
                                Value: [
                                    { item: '门诊患者分离株所占比例', sort: 1, value: that.source_input_1 + '%', score: that.source_input_1_score },
                                    { item: '血液和脑脊液标本分离株来源占比', sort: 2, value: that.source_input_2 + '%', score: that.source_input_2_score }
                                ]
                            },
                            {
                                Key: '菌株数量(20分)',
                                Value: [
                                    { item: '二级医院每年菌株数量', sort: 1, value: that.strain_input_1, score: that.strain_input_1_score },
                                    { item: '三级医院每年菌株数量', sort: 2, value: that.strain_input_2, score: that.strain_input_2_score }
                                ]
                            },
                            {
                                Key: '药敏品种合理性(30分)',
                                Value: [
                                    { item: '大肠埃希菌/肺炎克雷伯菌', sort: 1, value: that.drugSensitive.item1, score: that.drugSensitive.item1 },
                                    { item: '铜绿假单胞菌', sort: 2, value: that.drugSensitive.item2, score: that.drugSensitive.item2 },
                                    { item: '鲍曼不动杆菌', sort: 3, value: that.drugSensitive.item3, score: that.drugSensitive.item3 },
                                    { item: '金黄色葡萄球菌', sort: 4, value: that.drugSensitive.item4, score: that.drugSensitive.item4 },
                                    { item: '肺炎链球菌', sort: 5, value: that.drugSensitive.item5, score: that.drugSensitive.item5 },
                                    { item: '粪肠球菌', sort: 6, value: that.drugSensitive.item6, score: that.drugSensitive.item6 },
                                    { item: '流感嗜血杆菌和卡他莫拉菌', sort: 7, value: that.drugSensitive.item7, score: that.drugSensitive.item7 }
                                ]
                            },
                            {
                                Key: '重点监测耐药菌(50分)',
                                Value: [
                                    { item: '甲氧西林耐药金葡菌', sort: 1, value: that.emp.mrsa_score_value, score: that.emp.mrsa_score },
                                    { item: '万古霉素耐药屎肠球菌', sort: 2, value: that.emp.vrefm_score_value, score: that.emp.vrefm_score },
                                    { item: '碳青霉烯类耐药肺炎克雷伯菌', sort: 3, value: that.emp.crkp_score_value, score: that.emp.crkp_score },
                                    { item: '碳青霉烯类耐药铜绿假单胞菌', sort: 4, value: that.emp.crpa_score_value, score: that.emp.crpa_score },
                                    { item: '碳青霉烯类耐药鲍曼不动杆菌', sort: 5, value: that.emp.crab_score_value, score: that.emp.crab_score },
                                    { item: '头孢噻肟/头孢曲松耐药大肠埃希菌', sort: 6, value: that.emp.ctx_score_value, score: that.emp.ctx_score }
                                ]
                            }
                        ];

                        $.ajax({
                            url: '/ScoringModule/EvaluateScoringForm',
                            type: 'POST',
                            data: {
                                id: that.$refs.hidden_id.value,
                                token: localStorage.getItem('token'),
                                signature: that.$refs.signature.value,
                                timestamp: that.$refs.timestamp.value,
                                nonce: that.$refs.nonce.value,
                                scoredata: scoredata
                                //sourceScore: that.source_score,
                                //strainScore: that.strain_score,
                                //drugAllergyScore: that.drugSensitive.total,
                                //emphasisScore: that.emp.total
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
            },
            totalDrugSensitiveScore() {
                var _total = this.drugSensitive.item1 + this.drugSensitive.item2 + this.drugSensitive.item3 + this.drugSensitive.item4 + this.drugSensitive.item5 + this.drugSensitive.item6 + this.drugSensitive.item7;
                if (_total <= 0) {
                    _total = _total * (-1);
                }
                return _total;
            },
            totalEmpScore() {
                var that = this;
                $.ajax({
                    url: '/ScoringModule/EvaluateScoring_CalculateEmp',
                    type: 'POST',
                    data: {
                        parameter: [
                            parseFloat(that.emp.mrsa_score_value) || 0,
                            parseFloat(that.emp.vrefm_score_value) || 0,
                            parseFloat(that.emp.crkp_score_value) || 0,
                            parseFloat(that.emp.crpa_score_value) || 0,
                            parseFloat(that.emp.crab_score_value) || 0,
                            parseFloat(that.emp.ctx_score_value) || 0
                        ]
                    },
                    dataType: 'JSON',
                    success: function (res) {
                        res = JSON.parse(res);
                        if (res.status) {
                            that.emp.total = res.total;
                            that.emp.score = res.score;
                        }
                        mySwiper.update();
                    }
                });
                //var _total = (parseFloat(that.emp.mrsa_score_value) || 0) + (parseFloat(that.emp.vrefm_score_value) || 0) + (parseFloat(that.emp.crkp_score_value) || 0) + (parseFloat(that.emp.crpa_score_value) || 0) + (parseFloat(that.emp.crab_score_value) || 0) + (parseFloat(that.emp.ctx_score_value) || 0);
                //_total = _total > 50 ? 50 : _total;
                //_total = _total <= 0 ? 0 : _total;
                //console.log(_total);
                //that.emp.total = _total;
            }
        },
        computed: {
            drugSensitive_item1() { return this.drugSensitive.item1; },
            drugSensitive_item2() { return this.drugSensitive.item2; },
            drugSensitive_item3() { return this.drugSensitive.item3; },
            drugSensitive_item4() { return this.drugSensitive.item4; },
            drugSensitive_item5() { return this.drugSensitive.item5; },
            drugSensitive_item6() { return this.drugSensitive.item6; },
            drugSensitive_item7() { return this.drugSensitive.item7; },
            emp_mrsa_score() { return this.emp.mrsa_score; },
            emp_vrefm_score() { return this.emp.vrefm_score; },
            emp_crkp_score() { return this.emp.crkp_score; },
            emp_crpa_score() { return this.emp.crpa_score; },
            emp_crab_score() { return this.emp.crab_score; },
            emp_ctx_score() { return this.emp.ctx_score; }
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
                }, 50);
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
                }, 50);
            },
            strain_input_1(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.strain_input_1 === curVal) {
                        that.strain_input_1_score = 0;
                        var val = parseInt(that.strain_input_1);
                        if (val < 100) {
                            that.strain_input_1_score = 0;
                        }
                        else if (val < 300) {
                            that.strain_input_1_score = 3;
                        }
                        else if (val < 800) {
                            that.strain_input_1_score = 5;
                        }
                        else if (val < 1500) {
                            that.strain_input_1_score = 7;
                        }
                        else if (val >= 1500) {
                            that.strain_input_1_score = 10;
                        }

                        that.strain_score = that.strain_input_1_score + that.strain_input_2_score;
                        if (that.strain_score > 20) {
                            that.strain_score = 20;
                        }
                    }
                }, 80);
            },
            strain_input_2(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.strain_input_2 === curVal) {
                        that.strain_input_2_score = 0;
                        var val = parseInt(that.strain_input_2);
                        if (val < 300) {
                            that.strain_input_2_score = 0;
                        }
                        else if (val < 1000) {
                            that.strain_input_2_score = 3;
                        }
                        else if (val < 2000) {
                            that.strain_input_2_score = 5;
                        }
                        else if (val < 4000) {
                            that.strain_input_2_score = 7;
                        }
                        else if (val >= 4000) {
                            that.strain_input_2_score = 10;
                        }
                        that.strain_score = that.strain_input_1_score + that.strain_input_2_score;
                        if (that.strain_score > 20) {
                            that.strain_score = 20;
                        }
                    }
                }, 80);
            },
            drugSensitive_item1(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item1 === curVal) {
                        var val = parseFloat(that.drugSensitive.item1) || 0;
                        if (val > 0) {
                            that.drugSensitive.item1 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item1 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item2(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item2 === curVal) {
                        var val = parseFloat(that.drugSensitive.item2) || 0;
                        if (val > 0) {
                            that.drugSensitive.item2 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item2 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item3(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item3 === curVal) {
                        var val = parseFloat(that.drugSensitive.item3) || 0;
                        if (val > 0) {
                            that.drugSensitive.item3 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item3 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item4(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item4 === curVal) {
                        var val = parseFloat(that.drugSensitive.item4) || 0;
                        if (val > 0) {
                            that.drugSensitive.item4 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item4 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item5(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item5 === curVal) {
                        var val = parseFloat(that.drugSensitive.item5) || 0;
                        if (val > 0) {
                            that.drugSensitive.item5 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item5 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item6(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item6 === curVal) {
                        var val = parseFloat(that.drugSensitive.item6) || 0;
                        if (val > 0) {
                            that.drugSensitive.item6 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item6 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            drugSensitive_item7(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.drugSensitive.item7 === curVal) {
                        var val = parseFloat(that.drugSensitive.item7) || 0;
                        if (val > 0) {
                            that.drugSensitive.item7 = 0 - val;
                        }
                        else {
                            that.drugSensitive.item7 = val;
                        }
                        that.drugSensitive.total = that.totalDrugSensitiveScore();
                        that.drugSensitive.total = 30 - that.drugSensitive.total > 0 ? 30 - that.drugSensitive.total : 0;
                    }
                }, 80);
            },
            emp_mrsa_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.mrsa_score === curVal) {
                        that.$validator.validate('emp_mrsa_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.mrsa_score) || 0;
                                if (val > 0) {
                                    that.emp.mrsa_score = val;
                                }
                                else {
                                    that.emp.mrsa_score = 0 - val;
                                }
                                that.emp.mrsa_score_value = that.emp.mrsa_score;
                            }
                            else {
                                that.emp.mrsa_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
                    }
                }, 800);
            },
            emp_vrefm_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.vrefm_score === curVal) {
                        that.$validator.validate('emp_vrefm_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.vrefm_score) || 0;
                                if (val > 0) {
                                    that.emp.vrefm_score = val;
                                }
                                else {
                                    that.emp.vrefm_score = 0 - val;
                                }
                                that.emp.vrefm_score_value = that.emp.vrefm_score;
                            }
                            else {
                                that.emp.vrefm_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
                    }
                }, 800);
            },
            emp_crkp_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.crkp_score === curVal) {
                        that.$validator.validate('emp_crkp_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.crkp_score) || 0;
                                if (val > 0) {
                                    that.emp.crkp_score = val;
                                }
                                else {
                                    that.emp.crkp_score = 0 - val;
                                }
                                that.emp.crkp_score_value = that.emp.crkp_score;
                            }
                            else {
                                that.emp.crkp_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
                    }
                }, 800);
            },
            emp_crpa_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.crpa_score === curVal) {
                        that.$validator.validate('emp_crpa_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.crpa_score) || 0;
                                if (val > 0) {
                                    that.emp.crpa_score = val;
                                }
                                else {
                                    that.emp.crpa_score = 0 - val;
                                }
                                that.emp.crpa_score_value = that.emp.crpa_score;
                            }
                            else {
                                that.emp.crpa_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
                    }
                }, 800);
            },
            emp_crab_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.crab_score === curVal) {
                        that.$validator.validate('emp_crab_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.crab_score) || 0;
                                if (val > 0) {
                                    that.emp.crab_score = val;
                                }
                                else {
                                    that.emp.crab_score = 0 - val;
                                }
                                that.emp.crab_score_value = that.emp.crab_score;
                            }
                            else {
                                that.emp.crab_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
                    }
                }, 800);
            },
            emp_ctx_score(curVal, oldVal) {
                var that = this;
                clearTimeout(that.timeout);
                that.timeout = setTimeout(() => {
                    if (that.emp.ctx_score === curVal) {
                        that.$validator.validate('emp_ctx_score').then(result => {
                            if (result) {
                                var val = parseFloat(that.emp.ctx_score) || 0;
                                if (val > 0) {
                                    that.emp.ctx_score = val;
                                }
                                else {
                                    that.emp.ctx_score = 0 - val;
                                }
                                that.emp.ctx_score_value = that.emp.ctx_score;
                            }
                            else {
                                that.emp.ctx_score_value = 0;
                            }
                            that.totalEmpScore();
                        });
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