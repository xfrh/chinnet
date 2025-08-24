var dataConfig = {
    '临床分离菌株前10位细菌': {
        title: '临床分离菌株前10位细菌(n=144373)(2018年)',
        legend: [],
        data: [
            {
                name: '细菌',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '嗜麦芽窄食单胞菌', value: '2.7' },
                    { name: '肠杆菌属', value: '3.8' },
                    { name: 'CNS', value: '4.1' },
                    { name: 'β溶血链球菌', value: '4.5' },
                    { name: '不动杆菌属', value: '8.0' },
                    { name: '铜绿假单细胞', value: '8.7' },
                    { name: '金葡菌', value: '9.1' },
                    { name: '肠球菌属', value: '10.0' },
                    { name: '克雷伯菌属', value: '14.6' },
                    { name: '大肠埃希菌', value: '20.6' }
                ]
            }
        ]
    },
    '临床分离菌在各类标本中的分布': {
        title: '临床分离菌在各类标本中的分布(2018年)',
        legend: [],
        data: [
            {
                name: '临床分离菌',
                type: 'bar',
                barWidth: 30,
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                },
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '粪便', value: '0.6' },
                    { name: '生殖道分泌液', value: '1.3' },
                    { name: '无菌体液', value: '5.2' },
                    { name: '伤口渗液', value: '6.8' },
                    { name: '血标本', value: '8.9' },
                    { name: '其他', value: '9.7' },
                    { name: '尿液标本', value: '28.9' },
                    { name: '呼吸道标本', value: '38.7' }
                ]
            }
        ]
    },
    '大肠埃希菌和肺炎克雷伯菌': {
        title: '大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['大肠埃希菌(29754株)', '肺炎克雷伯菌(20137株)'],
        data: [
            {
                name: '大肠埃希菌(29754株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '2.0' },
                    { name: '美罗培南', value: '2.2' },
                    { name: '阿米卡星', value: '2.6' },
                    { name: '哌拉西林/他唑巴坦', value: '3.8' },
                    { name: '头孢哌酮/舒巴坦', value: '6.2' },
                    { name: '磷霉素', value: '7.5' },
                    { name: '头孢美唑', value: '8.2' },
                    { name: '头孢吡肟', value: '24.4' },
                    { name: '头孢他啶', value: '26.4' },
                    { name: '庆大霉素', value: '33.8' },
                    { name: '氨苄西林/舒巴坦', value: '43.7' },
                    { name: '复方磺胺甲噁唑', value: '47.0' },
                    { name: '环丙沙星', value: '57.1' },
                    { name: '头孢噻肟', value: '58.5' },
                    { name: '头孢呋辛', value: '60.3' },
                    { name: '哌拉西林', value: '71.0' },
                    { name: '氨苄西林', value: '81.9' }
                ]
            },
            {
                name: '肺炎克雷伯菌(20137株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '27.4' },
                    { name: '美罗培南', value: '28.4' },
                    { name: '阿米卡星', value: '18.8' },
                    { name: '哌拉西林/他唑巴坦', value: '29.5' },
                    { name: '头孢哌酮/舒巴坦', value: '31.9' },
                    { name: '磷霉素', value: '22.5' },
                    { name: '头孢美唑', value: '35.5' },
                    { name: '头孢吡肟', value: '36.0' },
                    { name: '头孢他啶', value: '39.2' },
                    { name: '庆大霉素', value: '32.6' },
                    { name: '氨苄西林/舒巴坦', value: '48.1' },
                    { name: '复方磺胺甲噁唑', value: '34.1' },
                    { name: '环丙沙星', value: '40.0' },
                    { name: '头孢噻肟', value: '52.6' },
                    { name: '头孢呋辛', value: '53.2' },
                    { name: '哌拉西林', value: '58.2' },
                    { name: '氨苄西林', value: '89.7' }
                ]
            }
        ]
    },
    '二级医院大肠埃希菌和肺炎克雷伯菌': {
        title: '二级医院分离大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['大肠埃希菌(10572株)', '肺炎克雷伯菌(6790株)'],
        data: [
            {
                name: '大肠埃希菌(10572株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '1.8' },
                    { name: '美罗培南', value: '1.8' },
                    { name: '阿米卡星', value: '2.6' },
                    { name: '哌拉西林/他唑巴坦', value: '3.4' },
                    { name: '头孢哌酮/舒巴坦', value: '5.4' },
                    { name: '磷霉素*(尿)', value: '6.8' },
                    { name: '头孢美唑', value: '9.3' },
                    { name: '头孢吡肟', value: '22.3' },
                    { name: '头孢他啶', value: '25.9' },
                    { name: '庆大霉素', value: '33.4' },
                    { name: '氨苄西林/舒巴坦', value: '42.7' },
                    { name: 'SMZ-TMP', value: '46.1' },
                    { name: '头孢呋辛', value: '56.3' },
                    { name: '环丙沙星', value: '57.0' },
                    { name: '头孢噻肟', value: '58.8' },
                    { name: '头孢唑啉', value: '59.1' },
                    { name: '哌拉西林', value: '61.5' },
                    { name: '氨苄西林', value: '81.2' }
                ]
            },
            {
                name: '肺炎克雷伯菌(6790株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '20.9' },
                    { name: '美罗培南', value: '21.9' },
                    { name: '阿米卡星', value: '15.2' },
                    { name: '哌拉西林/他唑巴坦', value: '22.9' },
                    { name: '头孢哌酮/舒巴坦', value: '24.1' },
                    { name: '磷霉素*(尿)', value: '21.6' },
                    { name: '头孢美唑', value: '37.0' },
                    { name: '头孢吡肟', value: '29.6' },
                    { name: '头孢他啶', value: '33.3' },
                    { name: '庆大霉素', value: '28.8' },
                    { name: '氨苄西林/舒巴坦', value: '41.6' },
                    { name: 'SMZ-TMP', value: '28.9' },
                    { name: '头孢呋辛', value: '45.6' },
                    { name: '环丙沙星', value: '34.4' },
                    { name: '头孢噻肟', value: '43.3' },
                    { name: '头孢唑啉', value: '45.7' },
                    { name: '哌拉西林', value: '39.2' },
                    { name: '氨苄西林', value: '92.0' }
                ]
            }
        ]
    },
   '三级医院大肠埃希菌和肺炎克雷伯菌': {
        title: '三级医院分离大肠埃希菌和肺炎克雷伯菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['大肠埃希菌(19182株)', '肺炎克雷伯菌(13347株)'],
        data: [
            {
                name: '大肠埃希菌(19182株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '2.2' },
                    { name: '美罗培南', value: '2.4' },
                    { name: '阿米卡星', value: '2.7' },
                    { name: '哌拉西林/他唑巴坦', value: '4.0' },
                    { name: '头孢哌酮/舒巴坦', value: '6.5' },
                    { name: '头孢美唑', value: '7.4' },
                    { name: '磷霉素*(尿)', value: '7.8' },
                    { name: '头孢吡肟', value: '25.6' },
                    { name: '头孢他啶', value: '26.7' },
                    { name: '庆大霉素', value: '34.0' },
                    { name: '氨苄西林/舒巴坦', value: '44.3' },
                    { name: 'SMZ-TMP', value: '47.5' },
                    { name: '环丙沙星', value: '57.2' },
                    { name: '头孢呋辛', value: '58.3' },
                    { name: '头孢噻肟', value: '58.4' },
                    { name: '头孢唑啉', value: '61.0' },
                    { name: '哌拉西林', value: '75.6' },
                    { name: '氨苄西林', value: '82.4' }
                ]
            },
            {
                name: '肺炎克雷伯菌(13347株)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '亚胺培南', value: '30.7' },
                    { name: '美罗培南', value: '31.6' },
                    { name: '阿米卡星', value: '20.6' },
                    { name: '哌拉西林/他唑巴坦', value: '32.8' },
                    { name: '头孢哌酮/舒巴坦', value: '35.7' },
                    { name: '头孢美唑', value: '34.5' },
                    { name: '磷霉素*(尿)', value: '23.1' },
                    { name: '头孢吡肟', value: '39.2' },
                    { name: '头孢他啶', value: '42.3' },
                    { name: '庆大霉素', value: '34.6' },
                    { name: '氨苄西林/舒巴坦', value: '51.8' },
                    { name: 'SMZ-TMP', value: '36.8' },
                    { name: '环丙沙星', value: '42.8' },
                    { name: '头孢呋辛', value: '56.2' },
                    { name: '头孢噻肟', value: '56.9' },
                    { name: '头孢唑啉', value: '57.2' },
                    { name: '哌拉西林', value: '65.7' },
                    { name: '氨苄西林', value: '88.3' }
                ]
            }
        ]
    },
    '不同肠杆菌科细菌': {
        title: '不同肠杆菌科细菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['肠杆菌属(5549株)', '变形杆菌属(4121株)', '沙雷菌属(1682株)', '柠檬酸杆菌属(1313株)', '摩根菌属(897株)', '普罗威登菌属(156株)'],
        data: [
            {
                name: '肠杆菌属(5549株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '2.2' },
                    { name: '美罗培南', value: '7.0' },
                    { name: '亚胺培南', value: '7.5' },
                    { name: '庆大霉素', value: '9.1' },
                    { name: '哌拉西林/他唑巴坦', value: '11.8' },
                    { name: '头孢哌酮/舒巴坦', value: '11.6' },
                    { name: '头孢吡肟', value: '13.3' },
                    { name: '环丙沙星', value: '13.5' },
                    { name: 'SMZ-TMP', value: '18.0' },
                    { name: '头孢他啶', value: '32.1' },
                    { name: '哌拉西林', value: '37.5' },
                    { name: '头孢噻肟', value: '44.7' },
                    { name: '头孢呋辛', value: '51.3' },
                    { name: '氨苄西林/舒巴坦', value: '57.1' },
                    { name: '氨苄西林', value: '85.9' },
                    { name: '头孢美唑', value: '87.6' },
                    { name: '头孢唑啉', value: '90.2' }
                ]
            },
            {
                name: '变形杆菌属(4121株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '5.4' },
                    { name: '美罗培南', value: '1.4' },
                    { name: '亚胺培南', value: '12.7' },
                    { name: '庆大霉素', value: '24.2' },
                    { name: '哌拉西林/他唑巴坦', value: '1.1' },
                    { name: '头孢哌酮/舒巴坦', value: '1.3' },
                    { name: '头孢吡肟', value: '15.9' },
                    { name: '环丙沙星', value: '48.4' },
                    { name: 'SMZ-TMP', value: '57.7' },
                    { name: '头孢他啶', value: '17.3' },
                    { name: '哌拉西林', value: '38.0' },
                    { name: '头孢噻肟', value: '48.7' },
                    { name: '头孢呋辛', value: '54.3' },
                    { name: '氨苄西林/舒巴坦', value: '31.3' },
                    { name: '氨苄西林', value: '67.3' },
                    { name: '头孢美唑', value: '4.2' },
                    { name: '头孢唑啉', value: '58.2' }
                ]
            },
            {
                name: '沙雷菌属(1682株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '1.8' },
                    { name: '美罗培南', value: '11.0' },
                    { name: '亚胺培南', value: '12.3' },
                    { name: '庆大霉素', value: '17.0' },
                    { name: '哌拉西林/他唑巴坦', value: '8.4' },
                    { name: '头孢哌酮/舒巴坦', value: '12.3' },
                    { name: '头孢吡肟', value: '12.3' },
                    { name: '环丙沙星', value: '21.6' },
                    { name: 'SMZ-TMP', value: '5.7' },
                    { name: '头孢他啶', value: '10.7' },
                    { name: '哌拉西林', value: '38.6' },
                    { name: '头孢噻肟', value: '37.8' },
                    { name: '头孢呋辛', value: '89.2' },
                    { name: '氨苄西林/舒巴坦', value: '68.2' },
                    { name: '氨苄西林', value: '82.3' },
                    { name: '头孢美唑', value: '9.6' },
                    { name: '头孢唑啉', value: '96.7' }
                ]
            },
            {
                name: '柠檬酸杆菌属(1313株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '1.4' },
                    { name: '美罗培南', value: '3.4' },
                    { name: '亚胺培南', value: '3.8' },
                    { name: '庆大霉素', value: '10.3' },
                    { name: '哌拉西林/他唑巴坦', value: '8.4' },
                    { name: '头孢哌酮/舒巴坦', value: '7.4' },
                    { name: '头孢吡肟', value: '7.7' },
                    { name: '环丙沙星', value: '16.3' },
                    { name: 'SMZ-TMP', value: '19.8' },
                    { name: '头孢他啶', value: '24.8' },
                    { name: '哌拉西林', value: '36.8' },
                    { name: '头孢噻肟', value: '37.2' },
                    { name: '头孢呋辛', value: '39.0' },
                    { name: '氨苄西林/舒巴坦', value: '40.8' },
                    { name: '氨苄西林', value: '82.3' },
                    { name: '头孢美唑', value: '38.9' },
                    { name: '头孢唑啉', value: '62.3' }
                ]
            },
            {
                name: '摩根菌属(897株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '2.0' },
                    { name: '美罗培南', value: '4.1' },
                    { name: '亚胺培南', value: '15.9' },
                    { name: '庆大霉素', value: '19.4' },
                    { name: '哌拉西林/他唑巴坦', value: '4.4' },
                    { name: '头孢哌酮/舒巴坦', value: '3.0' },
                    { name: '头孢吡肟', value: '3.6' },
                    { name: '环丙沙星', value: '22.0' },
                    { name: 'SMZ-TMP', value: '36.4' },
                    { name: '头孢他啶', value: '15.3' },
                    { name: '哌拉西林', value: '25.4' },
                    { name: '头孢噻肟', value: '30.0' },
                    { name: '头孢呋辛', value: '80.0' },
                    { name: '氨苄西林/舒巴坦', value: '63.5' },
                    { name: '氨苄西林', value: '96.8' },
                    { name: '头孢美唑', value: '9.5' },
                    { name: '头孢唑啉', value: '97.7' }
                ]
            },
            {
                name: '普罗威登菌属(156株)',
                type: 'bar',
                barWidth: 6,
                label: {
                    normal: {
                        show: false,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '7.1' },
                    { name: '美罗培南', value: '4.2' },
                    { name: '亚胺培南', value: '8.0' },
                    { name: '庆大霉素', value: '14.4' },
                    { name: '哌拉西林/他唑巴坦', value: '9.6' },
                    { name: '头孢哌酮/舒巴坦', value: '7.6' },
                    { name: '头孢吡肟', value: '10.3' },
                    { name: '环丙沙星', value: '44.1' },
                    { name: 'SMZ-TMP', value: '32.5' },
                    { name: '头孢他啶', value: '23.1' },
                    { name: '哌拉西林', value: '25.6' },
                    { name: '头孢噻肟', value: '35.4' },
                    { name: '头孢呋辛', value: '45.7' },
                    { name: '氨苄西林/舒巴坦', value: '48.5' },
                    { name: '氨苄西林', value: '67.1' },
                    { name: '头孢美唑', value: '6.2' },
                    { name: '头孢唑啉', value: '82.3' }
                ]
            }
        ]
    },
    '肠杆菌科细菌(不同医院来源)': {
        title: '肠杆菌科细菌对8种抗生素的敏感性（%）(2018年)',
        legend: ['50家医院(n=65868)', '二级医院(n=22665)', '三级医院(n=43203)'],
        data: [
            {
                name: '50家医院(n=65868)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '环丙沙星', value: '52.3' },
                    { name: '头孢吡肟', value: '67.4' },
                    { name: '头孢他啶', value: '67.7' },
                    { name: '头孢哌酮/舒巴坦', value: '76.0' },
                    { name: '哌拉西林/他唑巴坦', value: '83.1' },
                    { name: '亚胺培南', value: '86.5' },
                    { name: '美罗培南', value: '88.0' },
                    { name: '阿米卡星', value: '91.6' }
                ]
            },
            {
                name: '二级医院(n=22665)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '环丙沙星', value: '53.6' },
                    { name: '头孢吡肟', value: '69.3' },
                    { name: '头孢他啶', value: '70.1' },
                    { name: '头孢哌酮/舒巴坦', value: '81.0' },
                    { name: '哌拉西林/他唑巴坦', value: '86.1' },
                    { name: '亚胺培南', value: '89.5' },
                    { name: '美罗培南', value: '90.6' },
                    { name: '阿米卡星', value: '92.8' }
                ]
            },
            {
                name: '三级医院(n=43203)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '环丙沙星', value: '51.6' },
                    { name: '头孢吡肟', value: '66.4' },
                    { name: '头孢他啶', value: '66.5' },
                    { name: '头孢哌酮/舒巴坦', value: '73.5' },
                    { name: '哌拉西林/他唑巴坦', value: '81.5' },
                    { name: '亚胺培南', value: '85.0' },
                    { name: '美罗培南', value: '86.7' },
                    { name: '阿米卡星', value: '91.0' }
                ]
            }
        ]
    },
    '沙门菌属细菌': {
        title: '沙门菌属细菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['肠炎沙门菌(224株)', '鼠伤寒沙门菌(285株)'],
        data: [
            {
                name: '肠炎沙门菌(224株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '环丙沙星', value: '3.9' },
                    { name: '氯霉素', value: '5.6' },
                    { name: 'SMZ-TMP', value: '7.2' },
                    { name: '头孢曲松', value: '9.2' },
                    { name: '氨苄西林/舒巴坦', value: '23.1' },
                    { name: '氨苄西林', value: '79.5' }
                ]
            },
            {
                name: '鼠伤寒沙门菌(285株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '环丙沙星', value: '10.1' },
                    { name: '氯霉素', value: '44.1' },
                    { name: 'SMZ-TMP', value: '41.5' },
                    { name: '头孢曲松', value: '17.8' },
                    { name: '氨苄西林/舒巴坦', value: '17.3' },
                    { name: '氨苄西林', value: '79.9' }
                ]
            }
        ]
    },
    '铜绿假单胞菌': {
        title: '12534株铜绿假单胞菌对抗菌药物的耐药率（%）(2018年)',
        legend: [],
        data: [
            {
                name: '铜绿假单胞菌',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '多黏菌素E', value: '0.5' },
                    { name: '阿米卡星', value: '6.2' },
                    { name: '哌拉西林/他唑巴坦', value: '10.2' },
                    { name: '庆大霉素', value: '11.2' },
                    { name: '头孢哌酮/舒巴坦', value: '12.9' },
                    { name: '头孢吡肟', value: '13.5' },
                    { name: '头孢他啶', value: '15.2' },
                    { name: '哌拉西林', value: '16.7' },
                    { name: '环丙沙星', value: '20.3' },
                    { name: '头孢哌酮', value: '21.6' },
                    { name: '氨曲南', value: '25.2' },
                    { name: '美罗培南', value: '24.9' },
                    { name: '亚胺培南', value: '26.2' }
                ]
            }
        ]
    },
    '铜绿假单胞菌(不同医院来源)': {
        title: '铜绿假单胞菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['二级医院(4057株)', '三级医院(8477株)'],
        data: [
            {
                name: '二级医院(4057株)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '3.9' },
                    { name: '哌拉西林/他唑巴坦', value: '8.6' },
                    { name: '庆大霉素', value: '9.5' },
                    { name: '头孢哌酮/舒巴坦', value: '10.1' },
                    { name: '头孢吡肟', value: '11.6' },
                    { name: '头孢他啶', value: '13.6' },
                    { name: '哌拉西林', value: '13.8' },
                    { name: '头孢哌酮', value: '17.1' },
                    { name: '美罗培南', value: '19.5' },
                    { name: '环丙沙星', value: '19.9' },
                    { name: '亚胺培南', value: '20.0' },
                    { name: '氨曲南', value: '20.6' }
                ]
            },
            {
                name: '三级医院(8477株)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '阿米卡星', value: '7.4' },
                    { name: '哌拉西林/他唑巴坦', value: '10.9' },
                    { name: '庆大霉素', value: '12.1' },
                    { name: '头孢哌酮/舒巴坦', value: '14.2' },
                    { name: '头孢吡肟', value: '14.5' },
                    { name: '头孢他啶', value: '16.0' },
                    { name: '哌拉西林', value: '18.0' },
                    { name: '头孢哌酮', value: '23.7' },
                    { name: '美罗培南', value: '27.3' },
                    { name: '环丙沙星', value: '20.4' },
                    { name: '亚胺培南', value: '29.1' },
                    { name: '氨曲南', value: '27.4' }
                ]
            }
        ]
    },
    '鲍曼不动杆菌': {
        title: '11254株鲍曼不动杆菌对抗菌药物的耐药率（%）(2018年)',
        legend: [],
        data: [
            {
                name: '不动杆菌属',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '多黏菌素E', value: '0.0' },
                    { name: '米诺环素', value: '24.0' },
                    { name: '头孢哌酮/舒巴坦', value: '35.2' },
                    { name: '复方磺胺甲噁唑', value: '48.1' },
                    { name: '阿米卡星', value: '50.5' },
                    { name: '氨苄西林/舒巴坦', value: '55.0' },
                    { name: '庆大霉素', value: '58.5' },
                    { name: '亚胺培南', value: '61.8' },
                    { name: '哌拉西林/他唑巴坦', value: '61.9' },
                    { name: '美罗培南', value: '62.4' },
                    { name: '头孢他啶', value: '63.1' },
                    { name: '头孢吡肟', value: '63.2' },
                    { name: '环丙沙星', value: '65.6' },
                    { name: '哌拉西林', value: '68.9' }
                ]
            }
        ]
    },
    '鲍曼不动杆菌(不同医院来源)': {
        title: '鲍曼不动杆菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['二级医院(3768株)', '三级医院(7486株)'],
        data: [
            {
                name: '二级医院(3768株)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '头孢哌酮/舒巴坦', value: '22.8' },
                    { name: '米诺环素', value: '28.0' },
                    { name: 'SMZ-TMP', value: '41.4' },
                    { name: '阿米卡星', value: '44.8' },
                    { name: '氨苄西林/舒巴坦', value: '51.8' },
                    { name: '庆大霉素', value: '53.3' },
                    { name: '哌拉西林/他唑巴坦', value: '53.4' },
                    { name: '美罗培南', value: '55.5' },
                    { name: '亚胺培南', value: '56.4' },
                    { name: '头孢吡肟', value: '58.7' },
                    { name: '头孢他啶', value: '58.8' },
                    { name: '环丙沙星', value: '60.9' },
                    { name: '哌拉西林', value: '63.2' }
                ]
            },
            {
                name: '三级医院(7486株)',
                type: 'bar',
                barWidth: 25,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '头孢哌酮/舒巴坦', value: '40.9' },
                    { name: '米诺环素', value: '22.1' },
                    { name: 'SMZ-TMP', value: '51.6' },
                    { name: '阿米卡星', value: '53.1' },
                    { name: '氨苄西林/舒巴坦', value: '56.7' },
                    { name: '庆大霉素', value: '61.3' },
                    { name: '哌拉西林/他唑巴坦', value: '65.4' },
                    { name: '美罗培南', value: '65.6' },
                    { name: '亚胺培南', value: '64.6' },
                    { name: '头孢吡肟', value: '65.4' },
                    { name: '头孢他啶', value: '65.4' },
                    { name: '环丙沙星', value: '68.0' },
                    { name: '哌拉西林', value: '71.8' }
                ]
            }
        ]
    },
    '洋葱伯克霍尔德菌和嗜麦芽窄食单胞菌': {
        title: '洋葱伯克霍尔德菌和嗜麦芽窄食单胞菌的耐药率（%）(2018年)',
        legend: ['洋葱伯克霍尔德菌(389株)', '嗜麦芽窄食单胞菌(3890株)'],
        data: [
            {
                name: '洋葱伯克霍尔德菌(389株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '哌拉西林/他唑巴坦', value: '7.5' },
                    { name: '米诺环素', value: '8.2' },
                    { name: 'SMZ-TMP', value: '8.3' },
                    { name: '头孢他啶', value: '11.9' },
                    { name: '美罗培南', value: '13.6' },
                    { name: '左氧氟沙星', value: '-' },
                    { name: '头孢哌酮/舒巴坦', value: '-' }
                ]
            },
            {
                name: '嗜麦芽窄食单胞菌(3890株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '哌拉西林/他唑巴坦', value: '-' },
                    { name: '米诺环素', value: '2.3' },
                    { name: 'SMZ-TMP', value: '6.6' },
                    { name: '头孢他啶', value: '-' },
                    { name: '美罗培南', value: '-' },
                    { name: '左氧氟沙星', value: '11.3' },
                    { name: '头孢哌酮/舒巴坦', value: '22.1' }
                ]
            }
        ]
    },
    '流感嗜血杆菌': {
        title: '流感嗜血杆菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['儿童(1536株)', '成人(904株)', '成人+儿童(2440株)'],
        data: [
            {
                name: '儿童(1536株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '左氧氟沙星', value: '2.3' },
                    { name: '氯霉素', value: '2.7' },
                    { name: '阿莫西林/克拉维酸', value: '10.3' },
                    { name: '氨苄/舒巴坦', value: '25.4' },
                    { name: '阿奇霉素', value: '29.2' },
                    { name: '头孢呋辛', value: '37.5' },
                    { name: '氨苄西林', value: '59.8' },
                    { name: 'SMZ-TMP', value: '62.0' }
                ]
            },
            {
                name: '成人(904株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '左氧氟沙星', value: '11.2' },
                    { name: '氯霉素', value: '5.7' },
                    { name: '阿莫西林/克拉维酸', value: '30.5' },
                    { name: '氨苄/舒巴坦', value: '31.5' },
                    { name: '阿奇霉素', value: '14.6' },
                    { name: '头孢呋辛', value: '29.4' },
                    { name: '氨苄西林', value: '44.7' },
                    { name: 'SMZ-TMP', value: '43.8' }
                ]
            },
            {
                name: '成人+儿童（2440株）',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '左氧氟沙星', value: '5.8' },
                    { name: '氯霉素', value: '3.6' },
                    { name: '阿莫西林/克拉维酸', value: '13.2' },
                    { name: '氨苄/舒巴坦', value: '27.1' },
                    { name: '阿奇霉素', value: '25.4' },
                    { name: '头孢呋辛', value: '35.3' },
                    { name: '氨苄西林', value: '55.0' },
                    { name: 'SMZ-TMP', value: '56.2' }
                ]
            }
        ]
    },
    '金黄色葡萄球菌': {
        title: '金黄色葡萄球菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['MRSA(6022)', 'MSSA(7087)'],
        data: [
            {
                name: 'MRSA(6022)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '2.8' },
                    { name: '复方新诺明', value: '6.8' },
                    { name: '庆大霉素', value: '32.2' },
                    { name: '左氧氟沙星', value: '53.3' },
                    { name: '克林霉素', value: '52.8' },
                    { name: '红霉素', value: '76.9' },
                    { name: '苯唑西林', value: '100.0' },
                    { name: '青霉素', value: '100.0' }
                ]
            },
            {
                name: 'MSSA(7087)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '0.6' },
                    { name: '复方新诺明', value: '8.2' },
                    { name: '庆大霉素', value: '4.9' },
                    { name: '左氧氟沙星', value: '9.6' },
                    { name: '克林霉素', value: '12.5' },
                    { name: '红霉素', value: '37.0' },
                    { name: '苯唑西林', value: '0.0' },
                    { name: '青霉素', value: '84.4' }
                ]
            }
        ]
    },
    '金黄色葡萄球菌（不同医院来源）': {
        title: '金黄色葡萄球菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['二级医院(20)MRSA(1931)', '二级医院(20)MSSA(1793)', '三级医院(30)MRSA(4091)', '三级医院(30)MSSA(5294)'],
        data: [
            {
                name: '二级医院(20)MRSA(1931)',
                type: 'bar',
                barWidth: 15,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '1.9' },
                    { name: 'SMZ-TMP', value: '6.7' },
                    { name: '庆大霉素', value: '37.8' },
                    { name: '克林霉素', value: '58.8' },
                    { name: '左氧氟沙星', value: '65.7' },
                    { name: '红霉素', value: '79.9' },
                    { name: '苯唑西林', value: '100.0' },
                    { name: '青霉素', value: '100.0' }
                ]
            },
            {
                name: '二级医院(20)MSSA(1793)',
                type: 'bar',
                barWidth: 15,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '0.4' },
                    { name: 'SMZ-TMP', value: '7.7' },
                    { name: '庆大霉素', value: '5.6' },
                    { name: '克林霉素', value: '13.7' },
                    { name: '左氧氟沙星', value: '11.8' },
                    { name: '红霉素', value: '35.2' },
                    { name: '苯唑西林', value: '0.0' },
                    { name: '青霉素', value: '86.9' }
                ]
            },
            {
                name: '三级医院(30)MRSA(4091)',
                type: 'bar',
                barWidth: 15,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '3.2' },
                    { name: 'SMZ-TMP', value: '6.9' },
                    { name: '庆大霉素', value: '29.6' },
                    { name: '克林霉素', value: '49.9' },
                    { name: '左氧氟沙星', value: '47.5' },
                    { name: '红霉素', value: '75.5' },
                    { name: '苯唑西林', value: '100.0' },
                    { name: '青霉素', value: '100.0' }
                ]
            },
            {
                name: '三级医院(30)MSSA(5294)',
                type: 'bar',
                barWidth: 15,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '利福平', value: '0.6' },
                    { name: 'SMZ-TMP', value: '8.4' },
                    { name: '庆大霉素', value: '4.7' },
                    { name: '克林霉素', value: '12.1' },
                    { name: '左氧氟沙星', value: '8.8' },
                    { name: '红霉素', value: '37.6' },
                    { name: '苯唑西林', value: '0.0' },
                    { name: '青霉素', value: '83.5' }
                ]
            }
        ]
    },
    '肠球菌属': {
        title: '肠球菌属对抗菌药物的耐药率（%）(2018年)',
        legend: ['粪肠球菌(6992)', '屎肠球菌(5232)'],
        data: [
            {
                name: '粪肠球菌(6992)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '替考拉宁', value: '0.4' },
                    { name: '利奈唑胺', value: '1.1' },
                    { name: '呋喃妥因', value: '1.9' },
                    { name: '磷霉素', value: '4.6' },
                    { name: '氨苄西林', value: '5.2' },
                    { name: '庆大霉素H', value: '34.3' },
                    { name: '左氧氟沙星', value: '38.1' }
                ]
            },
            {
                name: '屎肠球菌(5232)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.7' },
                    { name: '替考拉宁', value: '1.7' },
                    { name: '利奈唑胺', value: '0.1' },
                    { name: '呋喃妥因', value: '37.5' },
                    { name: '磷霉素', value: '19.9' },
                    { name: '氨苄西林', value: '90.8' },
                    { name: '庆大霉素H', value: '36.3' },
                    { name: '左氧氟沙星', value: '88.6' }
                ]
            }
        ]
    },
    '肠球菌属（不同医院来源）': {
        title: '肠球菌属对抗菌药物的耐药率（%）(2018年)',
        legend: ['二级医院(20)粪肠球菌(2334)', '二级医院(20)屎肠球菌(2102)', '三级医院(30)粪肠球菌(7713)', '三级医院(30)屎肠球菌(5943)'],
        data: [
            {
                name: '二级医院(20)粪肠球菌(2334)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '替考拉宁', value: '0.0' },
                    { name: '利奈唑胺', value: '0.2' },
                    { name: '呋喃妥因', value: '2.2' },
                    { name: '磷霉素', value: '6.9' },
                    { name: '氨苄西林', value: '7.8' },
                    { name: '高浓度庆大霉素', value: '38' },
                    { name: '左氧氟沙星', value: '47.8' }
                ]
            },
            {
                name: '二级医院(20)屎肠球菌(2102)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.2' },
                    { name: '替考拉宁', value: '0.7' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '呋喃妥因', value: '40.1' },
                    { name: '磷霉素', value: '26.5' },
                    { name: '氨苄西林', value: '90.8' },
                    { name: '高浓度庆大霉素', value: '35.0' },
                    { name: '左氧氟沙星', value: '92.3' }
                ]
            },
            {
                name: '三级医院(30)粪肠球菌(7713)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '替考拉宁', value: '0.5' },
                    { name: '利奈唑胺', value: '1.5' },
                    { name: '呋喃妥因', value: '1.8' },
                    { name: '磷霉素', value: '3.6' },
                    { name: '氨苄西林', value: '4.0' },
                    { name: '高浓度庆大霉素', value: '32.6' },
                    { name: '左氧氟沙星', value: '33.7' }
                ]
            },
            {
                name: '三级医院(30)屎肠球菌(5943)',
                type: 'bar',
                barWidth: 20,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '1.0' },
                    { name: '替考拉宁', value: '2.1' },
                    { name: '利奈唑胺', value: '0.2' },
                    { name: '呋喃妥因', value: '35.7' },
                    { name: '磷霉素', value: '14.6' },
                    { name: '氨苄西林', value: '90.9' },
                    { name: '高浓度庆大霉素', value: '37.0' },
                    { name: '左氧氟沙星', value: '86.5' }
                ]
            }
        ]

    },
    '成人非脑膜炎肺炎链球菌（青霉素）': {
        title: '976株成人非脑膜炎肺炎链球菌对青霉素的敏感性（%）(2018年)',
        //legend: ['PSSP(904株)', 'PISP(37株)', 'PRSP(35株)'],
        legend: ['2016年', '2017年', '2018年'],
        data: [
            {
                name: '2016年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(904株)', value: '94.0', count: '268' },
                    { name: 'PISP(37株)', value: '4.2', count: '12' },
                    { name: 'PRSP(35株)', value: '1.8', count: '5' }
                ]
            },
            {
                name: '2017年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(904株)', value: '89.4', count: '295' },
                    { name: 'PISP(37株)', value: '4.2', count: '14' },
                    { name: 'PRSP(35株)', value: '6.4', count: '21' }
                ]
            },
            {
                name: '2018年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(904株)', value: '94.5', count: '341' },
                    { name: 'PISP(37株)', value: '3.0', count: '11' },
                    { name: 'PISP(35株)', value: '2.5', count: '9' }
                ]
            }
        ]
    },
    '儿童非脑膜炎肺炎链球菌（青霉素）': {
        title: '3356株儿童非脑膜炎肺炎链球菌对青霉素的敏感性（%）(2018年)',
        //legend: ['PSSP(2751株)', 'PISP(354株)', 'PRSP(251株)'],
        legend: ['2016年','2017年','2018年'],
        data: [
            {
                name: '2016年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(2751株)', value: '77.4', count: '860' },
                    { name: 'PISP(354株)', value: '13.2', count: '147' },
                    { name: 'PRSP(251株)', value: '9.4', count: '104' }
                ]
            },
            {
                name: '2017年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(2751株)', value: '84.3', count: '878' },
                    { name: 'PISP(354株)', value: '7.8', count: '81' },
                    { name: 'PRSP(251株)', value: '7.9', count: '82' }
                ]
            },
            {
                name: '2018年',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: 'PSSP(2751株)', value: '84.1', count: '1013' },
                    { name: 'PISP(354株)', value: '10.5', count: '126' },
                    { name: 'PRSP(251株)', value: '5.4', count: '65' }
                ]
            }
        ]
    },
    '非脑膜炎肺炎链球菌分离株（成人）': {
        title: '341株成人非脑膜炎肺炎链球菌分离株对抗菌药物的耐药率（%）(2018年)',
        legend: [],
        data: [
            {
                name: 'PSSP(341株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '青霉素', value: '0.0' },
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '莫西沙星', value: '1.6' },
                    { name: '左氧氟沙星', value: '3.2' },
                    { name: 'SMZ-TMP', value: '54.7' },
                    { name: '克林霉素', value: '80.5' },
                    { name: '红霉素', value: '88.9' }
                ]
            }
        ]
    },
    '非脑膜炎肺炎链球菌分离株（儿童）': {
        title: '1204株儿童非脑膜炎肺炎链球菌分离株对抗菌药物的耐药率（%）(2018年)',
        legend: ['PSSP(1013株)', 'PISP(126株)', 'PRSP(65株)'],
        data: [
            {
                name: 'PSSP(n=1013株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '莫西沙星', value: '0.0' },
                    { name: '左氧氟沙星', value: '0.0' },
                    { name: '青霉素', value: '0.0' },
                    { name: 'SMZ-TMP', value: '74.5' },
                    { name: '克林霉素', value: '97.5' },
                    { name: '红霉素', value: '98.5' }
                ]
            },
            {
                name: 'PISP(126株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '莫西沙星', value: '0.0' },
                    { name: '左氧氟沙星', value: '0.0' },
                    { name: '青霉素', value: '0.0' },
                    { name: 'SMZ-TMP', value: '93.6' },
                    { name: '克林霉素', value: '96.0' },
                    { name: '红霉素', value: '98.4' }
                ]
            },
            {
                name: 'PRSP(65株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '莫西沙星', value: '1.8' },
                    { name: '左氧氟沙星', value: '3.2' },
                    { name: '青霉素', value: '100.0' },
                    { name: 'SMZ-TMP', value: '96.5' },
                    { name: '克林霉素', value: '92.1' },
                    { name: '红霉素', value: '100.0' }
                ]
            }
        ]
    },
    'β-溶血链球菌': {
        title: 'β-溶血链球菌对抗菌药物的耐药率（%）(2018年)',
        legend: ['A组(n=3531株)', 'B组(n=2859株)', 'C组(n=327株)'],
        data: [
            {
                name: 'A组(n=3531株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '青霉素', value: '0.0' },
                    { name: '头孢曲松', value: '0.0' },
                    { name: '左氧氟沙星', value: '0.2' },
                    { name: '克林霉素', value: '92.8' },
                    { name: '红霉素', value: '95.0' }
                ]
            },
            {
                name: 'B组(n=2859株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '青霉素', value: '0.0' },
                    { name: '头孢曲松', value: '0.0' },
                    { name: '左氧氟沙星', value: '38.8' },
                    { name: '克林霉素', value: '44.4' },
                    { name: '红霉素', value: '62.7' }
                ]
            },
            {
                name: 'C组(n=327株)',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '青霉素', value: '0.0' },
                    { name: '头孢曲松', value: '0.0' },
                    { name: '左氧氟沙星', value: '4.0' },
                    { name: '克林霉素', value: '54.1' },
                    { name: '红霉素', value: '61.9' }
                ]
            }
        ]
    },
    '草绿色链球菌': {
        title: '646株草绿色链球菌对抗菌药物的耐药率（%）(2018年)',
        legend: [],
        data: [
            {
                name: '草绿色链球菌',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '万古霉素', value: '0.0' },
                    { name: '利奈唑胺', value: '0.0' },
                    { name: '青霉素', value: '5.8' },
                    { name: '头孢曲松', value: '12.0' },
                    { name: '左氧氟沙星', value: '14.5' },
                    { name: '克林霉素', value: '38.9' },
                    { name: '红霉素', value: '49.1' }
                ]
            }
        ]
    }
};

