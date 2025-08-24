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