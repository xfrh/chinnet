
$(function () {
    //设置帮助的顺序
    $(".page-footer-main").attr("data-step", "6");
    $('a[href="/Data/GermYear"][data-nav-name="趋势图"]').closest('ul').find('li.active').removeClass('active');
    $('a[href="/Data/GermYear"][data-nav-name="趋势图"]').closest('li').addClass('active');
});

var data = new Array();
data['大肠埃希菌（碳青霉烯类）'] = {
    identity: 10000001,
    title: '大肠埃希菌对碳青霉烯类耐药变迁',
    legend: ['亚胺培南', '美罗培南'],
    data: [
        {
            name: '亚胺培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 1.1 },
                { year: '2006年', value: 1.4 },
                { year: '2007年', value: 0.7 },
                { year: '2008年', value: 1.2 },
                { year: '2009年', value: 1.7 },
                { year: '2010年', value: 1.6 },
                { year: '2011年', value: '1.0' },
                { year: '2012年', value: 0.9 },
                { year: '2013年', value: '1.0' },
                { year: '2014年', value: 0.9 },
                { year: '2015年', value: 1.4 },
                { year: '2016年', value: 1.3 },
                { year: '2017年', value: 1.9 },
                { year: '2018年', value: '2.0' }
            ]
        },
        {
            name: '美罗培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 1.4 },
                { year: '2006年', value: 0.8 },
                { year: '2007年', value: 0.8 },
                { year: '2008年', value: 0.9 },
                { year: '2009年', value: '1.0' },
                { year: '2010年', value: 1.4 },
                { year: '2011年', value: 1.2 },
                { year: '2012年', value: '1.0' },
                { year: '2013年', value: '3.0' },
                { year: '2014年', value: '1.0' },
                { year: '2015年', value: 1.6 },
                { year: '2016年', value: 1.8 },
                { year: '2017年', value: 2.3 },
                { year: '2018年', value: 2.1 }
            ]
        }
    ]
};
data['肺炎克雷伯菌（碳青霉烯类）'] = {
    identity: 10000002,
    title: '肺炎克雷伯菌对碳青霉烯类耐药变迁',
    legend: ['亚胺培南', '美罗培南'],
    data: [
        {
            name: '亚胺培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '3.0' },
                { year: '2006年', value: 3.4 },
                { year: '2007年', value: 2.4 },
                { year: '2008年', value: '4.0' },
                { year: '2009年', value: 4.9 },
                { year: '2010年', value: 9.2 },
                { year: '2011年', value: '9.0' },
                { year: '2012年', value: '10.0' },
                { year: '2013年', value: 10.3 },
                { year: '2014年', value: '11.0' },
                { year: '2015年', value: 15.6 },
                { year: '2016年', value: 16.1 },
                { year: '2017年', value: 20.9 },
                { year: '2018年', value: 25 }
                //{ year: '2018年', value: 26.1 }
            ]
        },
        {
            name: '美罗培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 2.9 },
                { year: '2006年', value: 2.6 },
                { year: '2007年', value: 2.9 },
                { year: '2008年', value: 3.8 },
                { year: '2009年', value: 4.8 },
                { year: '2010年', value: 9.2 },
                { year: '2011年', value: '9.0' },
                { year: '2012年', value: 11.8 },
                { year: '2013年', value: 14.1 },
                { year: '2014年', value: 14.1 },
                { year: '2015年', value: 14.4 },
                { year: '2016年', value: 18.8 },
                { year: '2017年', value: '24.0' },
                { year: '2018年', value: 26.3 }
                //{ year: '2018年', value: 28.6 }
            ]
        }
    ]
};
data['铜绿假单胞菌（碳青霉烯类）'] = {
    identity: 10000004,
    title: '铜绿假单细胞菌对碳青霉烯类耐药变迁',
    legend: ['亚胺培南', '美罗培南'],
    data: [
        {
            name: '亚胺培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 32.5 },
                { year: '2006年', value: 32.1 },
                { year: '2007年', value: 35.8 },
                { year: '2008年', value: 30.5 },
                { year: '2009年', value: 30.5 },
                { year: '2010年', value: 30.8 },
                { year: '2011年', value: 29.1 },
                { year: '2012年', value: 29.1 },
                { year: '2013年', value: 27.1 },
                { year: '2014年', value: 29.1 },
                { year: '2015年', value: 27.6 },
                { year: '2016年', value: 28.7 },
                { year: '2017年', value: 23.6 },
                { year: '2018年', value: '30.7' }
            ]
        },
        {
            name: '美罗培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 31.6 },
                { year: '2006年', value: 26.4 },
                { year: '2007年', value: 28.5 },
                { year: '2008年', value: 24.5 },
                { year: '2009年', value: 25.2 },
                { year: '2010年', value: 25.8 },
                { year: '2011年', value: '25.0' },
                { year: '2012年', value: 27.1 },
                { year: '2013年', value: 25.1 },
                { year: '2014年', value: 26.1 },
                { year: '2015年', value: 23.4 },
                { year: '2016年', value: 25.3 },
                { year: '2017年', value: 20.9 },
                { year: '2018年', value: '25.8' }
            ]
        }
    ]
};
data['鲍曼不动杆菌（碳青霉烯类）'] = {
    identity: 10000005,
    title: '鲍曼不动杆菌对碳青霉烯类耐药变迁',
    legend: ['亚胺培南', '美罗培南'],
    data: [
        {
            name: '亚胺培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '31.0' },
                { year: '2006年', value: 30.1 },
                { year: '2007年', value: 35.3 },
                { year: '2008年', value: 48.1 },
                { year: '2009年', value: '50.0' },
                { year: '2010年', value: 57.1 },
                { year: '2011年', value: 60.4 },
                { year: '2012年', value: 56.8 },
                { year: '2013年', value: 62.8 },
                { year: '2014年', value: 62.4 },
                { year: '2015年', value: 65.7 },
                { year: '2016年', value: 69.7 },
                { year: '2017年', value: 66.7 },
                { year: '2018年', value: '77.1' }
            ]
        },
        {
            name: '美罗培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '39.0' },
                { year: '2006年', value: 40.9 },
                { year: '2007年', value: 39.9 },
                { year: '2008年', value: 49.3 },
                { year: '2009年', value: 52.4 },
                { year: '2010年', value: 58.3 },
                { year: '2011年', value: 61.4 },
                { year: '2012年', value: 61.4 },
                { year: '2013年', value: 59.4 },
                { year: '2014年', value: 66.7 },
                { year: '2015年', value: 72.9 },
                { year: '2016年', value: 72.9 },
                { year: '2017年', value: 69.3 },
                { year: '2018年', value: '78.1' }
            ]
        }
    ]
};

