Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q1: {
            title: '1. <span style="color: #f63131;">大肠埃希菌</span>检出株数（株/年）？',
            type: 'input',
            name: 'Q1',
            placeholder: '填写检出株数',
            answer: null
        },
        Q2: {
            title: '2. 碳青霉烯类耐药<span style="color: #f63131;">大肠埃希菌(CR-ECO)</span>检出株数（株/年）?',
            type: 'input',
            name: 'Q2',
            placeholder: '填写检出株数',
            answer: null
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
                page: 1
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
            else if (localStorage.getItem('TBSurveyTemp_Page1')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page1'));
            }

            if (json_data) {
                that.Q1 = json_data.Q1;
                that.Q2 = json_data.Q2;
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
        onSubmit() {
            var that = this;
            that.$nextTick(function () {
                that.$validator.validateAll().then(result => {
                    if (result) {

                        var nextUrl = '/survey/page2/' + that.$refs.hidden_id.value;
                        if (that.Q1.answer === '0' || that.Q1.answer === 0 || that.Q2.answer === '0' || that.Q2.answer === 0) {
                            nextUrl = '/survey/page3/' + that.$refs.hidden_id.value;
                        }

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
                                        page: 1
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 1,
                                        data: JSON.stringify({
                                            Q1: that.Q1,
                                            Q2: that.Q2
                                        })
                                    },
                                    set: {
                                        Content1: that.Q1.answer,
                                        Content2: that.Q2.answer
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
                                TBSurvey.Content1 = that.Q1.answer;
                                TBSurvey.Content2 = that.Q2.answer;
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page1', JSON.stringify({ Q1: that.Q1, Q2: that.Q2 }));
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
        onBlur() {
            var that = this;
            that.$nextTick(function () {
                if (that.Q1.answer === '0') {
                    if (window.indexedDB) {
                        var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                        jsstoreConn.initDb(window.CHINET_SurveyDB());

                        jsstoreConn.transaction({
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
                        }).then(function (r) {
                            if (r.status) {
                                window.location.href = nextUrl;
                            }
                        });
                    }
                    else {
                        if (localStorage.getItem('TBSurvey')) {
                            var TBSurvey = JSON.parse(localStorage.getItem('TBSurvey'));
                            TBSurvey.Content1 = that.Q1.answer;
                            TBSurvey.Content2 = that.Q2.answer;
                            localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                        }

                        localStorage.setItem('TBSurveyTemp_Page1', JSON.stringify({ Q1: that.Q1, Q2: that.Q2 }));
                        window.location.href = nextUrl;
                    }
                }
                if (that.Q2.answer === '0') {
                    console.log(2);
                }
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