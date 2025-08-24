window.onload = function() { 
    const app = new Vue({
        el: '#app',

        data: {
            colspan:0,
            isrotate: 0,
            rot: 1,
            table_title: [],            
            mic: [],
            isdefault: true,
            show_years: false,
            show_antibiotic_pill: false,
            show_bacterium: false,
            show_loading_left: false, //加载中，默认false
            show_loading_right: false,
            echarts_Xdata: [],
            search_data: [],
            //表格数据
            table_th_head: [],
            table_th: [], //这是数据th
            table_tr_td_head: [],
            table_tr_td: [],
            main: [],
            sums:[],
            is_tip: "年度、抗菌药物、细菌",
            //数据描述
            describe: [],

            //years
            checkAll: false,
            checkedYears: [], //年度已选择
            yearsbefore: [],           

            isIndeterminate: true,
            inputyears: '', //文本框的内容
            input_message_years: '', //提示信息

            //antibiotic_pill
            checkedPill: [], //抗菌药物已选择
            //default_pill: [], //默认药物
            antibiotic_pillbefore: [],
            isIndeterminatePill: true,


            //bacterium
            checkAll_bacterium: false,
            checkedBacterium: [], //细菌已选择
            //default_bacterium: [], //默认细菌
            bacteriumbefore: [],
            isIndeterminateBacterium: true,

        },
        //两面保存一致性
        watch: {
            'checkedPill': function () {
                if (this.checkedPill) {
                    this.checkedPill.isdefault = true
                }
            },
            'checkedBacterium': function () {
                if (this.checkedBacterium) {
                    for (let i = 0; i < this.checkedBacterium.length; i++) {
                        this.checkedBacterium[i].isdefault = true
                    }

                }
            }
        },

        mounted() {
            this.init_data();
        },



        methods: {

            // 左右div高度相等
            one_height() {
                if ($("#left_box").height() > $("#right_box").height()) { $("#right_box").css("height", $("#left_box").height()); } else { $("#left_box").css("height", $("#right_box").height()); }
            },

            isMobile() {
                if (/(iPhone|iPad|iPod|iOS|Android)/i.test(navigator.userAgent)) { //移动端
                    this.isrotate = 40
                } else {
                    this.isrotate = 0
                }
            },

            getEchartsData(num, pill, names, x, d, i) { //x:echarts图表x轴的下标，d:echarts图表数据，i:echarts图表id

                var a;
                a = document.getElementById("" + i + "")
                if (a) {
                    var myChart = echarts.init(a);
                    option = {
                        //legend: {
                        //    orient: 'vertical',
                        //    x: 'center',
                        //    y: '15px',
                        //    show:false
                        //},
                        title: [{
                            show: num == 0,
                            textStyle: {
                                color: "grey",
                                fontSize: 30
                            },
                            text: "暂无数据",
                            left: "center",
                            top: "center"
                        },
                            {
                                textStyle: {
                                    color: "grey",
                                    fontSize: 20,
                                    fontWeight: 400,
                                },
                                text: "株\n数",
                                left: "left",
                                top: "center"
                            },
                            {
                                textStyle: {
                                    color: "grey",
                                    fontSize: 20,
                                    fontWeight: 400,
                                },
                                text: "mg/L",
                                left: "center",
                                bottom: "bottom"
                            },
                        ],
                        tooltip: {
                            //提示框组件
                            trigger: 'axis',
                            axisPointer: { type: 'none' },
                            formatter: function(params) {
                                var returnData = ''
                                returnData += pill[0] + '<br/>' + "N=" + params[0].data
                                return returnData
                            }
                        },
                        xAxis: {
                            type: 'category',
                            data: x,
                            axisLabel: {
                                interval: 0,
                                rotate: this.isrotate
                            }
                        },
                        yAxis: {},
                        series: [{
                            name: names,
                            type: 'bar',
                            data: d,
                            itemStyle: {
                                normal: {
                                    color: '#337ab7'
                                }
                            }

                        }],

                    };
                    myChart.setOption(option);
                    window.addEventListener("resize", function() {
                        myChart.resize();
                    });
                } else {
                    return false;
                }



            },


            //删除数组
            remove_years: function(index) {
                this.checkedYears.splice(index, 1)
            },
            remove_antibiotic_pill: function() {
                this.checkedPill = [];
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

            //返回按钮
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


            ////year的监听事件和全选事件 开始
            //handleCheckAllChange(val) {
            //    this.checkedYears = val ? this.yearsbefore.concat() : [];
            //    this.isIndeterminate = false;
            //},
            //handleCheckedYearsChange(value) {
            //    let checkedCount = value.length;
            //    this.checkAll = checkedCount === this.yearsbefore.length;
            //    this.isIndeterminate = checkedCount > 0 && checkedCount < this.yearsbefore.length;
            //},


            //bacterium的监听事件和全选事件 开始
            handleCheckAllChangeBacterium(val) {
                this.checkedBacterium = val ? this.bacteriumbefore.concat() : [];
                this.isIndeterminate = false;
            },
            handleCheckedBacteriumChange(value) {
                let checkedCount = value.length;
                this.checkAll_bacterium = checkedCount === this.bacteriumbefore.length;
                this.isIndeterminateBacterium = checkedCount > 0 && checkedCount < this.bacteriumbefore.length;
            },


            //条件列表数据渲染
            glamorize_data() {
                axios.post('/DataDistribution/MicDistribution/GetddYear', {})
                    .then(res => {
                        //年份
                        for (let i = 0; i < res.data.length; i++) {
                            this.yearsbefore.push({ "year": res.data[i].title, "year_id": res.data[i].year_id });
                        }
                    })
                    .catch(error => {
                        console.log(error);
                    });

                //抗菌药物
                axios.post('/DataDistribution/MicDistribution/GetDdAntibiotics', {})
                    .then(res => {
                        //抗菌药物                     
                        for (let i = 0; i < res.data.length; i++) {
                            this.antibiotic_pillbefore.push({ "value": res.data[i].title, "code": res.data[i].code, "isdefault": res.data[i].isdefault })
                        }
                    })
                    .catch(error => {
                        console.log(error);
                    });



                //细菌

                axios.post('/DataDistribution/MicDistribution/GetGerms', {

                    })
                    .then(res => {
                        //console.log(res);
                        //细菌
                        for (let i = 0; i < res.data.length; i++) {
                            this.bacteriumbefore.push({ "value": res.data[i].title, "code": res.data[i].code, "isdefault": res.data[i].isdefault, "id_index": i })
                        }
                    })
                    .catch(error => {
                        console.log(error);
                    });


            },



            //开始查询。。。
            kickstart_inquire() {               
                for (let i = 0, len = this.checkedBacterium.length; i < len; i++) {
                    for (let j = i + 1; j < len; j++) {
                        if (this.checkedBacterium[i].id_index > this.checkedBacterium[j].id_index) {
                            [this.checkedBacterium[i], this.checkedBacterium[j]] = [this.checkedBacterium[j], this.checkedBacterium[i]];
                        }
                    }
                }

                this.table_title = [];
                this.table_title.push(this.checkedPill.value)
                var myYear = "";
                var myPill = "";
                var myBacterium = []
                //for (let i = 0; i < this.checkedYears.length; i++) {
                //    myYear.push(this.checkedYears[i].year_id)
                //}


				if (this.checkedYears == 0) {
                    myYear = ""
                } else {
                    myYear = this.checkedYears.year_id;
                    //console.log(myYear);

                }
                if (this.checkedPill == 0) {
                    myPill = ""
                } else {
                    myPill = this.checkedPill.code;
                }

                for (let i = 0; i < this.checkedBacterium.length; i++) {
                    myBacterium.push(this.checkedBacterium[i].code)
                }
                console.log(myPill);
                if (myYear != "" && myPill != "" && myBacterium.length != 0) {
                    //调用axios查询
                    //console.log(myPill);
                    axios({
                            method: 'post',
                            url: '/DataDistribution/MicDistribution/GetddDocumentItem',
                            data: {
                                'yeraid': myYear,
                                'antibiotics': myPill,
                                'gremcode': myBacterium
                            }
                        })
                        .then(res => {
                            //console.log(res);
                            this.table_th_head = [];
                            this.table_tr_td_head = [];
                            this.table_th = [];
                            this.table_tr_td = [];
                            this.mic = [];
                            this.echarts_Xdata = [];

                            console.log(res);
                            if (res) {


                                this.search_data = res.data;
                                //console.log(this.search_data);

                                //赋值
                                this.main = [];
                                for (var i = 0; i < this.search_data.length; i++) {
                                    this.table_th_head.push(this.search_data[i][1])
                                    this.table_th.push(this.search_data[i][2])
                                    this.table_tr_td_head.push(this.search_data[i][3])
                                    this.table_tr_td.push(this.search_data[i][4])
                                    this.mic.push(this.search_data[i][0])
                                    this.echarts_Xdata.push(...this.table_th[i])
                                    this.main.push(i);
                                    //console.log(main);
                                }
                                
                                //拼接数组
                                for (var i = 0; i < this.search_data.length; i++) {
                                    this.table_th_head[i].push(...this.table_th[i])
                                    this.table_tr_td_head[i].push(...this.table_tr_td[i])
                                }                                                             
                                this.colspan = this.search_data[0][1].length;
                                this.sums = []; 
                                //图表赋值
                                for (var i = 0; i < this.search_data.length; i++) {
                                                             
                                    this.getEchartsData(this.search_data[i][3][1], this.search_data[i][3], this.search_data[i][0], this.search_data[i][2], this.search_data[i][4], i);
                                    this.sums.push(this.search_data[i][3][1]);
                                }
                            }
                        })
                        .catch(err => {

                            console.log(err);
                        });


                } else {
                    this.search_data.splice(0, this.search_data.length); //清空数组;
                    if (myYear== "" && myPill == "" && myBacterium.length == 0) {
                        this.is_tip = "年度、抗菌药物、细菌";
                    } else if (myYear == "" && myPill != "" && myBacterium.length != 0) {
                        this.is_tip = "年度";
                    } else if (myYear != "" && myPill == "" && myBacterium.length != 0) {
                        this.is_tip = "抗菌药物";
                    } else if (myYear != "" && myPill != "" && myBacterium.length == 0) {
                        this.is_tip = "细菌";
                    } else if (myYear!= "" && myPill == "" && myBacterium.length == 0) {
                        this.is_tip = "抗菌药物、细菌";
                    } else if (myYea == "" && myPill != "" && myBacterium.length == 0) {
                        this.is_tip = "年度、细菌";
                    } else if (myYear== "" && myPill == "" && myBacterium.length != 0) {
                        this.is_tip = "年度、抗菌药物";
                    }
                }


            },

            //页面初始化数据
            init_data() {
                //初始化条件列表数据
                this.glamorize_data(); 
                this.isMobile();
                this.getDescription();                //初始化图表数据
                //this.kickstart_inquire(); //加载图表

            },

            download_Excel(index1) {
                var table_name = this.table_title[0] + "对" +  this.search_data[index1][0]
                var table_th = this.search_data[index1][1]
                var table_td = this.search_data[index1][3]

                location.href = "/DataDistribution/MicDistribution/Exportexcel?name=" + table_name + "&&head=" + table_th + "&&value=" + table_td + ""
            },

            desc_btn(){

                if (this.rot == 1) {
                    $("#desc_btn").css({ 'transform': 'rotate(180deg) scale(0.5)' })
                    this.rot = 2;

                } else {
                    $("#desc_btn").css({ 'transform': 'rotate(0deg) scale(0.5)' })
                    this.rot = 1;

                }
            },
            getDescription() {
                axios({
                    method: 'post',
                    url: '/DataDistribution/MicDistribution/GetTeamlist'

                })
                    .then(res => {
                        this.describe = res.data;
                    })

            },      

        }
    })


}