var dataTable = {
    "白念珠菌":
    {
        type: 'table',
        title: '204株白念珠菌对抗真菌药物的敏感性',
        data: [
            { Name: '两性霉素 B', MIC_Range: '≤0.12-1', MIC50: '0.25', MIC90: '0.5', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '氟康唑', MIC_Range: '≤0.12-32', MIC50: '0.5', MIC90: '1', S: '94.6', SDD: '3.92', I: '-', R: '1.96' },
            { Name: '伏立康唑', MIC_Range: '≤0.008-8', MIC50: '≤0.008', MIC90: '0.03', S: '94.6', SDD: '-', I: '3.92', R: '1.96' },
            { Name: '泊沙康唑', MIC_Range: '≤0.008-1', MIC50: '0.03', MIC90: '0.06', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '伊曲康唑', MIC_Range: '≤0.015-0.5', MIC50: '0.06', MIC90: '0.12', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '卡泊芬净', MIC_Range: '0.015-0.5', MIC50: '0.06', MIC90: '0.12', S: '99.51', SDD: '-', I: '0.49', R: '-' },
            { Name: '米卡芬净', MIC_Range: '≤0.008-0.5', MIC50: '0.015', MIC90: '0.015', S: '99.02', SDD: '-', I: '0.98', R: '-' },
            { Name: '阿尼芬净', MIC_Range: '≤0.015-2', MIC50: '0.06', MIC90: '0.12', S: '99.51', SDD: '-', I: '-', R: '0.49' },
            { Name: '氟胞嘧啶', MIC_Range: '≤0.06-64', MIC50: '0.12', MIC90: '0.12', S: '-', SDD: '-', I: '-', R: '-' }
        ]
    },
    "近平滑念珠菌": {
        type: 'table',
        title: '79株近平滑念珠菌对抗真菌药物的敏感性',
        data: [
            { Name: '两性霉素 B', MIC_Range: '≤0.12-1', MIC50: '0.5', MIC90: '0.5', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '氟康唑', MIC_Range: '0.5-64', MIC50: '0.5', MIC90: '2', S: '91.14', SDD: '3.8', I: '-', R: '5.06' },
            { Name: '伏立康唑', MIC_Range: '≤0.008-2', MIC50: '≤0.015', MIC90: '0.06', S: '92.41', SDD: '-', I: '5.06', R: '2.53' },
            { Name: '泊沙康唑', MIC_Range: '0.015-0.5', MIC50: '0.06', MIC90: '0.12', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '伊曲康唑', MIC_Range: '0.03-0.5', MIC50: '0.12', MIC90: '0.12', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '卡泊芬净', MIC_Range: '0.12-4', MIC50: '0.5', MIC90: '1', S: '98.73', SDD: '-', I: '1.27', R: '-' },
            { Name: '米卡芬净', MIC_Range: '0.06-2', MIC50: '1', MIC90: '2', S: '100.0', SDD: '-', I: '-', R: '-' },
            { Name: '阿尼芬净', MIC_Range: '0.12-2', MIC50: '1', MIC90: '2', S: '100.0', SDD: '-', I: '-', R: '-' },
            { Name: '氟胞嘧啶', MIC_Range: '≤0.06-1', MIC50: '0.12', MIC90: '0.25', S: '-', SDD: '-', I: '-', R: '-' }
        ]
    },
    "热带念珠菌": {
        type: 'table',
        title: '51株热带念珠菌对抗真菌药物的敏感性',
        data: [
            { Name: '两性霉素 B', MIC_Range: '0.25-1', MIC50: '0.5', MIC90: '1', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '氟康唑', MIC_Range: '0.25-256', MIC50: '2', MIC90: '256', S: '56.86', SDD: '15.69', I: '-', R: '27.45' },
            { Name: '伏立康唑', MIC_Range: '≤0.008-8', MIC50: '0.25', MIC90: '8', S: '39.22', SDD: '-', I: '35.29', R: '25.49' },
            { Name: '泊沙康唑', MIC_Range: '0.03-4', MIC50: '0.25', MIC90: '1', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '伊曲康唑', MIC_Range: '0.03-2', MIC50: '0.25', MIC90: '1', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '卡泊芬净', MIC_Range: '0.03-0.5', MIC50: '0.12', MIC90: '0.25', S: '98.04', SDD: '-', I: '1.96', R: '-' },
            { Name: '米卡芬净', MIC_Range: '0.03-1', MIC50: '0.06', MIC90: '0.03', S: '98.04', SDD: '-', I: '-', R: '1.96' },
            { Name: '阿尼芬净', MIC_Range: '≤0.015-2', MIC50: '0.12', MIC90: '0.25', S: '98.04', SDD: '-', I: '-', R: '1.96' },
            { Name: '氟胞嘧啶', MIC_Range: '≤0.06-0.25', MIC50: '≤0.06', MIC90: '0.12', S: '-', SDD: '-', I: '-', R: '-' }
        ]
    },
    "光滑念珠菌": {
        type: 'table',
        title: '52株光滑念珠菌对抗真菌药物的敏感性',
        data: [
            { Name: '两性霉素 B', MIC_Range: '0.5', MIC50: '1', MIC90: '-', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '氟康唑', MIC_Range: '1-128', MIC50: '8', MIC90: '16', S: '-', SDD: '98.08', I: '-', R: '1.92' },
            { Name: '伏立康唑', MIC_Range: '0.03-1', MIC50: '0.25', MIC90: '0.5', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '泊沙康唑', MIC_Range: '0.03-2', MIC50: '1', MIC90: '2', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '伊曲康唑', MIC_Range: '0.12-1', MIC50: '0.5', MIC90: '1', S: '-', SDD: '-', I: '-', R: '-' },
            { Name: '卡泊芬净', MIC_Range: '0.03-0.25', MIC50: '0.12', MIC90: '0.25', S: '90.38', SDD: '-', I: '9.62', R: '-' },
            { Name: '米卡芬净', MIC_Range: '≤0.008-0.03', MIC50: '0.015', MIC90: '0.015', S: '100', SDD: '-', I: '-', R: '-' },
            { Name: '阿尼芬净', MIC_Range: '≤0.015-0.12', MIC50: '0.03', MIC90: '0.06', S: '100', SDD: '-', I: '-', R: '-' },
            { Name: '氟胞嘧啶', MIC_Range: '≤0.06-0.12', MIC50: '≤0.06', MIC90: '≤0.06', S: '-', SDD: '-', I: '-', R: '-' }
        ]
    },
    '标本分布': {
        type: 'bar',
        title: '标本分布',
        legend: [],
        data: [
            {
                name: '标本分布',
                type: 'bar',
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: [
                    { name: '组织', value: '1.94' },
                    { name: '穿刺液', value: '3.01' },
                    { name: '透析液', value: '1.51' },
                    { name: '其他', value: '0.65' },
                    { name: '脓液', value: '4.30' },
                    { name: '分泌物', value: '5.81' },
                    { name: 'CSF', value: '4.73' },
                    { name: '导管', value: '7.96' },
                    { name: 'BALF', value: '6.02' },
                    { name: '引流液', value: '8.39' },
                    { name: '胆汁', value: '7.74' },
                    { name: '胸腹水', value: '8.60' },
                    { name: '血培养', value: '38.28' }
                ]
            }
        ]
    }
};

