$(function () {
    //设置帮助的顺序
    $(".page-footer-main").attr("data-step", "12");
    $('a[href="/Data/AntibioticDrugFast"][data-nav-name="柱状图"]').closest('ul').find('li.active').removeClass('active');
    $('a[href="/Data/AntibioticDrugFast"][data-nav-name="柱状图"]').closest('li').addClass('active');
});

var data = [];

// 主要菌种分布数据

data['c.100'] = {
    code: 100,
    title: "222494株临床分离菌株主要菌种分布",
    dataZoomArea: 70,
    data: [
        { name: '大肠埃希菌', value: '19.0' },
        { name: '肺炎克雷伯菌', value: '15.0' },
        { name: '铜绿假单胞菌', value: '10.0' },
        { name: '金葡菌', value: '9.0' },
        { name: '鲍曼不动杆菌', value: '9.0' },
        { name: '屎肠球菌', value: '4.0' },
        { name: '流感嗜血杆菌', value: '3.0' },
        { name: '粪肠球菌', value: '3.0' },
        { name: '嗜麦芽窄食单胞菌', value: '3.0' },
        { name: '阴沟肠杆菌', value: '3.0' },
        { name: '肺炎链球菌', value: '2.0' },
        { name: '表皮葡萄球菌', value: '2.0' },
        { name: '奇异变形杆菌', value: '2.0' },
        { name: '化脓链球菌', value: '2.0' },
        { name: '无乳链球菌', value: '1.0' },
        { name: '人葡萄球菌', value: '1.0' },
        { name: '黏质沙雷菌', value: '1.0' },
        { name: '产气克雷伯菌', value: '1.0' },
        { name: '洋葱伯克霍尔德菌', value: '1.0' },
        { name: '产酸克雷伯菌', value: '1.0' }
        //{ name: '大肠埃希菌', value: 19.27 },
        //{ name: '克雷伯菌属', value: 14.68 },
        //{ name: '不动杆菌属', value: 10.1 },
        //{ name: '金葡菌', value: 9.03 },
        //{ name: '铜绿假单胞菌', value: 8.69 },
        //{ name: '肠球菌属', value: 8.42 },
        //{ name: '凝固酶阴性葡萄球菌', value: 4.39 },
        //{ name: '肠杆菌属', value: 3.93 },
        //{ name: 'B溶血性链球菌', value: 3.57 },
        //{ name: '嗜麦芽窄食单胞菌', value: 2.87 },
        //{ name: '流感嗜血杆菌', value: 2.66 },
        //{ name: '肺炎链球菌', value: 2.41 },
        //{ name: '变形杆菌属', value: 1.91 },
        //{ name: '沙雷菌属', value: 1.22 },
        //{ name: '草绿色链球菌', value: 1.04 },
        //{ name: '伯克霍尔德菌属', value: 1.01 },
        //{ name: '柠檬酸杆菌属', value: 0.89 },
        //{ name: '卡他莫拉菌', value: 0.81 },
        //{ name: '沙门菌属', value: 0.65 },
        //{ name: '摩根菌属', value: 0.49 }
    ]
};
data['c.101'] = {
    code: 101,
    title: '97297株呼吸道标本分离菌主要菌种分布',
    dataZoomArea: 70,
    data: [
        { name: '肺炎克雷伯菌', value: 19.36 },
        { name: '鲍曼不动杆菌', value: 17 },
        { name: '铜绿假单胞菌', value: 16.04 },
        { name: '金黄色葡萄球菌', value: 7.93 },
        { name: '流感嗜血杆菌', value: 7.06 },
        { name: '肺炎链球菌', value: 5.25 },
        { name: '嗜麦芽窄食单胞菌', value: 5.24 },
        { name: '大肠埃希菌', value: 4.74 },
        { name: '化脓链球菌', value: 3.19 },
        { name: '阴沟肠杆菌', value: 2.42 },
        { name: '卡他莫拉菌', value: '2.0' },
        { name: '黏质沙雷菌', value: 1.42 },
        { name: '洋葱伯克霍尔德菌', value: 1.26 },
        { name: '产气肠杆菌', value: 1.03 },
        { name: '产酸克雷伯菌', value: 0.98 },
        { name: '奇异变形杆菌', value: 0.83 },
        { name: '副流感嗜血杆菌', value: 0.34 }
    ]
};
data['c.102'] = {
    code: 102,
    title: '46081株尿道标本分离菌主要菌种分布',
    dataZoomArea: 70,
    data: [
        { name: '大肠埃希菌', value: 46.75 },
        { name: '屎肠球菌', value: 10.61 },
        { name: '肺炎克雷伯菌', value: 10.34 },
        { name: '粪肠球菌', value: 7.58 },
        { name: '铜绿假单胞菌', value: 3.88 },
        { name: '奇异变形杆菌', value: 3.55 },
        { name: '无乳链球菌', value: 2.42 },
        { name: '阴沟肠杆菌', value: 1.99 },
        { name: '鲍曼不动杆菌', value: 1.9 },
        { name: '金黄色葡萄球菌', value: 1.47 },
        { name: '弗劳地柠檬酸杆菌', value: 0.88 },
        { name: '摩根摩根菌', value: 0.86 },
        { name: '产酸克雷伯菌', value: 0.83 },
        { name: '产气肠杆菌', value: 0.8 },
        { name: '黏质沙雷菌', value: 0.6 },
        { name: '洋葱伯克霍尔德菌', value: 0.54 },
        { name: '嗜麦芽窄食单胞菌', value: 0.53 }
    ]
};
data['c.103'] = {
    code: 103,
    title: '36359株血液标本分离菌主要菌种分布',
    dataZoomArea: 70,
    data: [
        { name: '大肠埃希菌', value: 23.05 },
        { name: '肺炎克雷伯菌', value: 15.45 },
        { name: '表皮葡萄球菌', value: 10.46 },
        { name: '金黄色葡萄球菌', value: 7.71 },
        { name: '人葡萄球菌', value: 7.08 },
        { name: '屎肠球菌', value: 3.9 },
        { name: '鲍曼不动杆菌', value: 3.2 },
        { name: '铜绿假单胞菌', value: 2.9 },
        { name: '溶血葡萄球菌', value: 2.26 },
        { name: '阴沟肠杆菌', value: 2.16 },
        { name: '粪肠球菌', value: 2.04 },
        { name: '头状葡萄球菌', value: 1.89 },
        { name: '肺炎链球菌', value: 1.29 },
        { name: '洋葱伯克霍尔德菌', value: 0.94 },
        { name: '黏质沙雷菌', value: 0.87 },
        { name: '嗜麦芽窄食单胞菌', value: 0.86 },
        { name: '缓症链球菌', value: 0.7 },
        { name: '产气肠杆菌', value: 0.64 },
        { name: '咽峡炎链球菌', value: 0.62 }
    ]
};
data['c.104'] = {
    code: 104,
    title: '15867株伤口脓液标本分离菌主要菌种分布',
    dataZoomArea: 70,
    data: [
        { name: '金黄色葡萄球菌', value: 26.05 },
        { name: '大肠埃希菌', value: 25.21 },
        { name: '肺炎克雷伯菌', value: 10.84 },
        { name: '铜绿假单胞菌', value: 8.44 },
        { name: '阴沟肠杆菌', value: 3.3 },
        { name: '粪肠球菌', value: 3.08 },
        { name: '鲍曼不动杆菌', value: 3.05 },
        { name: '屎肠球菌', value: 2.48 },
        { name: '奇异变形杆菌', value: 2.21 },
        { name: '无乳链球菌', value: 1.73 },
        { name: '化脓链球菌', value: 1.1 },
        { name: '黏质沙雷菌', value: 1.01 },
        { name: '产酸克雷伯菌', value: 0.99 },
        { name: '弗劳地柠檬酸杆菌', value: 0.84 },
        { name: '鸟肠球菌', value: 0.84 },
        { name: '摩根摩根菌', value: 0.83 },
        { name: '嗜麦芽窄食单胞菌', value: 0.77 },
        { name: '产酸肠杆菌', value: 0.77 },
        { name: '肺炎链球菌', value: 0.67 },
        { name: '普通变形杆菌', value: 0.48 },
        { name: '停乳链球菌', value: 0.46 },
        { name: '副流感嗜血菌', value: 0.3 }
    ]
};
data['c.105'] = {
    code: 105,
    title: '12852株其他无菌体液标本分离菌主要菌种分布',
    dataZoomArea: 60,
    data: [
        { name: '大肠埃希菌', value: 21.85 },
        { name: '肺炎克雷伯菌', value: 13.12 },
        { name: '屎肠球菌', value: 10.82 },
        { name: '粪肠球菌', value: 6.66 },
        { name: '铜绿假单胞菌', value: 6.53 },
        { name: '鲍曼不动杆菌', value: 4.75 },
        { name: '金黄色葡萄球菌', value: 4.01 },
        { name: '表皮葡萄球菌', value: 3.99 },
        { name: '阴沟肠杆菌', value: 3.19 },
        { name: '嗜麦芽窄食单胞菌', value: 1.77 },
        { name: '溶血葡萄球菌', value: 1.49 },
        { name: '产气肠杆菌', value: 1.23 },
        { name: '奇异变形杆菌', value: 1.21 },
        { name: '人葡萄球菌', value: 1.06 },
        { name: '鸟肠球菌', value: 1.03 },
        { name: '弗劳地柠檬酸杆菌', value: 1.03 },
        { name: '黏质沙雷菌', value: 0.99 },
        { name: '铅黄肠球菌', value: 0.9 },
        { name: '鹑鸡肠球菌', value: 0.89 },
        { name: '产酸克雷伯菌', value: 0.82 },
        { name: '咽峡炎链球菌', value: 0.82 },
        { name: '星座链球菌', value: 0.75 },
        { name: '摩根摩根菌', value: 0.75 },
        { name: '缓症链球菌', value: 0.65 },
        { name: '无乳链球菌', value: 0.63 }
    ]
};
data['c.106'] = {
    code: 106,
    title: '3157株脑脊液标本分离菌主要菌种分布',
    dataZoomArea: 70,
    data: [
        { name: '表皮葡萄球菌', value: 21.57 },
        { name: '鲍曼不动杆菌', value: 12.48 },
        { name: '人葡萄球菌', value: 10.64 },
        { name: '肺炎克雷伯菌', value: 8.36 },
        { name: '溶血葡萄球菌', value: 5.96 },
        { name: '头状葡萄球菌', value: 5.45 },
        { name: '大肠埃希菌', value: 3.86 },
        { name: '金黄色葡萄球菌', value: 3.48 },
        { name: '屎肠球菌', value: 3.45 },
        { name: '肺炎链球菌', value: 2.12 },
        { name: '铜绿假单胞菌', value: 2.06 },
        { name: '沃氏葡萄球菌', value: '2.0' },
        { name: '阴沟肠杆菌', value: 1.24 },
        { name: '粪肠球菌', value: 1.24 },
        { name: '嗜麦芽窄食单胞菌', value: 1.05 },
        { name: '不动杆菌属', value: 0.76 },
        { name: '无乳链球菌', value: 0.76 },
        { name: '科氏葡萄球菌', value: 0.67 },
        { name: '洛菲不动杆菌', value: 0.6 },
        { name: '黏质沙雷菌', value: 0.57 },
        { name: '产吲哚金黄杆菌', value: 0.54 },
        { name: '产酸克雷伯菌', value: 0.48 },
        { name: '产单核细胞李斯特菌', value: 0.44 }
    ]
};
data['c.107'] = {
    code: 107,
    title: '244843株临床分离菌在各类标本中的分布',
    dataZoomArea: 70,
    data: [
        { name: '呼吸道标本', value: 39.7 },
        { name: '尿液标本', value: 18.8 },
        { name: '血液标本', value: 14.8 },
        { name: '伤口脓液', value: 6.5 },
        { name: '无菌液体', value: 5.2 },
        { name: '生殖道分泌物', value: 1.3 },
        { name: '脑脊液', value: 1.3 },
        { name: '粪便标本', value: 0.9 },
        { name: '其他', value: 11.4 }
    ]
};

