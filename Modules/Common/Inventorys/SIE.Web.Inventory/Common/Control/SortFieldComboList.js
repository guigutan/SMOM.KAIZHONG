Ext.define('SIE.Web.Inventory.Common.Control.SortFieldComboList', {
    extend: 'SIE.control.XComboBox',
    alias: 'widget.sortfieldcombolist',
    listeners: {
        select: function (combo, record, index) {
            var me = this;
            entity = combo.up().context.record;
            me.onSortFieldChanged(entity, record);
        }
    },
    onSortFieldChanged: function (entity, record) {
        var data = record.data;
        SIE.invokeCommand({
            async: false,
            cmd: "SIE.Web.Inventory.Strategy.Commands.TurnOverDefinitionChangeCommand",
            data: { Data: data.value },
            token: $view.token,
            callback: function (res) {
                var data = res.Result;
                entity.setFieldType(data.FieldType);
                entity.setSortType(data.SortType);
                entity.setEqualValue(data.EqualValue);
                entity.setLowerLimit(data.LowerLimit);
                entity.setUpperLimit(data.UpperLimit);
                entity.setLowerLimitDay(data.LowerLimitDay);
                entity.setUpperLimitDay(data.UpperLimitDay);
            }
        });
    }
});