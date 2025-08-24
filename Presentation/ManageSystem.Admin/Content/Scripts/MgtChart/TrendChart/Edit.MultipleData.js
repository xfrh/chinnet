String.prototype.gblen = function () {
    var len = 0;
    for (var i = 0; i < this.length; i++) {
        if (this.charCodeAt(i) > 127 || this.charCodeAt(i) === 94) {
            len += 2;
        }
        else {
            len++;
        }
    }
    return len;
};

window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;

var jsstoreConn = undefined;


async function initDb() {
    //await jsstoreConn.dropDb();
    var isDbCreated = await jsstoreConn.initDb(getDbSchema());
    if (isDbCreated) {
        console.log('db created');
    }
    else {
        console.log('db opened');
    }
}

function getDbSchema() {
    var table = {
        name: 'TBPanelHtml',
        columns: {
            id: { primaryKey: true, autoIncrement: true },
            dataitemtype: { notNull: true, dataType: 'number' },
            html: { notNull: true, dataType: 'string' },
            data: { notNull: false, dataType: 'array' }
        }
    };
    var db = {
        name: '',
        tables: [table]
    };
    return db;
}

/* 生成16进制颜色代码 */
function generateColorTo16() {//十六进制颜色随机
    var r = Math.floor(Math.random() * 256);
    var g = Math.floor(Math.random() * 256);
    var b = Math.floor(Math.random() * 256);
    var color = '#' + r.toString(16) + g.toString(16) + b.toString(16);
    return color;
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
            var color = 'ItemModel[' + i + '].ChartColor';
            var display = 'ItemModel[' + i + '].Display';
            var chartType = 'ItemModel[' + i + '].ChartType';

            $(item).find('td:eq(1) > input[name^="ItemModel"][name$=".Name"][type="text"]').attr('name', name);
            $(item).find('td:eq(1) > span[data-valmsg-for]').attr('data-valmsg-for', name);

            $(item).find('td:eq(2) > input[name^="ItemModel"][name$=".ChartColor"]').attr('name', color);

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

        $table.closest('form#TrendChartForm').updateValidation();
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

$(validateInsertColForm());

function validateInsertColForm() {
    return $('#insertColForm').validate({
        debug: true,
        errorContainer: '#AntibioticNameError',
        errorLabelContainer: '#AntibioticNameError',
        errorElement: "span",
        errorClass: "field-validation-error",
        onkeyup: function (element) {
            this.element(element);
        },
        onfocusout: function (element) {
            this.element(element);
        },
        rules: {
            AntibioticName: {
                required: true,
                unique_antibiotic: true,
                minlength: 2
            }
        },
        messages: {
            AntibioticName: {
                required: '请输入年份',
                minlength: '输入年份不正确'
            }
        },
        submitHandler: function (form) {
            insertTableCol();
            return false;
        }
    });
}


var arrColors = ['#015baa', '#c1232b', '#fe8463', '#ecbf00', '#cf7ca6', '#749f83'];

/* 打开添加列modal */
onAddTableCol = function () {
    $('#modal-insertCol').modal('show');
};

/* 提交添加列modal */
onSubmitTableCol = function () {
    if (validateInsertColForm().form()) {
        insertTableCol();
    }
    return false;
};

function insertTableCol() {
    var $modal = $('#modal-insertCol'),
        antibiotic = $modal.find('input#AntibioticName').val(),
        c = document.getElementById('multiple_datatable'), // 获取表格信息
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
    return false;
}

/* 添加列modal隐藏事件 */
$('#modal-insertCol').on('shown.bs.modal', function () {
    $('#modal-insertCol #AntibioticName').focus();
});

/* 添加列modal隐藏事件 */
$('#modal-insertCol').on('hidden.bs.modal', function () {
    $('#modal-insertCol #AntibioticName').val(null);
    validateInsertColForm().resetForm();
});

/* 删除多数据添加的列 */
onTableDeleteCol = function (obj) {
    var $modal = $('#modal-deleteCol');
    $modal.find('div.modal-body').html(template('template_deleteCol', {
        uuid: obj.dataset.cell,
        antibiotic: obj.dataset.name
    }));
    $modal.modal('show');
};

/* 删除列modal隐藏事件 */
$('#modal-deleteCol').on('hidden.bs.modal', function () {
    $('#modal-deleteCol div.modal-body').empty();
});

/* 确认删除列 */
onSubmitTableDeleteCol = function () {
    var l = Ladda.create(document.getElementById('btnMultipleTableCellDelete'));
    if (!l.isLoading()) {
        l.start();
        var $modal = $('#modal-deleteCol');
        var uuid = $modal.find('#multiple_tablecell_delete_uuid').val();
        console.log(uuid);
        $('table#multiple_datatable').find('th[data-cell="' + uuid + '"],td[data-cell="' + uuid + '"]').remove();

        setTimeout(function () {
            $modal.modal('hide');
            $('#multiple_datatable').updateMultipleDataTableName();
            l.stop();
        }, 500);
    }
    return false;
};


/* 所数据添加行 */
insertRow = function () {
    var l = Ladda.create(document.getElementById('btnInsertRow'));
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
        newTR.innerHTML = template('tpl_multiple_table_insertRow', {
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
deleteRow = function (id) {
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
    return true;
};

/* 预览 */
onPreview = function () {
    $('#modal_preview').modal('show');
};

/* 预览modal显示事件 */
$('#modal_preview').on('shown.bs.modal', function () {
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
        var _color = $(mtrItem).find('input[id][name^="ItemModel"][name$=".ChartColor"]').val();
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
            _value = !s.isBlank(_value) ? _value : '0.0';
            _value = _display ? _value : '-';
            _msdata.push(_value);
        });

        if (!s.isBlank(_name)) {
            mseries.push({
                name: _name,
                type: _type === 'Bar' || _type === '2' || _type === 2 ? 'bar' : 'line',
                stack: '',
                barWidth: 20,
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
});

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
            url: '/MgtChart/BarChartMultipleExcel_OnSubmit',
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