// 不用医院耐药菌分布
data['c.200'] = {
    code: 200,
    title: '各医院金葡菌MR菌株检出率',
    dataZoomArea: 40,
    data: [
        { name: 'sxe', value: 10.3 },
        { name: 'hyd', value: 16.2 },
        { name: 'nmg', value: '19.0' },
        { name: 'ZGH', value: 19.1 },
        { name: 'SYH', value: 21.8 },
        { name: 'TJJ', value: 22.1 },
        { name: 'XJH', value: 22.6 },
        { name: 'SZS', value: 23.6 },
        { name: 'JJH', value: 24.9 },
        { name: 'JLH', value: 25.1 },
        { name: 'PUH', value: 25.7 },
        { name: 'nxz', value: 26.3 },
        { name: 'SCS', value: 27.1 },
        { name: 'XYH', value: 27.3 },
        { name: 'bch', value: 27.7 },
        { name: 'KMH', value: '28.0' },
        { name: 'HXH', value: 28.2 },
        { name: 'SDH', value: 29.2 },
        { name: 'PDH', value: '35.0' },
        { name: 'SYF', value: 36.5 },
        { name: 'PED', value: 36.9 },
        { name: 'JXE', value: 41.3 },
        { name: 'bjh', value: '47.0' },
        { name: 'SCH', value: 47.1 },
        { name: 'GLH', value: 47.9 },
        { name: 'RJH', value: 48.8 },
        { name: 'GSY', value: 49.1 },
        { name: 'ayd', value: '51.0' },
        { name: 'LSH', value: 51.7 },
        { name: 'GZH', value: '52.0' },
        { name: 'HNH', value: 53.3 },
        { name: 'HSH', value: '54.0' },
        { name: 'TJH', value: 59.9 },
        { name: 'SXS', value: 62.1 }
    ]
};
data['c.201'] = {
    code: 201,
    title: '各医院凝固酶阴性葡萄球菌MR菌株检出率',
    dataZoomArea: 40,
    data: [
        { name: 'PDH', value: 49.3 },
        { name: 'JXE', value: 66.7 },
        { name: 'XJH', value: 68.5 },
        { name: 'LSH', value: 69.6 },
        { name: 'SZS', value: 72.1 },
        { name: 'PUH', value: 73.6 },
        { name: 'hyd', value: '74.0' },
        { name: 'SYF', value: '75.0' },
        { name: 'sxe', value: 75.5 },
        { name: 'GLH', value: 77.7 },
        { name: 'SCS', value: 78.4 },
        { name: 'HSH', value: 78.8 },
        { name: 'XYH', value: 79.1 },
        { name: 'JLH', value: 80.1 },
        { name: 'SDH', value: 80.4 },
        { name: 'GSY', value: 80.6 },
        { name: 'HXH', value: 81.3 },
        { name: 'SXS', value: '82.0' },
        { name: 'nmg', value: 82.2 },
        { name: 'ayd', value: 82.5 },
        { name: 'PED', value: 82.6 },
        { name: 'bch', value: '83.0' },
        { name: 'KMH', value: 83.3 },
        { name: 'GZH', value: 83.5 },
        { name: 'SCH', value: 83.9 },
        { name: 'TJH', value: '84.0' },
        { name: 'nxz', value: 84.6 },
        { name: 'SYH', value: 84.8 },
        { name: 'TJJ', value: 85.2 },
        { name: 'bjh', value: 85.8 },
        { name: 'RJH', value: 86.8 },
        { name: 'HNH', value: 88.7 },
        { name: 'ZGH', value: '100.0' },
        { name: 'JJH', value: '100.0' }
    ]
};
data['c.202'] = {
    code: 202,
    title: '各医院分离铜绿假单胞菌对亚胺培南的耐药率（68-1025株）',
    dataZoomArea: 40,
    data: [
        { name: 'ZGH', value: 1.7 },
        { name: 'JXE', value: 11.8 },
        { name: 'NXZ', value: 11.8 },
        { name: 'PDH', value: '13.0' },
        { name: 'PUH', value: 14.8 },
        { name: 'XJH', value: 15.7 },
        { name: 'JLH', value: 16.3 },
        { name: 'HYD', value: 19.8 },
        { name: 'GSY', value: 20.4 },
        { name: 'NMG', value: 20.4 },
        { name: 'AYD', value: 20.7 },
        { name: 'GZH', value: 22.1 },
        { name: 'HXH', value: 22.4 },
        { name: 'SZS', value: 22.9 },
        { name: 'LSH', value: 24.1 },
        { name: 'GLH', value: 24.3 },
        { name: 'SCH', value: 24.7 },
        { name: 'SCS', value: 24.9 },
        { name: 'BCH', value: 25.2 },
        { name: 'SXE', value: 25.2 },
        { name: 'TJJ', value: '28.0' },
        { name: 'XYH', value: 29.8 },
        { name: 'JJH', value: 30.1 },
        { name: 'PED', value: '31.0' },
        { name: 'SDH', value: 31.2 },
        { name: 'TJH', value: 31.6 },
        { name: 'SYH', value: 33.3 },
        { name: 'HNH', value: 35.8 },
        { name: 'RJH', value: 35.8 },
        { name: 'SXS', value: 40.9 },
        { name: 'BJH', value: 41.2 },
        { name: 'HSH', value: 41.2 },
        { name: 'SYF', value: 43.9 },
        { name: 'KMH', value: 45.2 }
    ]
};
data['c.203'] = {
    code: 203,
    title: '各医院分离鲍曼不动杆菌对亚胺培南的耐药率（19-1299株）',
    dataZoomArea: 40,
    data: [
        { name: 'LSH', value: 3.8 },
        { name: 'JXE', value: 21.7 },
        { name: 'ZGH', value: 26.5 },
        { name: 'SXE', value: 34.1 },
        { name: 'PDH', value: 37.9 },
        { name: 'JLH', value: 42.7 },
        { name: 'BCH', value: '43.0' },
        { name: 'TJJ', value: 46.9 },
        { name: 'PED', value: 50.4 },
        { name: 'NMG', value: 50.7 },
        { name: 'BJH', value: 51.5 },
        { name: 'XJH', value: '54.0' },
        { name: 'RJH', value: 55.4 },
        { name: 'GZH', value: '57.0' },
        { name: 'JJH', value: 62.7 },
        { name: 'SDH', value: '64.0' },
        { name: 'GLH', value: 64.4 },
        { name: 'PUH', value: 71.7 },
        { name: 'AYD', value: 72.4 },
        { name: 'SCH', value: 72.6 },
        { name: 'HXH', value: 72.7 },
        { name: 'SCS', value: 73.2 },
        { name: 'SZS', value: 74.2 },
        { name: 'GSY', value: 74.3 },
        { name: 'NXZ', value: 74.9 },
        { name: 'HSH', value: 76.7 },
        { name: 'SYF', value: 78.9 },
        { name: 'HYD', value: 80.6 },
        { name: 'KMH', value: 81.2 },
        { name: 'SYH', value: 83.6 },
        { name: 'XYH', value: 86.4 },
        { name: 'SXS', value: '87.0' },
        { name: 'HNH', value: 91.3 },
        { name: 'TJH', value: 91.4 }
    ]
};
data['c.204'] = {
    code: 204,
    title: '各医院分离肺炎克雷伯菌对亚胺培南的耐药率（66-1838株）',
    dataZoomArea: 40,
    data: [
        { name: 'JJH', value: '0.0' },
        { name: 'ZGH', value: 0.6 },
        { name: 'LSH', value: 1.7 },
        { name: 'GSY', value: 2.1 },
        { name: 'JLH', value: 2.1 },
        { name: 'SXE', value: 2.5 },
        { name: 'XJH', value: 3.2 },
        { name: 'NXZ', value: 3.5 },
        { name: 'NMG', value: '5.0' },
        { name: 'PDH', value: 5.5 },
        { name: 'GZH', value: 5.6 },
        { name: 'SDH', value: 6.4 },
        { name: 'HYD', value: 7.3 },
        { name: 'TJJ', value: 7.4 },
        { name: 'PUH', value: 9.3 },
        { name: 'GLH', value: 10.4 },
        { name: 'SZS', value: 15.1 },
        { name: 'HXH', value: 15.2 },
        { name: 'SCS', value: 15.8 },
        { name: 'SYH', value: 19.7 },
        { name: 'AYD', value: 26.3 },
        { name: 'KMH', value: 26.3 },
        { name: 'RJH', value: 27.6 },
        { name: 'XYH', value: 29.1 },
        { name: 'PED', value: 32.1 },
        { name: 'BJH', value: 33.4 },
        { name: 'SXS', value: 36.4 },
        { name: 'SCH', value: 38.7 },
        { name: 'BCH', value: '40.0' },
        { name: 'SYF', value: 42.5 },
        { name: 'TJH', value: 42.9 },
        { name: 'JXE', value: 45.5 },
        { name: 'HSH', value: 52.7 },
        { name: 'HNH', value: 53.1 }
    ]
};

