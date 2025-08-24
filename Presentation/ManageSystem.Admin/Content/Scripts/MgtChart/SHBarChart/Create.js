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
        });

        $table.closest('form#BarChartForm').updateValidation();
        return $this;
    };

    $.fn.updateMultipleDataTableName = function () {
        var $this = $(this);
        var $table = $this.closest('table');

        $table.find('thead > tr > th[id]').each(function (i, item) {
            var $guid = $(item).find('input[name^="Antibiotics"][name$=".Guid"]');
            var $name = $(item).find('input[id][name^="Antibiotic"][name$=".Name"]');
            $guid.attr('name', 'Antibiotics[' + i + '].Guid');
            $name.attr('name', 'Antibiotics[' + i + '].Name');
        });

        $table.find('tbody > tr[id]').each(function (i, item) {
            var name = 'ItemModel[' + i + '].Name';
            var color = 'ItemModel[' + i + '].BarColor';
            var display = 'ItemModel[' + i + '].Display';
            var chartType = 'ItemModel[' + i + '].ChartType';

            $(item).find('td:eq(1) > input[name^="ItemModel"][name$=".Name"][type="text"]').attr('name', name);
            $(item).find('td:eq(1) > span[data-valmsg-for]').attr('data-valmsg-for', name);

            $(item).find('td:eq(2) > input[name^="ItemModel"][name$=".BarColor"]').attr('name', color);

            $(item).find('td:eq(3) > select[name^="ItemModel"][name$=".Display"]').attr('name', display);

            $(item).find('td:eq(4) > select[name^="ItemModel"][name$=".ChartType"]').attr('name', chartType);

            for (var idx = 0, len = item.cells.length - 5; idx < len; idx++) {
                var __uuid = 'ItemModel[' + i + '].Antibiotics[' + idx + '].Guid';
                var __name = 'ItemModel[' + i + '].Antibiotics[' + idx + '].Value';
                $(item).find('td:eq(' + (5 + idx) + ') > input[name^="ItemModel"][name*=".Antibiotics"][name$=".Guid"][type="hidden"]').attr('name', __uuid);
                $(item).find('td:eq(' + (5 + idx) + ') > input[name^="ItemModel"][name*=".Antibiotics"][name$=".Value"][type="tel"]').attr('name', __name);
                $(item).find('td:eq(' + (5 + idx) + ') > span[data-valmsg-for]').attr('data-valmsg-for', __name);
            }
        });

        if ($table.find('thead > tr > th[id] > input[name^="Antibiotics"][name$=".Name"]').length === 0) {
            $('#multiple_cell_tooptip').show();
        }
        else {
            $('#multiple_cell_tooptip').hide();
        }

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
        },
        submitHandler: function (form) {

            //return false;
            form.submit();
        }
    });
})(jQuery);

var arrColors = ['#015baa', '#c1232b', '#fe8463', '#ecbf00', '#cf7ca6', '#749f83'];

$(validateMultipleCreateTableCellForm());

$(function () {
    setTimeout(function () {
        $('.loading-container').removeClass('loading-inactive');
    }, 100);
    onAddItemRow();
    onAddMultipleItemRow();
    setTimeout(function () {
        init();
        setTimeout(function () {
            $('.loading-container').addClass('loading-inactive');
        }, 300);
    }, 800);
});

