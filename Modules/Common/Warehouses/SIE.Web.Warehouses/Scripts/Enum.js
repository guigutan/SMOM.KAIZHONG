Ext.define('SIE.Warehouses.TemperatureType', {
    statics: {
        Normal: { value: 0, text: 'Normal', label: '常温' },
        Low: { value: 1, text: 'Low', label: '低温' },
        Cold: { value: 2, text: 'Cold', label: '冷藏' },
        Freezing: { value: 3, text: 'Freezing', label: '冷冻' },
        Custom: { value: 4, text: 'Custom', label: '自定义' },
    }
});

//SIE:classEnd
Ext.define('SIE.Warehouses.HumidityType', {
    statics: {
        Normal: { value: 0, text: 'Normal', label: '常湿' },
        Low: { value: 1, text: 'Low', label: '低湿' },
        Dry: { value: 2, text: 'Dry', label: '干燥' },
        Custom: { value: 3, text: 'Custom', label: '自定义' },
    }
});