// 不同标本分离肺炎克雷伯菌对抗菌药物的耐药率
data['肺炎克雷伯菌'] = {
    title: '不同标本分离肺炎克雷伯菌对抗菌药物的耐药率',
    color: ['#FE8463', '#ECBF00', '#C1232B', '#015BAA', '#CF7CA6'],
    count: {
        '伤口脓液': '(1720株)',
        '尿液': '(4763株)',
        '呼吸道': '(18834株)',
        '血液': '(5616株)',
        '脑脊液': '(264株)'
    },
    items: [
        {
            name: '伤口脓液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: '12.0' },
                { name: '头孢曲松', value: 31.6 },
                { name: '头孢哌酮/舒巴坦', value: 16.7 }
            ]
        },
        {
            name: '尿液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 19.5 },
                { name: '头孢曲松', value: '53.0' },
                { name: '头孢哌酮/舒巴坦', value: 28.8 }
            ]
        },
        {
            name: '呼吸道',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 25.8 },
                { name: '头孢曲松', value: 43.3 },
                { name: '头孢哌酮/舒巴坦', value: 32.4 }
            ]
        },
        {
            name: '血液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: '34.0' },
                { name: '头孢曲松', value: '52.0' },
                { name: '头孢哌酮/舒巴坦', value: 40.1 }
            ]
        },
        {
            name: '脑脊液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 61.2 },
                { name: '头孢曲松', value: 69.6 },
                { name: '头孢哌酮/舒巴坦', value: '69.0' }
            ]
        }
    ]
};
// 不同标本分离铜绿假单胞菌对抗菌药物的耐药率
data['铜绿假单胞菌'] = {
    title: '不同标本分离铜绿假单胞菌对抗菌药物的耐药率',
    color: ['#FE8463', '#ECBF00', '#C1232B', '#015BAA', '#CF7CA6'],
    count: {
        '伤口脓液': '(1339株)',
        '尿液': '(1787株)',
        '呼吸道': '(1054株)',
        '血液': '(15611株)',
        '脑脊液': '(65株)'
    },
    items: [
        {
            name: '伤口脓液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 13.4 },
                { name: '头孢他啶', value: 12.5 },
                { name: '头孢哌酮/舒巴坦', value: 9.6 }
            ]
        },
        {
            name: '尿液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 21.1 },
                { name: '头孢他啶', value: 14.6 },
                { name: '头孢哌酮/舒巴坦', value: 15.1 }
            ]
        },
        {
            name: '血液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 29.2 },
                { name: '头孢他啶', value: 18.4 },
                { name: '头孢哌酮/舒巴坦', value: 14.3 }
            ]
        },
        {
            name: '呼吸道',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 33.9 },
                { name: '头孢他啶', value: 20.2 },
                { name: '头孢哌酮/舒巴坦', value: '18.0' }
            ]
        },
        {
            name: '脑脊液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 55.4 },
                { name: '头孢他啶', value: 31.2 },
                { name: '头孢哌酮/舒巴坦', value: 32.1 }
            ]
        }
    ]
};
// 不同标本分离鲍曼不动杆菌对抗菌药物的耐药率
data['鲍曼不动杆菌'] = {
    title: '不同标本分离鲍曼不动杆菌对抗菌药物的耐药率',
    color: ['#FE8463', '#ECBF00', '#C1232B', '#015BAA', '#CF7CA6'],
    count: {
        '尿液': '(877株)',
        '伤口脓液': '(484株)',
        '呼吸道': '(16543株)',
        '血液': '(1164株)',
        '脑脊液': '(394株)'
    },
    items: [
        {
            name: '尿液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 46.8 },
                { name: '头孢他啶', value: 49.6 },
                { name: '头孢哌酮/舒巴坦', value: 31.6 }
            ]
        },
        {
            name: '伤口脓液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 66.6 },
                { name: '头孢他啶', value: 71.3 },
                { name: '头孢哌酮/舒巴坦', value: 44.8 }
            ]
        },
        {
            name: '呼吸道',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 78.8 },
                { name: '头孢他啶', value: 79.2 },
                { name: '头孢哌酮/舒巴坦', value: 52.7 }
            ]
        },
        {
            name: '血液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 78.9 },
                { name: '头孢他啶', value: 78.6 },
                { name: '头孢哌酮/舒巴坦', value: 59.2 }
            ]
        },
        {
            name: '脑脊液',
            type: 'bar',
            stack: '',
            barWidth: 30,
            label: {
                normal: {
                    show: true,
                    position: 'top',
                    formatter: '{c}'
                }
            },
            data: [
                { name: '亚胺培南', value: 85.1 },
                { name: '头孢他啶', value: '85.0' },
                { name: '头孢哌酮/舒巴坦', value: 60.9 }
            ]
        }
    ]
};

