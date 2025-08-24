function compare(property) {
    return function (a, b) {
        var value1 = a[property];
        var value2 = b[property];
        return value1 - value2;
    };
}

(function ($) {
    $.fn.updateValidation = function () {
        var $this = $(this);
        var form = $this.closest("form")
            .removeData("validator")
            .removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse(form);

        return $this;
    };

    $.fn.updateSingleDataTableName = function () {
        //console.log("调用$('#single_datatable').updateSingleDataTableName();");
        var $this = $(this);
        var $table = $this.closest('table');
        $table.find('tbody > tr[id]').each(function (i, item) {

            var name = 'ItemModel[' + i + '].Name';
            var value = 'ItemModel[' + i + '].Value';
            var display = 'ItemModel[' + i + '].Display';
            var sort = 'ItemModel[' + i + '].Sort';

            item.cells[0].querySelector('input[name^="ItemModel"]').name = name;
            item.cells[0].querySelector('span[data-valmsg-for]').setAttribute('data-valmsg-for', name);

            item.cells[1].querySelector('input[name^="ItemModel"]').name = value;
            item.cells[1].querySelector('span[data-valmsg-for]').setAttribute('data-valmsg-for', value);

            item.cells[2].querySelector('select[name^="ItemModel"]').name = display;

            item.cells[3].querySelector('input[name^="ItemModel"]').name = sort;
            item.cells[3].querySelector('span[data-valmsg-for]').setAttribute('data-valmsg-for', sort);

            item.cells[3].querySelector('input[name^="ItemModel"]').value = i + 1;
        });

        $table.closest('form#BarChartForm').updateValidation();
        return $this;
    };

    $.validator.setDefaults({
        onkeyup: function (element) {
            this.element(element);
        },
        onfocusout: function (element) {
            this.element(element);
        },
        onfocusin: function (element, event) {
            //找到显示错误提示的标签并移除,针对jquery.validate.unobtrusive
            var errorElement = $(element).next('span.field-validation-error');
            if (errorElement) {
                //errorElement.children().remove();
            }
        }
    });
})(jQuery);

jQuery.validator.addMethod("unique_dataitem_name", function (value, element, params) {
    var __names = [];
    if (params.type === '2' || params.type === 2) {
        $('#multiple_datatable > tbody > tr[id]').each(function (i, item) {
            var _value = $(item).find('td:eq(1) > input[id][name^="ItemModel"][name$=".Name"]').val();
            if (!s.isBlank(_value)) {
                __names.push(_value);
            }
        });
    }
    else {
        $('#single_datatable > tbody > tr[id]').each(function (i, item) {
            var _value = $(item).find('td:eq(0) > input[id][name^="ItemModel"][name$=".Name"]').val();
            if (!s.isBlank(_value)) {
                __names.push(_value);
            }
        });
    }
    var result = __names.filter(function (_element, index, array) { return _element === value; });
    return this.optional(element) || result.length <= 1;
}, '此数据名称已经存在');

$.validator.unobtrusive.adapters.add('unique_dataitem_name', ['type'], function (options) {
    options.rules["unique_dataitem_name"] = {
        type: options.params.type
    };
    options.messages["unique_dataitem_name"] = options.message;
});

