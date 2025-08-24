


//保存
function AddProject() {

    if ($("#HospitalName").val() == "") {
        ErrorWindow("请选择输入医院名称");
        return;
    }
    if ($("#HospitalGrade").val() == "") {
        ErrorWindow("请选择输入医院等级");
        return;
    }

    if ($("#ddlProvince").val() == "" || $("#ddlProvince").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#ddlCity").val() == "" || $("#ddlCity").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#ddlArea").val() == "" || $("#ddlArea").val() == "0") {
        ErrorWindow("请选择省市区");
        return;
    }

    if ($("#Address").val() == "") {
        ErrorWindow("请选择输入详细地址");
        return;
    }

    if ($("#CREDetectionRate").val() == "") {
        ErrorWindow("请选择输入上年CRE平均检出率");
        return;
    }
    if ($("#CREDrugRate").val() == "") {
        ErrorWindow("请选择输入上年CRAB平均检出率");
        return;
    }
    if (!$("#MHSource").val() || $("#MHSource").val() === "") {
        ErrorWindow("请输入MH平板来源厂家");
        return;
    }
    if ($("#Name").val() == "") {
        ErrorWindow("请选择输入负责人姓名");
        return;
    }

    if ($("#Phone").val() == "") {
        ErrorWindow("请选择输入联系电话");
        return;
    }

    var email = $("#Email").val();
    if (email.indexOf("@") == -1) {
        ErrorWindow("邮箱格式不正确");
        return;
    }


    var data = {
        Id: $("#Id").val(),
        HospitalName: $("#HospitalName").val(),
        HospitalGrade: $("#HospitalGrade").val(),
        ProvinceId: $("#ddlProvince").val(),
        CityId: $("#ddlCity").val(),
        DistrictsId: $("#ddlArea").val(),
        Area: $('#ddlProvince option:selected').text() + " " + $('#ddlCity option:selected').text() + " " + $('#ddlArea option:selected').text(),
        Address: $("#Address").val(),

        CREDetectionRate: $("#CREDetectionRate").val(),
        CREDrugRate: $("#CREDrugRate").val(),
        MHSource: $("#MHSource").val(),

        Name: $("#Name").val(),
        Phone: $("#Phone").val(),
        Email: $("#Email").val(),
        HospitalId: $("#HospitalId").val()
    };

    $.ajax({
        async: false,
        type: "POST",
        url: "/CRProject/Add",
        data: data,
        dataType: "json",
        success: function (json) {

            if (json == null || json == "") {
                ErrorWindow("添加失败，请重试！", true);
                return;
            }
            if (json.status == "true") {
                SuccessWindows("<div style='text-align:center;'>数据保存成功！<br />请<a  style='color:red; background-color: #fffa00;' href='/Content/File/CR复敏-数据采集模板.xlsx'>下载数据模板</a>用于记录实验结果。<br />待实验完成后，再次登录CHINET上传数据。</div>", false, "/CRProject/Upload");
            } else {
                ErrorWindow(json.message, false);
            }
        }

    });
}

//上传数据
function Upload() {
    ErrorWindow("请先填写基本信息");
}