var germCountConfig = new Array();
germCountConfig['大肠埃希菌'] = 46997;
germCountConfig['克雷伯菌属'] = 38635;
germCountConfig['变形杆菌属'] = 4819;
germCountConfig['肠杆菌属'] = 8840;
germCountConfig['沙雷菌属'] = 2861;
germCountConfig['柠檬酸杆菌属'] = 1901;
germCountConfig['摩根菌属'] = 1136;
germCountConfig['铜绿假单胞菌'] = 23431;
germCountConfig['不动杆菌属'] = 23573;
germCountConfig['流感嗜血杆菌'] = 7538;
germCountConfig['流感嗜血杆菌(儿童)'] = 4888;
germCountConfig['流感嗜血杆菌(成人)'] = 2354;
germCountConfig['嗜麦芽窄食单胞菌'] = 6465;
germCountConfig['伯克霍尔德菌属'] = 2083;
germCountConfig['肠杆菌科细菌'] = 107841;
germCountConfig['鼠伤寒沙门菌'] = 468;
germCountConfig['肠炎沙门菌'] = 383;
germCountConfig['伤寒沙门菌'] = 65;
germCountConfig['MSSA'] = 14237;
germCountConfig['MRSA'] = 7327;
germCountConfig['MSCNS'] = 2280;
germCountConfig['MRCNS'] = 8521;
germCountConfig['粪肠球菌'] = 7394;
germCountConfig['屎肠球菌'] = 9885;
germCountConfig['PSSP(儿童)'] = 3323;
germCountConfig['PSSP(成人)'] = 1359;
germCountConfig['PISP(儿童)'] = 330;
germCountConfig['PISP(成人)'] = 45;
germCountConfig['PRSP(儿童)'] = 65;
germCountConfig['PRSP(成人)'] = 23;



