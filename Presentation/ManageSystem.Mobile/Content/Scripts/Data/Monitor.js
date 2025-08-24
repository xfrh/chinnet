var mySwiper;
var data = [];
var startend;
var titlename;
var app = new Vue({
    el: '#app',
    data: {
        // 用于存放数据
        items: [],
        antibioticItems: [],
        currIdentity: 0,
        // 百度echarts
        myChart: null,
        pageContentTitle: null
    },
    /**
     * vue加载前初始化数据
     */
    created: function () {
        var that = this;
        that.$nextTick(function () {
            //细菌数据
            $.getJSON('/Content/Scripts/Data/Monitor.json', function (data) {
                that.items = data;
            });

            //抗菌药物数据
            $.getJSON('/Content/Scripts/Data/Monitor2.json', function (data) {
                that.antibioticItems = data;
            });

        });
    },
    /**
     * vue实例化完成后默认加载一个数据项
     */
    mounted: function () {
        var that = this;
        setTimeout(function () {
            // 基于准备好的dom，初始化echarts实例
            that.myChart = echarts.init(that.$refs.main);

            // 默认加载一个数据项
            that.SelectItem(101);
            setTimeout(function () {
                that.initSwiper();
            }, 100);
        }, 150);
    },
    methods: {
        /**
         * 选择数据项
         * @param {any} identity 标识id
         */
        SelectItem: function (identity) {
            var that = this;
            that.currIdentity = identity;
            that.items.forEach(function (item, i) {
                if (item.identity === identity) {
                    var text = [];

                    item.data.forEach(function (v, j) {
                        text.push(v.name);
                    });
                    that.pageContentTitle = item.title + '对抗菌药物的耐药率（%）';
                    that.LoadCharts(text, item.data, item.xRotate, item.title + '对抗菌药物的耐药率（%）', item.barWidth, item.dataZoomArea);
                    return;
                }
            });
        },

        //选择抗菌药物，重新加载报表
        SelectItem2: function (identity) {

            var that = this;
            that.currIdentity = identity;
            that.antibioticItems.forEach(function (item, i) {
                if (item.identity === identity) {
                    var text = [];

                    item.data.forEach(function (v, j) {
                        text.push(v.name);
                    });
                    that.pageContentTitle = item.title;
                    that.LoadCharts(text, item.data, item.xRotate, item.title, item.barWidth, item.dataZoomArea);
                    return;
                }
            });
        },

        /**
         * 生成百度图表
         * @param {any} xData x轴数据
         * @param {any} seriesData 图表数据
         * @param {any} xRotate x轴文字旋转角度
         * @param {any} title 图表标题
         * @param {any} barWidth 图表柱子宽度
         * @param {any} dataZoomArea 缩放比例
         */
        LoadCharts: function (xData, seriesData, xRotate, title, barWidth, dataZoomArea) {
            var that = this;

            if (that.myChart) {
                that.myChart = echarts.init(that.$refs.main);
            }

            that.myChart.clear();
            that.myChart.resize();
            that.myChart.showLoading();

            var option = {
                color: ['#015BAA'],
                title: {
                    text: title,
                    subtext: '',
                    left: 'center',
                    show: true,
                    top:'4%',
                    textStyle: {
                        fontWeight: 'normal',
                        fontSize: 20
                    }
                },
                tooltip: {
                    trigger: 'axis',
                    axisPointer: { // 坐标轴指示器，坐标轴触发有效
                        type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                    }
                },
                grid: {
                    top: '10%',
                    left: '5%',
                    right: '4%',
                    bottom: '25%',
                    //height: '60%',
                    containLabel: true
                },
                xAxis: [
                    {
                        type: 'category',
                        axisLabel: {
                            interval: 0,
                            rotate: xRotate,
                            margin: 10,
                            textStyle: {
                                fontSize: 24
                            }
                        },
                        data: xData,
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
                            textStyle: {
                               fontSize: 20
                            },
                            formatter: '{value} %'
                        },
                        show: true
                    }
                ],
                toolbox: {
                    show: false,
                    orient: 'vertical',
                    left: 'right',
                    top: 'center',
                    feature: {
                        dataView: {
                            title: "数据",
                            readOnly: false
                        },
                        restore: {},
                        saveAsImage: {
                            title: "下载",
                            type: "jpeg"
                        }
                    }
                },
                series: [
                    {
                        name: '数值',
                        type: 'bar',
                        barWidth: 30,
                        data: seriesData,
                        itemStyle: {
                            normal: {
                                label: {
                                    show: true,
                                    position: 'top',
                                    // formatter: '{c}'
                                    formatter: '{c}'
                                }
                            }
                        }
                    }
                ],
                textStyle:
                {
                    fontSize: 20
                },
                dataZoom: [
                    {
                        type: 'slider',
                        height:'50px',
                        show: true,
                        xAxisIndex: [0],
                        start: 0,
                        end: dataZoomArea,
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
                    },
                ]
            };

            that.myChart.setOption(option);
            that.myChart.resize();
            setTimeout(function () {
                that.myChart.hideLoading();
            }, 300);
            that.myChart.on('dataZoom', function (param) {
                startend = param.start + param.end;
            });
            if (mySwiper) {
                //页面回到顶部
                that.GoTop();
            };          
            titlename = title;
            data = seriesData;
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
        QRcodesharing() {
            var that = this;
            that.myChart.hideLoading();
            var imgURL = that.myChart.getDataURL({
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
                    //var href = 'http://www.chinets.com' + json;
                    //$('#erWeiMa').html("<a href=" + href + "><img src=" + imgURL + "></a>");
                    //$('#erWeiMa').empty();
                    //$('#erWeiMa').qrcode('http://www.chinets.com' + json);
                    $('#erWeiMa').html("<img src=" + imgURL + ">");
                },
                error: function (response) { alert("error"); }
            });
        }
    }
});