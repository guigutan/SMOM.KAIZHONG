Ext.define('SIE.Items.Items.CategoryType', {
    statics: {
        Item: { value: 0, text: 'Item', label: '库存类别' },
        Quality: { value: 1, text: 'Quality', label: '质量类别' },
        Kit: { value: 2, text: 'Kit', label: '齐套类别' },
    }
});


Ext.define('SIE.Items.ItemType', {
    statics: {
        Product: { value: 0, text: 'Product', label: '成品' },
        Material: { value: 1, text: 'Material', label: '原材料' },
        SemiFinished: { value: 2, text: 'SemiFinished', label: '半成品' },
    }
});