/* 添加数据项 */
onAddItemRow = function () {
    var l = Ladda.create(document.getElementById('btnAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();

    var $tbody = $('#single_datatable > tbody');
    var arrSort = [],
        arrData = [];

    $tbody.find('input[name^="ItemModel"][name$=".Sort"]').each(function (i, item) {
        if (!s.isBlank(item.value)) {
            var _sort = parseInt(item.value) || 0;
            if (_sort && _sort > 0) {
                arrSort.push(_sort);
            }
        }
    });

    arrData.push({
        uuid: Utils.Common.getUuid(),
        name: null,
        value: null,
        display: true,
        sort: arrSort.length > 0 ? Math.max(...arrSort) + 1 : null
    });

    setTimeout(function () {
        $tbody.append(template('template_singletable_tbody', { data: arrData })).updateSingleDataTableName();
        setTimeout(function () {
            l.stop();
        }, 300);
    }, 300);
    return false;
    //var table = document.getElementById('single_datatable'); // 获取表格信息
    //var celllen = table.rows[0].cells.length; // 获得列数
    //var newTR = table.tBodies[0].insertRow(); // 插入行
    //newTR.id = 'r' + Utils.Common.getUuid();
    //setTimeout(function () {
    //    newTR.innerHTML = template('template_table_insertRow', { rowid: newTR.id, uuid: Utils.Common.getUuid() });
    //    setTimeout(function () {
    //        $(table).updateSingleDataTableName();
    //        l.stop();
    //    }, 200);
    //}, 300);
};

/* 删除数据项 */
onDeleteItem = function (id) {
    if (id) {
        $(id).remove();
        $('#single_datatable').updateSingleDataTableName();
    }
};

//排序数据项
onSortItem = function () {
    $('#single_datatable').updateSingleDataTableName();
};

/* 预览 */
onPreview = function () {
    $('#modal_preview').modal('show');
};

/* 预览modal显示事件 */
$('#modal_preview').on('shown.bs.modal', function () {
    var xAxisData = [],
        seriesData = [],
        _data = [];

    $('#single_datatable > tbody > tr[id]').each(function (strIndex, strItem) {
        var _name = $(strItem).find('input[id][name^="ItemModel"][name$=".Name"]').val(),
            _value = $(strItem).find('input[id][name^="ItemModel"][name$=".Value"]').val(),
            _display = $(strItem).find('select[id][name^="ItemModel"][name$=".Display"]').val(),
            _sort = $(strItem).find('input[id][name^="ItemModel"][name$=".Sort"]').val();

        if (!s.isBlank(_name)) {
            _value = !s.isBlank(_value) ? _value : '0.0';
            _value = _display === true || _display === 'True' || _display === 'true' ? _value : '-';
            _sort = !s.isBlank(_sort) ? parseInt(_sort) || 1 : 1;
            _data.push({
                name: _name,
                value: _value,
                sort: _sort
            });
        }
    });

    _data = _data.sort(compare('sort'));

    for (var i = 0, len = _data.length; i < len; i++) {
        xAxisData.push(_data[i].name);
        seriesData.push(_data[i].value);
    }
    initSingleDataChart(xAxisData, seriesData);
});

function initSingleDataChart(xAxisData, seriesData) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('chartmain'));
    var chartType = $('#single_container #ChartType').val();
    chartType = chartType === '2' || chartType === 2 || chartType === 'Bar' ? 'bar' : 'line';
    myChart.showLoading();
    var xRotate = 0;
    if (xAxisData.length > 8 && xAxisData.length <= 12) {
        xRotate = 35;
    }
    else if (xAxisData.length > 12 && xAxisData.length <= 18) {
        xRotate = 45;
    }
    else if (xAxisData.length > 18 && xAxisData.length <= 25) {
        xRotate = 60;
    }
    else if (xAxisData.length > 25) {
        xRotate = 90;
    }
    option = {
        color: [$('#single_container  #BarColor').val()],
        title: {
            text: document.getElementById('Title').value,
            subtext: document.getElementById('SubTitle').value,
            left: 'center',
            show: true
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
            top: '18%',
            left: '3%',
            right: '4%',
            bottom: '15%',
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
                        fontSize: "14px"
                    }
                },
                data: xAxisData,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
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
        toolbox: {
            show: true,
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
                type: chartType,
                barWidth: 30,//柱图宽度
                data: seriesData,//0, 0, 0, 12.7, 31.2, 56.7, 55.7, 42.9, 54.7, 85.7, 100, 100
                //以下为是否显示，显示位置和显示格式的设置了
                itemStyle: {
                    normal: {

                        //以下为是否显示，显示位置和显示格式的设置了
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
            fontSize: 12
        }
    };

    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);
}


/* 打开excel导入modal */
$('#singleExcelModal').on('shown.bs.modal', function () {

});

$('#singleExcelModal').on('hidden.bs.modal', function () {
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        $('#singleExcelModal #excelFile').replaceWith($('#singleExcelModal #excelFile').clone(true));
    }
    else {
        $('#singleExcelModal #excelFile').val('');
    }
});

/* 提交excel导入 */
onSingleExcelSubmit = function () {
    var l = Ladda.create(document.getElementById('btnSingleExcelSubmit')),
        $modal = $('#singleExcelModal'),
        files = $modal.find('#excelFile')[0].files,
        formData = new FormData();

    if (!files || files.length <= 0) {
        alert('请选择柱状图数据上传文件');
        return false;
    }
    if (!l.isLoading()) {
        formData.append('file', files[0]);
        $.ajax({
            url: '/MgtChart/BarChartSingleExcel_OnSubmit',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () { l.start(); },
            complete: function () { setTimeout(function () { l.stop(); }, 600); },
            success: function (res) {
                if (res.status) {
                    var arrData = res.data;
                    $('#single_datatable > tbody > tr[id]').each(function (i, item) {
                        var $tr = $(item),
                            item_name = $tr.find('input[id][name^="ItemModel"][name$=".Name"]').val();

                        if (!s.isBlank(item_name)) {
                            var item_value = $tr.find('input[id][name^="ItemModel"][name$=".Value"]').val(),
                                item_display = $tr.find('select[id][name^="ItemModel"][name$=".Display"]').val(),
                                item_sort = $tr.find('input[id][name^="ItemModel"][name$=".Sort"]').val();
                            if (!s.isBlank(item_value)) {
                                var item_exist = arrData.findIndex(function (value, index, arr) {
                                    return value.name === item_name;
                                });

                                if (item_exist === -1) {
                                    arrData.push({
                                        uuid: Utils.Common.getUuid(),
                                        name: item_name,
                                        value: item_value,
                                        display: item_display === 'True',
                                        sort: item_sort
                                    });
                                }
                            }
                        }
                    });

                    arrData = arrData.sort(compare('sort'));
                    // 重新生成table
                    var _table = document.getElementById('single_datatable'); // 获取表格信息
                    var _tbody = _table.tBodies[0];
                    _tbody.innerHTML = template('template_singletable_tbody', { data: arrData });
                    $(_table).updateSingleDataTableName();
                    $modal.modal('hide');
                }
                else {
                    alert(res.message);
                }
            }
        });
    }
    return false;
};