// 基于准备好的dom，初始化echarts实例
var myChart = echarts.init(document.getElementById('main'));


$(document).ready(function () {
    $("#DefaultItem").click();
});

/**
 * 选择主要标本菌种分布
 * @param {any} obj 点击事件
 */
function SelectMainGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    var code = $(obj).attr("data-code"),
        text = [],
        value = [],
        title = "";

    var item = data['c.' + code];
    if (item) {
        title = item.title;
        $("#PageContentTitle").html(title);
        if (code === '99') {
            item.data.forEach(function (v, j) {
                v.data.forEach(function (d, j) {
                    if (text.indexOf(d.name) === -1) {
                        text.push(d.name);
                    }
                });
            });
            LoadEcharts2(item.legend, text, item.yAxis, item.data, item.color, 45, item.grid);
        }
        else {
            item.data.forEach(function (v, j) {
                if ($.inArray(v.name, text) === -1) {
                    text.push(v.name);
                }
                value.push(v.value);
            });
            LoadEcharts(text, value, 45, 30);
        }
    }
}

/**
 * 选择不同标本分离菌对抗菌药物的耐药率
 * @param {any} obj 点击事件
 */
function SelectMainGerm2(obj) {
    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    var name = $(obj).data("name"),
        legend = [],
        text = [],
        title = '';

    var item = data[name];
    if (item) {
        title = item.title;
        Object.keys(item.count).forEach(function (v, i) {
            legend.push(v + item.count[v]);
        });

        item.items.forEach(function (v, i) {
            v.data.forEach(function (k, j) {
                if (text.indexOf(k.name) === -1) {
                    text.push(k.name);
                }
            });
        });
        $("#PageContentTitle").html(title);
        LoadEcharts2(legend, text, null, item.items, item.color);
    }
}

