var app = new Vue({
    el: '#app',
    data: {},
    created: function () {
        this.$nextTick(() => {
        });
    },
    methods: {
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        logout(url) {
            if (url) {
                localStorage.removeItem('token');
                location.href = url;
            }
        }
    }
});