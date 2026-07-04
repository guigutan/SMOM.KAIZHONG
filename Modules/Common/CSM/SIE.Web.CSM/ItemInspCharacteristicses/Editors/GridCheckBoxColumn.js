/**
 * 表格复选框列,设置物料周期检和确认检只能选其一
 */
Ext.define('SIE.Web.CSM.ItemInspCharacteristics.GridCheckBoxColumn', {
    extend: 'Ext.grid.column.Check',
    alias: 'widget.CSM_ItemInspCharacteristics_GridCheckBoxColumn',
    listeners: {
        checkchange: function (checkCol, rowIndex, checked, record, e, eOpts) {
            if (checkCol.dataIndex === "RecurringInspection") {   //物料周期检
                if (checked) {
                    record.setConfirmInspection(false);
                    record.setInspectionFree(false);
                }
            } else if (checkCol.dataIndex === "ConfirmInspection") {   //确认检
                if (checked) {
                    record.setRecurringInspection(false);
                    record.setInspectionFree(false);
                }
            } else if (checkCol.dataIndex === "InspectionFree") {   //免检
                if (checked) {
                    record.setConfirmInspection(false);
                    record.setRecurringInspection(false);
                }
            }

        },
        beforecheckchange: function (checkCol, rowIndex, checked, record, e, eOpts) {
            //供方状态禁用时只读
            return record.getSupplierState() === SIE.Domain.State.Enable.value;
        }
    }
});