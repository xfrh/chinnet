
$(document).ready(function () {
    LayPageData(0);
})


//加载数据验证的列表数据
function LayPageData(curr) {
    var aa = $("#DataId").val();
    $.ajax({
        type: 'POST',
        url: "/MicDistribution/GetDataList",
        dataType: 'json',
        data: {
            DataId: aa,
            Page: curr || 1,  //向服务端传的参数，此处只是演示
            PageSize: 100//每页10行
        },
        success: function (result) {

            //绑定页面的数据
            if (result.rows != null && result.rows.length > 0) {
                var html = "";
                var index = 0;
                for (var i = 0; i < result.rows.length; i++) {
                    var strs = new Array(); //定义一数组
                    strs = result.rows[i].antibiotics_value.split(","); //字符分割
                    var item = result.rows[i];
                    var td = "";
                    for (var j = 0; j < strs.length - 1; j++) {
                       td += "  <td> 	" + strs[j] + "</td>    "
                    }
                    html += "  <tr onclick='SelectDataItem(this)' SelectItem='0'>" +                       
                        "  <td> 	" + item.patinet_id + "</td> " +
                        "  <td> 	" + item.ward + "</td>         " +
                        "  <td> 	" + item.specimem + "</td>     " +
                        "  <td> 	" + item.specimem_date + "</td>                 " +
                        "  <td> 	" + item.Specimen_Type + "</td>         " +
                        "  <td> 	" + item.organism + "</td>     " +
                        "  <td> 	" + item.organism_Type + "</td> " + td +    
                        //"  <td><a href='/MicDistribution/DocumentDataView/" + item.tem_id+"'>编辑</a></td> "+
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
