Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q11: {
            title: '11. <span style="color: #f63131;">CR-ECL</span>主要标本来源有哪些？（列举前三位标本来源及百分比）',
            options: [
                { id: 'checkbox__11_1', name: '_11_biaoben', text: '痰', value: '痰', rate: null, rate_name: '_11_biaoben_1_rate' },
                { id: 'checkbox__11_2', name: '_11_biaoben', text: '血液', value: '血液', rate: null, rate_name: '_11_biaoben_2_rate' },
                { id: 'checkbox__11_3', name: '_11_biaoben', text: '粪便', value: '粪便', rate: null, rate_name: '_11_biaoben_3_rate' },
                { id: 'checkbox__11_4', name: '_11_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液', rate: null, rate_name: '_11_biaoben_4_rate' },
                { id: 'checkbox__11_5', name: '_11_biaoben', text: '脓液/伤口', value: '脓液/伤口', rate: null, rate_name: '_11_biaoben_5_rate' },
                { id: 'checkbox__11_6', name: '_11_biaoben', text: '尿道', value: '尿道', rate: null, rate_name: '_11_biaoben_6_rate' },
                { id: 'checkbox__11_7', name: '_11_biaoben', text: '中心静脉导管', value: '中心静脉导管', rate: null, rate_name: '_11_biaoben_7_rate' },
                { id: 'checkbox__11_8', name: '_11_biaoben', text: '穿刺液', value: '穿刺液', rate: null, rate_name: '_11_biaoben_8_rate' },
                { id: 'checkbox__11_9', name: '_11_biaoben', text: '其他：', value: '其他', rate: null, rate_name: '_11_biaoben_9_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        },
        Q12: {
            title: '12. <span style="color: #f63131;">CR-ECL</span>主要科室来源有哪些？（列举前三位科室来源及百分比）',
            options: [
                { id: 'checkbox__12_1', name: '_12_keshi', text: '血液科', value: '血液科', rate: null, rate_name: '_12_keshi_1_rate' },
                { id: 'checkbox__12_2', name: '_12_keshi', text: 'ICU', value: 'ICU', rate: null, rate_name: '_12_keshi_2_rate' },
                { id: 'checkbox__12_3', name: '_12_keshi', text: '呼吸科', value: '呼吸科', rate: null, rate_name: '_12_keshi_3_rate' },
                { id: 'checkbox__12_4', name: '_12_keshi', text: '感染科', value: '感染科', rate: null, rate_name: '_12_keshi_4_rate' },
                { id: 'checkbox__12_5', name: '_12_keshi', text: '移植科', value: '移植科', rate: null, rate_name: '_12_keshi_5_rate' },
                { id: 'checkbox__12_6', name: '_12_keshi', text: '其他：', value: '其他', rate: null, rate_name: '_12_keshi_6_rate' }
            ],
            answer: [],
            other: null,
            picker: [
                { label: '0-5%', value: '0-5%' },
                { label: '6-10%', value: '6-10%' },
                { label: '11-15%', value: '11-15%' },
                { label: '16-20%', value: '16-20%' },
                { label: '21-30%', value: '21-30%' },
                { label: '31-40%', value: '31-40%' },
                { label: '41-50%', value: '41-50%' },
                { label: '51%以上', value: '51%以上' }
            ]
        }
    },
    watch: {
        'Q11.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q11.answer.length > 3) {
                    that.Q11.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q12.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q12.answer.length > 3) {
                    that.Q12.answer.shift();
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

            if (window.indexedDB) {
                var where = {
                    record_id: that.$refs.hidden_id.value,
                    page: 6
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
            else if (localStorage.getItem('TBSurveyTemp_Page6')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page6'));
            }
            if (json_data) {
                that.Q11 = json_data.Q11;
                that.Q12 = json_data.Q12;
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
        onQ11Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q11.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q11.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },
        onQ12Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q12.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q12.picker, {
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
                        //#region Q11, Q12答案处理
                        var Q11_Answer = [];
                        that.Q11.answer.map(item => {
                            var _item = that.Q11.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q11_Answer.push(item + ':' + that.Q11.other + ':' + _item.rate);
                            }
                            else {
                                Q11_Answer.push(item + ':' + _item.rate);
                            }
                        });

                        var Q12_Answer = [];
                        that.Q12.answer.map(item => {
                            var _item = that.Q12.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q12_Answer.push(item + ':' + that.Q12.other + ':' + _item.rate);
                            }
                            else {
                                Q12_Answer.push(item + ':' + _item.rate);
                            }
                        });
                        //#endregion

                        var nextUrl = '/survey/page7/' + that.$refs.hidden_id.value;
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
                                        page: 6
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 6,
                                        data: JSON.stringify({
                                            Q11: that.Q11,
                                            Q12: that.Q12
                                        })
                                    },
                                    set: {
                                        Content11: Q11_Answer.join(','),
                                        Content12: Q12_Answer.join(',')
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
                                TBSurvey.Content11 = Q11_Answer.join(',');
                                TBSurvey.Content12 = Q12_Answer.join(',');
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page6', JSON.stringify({ Q11: that.Q11, Q12: that.Q12 }));
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