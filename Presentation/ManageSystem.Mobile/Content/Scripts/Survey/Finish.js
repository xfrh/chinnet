
var vm = new Vue({
    el: '#app',
    created: function () {
        var that = this;
        that.$nextTick(function () {
            if (localStorage.getItem('survey_dom_title')) {
                document.title = localStorage.getItem('survey_dom_title');
            }
            else {
                document.title = '感谢您的参与！';
            }

            CHINET_Survey.DropDB();
            localStorage.clear();
        });
    },
    mounted: function () {
        var that = this;
        that.$nextTick(function () {
           
        });
    },
    methods: {
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