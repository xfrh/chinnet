window.indexedDB = window.indexedDB || window.mozIndexedDB || window.webkitIndexedDB || window.msIndexedDB;

window.CHINET_SurveyDB = function () {
    var tbSurvey = {
        name: 'TBSurvey',
        columns: {
            id: { primaryKey: true, autoIncrement: true },
            Token: { notNull: false, dataType: 'string' },
            Record_Id: { notNull: true, dataType: 'string' },
            Survey_Id: { notNull: true, dataType: 'string' },
            Content1: { notNull: false, dataType: 'string' },
            Content2: { notNull: false, dataType: 'string' },
            Content3: { notNull: false, dataType: 'string' },
            Content4: { notNull: false, dataType: 'string' },
            Content5: { notNull: false, dataType: 'string' },
            Content6: { notNull: false, dataType: 'string' },
            Content7: { notNull: false, dataType: 'string' },
            Content8: { notNull: false, dataType: 'string' },
            Content9: { notNull: false, dataType: 'string' },
            Content10: { notNull: false, dataType: 'string' },
            Content11: { notNull: false, dataType: 'string' },
            Content12: { notNull: false, dataType: 'string' },
            Content13: { notNull: false, dataType: 'string' },
            Content14: { notNull: false, dataType: 'string' },
            Content15: { notNull: false, dataType: 'string' },
            Content16: { notNull: false, dataType: 'string' },
            Content17: { notNull: false, dataType: 'string' },
            Content18: { notNull: false, dataType: 'string' },
            Content19: { notNull: false, dataType: 'string' },
            Content20: { notNull: false, dataType: 'string' },
            Content21: { notNull: false, dataType: 'string' },
            Content22: { notNull: false, dataType: 'string' },
            Content23: { notNull: false, dataType: 'string' },
            Content24: { notNull: false, dataType: 'string' },
            Content25: { notNull: false, dataType: 'string' },
            Content26: { notNull: false, dataType: 'string' },
            Content27: { notNull: false, dataType: 'string' },
            Content28: { notNull: false, dataType: 'string' },
            Content29: { notNull: false, dataType: 'string' },
            Content30: { notNull: false, dataType: 'string' },
            Content31: { notNull: false, dataType: 'string' }
        }
    };
    var tbSurveyTemp = {
        name: 'TBSurveyTemp',
        columns: {
            id: { primaryKey: true, autoIncrement: true },
            record_id: { notNull: true, dataType: 'string' },
            page: { notNull: true, dataType: 'number' },
            data: { notNull: true, dataType: 'string' }
        }
    };
    return {
        name: 'CHINET_Survey',
        tables: [tbSurvey, tbSurveyTemp]
    };
};

var CHINET_Survey = {
    DropDB: function () {
        if (window.indexedDB) {
            var jsstoreConn = new JsStore.Instance(new Worker("/Content/Scripts/jsstore/jsstore.worker.js"));
            jsstoreConn.initDb(window.CHINET_SurveyDB());
            jsstoreConn.dropDb().then(function () {
                console.log('Db deleted successfully');
            }).catch(function (error) {
                console.log(error);
            });
        }
    }
};