data['MRSA和MRCNS'] = {
    identity: 10000006,
    title: 'CHINET监测历年MRSA和MRCNS检出变迁',
    legend: ['MRSA', 'MRCNS'],
    data: [
        {
            name: 'MRSA',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '69.0' },
                { year: '2006年', value: 58.4 },
                { year: '2007年', value: '58.0' },
                { year: '2008年', value: 55.9 },
                { year: '2009年', value: 52.7 },
                { year: '2010年', value: 51.7 },
                { year: '2011年', value: 50.6 },
                { year: '2012年', value: 47.9 },
                { year: '2013年', value: 45.2 },
                { year: '2014年', value: 44.6 },
                { year: '2015年', value: 42.2 },
                { year: '2016年', value: 38.4 },
                { year: '2017年', value: 35.3 },
                { year: '2018年', value: '34.0' }
            ]
        },
        {
            name: 'MRCNS',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '82.0' },
                { year: '2006年', value: 76.3 },
                { year: '2007年', value: '77.0' },
                { year: '2008年', value: 75.9 },
                { year: '2009年', value: 71.7 },
                { year: '2010年', value: 71.6 },
                { year: '2011年', value: 74.6 },
                { year: '2012年', value: 77.1 },
                { year: '2013年', value: 73.5 },
                { year: '2014年', value: '83.0' },
                { year: '2015年', value: 82.6 },
                { year: '2016年', value: 77.6 },
                { year: '2017年', value: 80.3 },
                { year: '2018年', value: 78.9 }
            ]
        }
    ]
};
data['MRSA检出率(成人和儿童)'] = {
    identity: 10000007,
    title: 'MRSA检出率(成人和儿童)',
    legend: ['成人', '儿童'],
    data: [
        {
            name: '成人',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 85.8 },
                { year: '2006年', value: 69.3 },
                { year: '2007年', value: 67.8 },
                { year: '2008年', value: 66.5 },
                { year: '2009年', value: 62.2 },
                { year: '2010年', value: 61.7 },
                { year: '2011年', value: 57.8 },
                { year: '2012年', value: 54.4 },
                { year: '2013年', value: 49.7 },
                { year: '2014年', value: 47.6 },
                { year: '2015年', value: 45.8 },
                { year: '2016年', value: 45.8 },
                { year: '2017年', value: 37.4 },
                { year: '2018年', value: '33.8' }
            ]
        },
        {
            name: '儿童',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 18 },
                { year: '2006年', value: 17.2 },
                { year: '2007年', value: 24.6 },
                { year: '2008年', value: 20.5 },
                { year: '2009年', value: 21.4 },
                { year: '2010年', value: 24.6 },
                { year: '2011年', value: 26.4 },
                { year: '2012年', value: 25.7 },
                { year: '2013年', value: 30.4 },
                { year: '2014年', value: 33.4 },
                { year: '2015年', value: 31.7 },
                { year: '2016年', value: 31.7 },
                { year: '2017年', value: 29.4 },
                { year: '2018年', value: '30.9' }
            ]
        }
    ]
};
data['万古霉素耐药肠球菌'] = {
    identity: 10000008,
    title: '万古霉素对肠球菌耐药变迁',
    legend: ['粪肠球菌', '屎肠球菌'],
    data: [
        {
            name: '粪肠球菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '0.0' },
                { year: '2006年', value: '0.0' },
                { year: '2007年', value: 0.49 },
                { year: '2008年', value: 0.39 },
                { year: '2009年', value: 0.28 },
                { year: '2010年', value: 0.55 },
                { year: '2011年', value: '0.10' },
                { year: '2012年', value: 0.26 },
                { year: '2013年', value: 0.03 },
                { year: '2014年', value: 0.19 },
                { year: '2015年', value: '0.20' },
                { year: '2016年', value: '0.40' },
                { year: '2017年', value: '0.10' },
                { year: '2018年', value: '0.10' }
            ]
        },
        {
            name: '屎肠球菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 0.35 },
                { year: '2006年', value: 1.06 },
                { year: '2007年', value: 2.06 },
                { year: '2008年', value: 3.21 },
                { year: '2009年', value: 3.49 },
                { year: '2010年', value: 3.52 },
                { year: '2011年', value: '2.60' },
                { year: '2012年', value: 2.49 },
                { year: '2013年', value: 2.12 },
                { year: '2014年', value: '3.50' },
                { year: '2015年', value: '2.40' },
                { year: '2016年', value: '1.90' },
                { year: '2017年', value: '1.40' },
                { year: '2018年', value: '1.20' }
            ]
        }
    ]
};
data['标本占比'] = {
    identity: 10000009,
    title: 'CHINET监测主要标本所占比例变迁',
    legend: ['呼吸道分泌物', '尿液', '血液', '伤口脓液'],
    data: [
        {
            name: '呼吸道分泌物',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: '45.0' },
                { year: '2006年', value: 50.1 },
                { year: '2007年', value: '50.0' },
                { year: '2008年', value: '49.0' },
                { year: '2009年', value: 49.7 },
                { year: '2010年', value: 46.9 },
                { year: '2011年', value: 45.8 },
                { year: '2012年', value: 44.4 },
                { year: '2013年', value: 43.2 },
                { year: '2014年', value: 41.6 },
                { year: '2015年', value: 42.8 },
                { year: '2016年', value: 41.6 },
                { year: '2017年', value: '40.0' },
                { year: '2018年', value: 39.7 }
            ]
        },
        {
            name: '尿液',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: '18.0' },
                { year: '2006年', value: 17.9 },
                { year: '2007年', value: 19.6 },
                { year: '2008年', value: 19.8 },
                { year: '2009年', value: 19.9 },
                { year: '2010年', value: 19.9 },
                { year: '2011年', value: 22.6 },
                { year: '2012年', value: 21.4 },
                { year: '2013年', value: 20.9 },
                { year: '2014年', value: 22.4 },
                { year: '2015年', value: 22.1 },
                { year: '2016年', value: 19.1 },
                { year: '2017年', value: 19.2 },
                { year: '2018年', value: 18.8 }
            ]
        },
        {
            name: '血液',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: '9.0' },
                { year: '2006年', value: 9.1 },
                { year: '2007年', value: '11.0' },
                { year: '2008年', value: 11.1 },
                { year: '2009年', value: 10.8 },
                { year: '2010年', value: 11.9 },
                { year: '2011年', value: 11.9 },
                { year: '2012年', value: 11.7 },
                { year: '2013年', value: 13.1 },
                { year: '2014年', value: 13.3 },
                { year: '2015年', value: '12.0' },
                { year: '2016年', value: 13.3 },
                { year: '2017年', value: 15.2 },
                { year: '2018年', value: 14.8 }
            ]
        },
        {
            name: '伤口脓液',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: '6.0' },
                { year: '2006年', value: 5.4 },
                { year: '2007年', value: 4.9 },
                { year: '2008年', value: 6.5 },
                { year: '2009年', value: 5.3 },
                { year: '2010年', value: 5.2 },
                { year: '2011年', value: 10.6 },
                { year: '2012年', value: 5.6 },
                { year: '2013年', value: 9.8 },
                { year: '2014年', value: 4.8 },
                { year: '2015年', value: 5.1 },
                { year: '2016年', value: 7.3 },
                { year: '2017年', value: 11.7 },
                { year: '2018年', value: 6.5 }
            ]
        }
    ]
};
data['主要革兰阴性杆菌检出率变迁'] = {
    identity: 10000010,
    title: '主要细菌在革兰阴性杆菌中所占比例变迁',
    legend: ['大肠埃希菌', '铜绿假单胞菌', '肺炎克雷伯菌', '鲍曼不动杆菌', '阴沟肠杆菌', '嗜麦芽窄食单胞菌'],
    data: [
        {
            name: '大肠埃希菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 25.9 },
                { year: '2006年', value: 26.6 },
                { year: '2007年', value: 27.6 },
                { year: '2008年', value: 26.5 },
                { year: '2009年', value: 25.8 },
                { year: '2010年', value: 26.9 },
                { year: '2011年', value: 28.4 },
                { year: '2012年', value: 27.3 },
                { year: '2013年', value: 27.2 },
                { year: '2014年', value: 28.8 },
                { year: '2015年', value: 27.8 },
                { year: '2016年', value: 27.2 },
                { year: '2017年', value: 27.2 },
                { year: '2018年', value: 26.7 }
            ]
        },
        {
            name: '铜绿假单胞菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 17.4 },
                { year: '2006年', value: 17.2 },
                { year: '2007年', value: 16.9 },
                { year: '2008年', value: 16.4 },
                { year: '2009年', value: 15.8 },
                { year: '2010年', value: 14.8 },
                { year: '2011年', value: 14.1 },
                { year: '2012年', value: '14.0' },
                { year: '2013年', value: 13.4 },
                { year: '2014年', value: '13.0' },
                { year: '2015年', value: 12.4 },
                { year: '2016年', value: 12.1 },
                { year: '2017年', value: 12.3 },
                { year: '2018年', value: 13.3 }
            ]
        },
        {
            name: '肺炎克雷伯菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: '14.0' },
                { year: '2006年', value: 13.6 },
                { year: '2007年', value: 12.8 },
                { year: '2008年', value: 13.6 },
                { year: '2009年', value: 14.7 },
                { year: '2010年', value: 14.7 },
                { year: '2011年', value: '15.0' },
                { year: '2012年', value: 16.5 },
                { year: '2013年', value: 17.9 },
                { year: '2014年', value: 18.1 },
                { year: '2015年', value: 18.2 },
                { year: '2016年', value: 17.9 },
                { year: '2017年', value: 19.5 },
                { year: '2018年', value: 20.7 }
            ]
        },
        {
            name: '鲍曼不动杆菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 13.2 },
                { year: '2006年', value: 11.1 },
                { year: '2007年', value: 15.5 },
                { year: '2008年', value: 12.4 },
                { year: '2009年', value: 13.4 },
                { year: '2010年', value: 14.4 },
                { year: '2011年', value: 13.9 },
                { year: '2012年', value: '15.0' },
                { year: '2013年', value: 14.6 },
                { year: '2014年', value: 14.2 },
                { year: '2015年', value: 14.2 },
                { year: '2016年', value: 13.6 },
                { year: '2017年', value: '13.0' },
                { year: '2018年', value: 12.4 }
            ]
        },
        {
            name: '阴沟肠杆菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 4.2 },
                { year: '2006年', value: 3.8 },
                { year: '2007年', value: 4.1 },
                { year: '2008年', value: 4.6 },
                { year: '2009年', value: 4.4 },
                { year: '2010年', value: 4.5 },
                { year: '2011年', value: 4.5 },
                { year: '2012年', value: 4.2 },
                { year: '2013年', value: 4.5 },
                { year: '2014年', value: 4.4 },
                { year: '2015年', value: 4.4 },
                { year: '2016年', value: 3.9 },
                { year: '2017年', value: 4.1 },
                { year: '2018年', value: 3.5 }
            ]
        },
        {
            name: '嗜麦芽窄食单胞菌',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 7.1 },
                { year: '2006年', value: 5.1 },
                { year: '2007年', value: '5.0' },
                { year: '2008年', value: 5.2 },
                { year: '2009年', value: 5.3 },
                { year: '2010年', value: 4.8 },
                { year: '2011年', value: 4.4 },
                { year: '2012年', value: 4.3 },
                { year: '2013年', value: '4.0' },
                { year: '2014年', value: 3.5 },
                { year: '2015年', value: 4.5 },
                { year: '2016年', value: 4.4 },
                { year: '2017年', value: 4.1 },
                { year: '2018年', value: 3.7 }
            ]
        }
    ]
};
data['革兰阴性菌/阳性菌占比'] = {
    identity: 10000012,
    title: 'CHINET监测历年分离革兰阴性菌和革兰阳性菌所占比例',
    legend: ['革兰阴性菌', '革兰阳性菌'],
    data: [
        {
            name: '革兰阴性菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 66.9 },
                { year: '2006年', value: 68.2 },
                { year: '2007年', value: 65.7 },
                { year: '2008年', value: 69.5 },
                { year: '2009年', value: '71.0' },
                { year: '2010年', value: 71.6 },
                { year: '2011年', value: 71.5 },
                { year: '2012年', value: 71.9 },
                { year: '2013年', value: '73.0' },
                { year: '2014年', value: 72.6 },
                { year: '2015年', value: 70.2 },
                { year: '2016年', value: 71.6 },
                { year: '2017年', value: 70.8 },
                { year: '2018年', value: 71.8 }
            ]
        },
        {
            name: '革兰阳性菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 33.1 },
                { year: '2006年', value: 31.8 },
                { year: '2007年', value: 34.3 },
                { year: '2008年', value: 30.5 },
                { year: '2009年', value: '29.0' },
                { year: '2010年', value: 28.4 },
                { year: '2011年', value: 28.5 },
                { year: '2012年', value: 28.1 },
                { year: '2013年', value: '27.0' },
                { year: '2014年', value: 27.4 },
                { year: '2015年', value: 29.8 },
                { year: '2016年', value: 28.4 },
                { year: '2017年', value: 29.2 },
                { year: '2018年', value: 28.2 }
            ]
        }
    ],
    datatable: [
        { year: '2005年', hospital: 8, count: 22774, item1: 15244, item2: 7530 },
        { year: '2006年', hospital: 12, count: 33811, item1: 23062, item2: 10749 },
        { year: '2007年', hospital: 12, count: 36001, item1: 23637, item2: 12346 },
        { year: '2008年', hospital: 12, count: 36216, item1: 25184, item2: 11032 },
        { year: '2009年', hospital: 14, count: 43670, item1: 31002, item2: 12668 },
        { year: '2010年', hospital: 14, count: 47850, item1: 34282, item2: 13568 },
        { year: '2011年', hospital: 15, count: 59287, item1: 42415, item2: 16872 },
        { year: '2012年', hospital: 15, count: 73297, item1: 52043, item2: 20354 },
        { year: '2013年', hospital: 16, count: 84572, item1: 61709, item2: 22863 },
        { year: '2014年', hospital: 17, count: 78955, item1: 57320, item2: 21635 },
        { year: '2015年', hospital: 20, count: 88778, item1: 62297, item2: 26481 },
        { year: '2016年', hospital: 30, count: 153084, item1: 109608, item2: 43476 },
        { year: '2017年', hospital: 34, count: 190610, item1: 134952, item2: 55658 },
        { year: '2018年', hospital: 44, count: 244843, item1: 175797, item2: 69046 }
    ]
};
data['住院和门急诊患者分离菌株占比'] = {
    identity: 10000013,
    title: 'CHINET监测历年住院和门急诊患者分离菌株数所占比例',
    legend: ['住院患者分离菌株', '门诊患者分离菌株'],
    data: [
        {
            name: '住院患者分离菌株',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 89.6 },
                { year: '2006年', value: 82.6 },
                { year: '2007年', value: '87.0' },
                { year: '2008年', value: 86.8 },
                { year: '2009年', value: 87.5 },
                { year: '2010年', value: 87.8 },
                { year: '2011年', value: 84.6 },
                { year: '2012年', value: 87.3 },
                { year: '2013年', value: 86.2 },
                { year: '2014年', value: 84.9 },
                { year: '2015年', value: 83.5 },
                { year: '2016年', value: 86.6 },
                { year: '2017年', value: 87.3 },
                { year: '2018年', value: '87.0' }
            ]
        },
        {
            name: '门诊患者分离菌株',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 10.4 },
                { year: '2006年', value: 17.4 },
                { year: '2007年', value: '13.0' },
                { year: '2008年', value: 13.2 },
                { year: '2009年', value: 12.5 },
                { year: '2010年', value: 12.2 },
                { year: '2011年', value: 15.4 },
                { year: '2012年', value: 12.7 },
                { year: '2013年', value: 13.8 },
                { year: '2014年', value: 15.1 },
                { year: '2015年', value: 16.5 },
                { year: '2016年', value: 13.4 },
                { year: '2017年', value: 12.7 },
                { year: '2018年', value: '13.0' }
            ]
        }
    ]
};
data['儿童患者非脑膜炎肺炎链球菌'] = {
    identity: 10000014,
    title: '儿童患者非脑膜炎肺炎链球菌的分布',
    legend: ['PSSP', 'PISP', 'PRSP'],
    data: [
        {
            name: 'PSSP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 89.6 },
                { year: '2017年', value: 86.8 },
                { year: '2018年', value: 89.4 }
            ]
        },
        {
            name: 'PISP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 7.4 },
                { year: '2017年', value: '11.0' },
                { year: '2018年', value: 8.9 }
            ]
        },
        {
            name: 'PRSP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 3.1 },
                { year: '2017年', value: 2.2 },
                { year: '2018年', value: 1.7 }
            ]
        }
    ]
};
data['成人患者非脑膜炎肺炎链球菌'] = {
    identity: 10000015,
    title: '成人患者非脑膜炎肺炎链球菌的分布',
    legend: ['PSSP', 'PISP', 'PRSP'],
    data: [
        {
            name: 'PSSP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 95.4 },
                { year: '2017年', value: 94.7 },
                { year: '2018年', value: 95.2 }
            ]
        },
        {
            name: 'PISP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 3.4 },
                { year: '2017年', value: 3.4 },
                { year: '2018年', value: 3.1 }
            ]
        },
        {
            name: 'PRSP',
            type: 'bar',
            stack: '',
            barWidth: 30,
            data: [
                { year: '2016年', value: 1.2 },
                { year: '2017年', value: 1.9 },
                { year: '2018年', value: 1.6 }
            ]
        }
    ]
};
data['肠杆菌科细菌对头孢噻肟(或头孢曲松)'] = {
    identity: 10000011,
    title: '肠杆菌科细菌对头孢噻肟(或头孢曲松)耐药菌株的检出率变迁',
    legend: ['大肠埃希菌', '肺炎克雷伯菌', '奇异变形杆菌'],
    data: [
        {
            name: '大肠埃希菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 15.9 },
                { year: '2006年', value: 54.5 },
                { year: '2007年', value: 59.5 },
                { year: '2008年', value: 61.2 },
                { year: '2009年', value: 59.8 },
                { year: '2010年', value: 57.9 },
                { year: '2011年', value: 57.2 },
                { year: '2012年', value: 61.7 },
                { year: '2013年', value: '61.0' },
                { year: '2014年', value: 61.1 },
                { year: '2015年', value: 59.6 },
                { year: '2016年', value: 57.4 },
                { year: '2017年', value: 58.1 },
                { year: '2018年', value: '57.0' }
            ]
        },
        {
            name: '肺炎克雷伯菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 49.3 },
                { year: '2006年', value: 52.2 },
                { year: '2007年', value: 50.9 },
                { year: '2008年', value: 51.4 },
                { year: '2009年', value: 47.5 },
                { year: '2010年', value: 45.2 },
                { year: '2011年', value: 46.4 },
                { year: '2012年', value: 44.8 },
                { year: '2013年', value: 43.6 },
                { year: '2014年', value: 40.6 },
                { year: '2015年', value: 41.9 },
                { year: '2016年', value: 42.7 },
                { year: '2017年', value: 44.9 },
                { year: '2018年', value: '45.8' }
            ]
        },
        {
            name: '奇异变形杆菌',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '16.0' },
                { year: '2006年', value: 25.1 },
                { year: '2007年', value: 22.2 },
                { year: '2008年', value: 23.8 },
                { year: '2009年', value: 25.5 },
                { year: '2010年', value: '23.0' },
                { year: '2011年', value: 27.8 },
                { year: '2012年', value: 30.3 },
                { year: '2013年', value: 32.4 },
                { year: '2014年', value: 34.7 },
                { year: '2015年', value: 33.5 },
                { year: '2016年', value: 35.1 },
                { year: '2017年', value: 33.9 },
                { year: '2018年', value: '36.3' }
            ]
        }
    ]
};

