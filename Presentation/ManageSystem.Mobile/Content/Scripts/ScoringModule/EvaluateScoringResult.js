var mySwiper,
    app = new Vue({
        el: '#app',
        data: {
            dataUrl: '/ScoringModule/HospitalList',
            items: [],
            pageConfig: {
                page: 1,
                pageSize: 15
            },
            nextPageLoading: false,
            noNetxPage: false,
            pageNull: false,
            ajaxLoading: false,
            resFailMessage: null,
            dialogContent: null,
            loadingText: '加载中'
        },
        created: function () {
            var that = this;
            that.$nextTick(() => {

            });
        },
        methods: {
            closeDialog() {
                $(this.$refs.iosDialog).fadeOut(200);
                this.dialogContent = null;
            }
        }
    });

sessionStorage.setItem('htmlFontSize', $('html').css('font-size'));
window.addEventListener('pageshow', function (e) {
    if (e.persisted) {
        $('html').css('font-size', this.sessionStorage.getItem('htmlFontSize'));
    }
});