var mainChart,
    defaultOption = {
        color: ['#015BAA', '#C1232B', '#FE8463', '#ECBF00', '#CF7CA6'],
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
        },
        title: {
            show: true,
            subtext: '',
            left: 'center',

            textStyle: {
                fontWeight: 'normal'
            }
        },
        legend: {
            top: '35px'
        },
        grid: {
            top: '80px;',
            left: '0',
            right: '0',
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
        xAxis: {
            type: 'category',
            axisLabel: {
                interval: 0,
                rotate: 0,
                margin: 10,
                textStyle: {
                    fontSize: "14px"
                }
            },
            axisTick: {
                show: false,
                alignWithLabel: true
            }
        },
        yAxis: [
            {
                show: true,
                type: 'value',
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    formatter: '{value} %'
                }
            }
        ]
    },
    mainOption;

$(function () {

    $('#data_item_group').on('click', 'a[data-key]', function () {
        var $that = $(this);
        $('#data_item_group li, #fungus_item_group li, #specimen_item_group li').removeClass('active');
        $that.closest('li').addClass('active');
        var key = $that.data('key'),
            source = $that.data('source');
        if (key) {

            if (source === '1' || source === 1) {
                var item = dataConfig[key],
                    xAxisData = [];
                item.data[0].data.forEach(function (value, i) {
                    if (xAxisData.indexOf(value.name) === -1) {
                        xAxisData.push(value.name);
                    }
                });

                $('#dataError, #fungus-container, #data-grid-chart, #data-grid, #data-grid-intro').hide();
                $('#mainChart').show();
                loadChart('mainChart', {
                    title: {
                        text: item.title
                    },
                    legend: {
                        data: item.legend
                    },
                    xAxis: {
                        show: item.data[0].type === 'bar' || item.data[0].type === 'line',
                        data: xAxisData,
                        axisLabel: {
                            rotate: getRotate(xAxisData)
                        }
                    },
                    yAxis: [
                        {
                            show: item.data[0].type === 'bar' || item.data[0].type === 'line',
                            min: 0
                        }
                    ],
                    series: item.data
                });
            }
            else {
                return false;
            }
        }
    });

    $('#fungus_item_group, #specimen_item_group').on('click', 'a[data-key]', function () {
        var $that = $(this);
        $('#data_item_group li, #fungus_item_group li, #specimen_item_group li').removeClass('active');
        $that.closest('li').addClass('active');
        var key = $that.data('key'),
            source = $that.data('source');
        if (key && source === 'table') {
            var gridData = dataTable[key].data;
            $('#dataError, #mainChart, #data-grid-chart').hide();
            $('#fungus-container, #data-grid, #data-grid-intro').show();
            var dataDisplayGrid = $("#data-grid").data("kendoGrid");
            if (dataDisplayGrid) {
                // 存在则销毁
                $('#data-grid').kendoGrid('destroy').empty();
            }
            $("#data-grid").kendoGrid({
                dataSource: gridData,
                scrollable: false,
                groupable: false,
                toolbar: dataTable[key].title,
                columns: [
                    { field: "Name", title: "抗真菌药物", width: 350, headerAttributes: { style: 'font-weight: bold;' } },
                    { field: "MIC_Range", title: "MIC范围", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "MIC50", title: "MIC<sub>50</sub>", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "MIC90", title: "MIC<sub>90</sub>", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "S", title: "敏感", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "SDD", title: "剂量依赖敏感", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "I", title: "中介", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } },
                    { field: "R", title: "耐药", width: 150, headerAttributes: { style: 'text-align: center; font-weight: bold;' }, attributes: { style: 'text-align: center;' } }
                ]
            });
        }
        else if (key && source === 'bar') {
            $('#dataError, #mainChart, #data-grid, #data-grid-intro').hide();
            $('#fungus-container, #data-grid-chart').show();
            var item = dataTable[key],
                xAxisData = [];
            item.data[0].data.forEach(function (value, i) {
                if (xAxisData.indexOf(value.name) === -1) {
                    xAxisData.push(value.name);
                }
            });
            loadChart('data-grid-chart', {
                title: {
                    text: item.title
                },
                legend: {
                    data: item.legend
                },
                xAxis: {
                    show: item.data[0].type === 'bar' || item.data[0].type === 'line',
                    data: xAxisData,
                    axisLabel: {
                        rotate: getRotate(xAxisData)
                    }
                },
                yAxis: [
                    {
                        show: item.data[0].type === 'bar' || item.data[0].type === 'line',
                        min: 0
                    }
                ],
                series: item.data
            });
        }
        else {
            return false;
        }
    });

    init();

    function init() {
        var data_items = [];
        for (var key in dataConfig) {
            data_items.push(key);
        }
        $('#data_item_group').html(template('template_data_item', { data: data_items }));

        var data_table = [],
            specimen_data = [];
        for (var tr in dataTable) {
            if (dataTable[tr].type === 'table') {
                data_table.push({ name: tr, type: dataTable[tr].type });
            }
            else if (dataTable[tr].type === 'bar') {
                specimen_data.push({ name: tr, type: dataTable[tr].type });
            }
        }
        $('#fungus_item_group').html(template('template_data_item_table', { data: data_table }));

        $('#specimen_item_group').html(template('template_data_item_table', { data: specimen_data }));

        // 默认选中第一个
        $('#data_item_group > li:first > a').click();
    }

    function loadChart(chartdom, option) {
        if (!mainChart) {
            echarts.dispose(mainChart);
        }
        mainChart = echarts.init(document.getElementById(chartdom));
        mainOption = {};
        mainChart.clear();
        mainChart.resize();
        mainChart.showLoading({
            text: '数据加载中,请稍等...',
            color: '#3c8dbc',
            textColor: '#000',
            maskColor: 'rgba(255, 255, 255, 0.8)',
            zlevel: 0
        });
        //if (option.legend.data.length > 4 && option.xAxis.data.length > 10 && option.series[0].type !== 'pie') {
        //    mainOption = $.extend(true, {}, defaultOption, option, {
        //        dataZoom: [
        //            {
        //                show: true,
        //                start: 0,
        //                end: 35
        //            },
        //            {
        //                type: 'inside',
        //                start: 0,
        //                end: 35
        //            }
        //        ]
        //    });
        //}
        //else if (option.legend.data.length >= 4 && option.xAxis.data.length >= 8 && option.series[0].type !== 'pie') {
        //    mainOption = $.extend(true, {}, defaultOption, option, {
        //        dataZoom: [
        //            {
        //                show: true,
        //                start: 0,
        //                end: 60
        //            },
        //            {
        //                type: 'inside',
        //                start: 0,
        //                end: 60
        //            }
        //        ]
        //    });
        //}
        //else if (option.legend.data.length >= 2 && option.xAxis.data.length >= 15 && option.series[0].type !== 'pie') {
        //    mainOption = $.extend(true, {}, defaultOption, option, {
        //        dataZoom: [
        //            {
        //                show: true,
        //                start: 0,
        //                end: 60
        //            },
        //            {
        //                type: 'inside',
        //                start: 0,
        //                end: 60
        //            }
        //        ]
        //    });
        //}
        //else if (option.series[0].type === 'pie') {
        //    mainOption = $.extend(true, {}, defaultOption, option, {
        //        tooltip: {
        //            trigger: 'item',
        //            formatter: "{a} <br/>{b} : {c} ({d}%)"
        //        }
        //    });
        //}
        //else {
        //    mainOption = $.extend(true, {}, defaultOption, option);
        //}

        mainOption = $.extend(true, {}, defaultOption, option);

        if (mainOption.legend.data.length === 0) {
            mainOption.grid.top = '50px';
        }

        mainChart.resize();
        mainChart.setOption(mainOption);
        setTimeout(function () {
            mainChart.hideLoading();
        }, 300);
    }

    function getRotate(xAxisData) {
        if (xAxisData.length > 10 && xAxisData.length <= 12) {
            return 30;
        }

        if (xAxisData.length > 13 && xAxisData.length <= 18) {
            return 60;
        }

        if (xAxisData.length > 19) {
            return 90;
        }

        return 0;
    }

    ClearSelectType = function () {
        $('#dataError').show();
        $('#data_item_group li').removeClass('active');
        $('#mainChart, #fungus-container, #data-grid, #data-grid-intro, #data-grid-chart').hide();
        if (!mainChart) {
            echarts.dispose(mainChart);
        }
        mainOption = {};
        //mainChart.clear();
    };


    ShowItemContent = function (dom) {
        var $itemContent = $(dom),
            $img = $(event.target);
        if ($itemContent.data('status') === '1') {
            $itemContent.slideDown(200, function () {
                $itemContent.data('status', '0');
                $img.attr('src', '/Content/Images/jiantou-xia.png');
            });
        }
        else {
            $itemContent.slideUp(200, function () {
                $itemContent.data('status', '1');
                $img.attr('src', '/Content/Images/jiantou-shang.png');
         
            });
        }
    };
});


//function ShowItemContent(dom) {
//    var $itemContent = $(dom),
//        $img = $(event.target);
//    if ($itemContent.data('status') === '1') {
//        $itemContent.slideDown(200, function () {
//            $itemContent.data('status', '0');
//            $img.attr('src', '/Content/Images/jiantou-shang.png');
//        });
//    }
//    else {
//        $itemContent.slideUp(200, function () {
//            $itemContent.data('status', '1');
//            $img.attr('src', '/Content/Images/jiantou-xia.png');
//        });
//    }
//}