data['肠杆菌科细菌对碳青霉烯类的耐药变迁'] = {
    identity: 10000017,
    title: '肠杆菌科细菌对碳青霉烯类的耐药变迁（7996-82754株）',
    legend: ['亚胺培南', '美罗培南'],
    data: [
        {
            name: '亚胺培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 3.1 },
                { year: '2006年', value: 3.4 },
                { year: '2007年', value: 2.2 },
                { year: '2008年', value: 3.2 },
                { year: '2009年', value: 3.6 },
                { year: '2010年', value: 4.6 },
                { year: '2011年', value: 4.5 },
                { year: '2012年', value: '5.0' },
                { year: '2013年', value: 5.1 },
                { year: '2014年', value: 5.3 },
                { year: '2015年', value: 7.1 },
                { year: '2016年', value: '7.0' },
                { year: '2017年', value: 9.4 },
                { year: '2018年', value: 11.4 }
            ]
        },
        {
            name: '美罗培南',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 2.1 },
                { year: '2006年', value: 1.6 },
                { year: '2007年', value: 1.7 },
                { year: '2008年', value: 2.1 },
                { year: '2009年', value: 2.5 },
                { year: '2010年', value: 4.2 },
                { year: '2011年', value: 3.8 },
                { year: '2012年', value: 4.4 },
                { year: '2013年', value: 6.7 },
                { year: '2014年', value: 5.4 },
                { year: '2015年', value: '7.0' },
                { year: '2016年', value: 7.2 },
                { year: '2017年', value: 9.8 },
                { year: '2018年', value: 11.6 }
            ]
        }
    ]
};
data['流感嗜血杆菌对氨苄西林耐药变迁'] = {
    identity: 10000018,
    title: '流感嗜血杆菌对氨苄西林耐药变迁',
    legend: ['儿童', '成人'],
    data: [
        {
            name: '儿童',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: 16.4 },
                { year: '2006年', value: 33.1 },
                { year: '2007年', value: '34.0' },
                { year: '2008年', value: 37.2 },
                { year: '2009年', value: 29.6 },
                { year: '2010年', value: 34.2 },
                { year: '2011年', value: 33.5 },
                { year: '2012年', value: 40.8 },
                { year: '2013年', value: 40.9 },
                { year: '2014年', value: 45.3 },
                { year: '2015年', value: 49.1 },
                { year: '2016年', value: 56.9 },
                { year: '2017年', value: 56.4 },
                { year: '2018年', value: '59.6' }
            ]
        },
        {
            name: '成人',
            type: 'bar',
            stack: '',
            data: [
                { year: '2005年', value: '23.0' },
                { year: '2006年', value: '8.0' },
                { year: '2007年', value: 14.6 },
                { year: '2008年', value: '20.0' },
                { year: '2009年', value: 18.7 },
                { year: '2010年', value: 27.7 },
                { year: '2011年', value: 22.7 },
                { year: '2012年', value: 25.7 },
                { year: '2013年', value: 36.4 },
                { year: '2014年', value: '36.0' },
                { year: '2015年', value: 35.5 },
                { year: '2016年', value: 47.2 },
                { year: '2017年', value: 44.9 },
                { year: '2018年', value: '50.5' }
            ]
        }
    ]
};
data['历年成员单位及监测总株数'] = {
    identity: 10000019,
    title: '历年成员单位及监测总株数',
    legend: ['医院数', '菌株数'],
    yAxis: [
        {
            name: '菌株数',
            type: 'value',
            position: 'right',
            axisLabel: {
                show: true,
                interval: 'auto'
            },
            axisLine: {
                lineStyle: {
                    color: '#015BAA'
                }
            },
            min: 0,
            max: 300000,
            splitNumber: 6,
            show: true
        },
        {
            name: '医院数',
            type: 'value',
            axisLabel: {
                show: true,
                interval: 'auto',
                formatter: '{value} %'
            },
            axisLine: {
                lineStyle: {
                    color: '#C1232B'
                }
            },
            min: 0,
            max: 50,
            splitNumber: 10,
            show: true
        }
    ],
    data: [
        {
            name: '医院数',
            type: 'bar',
            stack: '',
            barWidth: 30,
            yAxisIndex: 1,
            data: [
                { year: '2005年', value: 8 },
                { year: '2006年', value: 12 },
                { year: '2007年', value: 12 },
                { year: '2008年', value: 12 },
                { year: '2009年', value: 14 },
                { year: '2010年', value: 14 },
                { year: '2011年', value: 15 },
                { year: '2012年', value: 15 },
                { year: '2013年', value: 16 },
                { year: '2014年', value: 17 },
                { year: '2015年', value: 20 },
                { year: '2016年', value: 30 },
                { year: '2017年', value: 34 },
                { year: '2018年', value: 44 }
            ]
        },
        {
            name: '菌株数',
            type: 'line',
            stack: '',
            data: [
                { year: '2005年', value: 22774 },
                { year: '2006年', value: 33811 },
                { year: '2007年', value: 36001 },
                { year: '2008年', value: 36216 },
                { year: '2009年', value: 43670 },
                { year: '2010年', value: 47580 },
                { year: '2011年', value: 59287 },
                { year: '2012年', value: 72397 },
                { year: '2013年', value: 84572 },
                { year: '2014年', value: 78955 },
                { year: '2015年', value: 88778 },
                { year: '2016年', value: 153084 },
                { year: '2017年', value: 190610 },
                { year: '2018年', value: 242655 }
            ]
        }
    ]
};