async function init() {
    var dataItemType = $('input#DataItemType').val();
    dataItemType = dataItemType === '2' || dataItemType === 2 ? 2 : 1;

    if (!Utils.Explorer.IEVersion() && window.indexedDB) {
        if (!jsstoreConn) {
            jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
        }

        initDb();

        var single_container_obj = await jsstoreConn.select({
            from: 'TBPanelHtml',
            where: { dataitemtype: 1 }
        });

        if (single_container_obj && single_container_obj.length > 0) {
            await jsstoreConn.update({
                in: 'TBPanelHtml',
                where: { dataitemtype: 1 },
                set: { html: $('#single_container').html(), data: [] }
            });

            await jsstoreConn.update({
                in: 'TBPanelHtml',
                where: { dataitemtype: 2 },
                set: { html: $('#multiple_container').html(), data: [] }
            });
        }
        else {
            var single_container_html = {
                dataitemtype: 1,
                html: $('#single_container').html(),
                data: []
            };

            var multiple_container_html = {
                dataitemtype: 2,
                html: $('#multiple_container').html(),
                data: []
            };

            var noOfDataInserted = await jsstoreConn.insert({
                into: 'TBPanelHtml',
                values: [single_container_html, multiple_container_html]
            });
        }

    }
    else {
        sessionStorage.setItem('single_container_cache', $single_container.html());
        sessionStorage.setItem('multiple_container_cache', $multiple_container.html());
    }
}

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
}, '此数据名称已经存在2');

$.validator.unobtrusive.adapters.add('unique_dataitem_name', ['type'], function (options) {
    options.rules["unique_dataitem_name"] = {
        type: options.params.type
    };
    options.messages["unique_dataitem_name"] = options.message;
});

jQuery.validator.addMethod("unique_antibiotic", function (value, element, params) {
    var __names = [];
    $('#multiple_datatable > thead > tr > th[id]').each(function (i, item) {
        var _value = $(item).find('input[id][name^="Antibiotics"][name$=".Name"]').val();
        if (!s.isBlank(_value)) {
            __names.push(_value);
        }
    });
    var result = __names.filter(function (_element, index, array) { return _element === value; });

    return this.optional(element) || result.length === 0;
}, '此抗生素已经存在');

$.validator.unobtrusive.adapters.add('unique_antibiotic', function (options) {
    options.rules["unique_antibiotic"] = {};
    options.messages["unique_antibiotic"] = options.message;
});

function validateMultipleCreateTableCellForm() {
    return $('#MultipleCreateTableCellForm').validate({
        debug: true,
        errorContainer: '#MultipleAntibioticNameError',
        errorLabelContainer: '#MultipleAntibioticNameError',
        errorElement: "span",
        errorClass: "field-validation-error",
        onkeyup: function (element) {
            this.element(element);
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            MultipleAntibioticName: {
                required: true,
                unique_antibiotic: true,
                minlength: 2
            }
        },
        messages: {
            MultipleAntibioticName: {
                required: '请输入抗生素名称',
                minlength: '抗生素名称至少需要2个字符'
            }
        },
        submitHandler: function (form) {
            insertTableCol();
            return false;
        }
    });
}