/**
 * 选择不同医院耐药菌分布
 * @param {any} obj 点击事件
 */
function SelectHospitalGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    var code = $(obj).attr("data-code"),
        text = [],
        value = [],
        title = "";

    var item = data['c.' + code];
    if (item) {
        title = item.title;
        item.data.forEach(function (v, j) {
            text.push(v.name);
            value.push(v.value);
        });

        $("#PageContentTitle").html(title);
        LoadEcharts(text, value, 45, 30);
    }

    $("#PageContentTitle").html(title);
    LoadEcharts(text, value, 45, 20);
}

//选择细菌
function SelectGerm(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    //加载报表数据
    LoadData($(obj).attr("data-name"), "");
}

//选择抗生素
function SelectAntibiotic(obj) {

    $(".date-item-va").removeClass("active");
    $(obj).addClass("active");

    //加载报表数据
    LoadData("", $(obj).attr("data-name"));
}

/**
 * 请求加载报表数据
 * @param {any} germValue 选择的细菌
 * @param {any} antibioticValue 选择的抗生素
 */
function LoadData(germValue, antibioticValue) {

    var pageContentTitle = "";
    var xRotate = 0;

    if (germValue && germValue !== '') {

        var germCount = germCountConfig[germValue];
        if (germCount && germCount !== "" && germCount !== undefined && typeof germCount !== 'undefined') {
            germCount += "株";
        }
        else {
            germCount = '';
        }


        //选择了细菌分类
        pageContentTitle = germCount + germValue + "对抗菌药物的耐药率";

        if (germValue === "克雷伯菌属" || germValue === "柠檬酸杆菌属" || germValue === "大肠埃希菌" || germValue === '摩根菌属' || germValue === '沙雷菌属' || germValue === '铜绿假单胞菌' || germValue === '肠杆菌属' || germValue === '变形杆菌属' || germValue === '不动杆菌属') {
            xRotate = 45;
        }

    }
    else if (antibioticValue && antibioticValue !== "") {
        //选择了抗菌药物
        pageContentTitle = "不同细菌对" + antibioticValue + "的耐药率";

        if (antibioticValue === '左氧氟沙星' ||
            antibioticValue === '环丙沙星' ||
            antibioticValue === '庆大霉素' ||
            antibioticValue === '复方磺胺甲噁唑' ||
            antibioticValue === '氨苄西林' ||
            antibioticValue === '氯霉素' ||
            antibioticValue === '美罗培南' ||
            antibioticValue === '氨苄西林/舒巴坦') {
            xRotate = 45;
        }

    }
    else {
        //什么都没有选择
        $("#dataError").css("display", "none");
        $(".main-content2").css("display", "block");
    }

    $("#PageContentTitle").html(pageContentTitle);

    $.ajax({
        type: "POST",
        url: "/Data/GetAntibioticDrugFastData",
        data: {
            GermValue: germValue,
            AntibioticValue: antibioticValue
        },
        dataType: "json",
        success: function (result) {

            if (result === null || result === "") {
                MessageWindow("获取数据失败");
                return;
            }

            if (result.Status) {

                var text = result.Text.split(",");
                var value = result.Value.split(",");

                LoadEcharts(text, value, xRotate, 30, pageContentTitle);

            }
            else {
                LoadEcharts([], []);
                MessageWindow(result.Message);
            }
        }
    });

}