// 基于准备好的dom，初始化echarts实例
var myChart = echarts.init(document.getElementById('main'));

$(document).ready(function () {
    var art_germ_li_data = [];
    Object.keys(data).forEach(function (key, v) {
        art_germ_li_data.push({ name: key, identity: data[key].identity });
    });
    var html = template('art_germ_li', { data: art_germ_li_data });
    document.getElementById('germ_list').innerHTML = html;

    // 默认选中
    SelectItem('大肠埃希菌（碳青霉烯类）');
});

//取消选择数据段名称
function ClearSelectType() {
    $('#germ_list li').removeClass('active');
    $('#dataError').css('display', 'block');
    $('.main-content2').css('display', 'none');
    $('#PageContentTitle').html('');
}

/**
 * 选取数据
 * @param {any} key 数据key
 */
function SelectItem(key) {
    var item = data[key];
    var $target = $('li#data-' + item.identity);
    $('#germ_list li').removeClass('active');
    $target.addClass('active');
    $('#table_10000012').empty();

    var legend = [],
        xAxisData = [],
        datas = [];

    item.data.forEach(function (v, i) {
        legend.push(v.name);
        if (!(key === '主要革兰阴性杆菌检出率变迁' && (v.name === '铜绿假单胞菌' || v.name === '鲍曼不动杆菌' || v.name === '阴沟肠杆菌'))) {
            $.extend(v, {
                itemStyle: {
                    normal: {
                        label: {
                            show: true,
                            position: 'top',
                            formatter: '{c}'
                        }
                    }
                }
            });
        }

        datas.push(v);
        v.data.forEach(function (d, j) {
            if (xAxisData.indexOf(d.year) === -1) {
                xAxisData.push(d.year);
            }
        });
    });

    if (item.identity === 10000012) {
        //$('#table_10000012').html(template('art_table_10000012', { data: item.datatable }));
    }
    LoadEcharts(legend, xAxisData, datas, item.title, item.yAxis);
}

