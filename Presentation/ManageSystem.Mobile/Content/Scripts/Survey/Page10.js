Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q19: {
            title: '19. <span style="color: #f63131;">CR-PAE</span>主要标本来源有哪些？（列举前三位标本来源及百分比）',
            options: [
                { id: 'checkbox__19_1', name: '_19_biaoben', text: '痰', value: '痰', rate: null, rate_name: '_19_biaoben_1_rate' },
                { id: 'checkbox__19_2', name: '_19_biaoben', text: '血液', value: '血液', rate: null, rate_name: '_19_biaoben_2_rate' },
                { id: 'checkbox__19_3', name: '_19_biaoben', text: '粪便', value: '粪便', rate: null, rate_name: '_19_biaoben_3_rate' },
                { id: 'checkbox__19_4', name: '_19_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液', rate: null, rate_name: '_19_biaoben_4_rate' },
                { id: 'checkbox__19_5', name: '_19_biaoben', text: '脓液/伤口', value: '脓液/伤口', rate: null, rate_name: '_19_biaoben_5_rate' },
                { id: 'checkbox__19_6', name: '_19_biaoben', text: '尿道', value: '尿道', rate: null, rate_name: '_19_biaoben_6_rate' },
                { id: 'checkbox__19_7', name: '_19_biaoben', text: '中心静脉导管', value: '中心静脉导管', rate: null, rate_name: '_19_biaoben_7_rate' },
                { id: 'checkbox__19_8', name: '_19_biaoben', text: '穿刺液', value: '穿刺液', rate: null, rate_name: '_19_biaoben_8_rate' },
                { id: 'checkbox__19_9', name: '_19_biaoben', text: '其他：', value: '其他', rate: null, rate_name: '_19_biaoben_9_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        },
        Q20: {
            title: '20. <span style="color: #f63131;">CR-PAE</span>主要科室来源有哪些？（列举前三位科室来源及百分比）',
            options: [
                { id: 'checkbox__20_1', name: '_20_keshi', text: '血液科', value: '血液科', rate: null, rate_name: '_20_keshi_1_rate' },
                { id: 'checkbox__20_2', name: '_20_keshi', text: 'ICU', value: 'ICU', rate: null, rate_name: '_20_keshi_2_rate' },
                { id: 'checkbox__20_3', name: '_20_keshi', text: '呼吸科', value: '呼吸科', rate: null, rate_name: '_20_keshi_3_rate' },
                { id: 'checkbox__20_4', name: '_20_keshi', text: '感染科', value: '感染科', rate: null, rate_name: '_20_keshi_4_rate' },
                { id: 'checkbox__20_5', name: '_20_keshi', text: '移植科', value: '移植科', rate: null, rate_name: '_20_keshi_5_rate' },
                { id: 'checkbox__20_6', name: '_20_keshi', text: '其他：', value: '其他', rate: null, rate_name: '_20_keshi_6_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '10-20%', value: '10-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51-60%', value: '51-60%' },
                { label: '61-70%', value: '61-70%' },
                { label: '71%以上', value: '71%以上' }
            ]
        }
    },
    watch: {
        'Q19.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q19.answer.length > 3) {
                    that.Q19.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q20.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q20.answer.length > 3) {
                    that.Q20.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
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
            var where = {
                record_id: that.$refs.hidden_id.value,
                page: 10
            };

            if (window.indexedDB) {
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
            else if (localStorage.getItem('TBSurveyTemp_Page10')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page10'));
            }
            if (json_data) {
                that.Q19 = json_data.Q19;
                that.Q20 = json_data.Q20;
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
                        that.page_loading.hide(function () {
                            that.page_loading = null;
                        });
                    }
                }
            });
        });
    },
    methods: {
        onQ19Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q19.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q19.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },
        onQ20Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q20.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q20.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },
        onSubmit() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        //#region Q19, Q20答案处理
                        var Q19_Answer = [];
                        that.Q19.answer.map(item => {
                            var _item = that.Q19.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q19_Answer.push(item + ':' + that.Q19.other + ':' + _item.rate);
                            }
                            else {
                                Q19_Answer.push(item + ':' + _item.rate);
                            }
                        });

                        var Q20_Answer = [];
                        that.Q20.answer.map(item => {
                            var _item = that.Q20.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q20_Answer.push(item + ':' + that.Q20.other + ':' + _item.rate);
                            }
                            else {
                                Q20_Answer.push(item + ':' + _item.rate);
                            }
                        });
                        //#endregion

                        var nextUrl = '/survey/page11/' + that.$refs.hidden_id.value;
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
                                        page: 10
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 10,
                                        data: JSON.stringify({
                                            Q19: that.Q19,
                                            Q20: that.Q20
                                        })
                                    },
                                    set: {
                                        Content19: Q19_Answer.join(','),
                                        Content20: Q20_Answer.join(',')
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
                                TBSurvey.Content19 = Q19_Answer.join(',');
                                TBSurvey.Content20 = Q20_Answer.join(',');
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page10', JSON.stringify({ Q19: that.Q19, Q20: that.Q20 }));
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