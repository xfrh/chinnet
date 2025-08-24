
$(document).ready(function () {
    LayPageData(0);
})


//加载数据验证的列表数据
function LayPageData(curr) {
    $.ajax({
        type: 'POST',
        url: "/Medicine/GetDataList",
        dataType: 'json',
        data: {
            DataId: $("#DataId").val(),
            Page: curr || 1,  //向服务端传的参数，此处只是演示
            PageSize: 100//每页10行
        },
        success: function (result) {

            //绑定页面的数据
            if (result.rows != null && result.rows.length > 0) {
                var html = "";
                var index = 0;
                for (var i = 0; i < result.rows.length; i++) {
                    var item = result.rows[i];

                    html += "  <tr onclick='SelectDataItem(this)' SelectItem='0'>" +
                        "<td> " + (++index) + "</td>" +
                        "  <td> 	" + item.UploadRowIndex + "</td> " +
                        "  <td> 	" + item.COUNTRY_A + "</td>         " +
                        "  <td> 	" + item.LABORATORY + "</td>     " +
                        "  <td> 	" + item.ORIGIN + "</td>                 " +
                        "  <td> 	" + item.PATIENT_ID + "</td>         " +
                        "  <td> 	" + item.FIRST_NAME + "</td>     " +
                        "  <td> 	" + item.LAST_NAME + "</td>         " +
                        "  <td> 	" + item.FULL_NAME + "</td>         " +
                        "  <td> 	" + item.SEX + "</td>                     " +
                        "  <td> 	" + item.AGE + "</td>                     " +
                        "  <td> 	" + item.PAT_TYPE + "</td>             " +
                        "  <td> 	" + item.WARD + "</td>                 " +
                        "  <td> 	" + item.WARD_TYPE + "</td>         " +
                        "  <td> 	" + item.INSTITUT + "</td>             " +
                        "  <td> 	" + item.DEPARTMENT + "</td>     " +
                        "  <td> 	" + item.SPEC_NUM + "</td>         " +
                        "  <td> 	" + item.SPEC_DATE + "</td>         " +
                        "  <td> 	" + item.SPEC_TYPE + "</td>         " +
                        "  <td> 	" + item.SPEC_CODE + "</td>         " +
                        "  <td> 	" + item.ORGANISM + "</td>         " +
                        "  <td> 	" + item.ORG_TYPE + "</td>         " +
                        "  <td> 	" + item.ESBL + "</td>                     " +
                        "  <td> 	" + item.BETA_LACT + "</td>         " +
                        "  <td> 	" + item.INDUC_CLI + "</td>         " +//新增字段
                        "  <td> 	" + item.CARBAPENEM + "</td>         " +//新增字段
                        "  <td> 	" + item.COMMENT + "</td>         " +//新增字段
                        "  <td>  " + item.AMC_ND20 + "</td>" +
                        "  <td>  " + item.AMC_NE + "</td>" +
                        "  <td>  " + item.AMC_NM + "</td>" +
                        "  <td>  " + item.AMK_ND30 + "</td>" +
                        "  <td>  " + item.AMK_NE + "</td>" +
                        "  <td>  " + item.AMK_NM + "</td>" +
                        "  <td>  " + item.AMP_ND10 + "</td>" +
                        "  <td>  " + item.AMP_NE + "</td>" +
                        "  <td>  " + item.AMP_NM + "</td>" +
                        "  <td>  " + item.ATM_ND30 + "</td>" +
                        "  <td>  " + item.ATM_NE + "</td>" +
                        "  <td>  " + item.ATM_NM + "</td>" +
                        "  <td>  " + item.AZA_ND30 + "</td>" +
                        "  <td>  " + item.AZA_NE + "</td>" +
                        "  <td>  " + item.AZA_NM + "</td>" +
                        "  <td>  " + item.AZM_ND15 + "</td>" +
                        "  <td>  " + item.AZM_NE + "</td>" +
                        "  <td>  " + item.AZM_NM + "</td>" +
                        "  <td>  " + item.CAZ_ND30 + "</td>" +
                        "  <td>  " + item.CAZ_NE + "</td>" +
                        "  <td>  " + item.CAZ_NM + "</td>" +
                        "  <td>  " + item.CEC_ND30 + "</td>" +
                        "  <td>  " + item.CEC_NE + "</td>" +
                        "  <td>  " + item.CEC_NM + "</td>" +
                        "  <td>  " + item.CFP_ND75 + "</td>" +
                        "  <td>  " + item.CFP_NE + "</td>" +
                        "  <td>  " + item.CFP_NM + "</td>" +
                        "  <td>  " + item.CHL_ND30 + "</td>" +
                        "  <td>  " + item.CHL_NE + "</td>" +
                        "  <td>  " + item.CHL_NM + "</td>" +
                        "  <td>  " + item.CIP_ND5 + "</td>" +
                        "  <td>  " + item.CIP_NE + "</td>" +
                        "  <td>  " + item.CIP_NM + "</td>" +
                        "  <td>  " + item.CLI_ND2 + "</td>" +
                        "  <td>  " + item.CLI_NE + "</td>" +
                        "  <td>  " + item.CLI_NM + "</td>" +
                        "  <td>  " + item.COL_ND10 + "</td>" +
                        "  <td>  " + item.COL_NE + "</td>" +
                        "  <td>  " + item.COL_NM + "</td>" +
                        "  <td>  " + item.CPT_ND30 + "</td>" +
                        "  <td>  " + item.CPT_NE + "</td>" +
                        "  <td>  " + item.CPT_NM + "</td>" +
                        "  <td>  " + item.CRO_ND30 + "</td>" +
                        "  <td>  " + item.CRO_NE + "</td>" +
                        "  <td>  " + item.CRO_NM + "</td>" +
                        "  <td>  " + item.CSL_ND30 + "</td>" +
                        "  <td>  " + item.CSL_ND75 + "</td>" +
                        "  <td>  " + item.CSL_NM + "</td>" +
                        "  <td>  " + item.CTT_ND30 + "</td>" +
                        "  <td>  " + item.CTT_NE + "</td>" +
                        "  <td>  " + item.CTT_NM + "</td>" +
                        "  <td>  " + item.CTX_ND30 + "</td>" +
                        "  <td>  " + item.CTX_NE + "</td>" +
                        "  <td>  " + item.CTX_NM + "</td>" +
                        "  <td>  " + item.CXM_ND30 + "</td>" +
                        "  <td>  " + item.CXM_NE + "</td>" +
                        "  <td>  " + item.CXM_NM + "</td>" +
                        "  <td>  " + item.CZA_ND30 + "</td>" +
                        "  <td>  " + item.CZA_NE + "</td>" +
                        "  <td>  " + item.CZA_NM + "</td>" +
                        "  <td>  " + item.CZO_ND30 + "</td>" +
                        "  <td>  " + item.CZO_NE + "</td>" +
                        "  <td>  " + item.CZO_NM + "</td>" +
                        "  <td>  " + item.CZT_ND30 + "</td>" +
                        "  <td>  " + item.CZT_NE + "</td>" +
                        "  <td>  " + item.CZT_NM + "</td>" +
                        "  <td>  " + item.DOR_ND10 + "</td>" +
                        "  <td>  " + item.DOR_NE + "</td>" +
                        "  <td>  " + item.DOR_NM + "</td>" +
                        "  <td>  " + item.DOX_ND30 + "</td>" +
                        "  <td>  " + item.DOX_NE + "</td>" +
                        "  <td>  " + item.DOX_NM + "</td>" +
                        "  <td>  " + item.ERY_ND15 + "</td>" +
                        "  <td>  " + item.ERY_NE + "</td>" +
                        "  <td>  " + item.ERY_NM + "</td>" +
                        "  <td>  " + item.ETP_ND10 + "</td>" +
                        "  <td>  " + item.ETP_NE + "</td>" +
                        "  <td>  " + item.ETP_NM + "</td>" +
                        "  <td>  " + item.FEP_ND30 + "</td>" +
                        "  <td>  " + item.FEP_NE + "</td>" +
                        "  <td>  " + item.FEP_NM + "</td>" +
                        "  <td>  " + item.FOS_ND200 + "</td>" +
                        "  <td>  " + item.FOS_NE + "</td>" +
                        "  <td>  " + item.FOS_NM + "</td>" +
                        "  <td>  " + item.FOX_ND30 + "</td>" +
                        "  <td>  " + item.FOX_NE + "</td>" +
                        "  <td>  " + item.FOX_NM + "</td>" +
                        "  <td>  " + item.GEH_ND120 + "</td>" +
                        "  <td>  " + item.GEH_NM + "</td>" +
                        "  <td>  " + item.GEN_ND10 + "</td>" +
                        "  <td>  " + item.GEN_NE + "</td>" +
                        "  <td>  " + item.GEN_NM + "</td>" +
                        "  <td>  " + item.IPM_ND10 + "</td>" +
                        "  <td>  " + item.IPM_NE + "</td>" +
                        "  <td>  " + item.IPM_NM + "</td>" +
                        "  <td>  " + item.LNZ_ND30 + "</td>" +
                        "  <td>  " + item.LNZ_NE + "</td>" +
                        "  <td>  " + item.LNZ_NM + "</td>" +
                        "  <td>  " + item.LVX_ND5 + "</td>" +
                        "  <td>  " + item.LVX_NE + "</td>" +
                        "  <td>  " + item.LVX_NM + "</td>" +
                        "  <td>  " + item.MEM_ND10 + "</td>" +
                        "  <td>  " + item.MEM_NE + "</td>" +
                        "  <td>  " + item.MEM_NM + "</td>" +
                        "  <td>  " + item.MFX_ND5 + "</td>" +
                        "  <td>  " + item.MFX_NE + "</td>" +
                        "  <td>  " + item.MFX_NM + "</td>" +
                        "  <td>  " + item.MNO_ND30 + "</td>" +
                        "  <td>  " + item.MNO_NE + "</td>" +
                        "  <td>  " + item.MNO_NM + "</td>" +
                        "  <td>  " + item.NET_ND30 + "</td>" +
                        "  <td>  " + item.NET_NE + "</td>" +
                        "  <td>  " + item.NET_NM + "</td>" +
                        "  <td>  " + item.NIT_ND300 + "</td>" +
                        "  <td>  " + item.NIT_NE + "</td>" +
                        "  <td>  " + item.NIT_NM + "</td>" +
                        "  <td>  " + item.OFX_ND5 + "</td>" +
                        "  <td>  " + item.OXA_ND1 + "</td>" +
                        "  <td>  " + item.OXA_NE + "</td>" +
                        "  <td>  " + item.OXA_NM + "</td>" +
                        "  <td>  " + item.PEN_ND10 + "</td>" +
                        "  <td>  " + item.PEN_NE + "</td>" +
                        "  <td>  " + item.PEN_NM + "</td>" +
                        "  <td>  " + item.PIP_ND100 + "</td>" +
                        "  <td>  " + item.PIP_NE + "</td>" +
                        "  <td>  " + item.PIP_NM + "</td>" +
                        "  <td>  " + item.POL_ND300 + "</td>" +
                        "  <td>  " + item.POL_NE + "</td>" +
                        "  <td>  " + item.POL_NM + "</td>" +
                        "  <td>  " + item.QDA_ND15 + "</td>" +
                        "  <td>  " + item.QDA_NE + "</td>" +
                        "  <td>  " + item.QDA_NM + "</td>" +
                        "  <td>  " + item.RIF_ND5 + "</td>" +
                        "  <td>  " + item.RIF_NE + "</td>" +
                        "  <td>  " + item.RIF_NM + "</td>" +
                        "  <td>  " + item.SAM_ND10 + "</td>" +
                        "  <td>  " + item.SAM_NE + "</td>" +
                        "  <td>  " + item.SAM_NM + "</td>" +
                        "  <td>  " + item.STH_ND300 + "</td>" +
                        "  <td>  " + item.STH_NE + "</td>" +
                        "  <td>  " + item.STH_NM + "</td>" +
                        "  <td>  " + item.STR_ND10 + "</td>" +
                        "  <td>  " + item.STR_NE + "</td>" +
                        "  <td>  " + item.STR_NM + "</td>" +
                        "  <td>  " + item.SXT_ND1_2 + "</td>" +
                        "  <td>  " + item.SXT_NE + "</td>" +
                        "  <td>  " + item.SXT_NM + "</td>" +
                        "  <td>  " + item.TCC_ND75 + "</td>" +
                        "  <td>  " + item.TCC_NE + "</td>" +
                        "  <td>  " + item.TCC_NM + "</td>" +
                        "  <td>  " + item.TCY_ND30 + "</td>" +
                        "  <td>  " + item.TCY_NE + "</td>" +
                        "  <td>  " + item.TCY_NM + "</td>" +
                        "  <td>  " + item.TEC_ND30 + "</td>" +
                        "  <td>  " + item.TEC_NE + "</td>" +
                        "  <td>  " + item.TEC_NM + "</td>" +
                        "  <td>  " + item.TGC_ND15 + "</td>" +
                        "  <td>  " + item.TGC_NE + "</td>" +
                        "  <td>  " + item.TGC_NM + "</td>" +
                        "  <td>  " + item.TIC_ND75 + "</td>" +
                        "  <td>  " + item.TIC_NE + "</td>" +
                        "  <td>  " + item.TIC_NM + "</td>" +
                        "  <td>  " + item.TOB_ND10 + "</td>" +
                        "  <td>  " + item.TOB_NE + "</td>" +
                        "  <td>  " + item.TOB_NM + "</td>" +
                        "  <td>  " + item.TZP_ND100 + "</td>" +
                        "  <td>  " + item.TZP_NE + "</td>" +
                        "  <td>  " + item.TZP_NM + "</td>" +
                        "  <td>  " + item.VAN_ND30 + "</td>" +
                        "  <td>  " + item.VAN_NE + "</td>" +
                        "  <td>  " + item.VAN_NM + "</td>" +
                    "</tr>";
                }
                $("#detailDataBody").html(html);
            } else {
                $("#detailDataBody").html("");
            }

            //显示分页
            laypage({
                cont: "detailDataPager", //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
                pages: result.total, //通过后台拿到的总页数
                curr: curr || 1, //当前页
                skip: true, //是否开启跳页
                skin: '#AF0000',
                groups: 7, //连续显示分页数
                jump: function (obj, first) { //触发分页后的回调
                    if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                        LayPageData(obj.curr);
                    }
                }
            });
        },
    });
}
