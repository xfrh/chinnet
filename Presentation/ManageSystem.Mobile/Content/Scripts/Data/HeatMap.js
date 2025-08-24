var mySwiper;
app = new Vue({
    el: '#app',
    data: {
        pageContentTitle: null,
        pageContentSubTitle: null,
        weui_toast_content: null
    },
    created: function () {
        var _that = this;
        _that.$nextTick(() => {
            _that.initSwiper();
        });
    },
    mounted: function () {
        var _that = this;

        setTimeout(function () {
            if ($('li[id^="listitem-"][data-default="1"].list-item').length > 0) {
                $('li[id^="listitem-"][data-default="1"].list-item').click();
            }
            else {
                $('li[id^="listitem-"].list-item:eq(0)').siblings('li[id^="listitem-"].list-item:last-child').click();
            }
        }, 50);
    },
    methods: {
        isInteger: function (n) {
            return parseInt(n) === parseFloat(n);
        },
        renderHeatMap(id, event) {
            if (id) {
                var _that = this,
                    myChart = echarts.init(_that.$refs.main);

                $('.sjd-list-group li.active').removeClass('active');
                $(event.target).addClass('active');

                $.ajax({
                    url: '/Data/HeatMapData',
                    method: 'POST',
                    data: { id: id },
                    dataType: 'JSON',
                    beforeSend: function () {
                        //_that.pageContentTitle = null;
                        //_that.pageContentSubTitle = null;
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

                        var _max = Math.max.apply(Math, res.data.map(item => { return item.value; }));

                        res.data.push({
                            name: '南海诸岛',
                            value: '0',
                            itemStyle: {
                                normal: {
                                    opacity: 0,
                                    label: {
                                        show: false
                                    }
                                }
                            }
                        });

                        var option = {
                            backgroundColor: '#FFFFFF',
                            title: {
                                show: true,
                                text: res.title,
                                subtext: res.subtitle,
                                x: 'center',
                                textStyle: {
                                    fontSize: 18
                                }
                            },
                            tooltip: {
                                trigger: 'item',
                                formatter: function (params) {
                                    if (params.value) {
                                        return params.name + ': ' + params.value + '%';
                                    }
                                }
                            },
                            //左侧小导航图标
                            visualMap: {
                                show: true,
                                type: 'piecewise',
                                x: 'left',
                                y: 'bottom',
                                pieces: [
                                    { min: Math.ceil(_max * 3 / 4), max: Math.ceil(_max + 1) },
                                    { min: Math.ceil(_max * 2 / 4), max: Math.ceil(_max * 3 / 4) },
                                    { min: Math.ceil(_max / 4), max: Math.ceil(_max * 2 / 4) },
                                    { min: 0, max: Math.ceil(_max / 4) }
                                ],
                                inverse: true,
                                color: ['#c61620', '#e38788', '#e4bcc4', '#fbdcd7'],
                                textStyle: {
                                    fontSize: 14,
                                    lineHeight: 56
                                },
                                top: '',
                                left: '5%',
                                bottom: '10%',
                                itemWidth: 25,
                                itemHeight: 10,
                                itemGap: 5,
                                itemSymbol: 'roundRect',
                                textGap: 5,
                                formatter: function (value, value2) {
                                    if (value2 === Math.ceil(_max + 1)) {
                                        return '>' + value + '%';
                                    }
                                    return value + '% - ' + value2 + '%';
                                }
                            },

                            //配置属性
                            series: [{
                                name: '数据',
                                type: 'map',
                                mapType: 'china',
                                roam: true,
                                zoom: 1.15,
                                label: {
                                    normal: {
                                        show: true, //省份名称
                                        textStyle: {
                                            fontSize: 14
                                        },
                                        formatter: function (params) {
                                            if (params.value) {
                                                //return params.name + '\n' + params.value + '%';
                                            }
                                        }
                                    },
                                    emphasis: {
                                        show: true,
                                        textStyle: {
                                            fontSize: 14
                                        }
                                    }
                                },
                                itemStyle: {
                                    normal: {
                                        areaColor: '#ddd',
                                        borderColor: '#fefefe',
                                        borderWidth: 2
                                    },
                                    emphasis: {
                                        show: true
                                    }
                                },
                                data: res.data  //数据
                            }],
                            textStyle: {
                                fontSize: 14
                            }
                        };

                        //_that.pageContentTitle = res.title;
                        //_that.pageContentSubTitle = res.subtitle;

                        //使用制定的配置项和数据显示图表
                        myChart.setOption(option);
                        myChart.resize();

                        myChart.on('mouseover', function (params) {
                            if (isNaN(params.value)) {
                                myChart.dispatchAction({
                                    type: 'downplay'
                                });
                            }
                        });

                        setTimeout(function () {
                            myChart.hideLoading();
                            $('#loadingToast').fadeOut(20);
                            mySwiper.update();
                        }, 350);
                    }
                });

            }
            return false;
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
        toPage(url) {
            if (url) {
                location.href = url;
            }
        }
    }
});