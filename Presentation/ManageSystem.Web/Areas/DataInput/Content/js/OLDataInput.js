window.onload = function() {
    const app = new Vue({
        el: '#app',
        data() {
            return {
                tr_id: 1,
                start_page: 0,
                end_page: 10,
                onepage_num: 10,
                total_page: 0,
                current_page: 0,
                project_checkAll: false,              
                text_model_checkAll: false,              
                user_dispense_checkAll: false,               
                look_data_checkAll: false,              
                add_project_radio: "",
                add_data_text: [],
                experimenter:"",

            }
        },  
        mounted() {
            this.user_project_init();
        },
        methods: {
            // 初始化函数
            init() {
             
            },
               
            // 添加一行
            addTableTr() {
                //var $td = $("#clo").clone(); //增加一行,克隆第一个对象
                this.tr_id += 1;
                var tr2 = "<tr style='text-align:center;background-color: #fff;height:35px;color: #000;line-height:58px;'><td style='text-align:center;box-sizing: border-box;'><input type='text' nam='' class='add_text_model_inp' placeholder='字段名称'></td><td><input type='text' code='' class='add_text_model_inp' placeholder='字段代码'></td><td><input type='text' val='' class='add_text_model_inp' placeholder='默认值'></td><td><div  style='padding-left: 10px;' ><a class='opera_btn' onclick='deleteCurrentRow(this)'>删除</a></div></td></tr>"
                $("#text_model_table").append(tr2);

            },
                                      
            user_project_init() {
                axios.post('/DataInput/OLDataInput/AddDataview', {
                     'Id':2 //用户ID(参数)
                })
                    .then(res => {
                        // var ress = [{ "Id": 4627138501394008940, "templateId": 1, "template_name": null, "field_name": "ss", "field_code": "ee", "default_value": "tt", "created_byId": 5151204562826061171, "InsertTime": "2020-08-19T14:18:02", "updatetime": "0001-01-01T00:00:00", "UpdateTime": "0001-01-01T00:00:00", "DeleteTime": "1900-01-01T00:00:00", "Version": 1, "Mark": 1, "Describe": "" }, { "Id": 4858480543274130678, "templateId": 1, "template_name": null, "field_name": "uu", "field_code": "oo", "default_value": "qq", "created_byId": 5151204562826061171, "InsertTime": "2020-08-19T14:18:02", "updatetime": "0001-01-01T00:00:00", "UpdateTime": "0001-01-01T00:00:00", "DeleteTime": "1900-01-01T00:00:00", "Version": 1, "Mark": 1, "Describe": "" }, { "Id": 5010588873629303461, "templateId": 1, "template_name": null, "field_name": "aa", "field_code": "nn", "default_value": "cc", "created_byId": 5151204562826061171, "InsertTime": "2020-08-19T14:18:02", "updatetime": "0001-01-01T00:00:00", "UpdateTime": "0001-01-01T00:00:00", "DeleteTime": "1900-01-01T00:00:00", "Version": 1, "Mark": 1, "Describe": "" }]

                        for (var i = 0; i < res.data.length; ++i) {
                            this.add_data_text.push(res.data[i].field_name)
                        }
                    })
                    .catch(error => {
                        console.log(error);
                    });

            },
            add_data_btn() { }

        }
     
    })

}