/**
 * 加载百度柱状图插件
 * @param {any} text 显示的文本内容
 * @param {any} value 对应的值
 * @param {any} xRotate x轴文字的显示方向
 * @param {any} barWidth 柱子的宽度
 * @param {any} pageName 标题
 */
function LoadEcharts(text, value, xRotate, barWidth, pageName) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    if (!myChart) {
        myChart = echarts.init(document.getElementById('main'));
    }

    myChart.clear();
    myChart.resize();
    myChart.showLoading();

    option = {
        color: ['#015BAA'],
        title: {
            text: '' + pageName + '',
            subtext: '',
            left: 'center',
            show: false
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        grid: {
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
                data: text,  //'替考拉宁', '万古霉素', '利奈唑胺', '利福平', '庆大霉素', '左氧氟沙星', '环丙沙星', '克林霉素', '复方磺胺甲恶唑', '红霉素', '青霉素', '苯唑西林'
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
                type: 'bar',
                barWidth: barWidth,//柱图宽度
                data: value,//0, 0, 0, 12.7, 31.2, 56.7, 55.7, 42.9, 54.7, 85.7, 100, 100
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
                },
            }
        ],
        textStyle:
        {
            fontSize: 10
        }
    };

    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);
}

/**
 * 加载百度柱状图插件
 * @param {any} legend 图例
 * @param {any} xAxisData x轴数据
 * @param {any} yAxis y轴数据
 * @param {any} value 值
 * @param {any} color 颜色
 * @param {any} xRotate x轴文字的显示方向
 * @param {any} grid 网格
 * @param {any} pageName 标题
 */