/* tab标签页切换 */
$('a[data-toggle="tab"]').on('shown.bs.tab', async function (e) {
    var dataitemtype = $(e.target).data('dataitemtype');
    dataitemtype = dataitemtype === '2' || dataitemtype === 2 ? 2 : 1;

    var $single_container = $('#single_container');
    var $multiple_container = $('#multiple_container');

    $('#DataItemType').val(dataitemtype);

    if (!Utils.Explorer.IEVersion() && window.indexedDB) {
        // 非IE
        if (dataitemtype === 2) {
            var data_list = [];
            $('#single_datatable > tbody > tr[id]').each(function (i, item) {
                var $name = $(item).find('td:eq(0) > input[name^="ItemModel"]');
                var $value = $(item).find('td:eq(1) > input[name^="ItemModel"]');
                var $display = $(item).find('td:eq(2) > select[name^="ItemModel"]');
                var $sort = $(item).find('td:eq(3) > input[name^="ItemModel"]');
                data_list.push({
                    name: { id: $name.attr('id'), value: $name.val() },
                    value: { id: $value.attr('id'), value: $value.val() },
                    display: { id: $display.attr('id'), value: $display.val() },
                    sort: { id: $sort.attr('id'), value: $sort.val() }
                });
            });
            await jsstoreConn.update({
                in: 'TBPanelHtml',
                where: { dataitemtype: 1 },
                set: { html: $single_container.html(), data: data_list }
            });
            await jsstoreConn.select({
                from: 'TBPanelHtml',
                where: { dataitemtype: 2 }
            }).then(function (result) {
                if (result && result.length > 0) {
                    $multiple_container.html(result[0].html);
                    for (var i = 0, len = result[0].data.length; i < len; i++) {
                        var __data = result[0].data[i];
                        $('#' + __data.name.id).val(__data.name.value);
                        $('#' + __data.color.id).val(__data.color.value);
                        $('#' + __data.display.id).val(__data.display.value);
                        $('#' + __data.chartType.id).val(__data.chartType.value);
                        if (__data.values && __data.values.length > 0) {
                            for (var ii = 0, ilen = __data.values.length; ii < ilen; ii++) {
                                $('#' + __data.values[ii].id).val(__data.values[ii].value);
                            }
                        }
                    }
                    $single_container.empty();
                }
            });
        }
        else {
            var data_list2 = [];
            $('#multiple_datatable > tbody > tr[id]').each(function (i, item) {
                var $name = $(item).find('td:eq(1) > input[name^="ItemModel"]');
                var $color = $(item).find('td:eq(2) > input[name^="ItemModel"]');
                var $display = $(item).find('td:eq(3) > select[name^="ItemModel"]');
                var $chartType = $(item).find('td:eq(4) > select[name^="ItemModel"]');
                var __values = [];
                $(item).find('td[data-cell]').each(function (ii, iitem) {
                    var $__input = $(iitem).find('input[id][name$=".Value"]');
                    __values.push({ id: $__input.attr('id'), value: $__input.val() });
                });
                data_list2.push({
                    name: { id: $name.attr('id'), value: $name.val() },
                    color: { id: $color.attr('id'), value: $color.val() },
                    display: { id: $display.attr('id'), value: $display.val() },
                    chartType: { id: $chartType.attr('id'), value: $chartType.val() },
                    values: __values
                });
            });

            await jsstoreConn.update({
                in: 'TBPanelHtml',
                where: { dataitemtype: 2 },
                set: { html: $multiple_container.html(), data: data_list2 }
            });

            await jsstoreConn.select({
                from: 'TBPanelHtml',
                where: { dataitemtype: 1 }
            }).then(function (result) {
                if (result && result.length > 0) {
                    $single_container.html(result[0].html);
                    for (var i = 0, len = result[0].data.length; i < len; i++) {
                        $('#' + result[0].data[i].name.id).val(result[0].data[i].name.value);
                        $('#' + result[0].data[i].value.id).val(result[0].data[i].value.value);
                        $('#' + result[0].data[i].display.id).val(result[0].data[i].display.value);
                        $('#' + result[0].data[i].sort.id).val(result[0].data[i].sort.value);
                    }
                    $multiple_container.empty();
                }
            });
        }

    }
    else {
        // IE
        if (dataitemtype === 2) {
            // 缓存单数据，显示多数据
            sessionStorage.setItem('single_container_cache', $single_container.html());
            $single_container.empty();
            $multiple_container.html(sessionStorage.getItem('multiple_container_cache'));
        }
        else {
            // 缓存多数据，显示单数据
            sessionStorage.setItem('multiple_container_cache', $multiple_container.html());
            $multiple_container.empty();
            $single_container.html(sessionStorage.getItem('single_container_cache'));
        }
    }

    if (dataitemtype === 2) {

        if ($('table#multiple_datatable > thead > tr > th[id] > input[name^="Antibiotics"][id]').length === 0) {
            $('#multiple_cell_tooptip').show();
        }
        else {
            $('#multiple_cell_tooptip').hide();
        }

        $('#multiple_datatable').updateMultipleDataTableName();
    }
    else {
        $('#single_datatable').updateSingleDataTableName();
    }
});

