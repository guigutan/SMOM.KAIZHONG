Ext.define('SIE.Inventory.Strategy.StrategyType', {
    statics: {
        Strategy01: { value: 10, text: 'Strategy01', label: '01.根据来源库位上架到目标库位' },
        Strategy02: { value: 20, text: 'Strategy02', label: '02.根据来源库位寻找目标库区的库位上架' },
        Strategy03: { value: 30, text: 'Strategy03', label: '03.直接寻找目标库区的库位上架' },
        Strategy04: { value: 40, text: 'Strategy04', label: '04.直接上架到目标库位' },
        Strategy05: { value: 50, text: 'Strategy05', label: '05.寻找物料档案指定库区的库位上架' },
        Strategy06: { value: 60, text: 'Strategy06', label: '06.直接上架到物料档案指定的库位' },
        Strategy07: { value: 70, text: 'Strategy07', label: '07.直接寻找目标逻辑分区的库位上架' },
        Strategy08: { value: 80, text: 'Strategy08', label: '08.根据来源逻辑分区寻找目标逻辑分区的库位上架' },
        Strategy09: { value: 90, text: 'Strategy09', label: '09.直接上架到目标站台' },
        Strategy10: { value: 100, text: 'Strategy10', label: '10.直接上架到目标站台组' },
    }
});

Ext.define('SIE.Inventory.Strategy.SceneType', {
    statics: {
        NotASRS: { value: 1, text: 'NotASRS', label: '非立库' },
        ASRS: { value: 2, text: 'ASRS', label: '立库' }
    }
});