/**
 * 加载柱状图
 * @param {any} legend 图例
 * @param {any} text x轴数据
 * @param {any} value 数据
 * @param {any} pageName 标题
 * @param {any} yAxis y轴数据
 */
function LoadEcharts(legend, text, value, pageName, yAxis) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    myChart.clear();
    myChart.resize();
    myChart.showLoading();

    var option = {
        color: ['#C1232B', '#015BAA', '#FE8463', '#000F1A', '#FAD860', '#F3A43B', '#60C0DD', '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#26C0C0'],
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        title: {
            text: '' + pageName + '',
            subtext: '',
            left: 'center',
            show: true,
            textStyle: {
                fontWeight: 'normal'
            }
        },
        legend: {
            data: legend,
            top: '30px'
        },
        grid: {
            top: '65px',
            left: '0',
            right: '3%',
            bottom: '8%',
            containLabel: true
        },
        toolbox: {
            show: true,
            orient: 'vertical',
            left: 'right',
            top: 'center',
            feature: {
                dataView: { title: "数据", readOnly: false },
                restore: {},
                saveAsImage: { title: "下载", type: "jpeg" }
            }
        },
        calculable: true,
        xAxis: [
            {
                type: 'category',
                axisTick: { show: false },
                data: text
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
        series: value
    };

    if (yAxis && yAxis.length > 0) {
        option.yAxis = yAxis;
    }
    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);
}

