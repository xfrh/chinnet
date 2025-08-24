Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q17: {
            title: '17. <span style="color: #f63131;">铜绿假单胞菌</span>检出株数（株/年）？',
            type: 'input',
            name: 'Q17',
            placeholder: '填写检出株数',
            answer: null
        },
        Q18: {
            title: '18. 碳青霉烯类耐药<span style="color: #f63131;">铜绿假单胞菌(CR-PAE)</span>检出株数（株/年）?',
            type: 'input',
            name: 'Q18',
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
                page: 9
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
            else if (localStorage.getItem('TBSurveyTemp_Page9')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page9'));
            }

            if (json_data) {
                that.Q17 = json_data.Q17;
                that.Q18 = json_data.Q18;
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
                        var nextUrl = '/survey/page10/' + that.$refs.hidden_id.value;
                        if (that.Q17.answer === '0' || that.Q17.answer === 0 || that.Q18.answer === '0' || that.Q18.answer === 0) {
                            nextUrl = '/survey/page11/' + that.$refs.hidden_id.value;
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
                                        page: 9
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 9,
                                        data: JSON.stringify({
                                            Q17: that.Q17,
                                            Q18: that.Q18
                                        })
                                    },
                                    set: {
                                        Content17: that.Q17.answer,
                                        Content18: that.Q18.answer
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
                                TBSurvey.Content17 = that.Q17.answer;
                                TBSurvey.Content18 = that.Q18.answer;
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page9', JSON.stringify({ Q17: that.Q17, Q18: that.Q18 }));
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