/************************************************************************************************************/
/********************************************************************************    单数据操作    ***********/
/************************************************************************************************************/

/* 添加数据项 */
onAddItemRow = function () {
    var l = Ladda.create(document.getElementById('btnAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();
    var $tbody = $('#single_datatable > tbody#item_tbody');
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
};
/*插入数据项*/
onAddItemRow1 = function (id)
{
    var l = Ladda.create(document.getElementById('btnAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();
    var $tbody = $('#single_datatable > tbody#item_tbody');
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
        //sort: arrSort.length > 0 ? Math.max(...arrSort) + 1 : null
    });
    setTimeout(function () {
        $(id).after(template('template_singletable_tbody', { data: arrData })).updateSingleDataTableName();
        var length = $tbody.find('input[name^="ItemModel"][name$=".Sort"]').length;
        for (var i = 0; i < length; i++) {
            $('input[name="ItemModel[' + i + '].Sort"]').val(i + 1);
        }
        setTimeout(function () {
            l.stop();
        }, 300);
    }, 300);
    
}

/* 删除数据项 */
onDeleteItem = function (id) {
    if (id) {
        $(id).remove();
        $('#single_datatable').updateSingleDataTableName();
    }
};

/************************************************************************************************************/
/********************************************************************************    预  览    ***************/
/************************************************************************************************************/

/* 预览 */
onPreview = function () {
    $('#modal_preview').modal('show');
};

/* 预览modal显示事件 */
$('#modal_preview').on('shown.bs.modal', function () {
    var dataItemType = $('#DataItemType').val();
    dataItemType = dataItemType === 'SingleData' || dataItemType === '1' || dataItemType === 1 ? 1 : 2;
    if (dataItemType === 1) {
        var xAxisData = [], seriesData = [];
        var _data = [];
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
    }
    else {
        var mxAxisData = [], mseries = [],
            mColorData = [];
        var _mdata = [];
        $('#multiple_datatable > thead > tr > th[id]').each(function (mthIndex, mthItem) {
            var _name = $(mthItem).find('input[id][name^="Antibiotics"][name$=".Name"]').val();
            if (!s.isBlank(_name)) {
                mxAxisData.push(_name);
            }
        });

        $('#multiple_datatable > tbody > tr[id]').each(function (mtrIndex, mtrItem) {
            var _color = $(mtrItem).find('input[id][name^="ItemModel"][name$=".BarColor"]').val();
            if (!s.isBlank(_color)) {
                mColorData.push(_color);
            }
            var _name = $(mtrItem).find('input[id][name^="ItemModel"][name$=".Name"]').val();
            var _display = $(mtrItem).find('select[id][name^="ItemModel"][name$=".Display"]').val();
            _display = _display === true || _display === 'True' || _display === 'true' ? true : false;
            var _type = $(mtrItem).find('select[id][name^="ItemModel"][name$=".ChartType"]').val();
            var _msdata = [];
            $(mtrItem).find('td[data-cell]').each(function (mtdIndex, mtdItem) {
                var _value = $(mtdItem).find('input[id][name^="ItemModel"][name*=".Antibiotics"][name$=".Value"]').val();
                _value = !s.isBlank(_value) ? _value : '-';
                _value = _display ? _value : '-';
                _msdata.push(_value);
            });

            if (!s.isBlank(_name)) {
                mseries.push({
                    name: _name,
                    type: _type === 'Bar' || _type === '2' || _type === 2 ? 'bar' : 'line',
                    stack: '',
                    barWidth: 30,
                    label: {
                        normal: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        }
                    },
                    data: _msdata
                });
            }
        });

        initMultipleDataChart(mxAxisData, mColorData, mseries);
    }
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
            text: document.getElementById('Title').value + document.getElementById('SubTitle').value,
            //subtext: document.getElementById('SubTitle').value,
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

function initMultipleDataChart(xAxisData, color, series) {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById('chartmain'));
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
        color: color,
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
        series: series,
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

/************************************************************************************************************/
/*****************************************************************************    单/多数据Excel导入    *******/
/************************************************************************************************************/
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
            url: '/MgtSHChart/SHBarChartSingleExcel_OnSubmit',
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

/*****************************************************************************************************************/

/* 打开excel导入modal */
$('#multipleExcelModal').on('shown.bs.modal', function () {

});

$('#multipleExcelModal').on('hidden.bs.modal', function () {
    if (!!window.ActiveXObject || "ActiveXObject" in window) {
        $('#multipleExcelModal #excelFile').replaceWith($('#multipleExcelModal #excelFile').clone(true));
    }
    else {
        $('#multipleExcelModal #excelFile').val('');
    }
});

/* 提交excel导入 */
onMultipleExcelSubmit = function () {
    var l = Ladda.create(document.getElementById('btnMultipleExcelSubmit')),
        $modal = $('#multipleExcelModal'),
        files = $modal.find('#excelFile')[0].files,
        formData = new FormData();

    if (!files || files.length <= 0) {
        alert('请选择柱状图多数据上传文件');
        return false;
    }
    if (!l.isLoading()) {
        formData.append('file', files[0]);
        $.ajax({
            url: '/SHMgtChart/SHBarChartMultipleExcel_OnSubmit',
            method: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () { l.start(); },
            complete: function () { setTimeout(function () { l.stop(); }, 600); },
            success: function (res) {
                if (res.status) {
                    var _table = document.getElementById('multiple_datatable'); // 获取表格信息
                    _table.innerHTML = template('template_multipletable_table', { antibiotics: res.antibiotics, data: res.tbody });
                    $(_table).updateMultipleDataTableName();
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

/************************************************************************************************************/
/********************************************************************************    多数据操作    ***********/
/************************************************************************************************************/
/* 打开添加列modal */
onMultipleAddTableCell = function () {
    $('#modal-tablecell').modal('show');
};

/* 提交添加列modal */
onSubmitMultipleTableCell = function () {
    if (validateMultipleCreateTableCellForm().form()) {
        insertTableCol();
    }
    return false;
};

function insertTableCol() {
    var $modal = $('#modal-tablecell');
    var antibiotic = $modal.find('input#MultipleAntibioticName').val();

    var c = document.getElementById('multiple_datatable'), // 获取表格信息
        rowlen = c.rows.length, // 获得行数
        celllen = c.rows[0].cells.length, // 获得列数
        newTH = c.rows[0].insertBefore(document.createElement('th'), null),
        _uuid = Utils.Common.getUuid();

    newTH.id = 'th' + _uuid;
    newTH.setAttribute('data-cell', _uuid);
    newTH.setAttribute('class', 'text-center pos-rel user-select-no cursor-pointer nowrap');
    var antblen = antibiotic.gblen();
    if (antblen <= 8) {
        newTH.classList.add('w8char');
    }
    else if (antblen > 8 && antblen <= 13) {
        newTH.classList.add('w12char');
    }
    else {
        newTH.classList.add('w15char');
    }

    newTH.innerHTML = template('template_table_thead', {
        antibiotics: [{
            Guid: _uuid,
            Name: antibiotic,
            Value: null
        }]
    });

    for (var rowindex = 1; rowindex < rowlen; rowindex++) {
        var newTD = c.rows[rowindex].insertCell(celllen); // 依次向每一行的末尾插入一个新列
        newTD.innerHTML = template('template_table_insertCol', { uuid: _uuid });
        newTD.setAttribute('data-cell', _uuid);
    }
    $modal.modal('hide');

    $(c).updateMultipleDataTableName();
}


/* 添加列modal隐藏事件 */
$('#modal-tablecell').on('hidden.bs.modal', function () {
    $('#modal-tablecell #MultipleAntibioticName').val(null);
    validateMultipleCreateTableCellForm().resetForm();
});

/* 删除多数据添加的列 */
onTableDeleteCol = function (obj) {
    var $modal = $('#modal-tablecell-delete');
    $modal.find('div.modal-body').html(template('tpl_multiple_tablecell_delete', {
        uuid: obj.dataset.cell,
        antibiotic: $('#multiple_datatable > thead > tr >th[id] > input[name^="Antibiotics"][name$=".Guid"][value="' + obj.dataset.cell + '"]').data('value')
    }));
    $modal.modal('show');
};

/* 删除列modal隐藏事件 */
$('#modal-tablecell-delete').on('hidden.bs.modal', function () {
    $('#modal-tablecell-delete div.modal-body').empty();
});

/* 确认删除列 */
onSubmitMultipleTableCellDelete = function () {
    var l = Ladda.create(document.getElementById('btnMultipleTableCellDelete'));
    if (!l.isLoading()) {
        l.start();

        var uuid = $('#modal-tablecell-delete').find('#multiple_tablecell_delete_uuid').val();

        $('table#multiple_datatable').find('th[data-cell="' + uuid + '"],td[data-cell="' + uuid + '"]').remove();

        setTimeout(function () {
            $('#modal-tablecell-delete').modal('hide');
            $('#multiple_datatable').updateMultipleDataTableName();
            l.stop();
        }, 500);
    }
    return false;
};

/* 所数据添加行 */
onAddMultipleItemRow = function () {

    var l = Ladda.create(document.getElementById('btnMultipleAddItemRow'));
    if (l.isLoading()) { return false; }
    l.start();

    var c = document.getElementById('multiple_datatable'); // 获取表格信息
    var rowlen = c.rows.length; // 获得行数

    if (rowlen > 8) {
        setTimeout(function () {
            toastr.warning('已达到添加行数据上限');
            setTimeout(function () {
                l.stop();
            }, 200);
        }, 300);
        return false;
    }

    var newTR = c.tBodies[0].insertRow(),
        _uuid = Utils.Common.getUuid(),
        _color = generateColorTo16(),
        newTHs = c.rows[0].querySelectorAll('th[data-cell]'),
        _antibiotics = [];

    for (var i = 0, len = newTHs.length; i < len; i++) {
        _antibiotics.push(newTHs[i].dataset.cell);
    }

    if (arrColors.length >= rowlen - 1) {
        _color = arrColors[rowlen - 1];
    }

    newTR.id = 'list-item-' + _uuid;
    setTimeout(function () {
        newTR.innerHTML = template('tpl_multiple_table_addrow', {
            uuid: _uuid,
            color: _color,
            antibiotics: _antibiotics
        });

        setTimeout(function () {
            $(c).updateMultipleDataTableName();
            l.stop();
        }, 200);
    }, 300);

    return false;
};

/* 删除行 */
onDeleteMultipleTableRow = function (id) {
    if (id) {
        $(id).remove();
        $('#multiple_datatable').updateMultipleDataTableName();
    }
};

/************************************************************************************************************/
/********************************************************************************    Form  提交    ***********/
/************************************************************************************************************/
/* Form提交 */
onSubmit = function () {
    var dataItemType = $('input#DataItemType').val() === '2' ? 2 : 1;
    switch (dataItemType) {
        case 1: // 单数据
            break;
        case 2: // 多数据
            if ($('table#multiple_datatable > thead > tr > th[id] > input[name^="Antibiotics"][name$=".Name"]').length === 0) {
                if ($('#BarChartForm').validate().form()) {
                    //alert('多数据展示请点击“添加一列数据”来更多的数据列');
                }
                alert('请添加更多数据列');
                return false;
            }

            if ($('table#multiple_datatable > tbody > tr[id]').length === 0) {
                if ($('#BarChartForm').validate().form()) {
                    //alert('多数据展示请点击“添加一列数据”来更多的数据列');
                }
                alert('请添加更多数据列');
                return false;
            }
            break;
        default:
            return false;
    }
    return true;
};












