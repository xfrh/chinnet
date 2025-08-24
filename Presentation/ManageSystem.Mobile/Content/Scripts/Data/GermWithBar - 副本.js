//import { Name } from "../pdfjs/pdf.worker";
var mySwiper;
var myChart;
var data = [];
var startend;
var titlename;
var app = new Vue({
    el: '#app',
    data: {
        searchText: null,
        clearSearchText: true,
        pageContentTitle: null,
        pageContentSubTitle: null,
        weui_toast_content: null,
        lookups: []
    },
    created: function () {
        var _that = this;
        _that.$nextTick(() => { _that.initSwiper(); });
    },
    mounted: function () {
        var _that = this;

        setTimeout(function () {
            _that.lookups = JSON.parse(_that.$refs.autocompletelookups.value);
            $('#searchText').autocomplete({
                lookup: _that.lookups,
                focus: function (event, ui) {
                    console.log(ui);
                    return false;
                },
                onSelect: function (suggestion, data) {
                    _that.searchText = suggestion.value;
                    _that.onSearch();
                }
            });
            if ($('li[id^="li-"][data-default="1"].list-item').length > 0) {
                $('li[id^="li-"][data-default="1"].list-item').click();
            }
            else {
                $('li[id^="li-"].list-item:eq(0)').click();
            }
        }, 50);
    },
    methods: {
        /**
         * 搜索
         */
        onSearch() {
            var _that = this;
            if (_that.searchText && _that.searchText.length > 0) {

                var reg = new RegExp(_that.searchText.replace('\(', '\\(').replace('\)', '\\)'), 'i');

                var item = _that.lookups.find(function (elem) {
                    if (elem.value) {
                        return elem.value === _that.searchText;
                    }
                    return undefined;
                });
                if (item) {
                    _that.clearSearchText = false;
                    $('li#li-' + item.data).click();
                }
                else {
                    weui.alert('没有对应的细菌或药物名称<br />请重新输入搜索', { className: '_search_item-null' });
                }
            }
            else {
                this.searchText = null;
            }
        },

        /**
         * 渲染图表
         * @param {any} id 报表id
         * @param {any} event 事件
         * @return {boolean} 默认false
         */
      
        renderBarChart(id, event) {
            var _that = this;
            if (id) {
                if (_that.clearSearchText) {
                    _that.searchText = null;
                }
                $('.sjd-list-group li.active').removeClass('active');
                $(event.target).addClass('active');

                 myChart = echarts.init(_that.$refs.main);

                $.ajax({
                    url: '/Data/GermWithBarData',
                    method: 'POST',
                    data: { id: id },
                    dataType: 'JSON',
                    beforeSend: function () {
                        _that.pageContentTitle = null;
                        _that.pageContentSubTitle = null;
                        //$('#loadingToast').fadeIn(100);
                        myChart.clear();
                        myChart.resize();
                        myChart.showLoading();

                        //页面回到顶部
                        if (mySwiper) {
                            mySwiper.slideTo(0, 100, false);
                            mySwiper.update();
                        }
                    },
                    complete: function () {
                        setTimeout(function () {
                            myChart.hideLoading();
                        }, 600);
                    },
                    success: function (res) {
                        var option = {
                            color: res.color,
                            title: {
                                text: res.title,
                                subtext: res.subtitle,
                                left: 'center',
                                show: true,
                                top:'4%',
                                textStyle: {
                                    fontSize: 14,
                                    fontWeight: 400
                                },
                                subtextStyle: {
                                    fontWeight: 400,
                                    fontSize: 12,
                                    color: '#000'
                                }
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: { // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                                }
                            },
                            grid: {
                                top: '19%',
                                left: '5%',
                                right: '4%',
                                bottom: '25%',
                                height:'60%',
                                containLabel: true
                            },
                            xAxis: [
                                {
                                    type: 'category',
                                    axisLabel: {
                                        interval: 0,
                                        rotate: res.rotate,
                                        margin: 10,
                                        textStyle: {
                                            fontSize: "20px"
                                        }
                                    },
                                    data: res.xAxis,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
                                    axisTick: {
                                        alignWithLabel: true
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    type: 'value',
                                    axisLabel: {
                                        show: true,
                                        interval: 'auto',
                                        formatter: '{value} %'
                                    },
                                    show: true
                                }
                            ],
                            series: res.series,
                            textStyle:
                            {
                                fontSize: 10
                            },
                            dataZoom: [
                                {
                                    type: 'slider',
                                    show: true,
                                    xAxisIndex: [0],
                                    start: 0,
                                    end: res.dataZoomArea,
                                    bottom: "0",
                                    left: "6%",
                                    right: "4%",
                                    textStyle: false,
                                    handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
                                    handleSize: '80%',
                                    handleStyle: {
                                        color: '#fff',
                                        shadowBlur: 3,
                                        shadowColor: 'rgba(0, 0, 0, 0.6)',
                                        shadowOffsetX: 2,
                                        shadowOffsetY: 2
                                    }
                                }
                            ]
                        };

                        _that.pageContentTitle = res.title;
                        _that.pageContentSubTitle = res.subtitle;

                        //使用制定的配置项和数据显示图表
                        myChart.setOption(option);
                        myChart.resize();
                        setTimeout(function () {
                            myChart.hideLoading();
                            //$('#loadingToast').fadeOut(20);
                            _that.clearSearchText = true;
                            mySwiper.update();
                        }, 350);
                        myChart.on('dataZoom', function (param) {
                            startend = param.start + param.end;                        
                        })
                        data = res.series[0].data;
                        titlename = res.title;                       
                    }
                });
            }
           
            return false;
        },

        /**
         * 页面回到顶部
         * @return {boolean} 默认false
         */
        GoTop() {
            mySwiper.slideTo(0, 100, false);
            return false;
        },

        /**
         * 跳转页面
         * @param {string} url 页面地址
         */
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
        /**
         * 初始化Swiper
         */
        initSwiper() {
            var _this = this;
            _this.$refs.swiper_container.style.height = _this.$refs.middle.offsetHeight + 'px';
            setTimeout(() => {
                mySwiper = new Swiper('.swiper-container', {
                    direction: 'vertical',
                    loop: false,
                    observer: true,
                    observeParents: false,
                    watchSlidesProgress: true,
                    freeMode: true,
                    slidesPerView: 'auto'
                });
            });
        },

        QRcodesharing() {
            myChart.hideLoading();
            var imgURL = myChart.getDataURL({
                type: "jpg",
                pixelRatio: 5,
                excludeComponents: ['toolbox'],
                excludeComponents: ['dataZoom'],
                backgroundColor: '#fff'
            });
            var dataName = startend;
            for (var i = 0; i < data.length; i++) {
                dataName += data[i].value + ",";
            }
            $.ajax({
                url: '/Data/Picturesharing',
                method: 'POST',
                data: { dataURL: imgURL, imgName: dataName, Name: titlename },
                success: function (json) {
                    var href = 'http://www.chinets.com' + json;
                    $('#erWeiMa').html("<img src=" + href + ">");
                    //$('#erWeiMa').html("<a href=" + href + "><img src=" + imgURL + "></a>");
                    //$('#erWeiMa').empty();
                    //$('#erWeiMa').qrcode('http://www.chinets.com' + json);
                },
                error: function (response) { alert("error"); }
            });                      
        }
    }
});