//加载折点图
//function LoadEcharts(legend, text, value) {

//    $("#dataError").css("display", "none");
//    $("#main").css("display", "block");

//    // 基于准备好的dom，初始化echarts实例
//    var myChart = echarts.init(document.getElementById('main'));

//    option = {
//        title: {
//            text: ''
//        },
//        tooltip: {
//            trigger: 'axis'
//        },
//        legend: {
//            data: legend
//        },
//        grid: {
//            left: '3%',
//            right: '4%',
//            bottom: '3%',
//            containLabel: true
//        },
//        toolbox: {
//            feature: {
//                saveAsImage: {}
//            }
//        },
//        xAxis: {
//            type: 'category',
//            boundaryGap: false,
//            data: text
//        },
//        yAxis: {
//            type: 'value',
//            axisLabel: {
//                show: true,
//                interval: 'auto',
//                formatter: '{value}'
//            },
//            show: true
//        },
//        series: value
//    };

//    // 使用刚指定的配置项和数据显示图表。
//    myChart.setOption(option);
//}

/**
 * 显示/隐藏数据项
 */
function ShowItemContent() {
    var $itemContent = $('#GermContent'),
        $img = $(event.target);
    if ($itemContent.data('status') === '1') {
        $('#GermContent').slideDown(200, function () {
            $itemContent.data('status', '0');
            $img.attr('src', '/Content/Images/jiantou-shang.png');
        });
    }
    else {
        $('#GermContent').slideUp(200, function () {
            $itemContent.data('status', '1');
            $img.attr('src', '/Content/Images/jiantou-xia.png');
        });
    }
}