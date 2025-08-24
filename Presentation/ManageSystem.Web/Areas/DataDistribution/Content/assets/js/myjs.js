window.onload = function() {

    const app = new Vue({
        el: '#app',

        data: {
            isdefault: true,
            show_years: false,
            show_antibiotic_pill: false,
            show_bacterium: false,

            //表格数据
            table_th: ['抗菌药', '株数', '0.06', '0.12', '0.25', '0.5', '1', '2', '4', '8', '16', '32', '64', '128', '256'],
            table_tr_td: [
                ['阿米卡星11', '588', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A'],
                ['阿奇霉素21', '588', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A'],
                ['阿奇霉素31', '588', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A'],
                ['阿奇霉素41', '588', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A', 'N/A'],
            ],



            //echarts图表数据
            Mysource: [
                
                    ['product', '阿米卡星', '阿奇霉素'],
                    ['0.06', 43.3, 85.8],
                    ['0.12', 83.1, 73.4],
                    ['0.25', 86.4, 65.2],
                    ['0.5', 72.4, 53.9],
                    ['1', 72.4, 53.9],
                    ['2', 72.4, 53.9],
                    ['4', 72.4, 53.9],
                    ['8', 90.4, 53.9],
                    ['16', 72.4, 53.9],
                    ['32', 79.4, 53.9],
                    ['64', 72.4, 53.9],
                    ['128', 88.4, 53.9],
                    ['256', 72.4, 53.9]
                
                
            ],
            //years
            checkAll: false,
            checkedYears: [], //年度已选择
            yearsbefore: ['2004', '2005', '2006', '2007', '2008', '2009', '2010', '2011', '2012', '2013', '2014', '2015', '2016'],
            yearsbefore2: ['2004', '2005', '2006', '2007', '2008', '2009', '2010', '2011', '2012', '2013', '2014', '2015', '2016'],
            isIndeterminate: true,
            inputyears: '', //文本框的内容
            input_message_years: '', //提示信息

            //antibiotic_pill
            checkAll_pill: false,
            checkedPill: [], //抗菌药物已选择
            antibiotic_pillbefore: [
                { value: '阿奇霉素', label: '阿奇霉素' },
                { value: '阿米卡星', label: '阿米卡星' },
            ],
            isIndeterminatePill: true,


            //bacterium
            checkAll_bacterium: false,
            checkedBacterium: [], //细菌已选择

            bacteriumbefore: [
                { value: '大肠埃希菌', label: '大肠埃希菌' },
                { value: '肺炎克雷伯菌', label: '肺炎克雷伯菌' },
                { value: '鲍曼不动杆菌', label: '鲍曼不动杆菌' },
                { value: '铜绿假单胞菌', label: '铜绿假单胞菌' },
            ],
            isIndeterminateBacterium: true,


        },
        mounted() {
            this.init_data();
        },

        methods: {

            getEchartsData() {

                
                var myChart = echarts.init(document.getElementById('main'));

                option = {
                    legend: {},
                    tooltip: {},
                    grid: {
                        right: '10%',
                        top: '10%',
                        left: '10%',
                        bottom: '10%'
                    },
                    dataset: {
                        source: this.Mysource
                    },
                    xAxis: {
                        type: 'category'
                    },
                    yAxis: {},
                    // Declare several bar series, each will be mapped
                    // to a column of dataset.source by default.
                    series: [{
                        type: 'bar'
                    }, {
                        type: 'bar'
                    }]
                };
                myChart.setOption(option);
            },


            //删除数组
            remove_years: function(index) {
                this.checkedYears.splice(index, 1)
            },
            remove_antibiotic_pill: function(index) {
                this.checkedPill.splice(index, 1)
            },
            remove_bacterium: function(index) {
                this.checkedBacterium.splice(index, 1)
            },

            select: function(num) {
                if (num == 1) {
                    this.isdefault = false;
                    this.show_years = true
                } else if (num == 2) {
                    this.isdefault = false;
                    this.show_antibiotic_pill = true
                } else if (num == 3) {
                    this.isdefault = false;
                    this.show_bacterium = true
                }
            },

            toback: function(num) {
                if (num == 1) {
                    this.isdefault = true;
                    this.show_years = false
                } else if (num == 2) {
                    this.isdefault = true;
                    this.show_antibiotic_pill = false
                } else if (num == 3) {
                    this.isdefault = true;
                    this.show_bacterium = false
                }
            },
            //year,pill,Bacterium的监听事件和全选事件 开始
            handleCheckAllChange(val) {
                this.checkedYears = val ? this.yearsbefore.concat() : [];
                this.isIndeterminate = false;
            },
            handleCheckedYearsChange(value) {
                let checkedCount = value.length;
                this.checkAll = checkedCount === this.yearsbefore.length;
                this.isIndeterminate = checkedCount > 0 && checkedCount < this.yearsbefore.length;
            },




            //year,pill,Bacterium的监听事件和全选事件 结束
            //文本框监听事件

            //年度文本框监听事件
            input_change_year(event) {
                let search = [];
                var before = this.yearsbefore2;



                for (let i = 0; i < before.length; i++) {

                    if (before[i].indexOf(event) >= 0) {
                        search.push(before[i]);

                    }
                }
                //console.log(search_yearsbefore);
                if (search.length == 0) {
                    this.input_message_years = "";
                    this.input_message_years = event + " is not found";



                    this.yearsbefore = search;
                } else {
                    //渲染查询列表
                    this.input_message_years = "";
                    this.yearsbefore = search;
                }

            },





            //开始查询。。。
            kickstart_inquire() {
                //获取当前所有条件
                //调用axios查询
                // axios({
                //      method: 'post',
                //      url: '/',
                //      data: {
                //      this.checkedYears: 'checkedYears',
                //      this.checkedPill: 'checkedPill',
                //      this.checkedBacterium: 'checkedBacterium'
                //      }
                //})
                //.then(res=> {
                //    console.log(response);
                //})
                //.catch(error=> {
                //    console.log(error);
                //});
            },

            //页面初始化数据
            init_data() {
                //初始化图表数据
                //setInterval(this.getEchartsData, 2000) //每2秒执行一次
                this.getEchartsData(); //加载图表

            }












        }
    })




    $(".years").on("click", function() {
        if ($(this).hasClass("label-default")) {
            $(this).removeClass("label-default");
            $(this).addClass("label-info fa fa-check");

        } else {
            $(this).removeClass("label-info");
            $(this).removeClass("fa");
            $(this).removeClass("fa-check");
            $(this).addClass("label-default");

        }
    });

    $(".antibios").on("click", function() {
        if ($(this).hasClass("label-default")) {
            $(this).removeClass("label-default");
            $(this).addClass("label-info fa fa-check");

        } else {
            $(this).removeClass("label-info");
            $(this).removeClass("fa");
            $(this).removeClass("fa-check");
            $(this).addClass("label-default");

        }
    });

    $(".germs").on("click", function() {
        if ($(this).hasClass("label-default")) {
            $(this).removeClass("label-default");
            $(this).addClass("label-info fa fa-check");

        } else {
            $(this).removeClass("label-info");
            $(this).removeClass("fa");
            $(this).removeClass("fa-check");
            $(this).addClass("label-default");

        }
    });

    $(".cc").on("click", function() {
        if ($(this).hasClass("fa-chevron-down")) {
            $(this).removeClass("fa-chevron-down");
            $(this).addClass("fa-chevron-up")
        } else if ($(this).hasClass("fa-chevron-up")) {
            $(this).removeClass("fa-chevron-up");
            $(this).addClass("fa-chevron-down")
        }
    });
}