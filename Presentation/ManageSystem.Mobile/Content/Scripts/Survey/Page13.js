Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q25: {
            title: '25. 目前实验室检测碳青霉烯酶的方法',
            type: 'checkbox',
            name: 'Q25',
            options: [
                { id: 'checkbox__25_1', name: 'Q25_checkbox', text: '(1) 仅以药敏试验结果判断CRE', value: '(1) 仅以药敏试验结果判断CRE' },
                { id: 'checkbox__25_2', name: 'Q25_checkbox', text: '(2) 改良Hodge试验', value: '(2) 改良Hodge试验' },
                { id: 'checkbox__25_3', name: 'Q25_checkbox', text: '(3) Carba NP', value: '(3) Carba NP' },
                { id: 'checkbox__25_4', name: 'Q25_checkbox', text: '(4) mCIM和eCIM', value: '(4) mCIM和eCIM' },
                { id: 'checkbox__25_5', name: 'Q25_checkbox', text: '(5) EDTA和APB抑制试验', value: '(5) EDTA和APB抑制试验' },
                { id: 'checkbox__25_6', name: 'Q25_checkbox', text: '(6) 金标免疫快速检测技术', value: '(6) 金标免疫快速检测技术' },
                { id: 'checkbox__25_7', name: 'Q25_checkbox', text: '(7) 常规PCR技术', value: '(7) 常规PCR技术' },
                { id: 'checkbox__25_8', name: 'Q25_checkbox', text: '(8) GeneXpert', value: '(8) GeneXpert' },
                { id: 'checkbox__25_9', name: 'Q25_checkbox', text: '(9) 国产碳青霉烯酶基因检测试剂盒', value: '(9) 国产碳青霉烯酶基因检测试剂盒' }
            ],
            answer: []
        },
        Q26: {
            title: '26. 药敏试验是否常规报告CRE所产碳青霉烯酶型别？',
            type: 'radio',
            name: 'Q26',
            options: [
                { id: 'radio_26_1', name: 'Q26_radio', text: '否', value: '否' },
                { id: 'radio_26_2', name: 'Q26_radio', text: '是', value: '是' }
            ],
            answer: null,
            other: null
        }
    },
    watch: {
        'Q26.answer': function (ov, nv) {
            var that = this;
            that.$nextTick(function () {
                that.mySwiper.updateSlides();

                if (that.mySwiper.height > that.mySwiper.slides[that.mySwiper.activeIndex].offsetHeight + 45) {
                    document.querySelector('footer.footer').classList.remove('flex');

                    document.querySelector('footer.footer').style.left = '-2px';
                    document.querySelector('footer.footer').style.top = document.documentElement.clientHeight - 45 + 'px';
                    document.querySelector('footer.footer').style.width = document.documentElement.clientWidth + 2 + 'px';
                    document.querySelector('footer.footer').classList.add('footer-fixed');
                }
                else {
                    document.querySelector('footer.footer').classList.remove('footer-fixed');
                    document.querySelector('footer.footer').style.left = 'initial';
                    document.querySelector('footer.footer').style.top = 'initial';
                    document.querySelector('footer.footer').classList.add('flex');
                }

                if (that.mySwiper.slides[that.mySwiper.activeIndex].offsetHeight > document.documentElement.clientHeight) {
                    that.mySwiper.setTranslate(0 - that.mySwiper.slides[that.mySwiper.activeIndex].offsetHeight + document.documentElement.clientHeight);
                }

                that.mySwiper.updateSlides();
            });
        }
    },
    created: function () {
        var that = this;
        that.$nextTick(async function () {
            that.page_loading = weui.loading('页面加载中');
            if (localStorage.getItem('survey_dom_title')) {
                document.title = localStorage.getItem('survey_dom_title');
            }
            var json_data;

            if (window.indexedDB) {
                var where = {
                    record_id: that.$refs.hidden_id.value,
                    page: 13
                };
                var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                jsstoreConn.initDb(window.CHINET_SurveyDB());
                await jsstoreConn.select({
                    from: 'TBSurveyTemp',
                    where: where
                }).then(function (results) {
                    if (results && results.length) {
                        try {
                            json_data = JSON.parse(results[0].data);
                        }
                        catch (e) {
                            console.log(e);
                        }
                    }
                });
            }
            else if (localStorage.getItem('TBSurveyTemp_Page13')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page13'));
            }
            if (json_data) {
                that.Q25 = json_data.Q25;
                that.Q26 = json_data.Q26;
            }
        });
    },
    mounted: function () {
        var that = this;
        that.$nextTick(function () {
            that.mySwiper = new Swiper('#page', {
                direction: 'vertical',
                freeMode: true,
                observer: true,
                observeSlideChildren: true,
                slidesPerView: 'auto',
                mousewheel: {
                    releaseOnEdges: true
                },
                watchSlidesProgress: true,
                resistanceRatio: 0,
                on: {
                    init: function () {
                        var thatSwiper = this;

                        thatSwiper.updateSlides();

                        that.page_loading.hide(function () {
                            that.page_loading = null;
                        });
                    }
                }
            });
        });
    },
    methods: {
        onOver() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        //#region 多选验证 Q25
                        if (that.Q25.answer.length === 0) {
                            weui.topTips('25题未作答');
                            return false;
                        }
                        //#endregion

                        //#region 26题答案处理
                        var Q26_Answer = null;
                        if (that.Q26.answer === '是') {
                            Q26_Answer = '是,采用方法:' + that.Q26.other;
                        }
                        else {
                            Q26_Answer = '否';
                        }
                        //#endregion


                        var nextUrl = '/survey/page15/' + that.$refs.hidden_id.value;
                        if (window.indexedDB) {
                            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                            jsstoreConn.initDb(window.CHINET_SurveyDB());

                            jsstoreConn.transaction({
                                tables: ['TBSurveyTemp', 'TBSurvey'],
                                logic: async function (ctx) {
                                    start();
                                    var temps = await ctx.select({
                                        from: ctx.data.tableTemp,
                                        where: ctx.data.where
                                    });

                                    if (!temps[0]) {
                                        // insert TBSurveyTemp
                                        ctx.insert({
                                            into: ctx.data.tableTemp,
                                            values: [ctx.data.temp],
                                            return: true
                                        });
                                    }
                                    else {
                                        // insert TBSurveyTemp
                                        ctx.update({
                                            in: ctx.data.tableTemp,
                                            where: ctx.data.where,
                                            set: {
                                                data: ctx.data.temp.data
                                            }
                                        });
                                    }

                                    // update TBSurvey
                                    ctx.update({
                                        in: ctx.data.tableName,
                                        where: { Record_Id: ctx.data.temp.record_id },
                                        set: ctx.data.set
                                    });
                                    setResult('status', true);
                                },
                                data: {
                                    tableTemp: 'TBSurveyTemp',
                                    tableName: 'TBSurvey',
                                    where: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 13
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 13,
                                        data: JSON.stringify({
                                            Q25: that.Q25,
                                            Q26: that.Q26
                                        })
                                    },
                                    set: {
                                        Content25: that.Q25.answer.join(','),
                                        Content26: Q26_Answer
                                    }
                                }
                            }).then(function (r) {
                                if (r.status) {
                                    window.location.href = nextUrl;
                                }
                            });
                        }
                        else {
                            if (localStorage.getItem('TBSurvey')) {
                                var TBSurvey = JSON.parse(localStorage.getItem('TBSurvey'));
                                TBSurvey.Content25 = that.Q25.answer.join(',');
                                TBSurvey.Content26 = Q26_Answer;
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page13', JSON.stringify({ Q25: that.Q25, Q26: that.Q26 }));
                            window.location.href = nextUrl;
                        }
                    }
                    else {
                        if (that.page_loading) {
                            setTimeout(function () {
                                that.page_loading.hide(function () {
                                    that.page_loading = null;
                                });
                            }, 50);
                        }
                        weui.topTips('请完善答题信息');
                    }
                });
            });
        },
        onSubmit() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        //#region 26题答案处理
                        var Q26_Answer = null;
                        if (that.Q26.answer === '是') {
                            Q26_Answer = '是,采用方法:' + that.Q26.other;
                        }
                        else {
                            Q26_Answer = '否';
                        }
                        //#endregion

                        var nextUrl = '/survey/page14/' + that.$refs.hidden_id.value;
                        if (window.indexedDB) {
                            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                            jsstoreConn.initDb(window.CHINET_SurveyDB());

                            jsstoreConn.transaction({
                                tables: ['TBSurveyTemp', 'TBSurvey'],
                                logic: async function (ctx) {
                                    start();
                                    var temps = await ctx.select({
                                        from: ctx.data.tableTemp,
                                        where: ctx.data.where
                                    });

                                    if (!temps[0]) {
                                        // insert TBSurveyTemp
                                        ctx.insert({
                                            into: ctx.data.tableTemp,
                                            values: [ctx.data.temp],
                                            return: true
                                        });
                                    }
                                    else {
                                        // insert TBSurveyTemp
                                        ctx.update({
                                            in: ctx.data.tableTemp,
                                            where: ctx.data.where,
                                            set: {
                                                data: ctx.data.temp.data
                                            }
                                        });
                                    }

                                    // update TBSurvey
                                    ctx.update({
                                        in: ctx.data.tableName,
                                        where: { Record_Id: ctx.data.temp.record_id },
                                        set: ctx.data.set
                                    });
                                    setResult('status', true);
                                },
                                data: {
                                    tableTemp: 'TBSurveyTemp',
                                    tableName: 'TBSurvey',
                                    where: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 13
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 13,
                                        data: JSON.stringify({
                                            Q25: that.Q25,
                                            Q26: that.Q26
                                        })
                                    },
                                    set: {
                                        Content25: that.Q25.answer.join(','),
                                        Content26: Q26_Answer
                                    }
                                }
                            }).then(function (r) {
                                if (r.status) {
                                    window.location.href = nextUrl;
                                }
                            });
                        }
                        else {
                            if (localStorage.getItem('TBSurvey')) {
                                var TBSurvey = JSON.parse(localStorage.getItem('TBSurvey'));
                                TBSurvey.Content25 = that.Q25.answer.join(',');
                                TBSurvey.Content26 = Q26_Answer;
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page13', JSON.stringify({ Q25: that.Q25, Q26: that.Q26 }));
                            window.location.href = nextUrl;
                        }
                    }
                    else {
                        if (that.page_loading) {
                            setTimeout(function () {
                                that.page_loading.hide(function () {
                                    that.page_loading = null;
                                });
                            }, 50);
                        }
                        weui.topTips('请完善答题信息');
                    }
                });
            });
        }
    }
});