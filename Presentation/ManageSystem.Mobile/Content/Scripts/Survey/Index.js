var vm = new Vue({
    el: '#index',
    data: {
        token: null
    },
    created: function () {
        var that = this;
        that.$nextTick(function () {
            if (that.$refs.need_login.value === '1' || that.$refs.need_login.value === 1) {
                that.token = localStorage.getItem('token') || null;
                if (!that.token || that.token === null || that.token === 'null' || typeof that.token === 'undefined') {
                    window.location.href = '/login.cshtml?ret_url=/survey?id=' + that.$refs.hidden_survey_id.value;
                }
            }

            localStorage.setItem('survey_dom_title', that.$refs.hidden_dom_title.value);

            CHINET_Survey.DropDB();
            localStorage.removeItem('TBSurvey');
        });
    },
    mounted: function () {
        var that = this;
        var mySwiper = new Swiper('#index', {
            direction: 'vertical',
            freeMode: true,
            observer: true,
            observeSlideChildren: true,
            slidesPerView: 'auto',
            mousewheel: { releaseOnEdges: true },
            watchSlidesProgress: true,
            resistanceRatio: 0,
            on: {
                init: function () {
                    var thatSwiper = this;
                    thatSwiper.updateSlides();
                }
            }
        });
    },
    methods: {
        onNext() {
            var that = this;
            that.$nextTick(function () {
                var TBSurvey = {
                    Token: localStorage.getItem('token'),
                    Record_Id: that.$refs.hidden_id.value,
                    Survey_Id: that.$refs.hidden_survey_id.value,
                    Content1: null,
                    Content2: null,
                    Content3: null,
                    Content4: null,
                    Content5: null,
                    Content6: null,
                    Content7: null,
                    Content8: null,
                    Content9: null,
                    Content10: null,
                    Content11: null,
                    Content12: null,
                    Content13: null,
                    Content14: null,
                    Content15: null,
                    Content16: null,
                    Content17: null,
                    Content18: null,
                    Content19: null,
                    Content20: null,
                    Content21: null,
                    Content22: null,
                    Content23: null,
                    Content24: null,
                    Content25: null,
                    Content26: null,
                    Content27: null,
                    Content28: null,
                    Content29: null,
                    Content30: null,
                    Content31: null
                };
                var nextUrl = '/survey/page1/' + TBSurvey.Record_Id;

                if (window.indexedDB) {
                    var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
                    jsstoreConn.initDb(window.CHINET_SurveyDB());
                    jsstoreConn.insert({
                        into: 'TBSurvey',
                        values: [TBSurvey]
                    }).then(result => {
                        if (result === 1) {
                            window.location.href = nextUrl;
                        }
                    });
                }
                else {
                    localStorage.setItem('TBSurvey', JSON.stringify(TBSurvey));
                    window.location.href = nextUrl;
                }
            });
        },
        getQueryString(name) {
            var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
            var r = window.location.search.substr(1).match(reg);
            if (r !== null) {
                return unescape(r[2]);
            }
            return null;
        }
    }
});

