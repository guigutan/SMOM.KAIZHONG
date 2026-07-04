/*
 ** 输入格式设置
 */
Ext.define('SIE.Web.Inventory.Commom.InputFormatOptions', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.DCInputFormat',
    layout: {
        type: 'hbox',
    },
    style: "width:100%",
    fieldStyle: "width:100%",
    items: [
        {
            width: 100,
            xtype: 'radio',
            checked: true,
            hideLabel: true,
            boxLabel: '年周'.t(),
            name: 'YearWeek',
            bind: '{p.YearWeek}'
        },
        {
            width: 100,
            xtype: 'radio',
            hideLabel: true,
            boxLabel: '周年'.t(),
            name: 'YearWeek',
            bind: '{p.WeekYear}'
        },
        {
            width: 100,
            xtype: 'radio',
            hideLabel: true,
            boxLabel: '年月日'.t(),
            name: 'YearWeek',
            bind: '{p.YearMonthDay}'
        }
    ]
});