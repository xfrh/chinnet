var mySwiper;
var myChart;
var data = [];
var startend;
var titlename;
var app = new Vue({
    el: '#app',
    data: {
        pageContentTitle: null,
        pageContentSubTitle: null,
        weui_toast_content: null,
        dataItems: []
    },
    created: function () {
        var _that = this;
        _that.$nextTick(() => {
            _that.initSwiper();
        });
    },
    mounted: function () {
        var _that = this;
        setTimeout(() => {
            if ($('li[id^="li-"][data-default="1"].list-item').length > 0) {
                $('li[id^="li-"][data-default="1"].list-item').click();
            }
            else {
                $('li[id^="li-"].list-item:eq(0)').click();
            }
        }, 50);

    },
    methods: {
        renderTrendChart(id, event) {
            var _that = this;
            if (id) {
                $('.sjd-list-group li.active').removeClass('active');
                $(event.target).addClass('active');

                 myChart = echarts.init(_that.$refs.main);

                $.ajax({
                    url: '/Data/GermWithYearData',
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
                                text: res.title+'（%）',
                                subtext: res.subtitle,
                                left: 'center',
                                show: true,
                                top: '4%',
                                textStyle: {
                                    fontSize: 24,
                                    fontWeight: 400
                                },
                                subtextStyle: {
                                    fontWeight: 400,
                                    fontSize: 22,
                                    color: '#000'
                                }
                            },
                            legend: {
                                data: res.legend,
                                top: '17%'
                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: { // 坐标轴指示器，坐标轴触发有效
                                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                                }
                            },
                            grid: {
                                top: '15%',
                                left: '5%',
                                right: '4%',
                                bottom: '25%',
                                height: '60%',
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
                                            fontSize: 24
                                        }
                                    },
                                    data: res.xAxis,
                                    axisTick: {
                                        alignWithLabel: true
                                    }
                                }
                            ],
                            yAxis: [
                                {
                                    name: '单位(%)',
                                    type: 'value',
                                    axisLabel: {
                                        show: true,
                                        interval: 'auto',
                                        textStyle: {
                                            fontSize: 20
                                        }

                                        //formatter: '{value} %'
                                    },
                                    show: true
                                }
                            ],
                            series: res.series,
                            textStyle:
                            {
                                fontSize: 20
                            },
                            calculable: true,
                            dataZoom: [
                                {
                                    type: 'slider',
                                    height:'75px',
                                    show: true,
                                    xAxisIndex: [0],
                                    start: 100 - res.dataZoomArea,
                                    end: 100,
                                    // start: 0,
                                    // end: dataZoomArea,
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

                        if (res.title === '历年成员单位及监测总株数') {
                            option.yAxis = [
                                {
                                    name: '菌株数',
                                    type: 'value',
                                    position: 'right',
                                    axisLabel: {
                                        show: true,
                                        interval: 'auto'
                                    },
                                    axisLine: {
                                        lineStyle: {
                                            color: '#015BAA'
                                        }
                                    },
                                    min: 0,
                                    max: 300000,
                                    splitNumber: 6,
                                    show: true
                                },
                                {
                                    name: '医院数',
                                    type: 'value',
                                    axisLabel: {
                                        show: true,
                                        interval: 'auto',
                                        formatter: '{value} %'
                                    },
                                    axisLine: {
                                        lineStyle: {
                                            color: '#C1232B'
                                        }
                                    },
                                    min: 0,
                                    max: 50,
                                    splitNumber: 10,
                                    show: true
                                }
                            ];
                            for (var i = 0, len = option.series.length; i < len; i++) {
                                if (option.series[i].name === '医院数') {
                                    option.series[i].yAxisIndex = 1;
                                    option.series[i].barWidth = 20;
                                }
                            }
                        }

                        _that.pageContentTitle = res.title;
                        _that.pageContentSubTitle = res.subtitle;

                        //使用制定的配置项和数据显示图表
                        myChart.setOption(option);
                        myChart.resize();
                        setTimeout(function () {
                            myChart.hideLoading();
                            mySwiper.update();
                            //$('#loadingToast').fadeOut(20);
                        }, 350);
                        myChart.on('dataZoom', function (param) {
                            startend = param.start + param.end;
                        });
                        data = res.series[0].data;
                        titlename = res.title;
                    }
                });
            }
            return false;
        },
        toPage(url) {
            if (url) {
                location.href = url;
            }
        },
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
                    //$('#erWeiMa').empty();
                    //$('#erWeiMa').qrcode('http://www.chinets.com' + json);

                    //var href = 'http://www.chinets.com' + json;
                    //$('#erWeiMa').html("<a href=" + href + "><img src=" + imgURL + "></a>");
                    $('#erWeiMa').html("<img src=" + imgURL + ">");
                },
                error: function (response) { alert("error"); }
            });
        }
    }
});