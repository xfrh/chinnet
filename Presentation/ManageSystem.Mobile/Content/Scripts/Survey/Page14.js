Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q27: {
            title: '27. 您单位分离的CRE主要产哪种碳青霉烯酶及所占比例？',
            type: 'radio',
            name: 'Q27',
            options: [
                { id: 'radio__27_1', name: 'Q27_radio', text: '(1) KPC 及所占比例%：', value: '(1) KPC 及所占比例：', name_input: 'Q27_input_1', rate: null },
                { id: 'radio__27_2', name: 'Q27_radio', text: '(2) NDM 及所占比例%：', value: '(2) NDM 及所占比例：', name_input: 'Q27_input_2', rate: null },
                { id: 'radio__27_3', name: 'Q27_radio', text: '(3) OXA-48 及所占比例%：', value: '(3) OXA-48 及所占比例：', name_input: 'Q27_input_3', rate: null },
                { id: 'radio__27_4', name: 'Q27_radio', text: '(4) IPM 及所占比例%：', value: '(4) IPM 及所占比例：', name_input: 'Q27_input_4', rate: null },
                { id: 'radio__27_5', name: 'Q27_radio', text: '(5) VIM 及所占比例%：', value: '(5) VIM 及所占比例：', name_input: 'Q27_input_5', rate: null },
                { id: 'radio__27_6', name: 'Q27_radio', text: '(6) 其他 及所占比例%：', value: '(6) 其他 及所占比例：', name_input: 'Q27_input_6', rate: null }
            ],
            answer: []
        },
        Q28: {
            title: '28. 实验室分离到CRE菌株时，补充加做以下哪些药物的药敏试验？',
            type: 'checkbox',
            name: 'Q28',
            options: [
                { id: 'checkbox__28_1', name: 'Q28_checkbox', text: '(1) 多黏菌素', value: '(1) 多黏菌素' },
                { id: 'checkbox__28_2', name: 'Q28_checkbox', text: '(2) 替加环素', value: '(2) 替加环素' },
                { id: 'checkbox__28_3', name: 'Q28_checkbox', text: '(3) 头孢他啶-阿维巴坦', value: '(3) 头孢他啶-阿维巴坦' },
                { id: 'checkbox__28_4', name: 'Q28_checkbox', text: '(4) 磷霉素', value: '(4) 磷霉素' },
                { id: 'checkbox__28_5', name: 'Q28_checkbox', text: '(5) 氯霉素', value: '(5) 氯霉素' },
                { id: 'checkbox__28_6', name: 'Q28_checkbox', text: '(6) 联合药敏试验', value: '(6) 联合药敏试验' },
                { id: 'checkbox__28_7', name: 'Q28_checkbox', text: '(7) 未补充其他药物', value: '(7) 未补充其他药物' }
            ],
            answer: []
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
                    page: 14
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
            else if (localStorage.getItem('TBSurveyTemp_Page14')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page14'));
            }
            if (json_data) {
                that.Q27 = json_data.Q27;
                that.Q28 = json_data.Q28;
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
        onSubmit() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        //#region 27题答案处理
                        var Q27_Answer = null;
                        var Q27_checked = that.Q27.options.filter(function (item, index) {
                            return item.value === that.Q27.answer;
                        });
                        Q27_Answer = that.Q27.answer + Q27_checked[0].rate + '%';
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
                                        page: 14
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 14,
                                        data: JSON.stringify({
                                            Q27: that.Q27,
                                            Q28: that.Q28
                                        })
                                    },
                                    set: {
                                        Content27: Q27_Answer,
                                        Content28: that.Q28.answer.join(',')
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
                                TBSurvey.Content27 = Q27_Answer;
                                TBSurvey.Content28 = that.Q28.answer.join(',');
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page14', JSON.stringify({ Q27: that.Q27, Q28: that.Q28 }));
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