function LoadEcharts2(legend, xAxisData, yAxis, value, color, xRotate, grid, pageName) {

    $("#dataError").css("display", "none");
    $(".main-content2").css("display", "block");

    if (!myChart) {
        myChart = echarts.init(document.getElementById('main'));
    }

    myChart.clear();
    myChart.resize();
    myChart.showLoading();
    var option = {
        color: color,
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        title: {
            text: pageName || '',
            subtext: '',
            left: 'center',
            show: false,
            textStyle: {
                fontWeight: 'normal'
            }
        },
        legend: {
            data: legend,
            top: '10px'
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '15%',
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
                axisLabel: {
                    interval: 0,
                    rotate: xRotate,
                    margin: 10,
                    textStyle: {
                        fontSize: "14px"
                    }
                },
                data: xAxisData,
                axisTick: {
                    show: false,
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
        series: value
    };

    if (yAxis && yAxis.length > 0) {
        option.yAxis = yAxis;
    }
    if (grid && grid !== 'undefined' && typeof grid !== undefined) {
        option.grid = grid;
    }

    myChart.resize();
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
    setTimeout(function () {
        myChart.hideLoading();
    }, 300);
}

//取消选择
function ClearSelect() {

    $(".date-item-va").removeClass("active");
    var itemList = $(".date-item-va");

    for (var i = 0; i < itemList.length; i++) {
        $(itemList[i]).attr("data-status", "0");
    }

    //隐藏
    $("#dataError").css("display", "block");
    $(".main-content2").css("display", "none");
    $("#PageContentTitle").html("");
}


//设置单个项目的显示和隐藏（公共函数）
function ShowStatus(className) {
    var $el = $("#" + className),
        status = $el.attr("data-status");

    if (status === "1" || status === 1) {
        //隐藏
        $el.slideUp(300, function () {
            $el.attr("data-status", "0");
            $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-shang.png");
        });
    } else {
        //显示
        $el.slideDown(300, function () {
            $el.attr("data-status", "1");
            $(".btn-image-" + className).attr("src", "/Content/Images/jiantou-xia.png");
        });
    }
}