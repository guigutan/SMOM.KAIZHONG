/**
 * 生命周期，调整form控件的宽度
 * @class SIE.Web.MES.TaskManagement.Reports.ReportRecordBehavior
 * @constructor
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.ReportRecordBehavior', {    
    onCreated: function (view) {
        var formField = view.getControl().query('textfield');
        formField.forEach(function(p) {
            p.labelWidth = 120;          
        });
        var areaField = view.getControl().query('textareafield')[0];
        areaField.width = '90%';
        view.getControl().bodyBorder = 0;
        view.getControl().border = 0;
    }
});

