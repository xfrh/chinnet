Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q21: {
            title: '21. <span style="color: #f63131;">鲍曼不动杆菌</span>检出株数（株/年）？',
            type: 'input',
            name: 'Q21',
            placeholder: '填写检出株数',
            answer: null
        },
        Q22: {
            title: '22. 碳青霉烯类耐药<span style="color: #f63131;">鲍曼不动杆菌(CR-ABA)</span>检出株数（株/年）?',
            type: 'input',
            name: 'Q22',
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
                page: 11
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
            else if (localStorage.getItem('TBSurveyTemp_Page11')) {
                json_data = JSON.parse(localStorage.getItem('TBSurveyTemp_Page11'));
            }

            if (json_data) {
                that.Q21 = json_data.Q21;
                that.Q22 = json_data.Q22;
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
                        var nextUrl = '/survey/page12/' + that.$refs.hidden_id.value;
                        if (that.Q21.answer === '0' || that.Q21.answer === 0 || that.Q22.answer === '0' || that.Q22.answer === 0) {
                            nextUrl = '/survey/page13/' + that.$refs.hidden_id.value;
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
                                        page: 11
                                    },
                                    temp: {
                                        record_id: that.$refs.hidden_id.value,
                                        page: 11,
                                        data: JSON.stringify({
                                            Q21: that.Q21,
                                            Q22: that.Q22
                                        })
                                    },
                                    set: {
                                        Content21: that.Q21.answer,
                                        Content22: that.Q22.answer
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
                                TBSurvey.Content21 = that.Q21.answer;
                                TBSurvey.Content22 = that.Q22.answer;
                                localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                            }

                            localStorage.setItem('TBSurveyTemp_Page11', JSON.stringify({ Q21: that.Q21, Q22: that.Q22 }));
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