Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        beginAnswer: false,
        showSubmit: false,
        page_loading: null,
        isSubmit: false,
        page: 2,
        /********************** Page 2 *******************************/
        Q3: {
            title: '3. <span style="color: #f63131;">CR-ECO</span>主要标本来源有哪些？（列举前三位标本来源及百分比）',
            options: [
                { id: 'checkbox__3_1', name: '_3_biaoben', text: '痰', value: '痰', rate: null, rate_name: '_3_biaoben_1_rate' },
                { id: 'checkbox__3_2', name: '_3_biaoben', text: '血液', value: '血液', rate: null, rate_name: '_3_biaoben_2_rate' },
                { id: 'checkbox__3_3', name: '_3_biaoben', text: '粪便', value: '粪便', rate: null, rate_name: '_3_biaoben_3_rate' },
                { id: 'checkbox__3_4', name: '_3_biaoben', text: '肺泡灌洗液', value: '肺泡灌洗液', rate: null, rate_name: '_3_biaoben_4_rate' },
                { id: 'checkbox__3_5', name: '_3_biaoben', text: '脓液/伤口', value: '脓液/伤口', rate: null, rate_name: '_3_biaoben_5_rate' },
                { id: 'checkbox__3_6', name: '_3_biaoben', text: '尿道', value: '尿道', rate: null, rate_name: '_3_biaoben_6_rate' },
                { id: 'checkbox__3_7', name: '_3_biaoben', text: '中心静脉导管', value: '中心静脉导管', rate: null, rate_name: '_3_biaoben_7_rate' },
                { id: 'checkbox__3_8', name: '_3_biaoben', text: '穿刺液', value: '穿刺液', rate: null, rate_name: '_3_biaoben_8_rate' },
                { id: 'checkbox__3_9', name: '_3_biaoben', text: '其他：', value: '其他', rate: null, rate_name: '_3_biaoben_9_rate' }
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
        Q4: {
            title: '4. <span style="color: #f63131;">CR-ECO</span>主要科室来源有哪些？（列举前三位科室来源及百分比）',
            options: [
                { id: 'checkbox__4_1', name: '_4_keshi', text: '血液科', value: '血液科', rate: null, rate_name: '_4_keshi_1_rate' },
                { id: 'checkbox__4_2', name: '_4_keshi', text: 'ICU', value: 'ICU', rate: null, rate_name: '_4_keshi_2_rate' },
                { id: 'checkbox__4_3', name: '_4_keshi', text: '呼吸科', value: '呼吸科', rate: null, rate_name: '_4_keshi_3_rate' },
                { id: 'checkbox__4_4', name: '_4_keshi', text: '感染科', value: '感染科', rate: null, rate_name: '_4_keshi_4_rate' },
                { id: 'checkbox__4_5', name: '_4_keshi', text: '移植科', value: '移植科', rate: null, rate_name: '_4_keshi_5_rate' },
                { id: 'checkbox__4_6', name: '_4_keshi', text: '其他：', value: '其他', rate: null, rate_name: '_4_keshi_6_rate' }
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
        'Q3.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q3.answer.length > 3) {
                    that.Q3.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        },
        'Q4.answer': function (newVal, oldVal) {
            var that = this;
            that.$nextTick(function () {
                if (that.Q4.answer.length > 3) {
                    that.Q4.answer.shift();
                }
                setTimeout(function () {
                    that.mySwiper.updateSlides();
                }, 50);
            });
        }
    },
    created: function () {
        var that = this;
        that.$nextTick(function () {
            that.page_loading = weui.loading('页面加载中');
            if (localStorage.getItem('survey_dom_title')) {
                document.title = localStorage.getItem('survey_dom_title');
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
                        if (window.indexedDB) {
                            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                            jsstoreConn.initDb(getDbSchema('CHINET_Survey'));
                            jsstoreConn.select({
                                from: 'TBSurveyTemp',
                                where: {
                                    record_id: that.$refs.hidden_id.value,
                                    page: that.page
                                }
                            }).then(function (results) {
                                if (results && results.length) {
                                    try {
                                        var json_data = JSON.parse(results[0].data);
                                        that.Q3 = json_data.Q3;
                                        that.Q4 = json_data.Q4;
                                    }
                                    catch (e) {
                                        console.log(e);
                                    }
                                }
                            });
                        }
                        else {
                            var dataBase = window.openDatabase('CHINET_Survey', '1.0.0', 'CHINET问卷', 4 * 1024 * 1024);
                            dataBase.transaction(function (trans) {
                                trans.executeSql("SELECT * FROM TBSurveyTemp WHERE id = ? AND page = ?", [that.$refs.hidden_id.value, 'page1'], function (ts, ts_data) {
                                    if (ts_data) {
                                        if (ts_data.rows.length > 0) {
                                            try {
                                                var json_data = JSON.parse(ts_data.rows.item(0).data);
                                                that.Q3 = json_data.Q3;
                                                that.Q4 = json_data.Q4;
                                            }
                                            catch (e) {
                                                console.log(e);
                                            }
                                        }
                                    }
                                });
                            });
                        }

                        that.page_loading.hide(function () {
                            that.page_loading = null;
                        });
                    }
                }
            });
        });
    },
    methods: {
        onQ3Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q3.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q3.picker, {
                    container: 'body',
                    defaultValue: [defaultValue],
                    onConfirm: function (result) {
                        obj.rate = result[0].value;
                    },
                    id: window.uuid(8, 18)
                });
            });
        },
        onQ4Picker: function (e, obj) {
            var that = this;
            that.$nextTick(function () {
                var defaultValue = that.Q4.picker[0].value;
                if (obj.rate && obj.rate.length > 0) {
                    defaultValue = obj.rate;
                }

                weui.picker(that.Q4.picker, {
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

                        //#region 多选验证 Q3-Q4
                        if (that.Q3.answer.length === 0) {
                            weui.topTips('3题未作答');
                            return false;
                        }
                        if (that.Q3.answer.length !== 3) {
                            weui.topTips('3题必须选择3项标本来源');
                            return false;
                        }
                        if (that.Q4.answer.length === 0) {
                            weui.topTips('4题未作答');
                            return false;
                        }
                        if (that.Q4.answer.length !== 3) {
                            weui.topTips('4题必须选择3项科室来源');
                            return false;
                        }
                        //#endregion

                        //#region Q3, Q4答案处理
                        var Q3_Answer = [];
                        that.Q3.answer.map(item => {
                            var _item = that.Q3.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q3_Answer.push(item + ':' + that.Q3.other + ':' + _item.rate);
                            }
                            else {
                                Q3_Answer.push(item + ':' + _item.rate);
                            }
                        });

                        var Q4_Answer = [];
                        that.Q4.answer.map(item => {
                            var _item = that.Q4.options.find(function (value) {
                                return value.value === item;
                            });
                            if (item === '其他') {
                                // 其他:名称:百分比
                                Q4_Answer.push(item + ':' + that.Q4.other + ':' + _item.rate);
                            }
                            else {
                                Q4_Answer.push(item + ':' + _item.rate);
                            }
                        });
                        //#endregion

                        if (window.indexedDB) {
                            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                            jsstoreConn.initDb(getDbSchema('CHINET_Survey'));

                            var results = jsstoreConn.transaction({
                                tables: ['TBSurveyTemp', 'TBSurvey'], // list of tables which will be used in transaction
                                logic: async function (ctx) {  // async is used to make code more clear
                                    start(); // start the transaction
                                    var temps = await ctx.select({
                                        from: 'TBSurveyTemp',
                                        where: {
                                            record_id: ctx.data.Record_Id,
                                            page: 1
                                        }
                                    });
                                    if (!temps[0]) {
                                        // insert TBSurveyTemp
                                        ctx.insert({
                                            into: 'TBSurveyTemp',
                                            values: [ctx.data.TBSurveyTemp],
                                            return: true
                                        });
                                    }
                                    else {
                                        // insert TBSurveyTemp
                                        ctx.update({
                                            in: 'TBSurveyTemp',
                                            where: {
                                                record_id: ctx.data.Record_Id,
                                                page: 1
                                            },
                                            set: {
                                                data: ctx.data.TBSurveyTemp.data
                                            }
                                        });
                                    }

                                    // update TBSurvey
                                    ctx.update({
                                        in: 'TBSurvey',
                                        where: { record_id: ctx.data.Record_Id },
                                        set: {
                                            Content1: ctx.data.Content1,
                                            Content2: ctx.data.Content2,
                                            Content3: ctx.data.Content3,
                                            Content4: ctx.data.Content4
                                        }
                                    });
                                    setResult('status', true);
                                },
                                data: {
                                    TBSurveyTemp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 1,
                                        data: JSON.stringify({ Q1: that.Q1, Q2: that.Q2, Q3: that.Q3, Q4: that.Q4 })
                                    },
                                    Record_Id: that.$refs.hidden_id.value,
                                    Content1: that.Q1.answer,
                                    Content2: that.Q2.answer,
                                    Content3: Q3_Answer.join(','),
                                    Content4: Q4_Answer.join(',')
                                }
                            });
                            results.then(function (r) {
                                if (r.status) {
                                    window.location.href = '/survey/page2/' + that.$refs.hidden_id.value;
                                }
                            });
                        }
                        else {
                            var dataBase = window.openDatabase('CHINET_Survey', '1.0.0', 'CHINET问卷', 4 * 1024 * 1024);
                            dataBase.transaction(function (trans) {
                                trans.executeSql('CREATE TABLE IF NOT EXISTS TBSurveyTemp( id TEXT NOT NULL, page TEXT NULL, data TEXT NULL )', [], function () {
                                    trans.executeSql('DELETE FROM TBSurveyTemp WHERE id = ? AND page = ?', [that.$refs.hidden_id.value, 'page1']);
                                    trans.executeSql("INSERT INTO TBSurveyTemp (id, page, data) VALUES (?, ?, ?)", [that.$refs.hidden_id.value, 'page1', JSON.stringify({ Q1: that.Q1, Q2: that.Q2, Q3: that.Q3, Q4: that.Q4 })]);
                                });

                                trans.executeSql('UPDATE TBSurvey SET Content1 = ?, Content2 = ?, Content3 = ?, Content4 = ? WHERE id = ?;', [
                                    that.Q1.answer,
                                    that.Q2.answer,
                                    Q3_Answer.join(','),
                                    Q4_Answer.join(','),
                                    that.$refs.hidden_id.value
                                ], function () {
                                    window.location.href = '/survey/page2/' + that.$refs.hidden_id.value;
                                }, function () {
                                    weui.topTips('数据处理失败');
                                });
                            });
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
        isWeiXin() {
            if (typeof window.WeixinJSBridge === 'undefined' || typeof window.WeixinJSBridge.invoke === 'undefined') {
                return false;
            }
            else {
                return true;
            }
        },
        onClosePage() {
            var that = this;
            if (that.isWeiXin()) {
                WeixinJSBridge.call('closeWindow');
            }
            else {
                window.location.href = "about:blank";
            }
        }
    }
});