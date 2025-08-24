Vue.use(VeeValidate, { locale: 'zh_CN' });

var vm = new Vue({
    el: '#app',
    data: {
        mySwiper: null,
        page_loading: null,
        Q29: {
            title: '29. 医院名称',
            hospital: null
        },
        Q30: {
            title: '30. 您的姓名',
            name: null
        },
        Q31: {
            title: '31. 请输入您的手机号码',
            mobile: null
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
                that.$validator.validateAll().then(async result => {
                    if (result) {

                        var param = [];
                        param.push({ Key: 'Content29', Value: that.Q29.hospital });
                        param.push({ Key: 'Content30', Value: that.Q30.name });
                        param.push({ Key: 'Content31', Value: that.Q31.mobile });
                        var surveyModel;
                        if (window.indexedDB) {
                            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                            jsstoreConn.initDb(window.CHINET_SurveyDB());
                            await jsstoreConn.select({
                                from: 'TBSurvey',
                                where: { Record_Id: that.$refs.hidden_id.value }
                            }).then(function (results) {
                                if (results && results.length) {
                                    try {
                                        surveyModel = results[0];
                                    }
                                    catch (e) {
                                        console.log(e);
                                    }
                                }
                            });
                        }
                        else if (localStorage.getItem('TBSurvey')) {
                            surveyModel = JSON.parse(localStorage.getItem('TBSurvey'));
                        }

                        if (surveyModel) {
                            param.push({ Key: 'Token', Value: surveyModel.Token });
                            param.push({ Key: 'Survey_Id', Value: surveyModel.Survey_Id });
                            param.push({ Key: 'Content1', Value: surveyModel.Content1 });
                            param.push({ Key: 'Content2', Value: surveyModel.Content2 });
                            param.push({ Key: 'Content3', Value: surveyModel.Content3 });
                            param.push({ Key: 'Content4', Value: surveyModel.Content4 });
                            param.push({ Key: 'Content5', Value: surveyModel.Content5 });
                            param.push({ Key: 'Content6', Value: surveyModel.Content6 });
                            param.push({ Key: 'Content7', Value: surveyModel.Content7 });
                            param.push({ Key: 'Content8', Value: surveyModel.Content8 });
                            param.push({ Key: 'Content9', Value: surveyModel.Content9 });
                            param.push({ Key: 'Content10', Value: surveyModel.Content10 });
                            param.push({ Key: 'Content11', Value: surveyModel.Content11 });
                            param.push({ Key: 'Content12', Value: surveyModel.Content12 });
                            param.push({ Key: 'Content13', Value: surveyModel.Content13 });
                            param.push({ Key: 'Content14', Value: surveyModel.Content14 });
                            param.push({ Key: 'Content15', Value: surveyModel.Content15 });
                            param.push({ Key: 'Content16', Value: surveyModel.Content16 });
                            param.push({ Key: 'Content17', Value: surveyModel.Content17 });
                            param.push({ Key: 'Content18', Value: surveyModel.Content18 });
                            param.push({ Key: 'Content19', Value: surveyModel.Content19 });
                            param.push({ Key: 'Content20', Value: surveyModel.Content20 });
                            param.push({ Key: 'Content21', Value: surveyModel.Content21 });
                            param.push({ Key: 'Content22', Value: surveyModel.Content22 });
                            param.push({ Key: 'Content23', Value: surveyModel.Content23 });
                            param.push({ Key: 'Content24', Value: surveyModel.Content24 });
                            param.push({ Key: 'Content25', Value: surveyModel.Content25 });
                            param.push({ Key: 'Content26', Value: surveyModel.Content26 });
                            param.push({ Key: 'Content27', Value: surveyModel.Content27 });
                            param.push({ Key: 'Content28', Value: surveyModel.Content28 });
                        }

                        if (param.length > 0) {
                            // 保存数据
                            that.page_loading = weui.loading('正在提交');
                            axios.post('/Survey/Issue_OnSubmit', param).then(res => {
                                if (that.page_loading) {
                                    setTimeout(function () {
                                        that.page_loading.hide(function () {
                                            that.page_loading = null;
                                        });
                                    }, 50);
                                }

                                if (res.status === 200) {
                                    if (res.data.status) {
                                        // 转至成功页面
                                        window.location.href = res.data.page;
                                    }
                                    else {
                                        weui.alert(res.data.message);
                                    }
                                }
                            }).catch(ex => {
                                if (that.page_loading) {
                                    setTimeout(function () {
                                        that.page_loading.hide(function () {
                                            that.page_loading = null;
                                        });
                                    }, 50);
                                }
                                weui.alert('提交失败');
                            });
                        }
                        else {
                            weui.topTips('提交失败[read data failed]');
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