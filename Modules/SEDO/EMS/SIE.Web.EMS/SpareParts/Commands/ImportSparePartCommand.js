SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.ImportSparePartCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    canVisible: function (view, source) {
        var isWmsControl = CRT.Context.PageContext.getContext('IsWmsControl');
        if (isWmsControl) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {

        var me = this;
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.SpareParts.DataQuery.SparePartDataQueryer",
            method: "VerifyIsWmsControl",
            params: [],
            async: false,
            token: view.token,
            callback: function (res) {
                if (res.Result) {
                    SIE.Msg.showError('备件启用了WMS管控，不能通过导入添加备件！'.t());
                    return false;
                }
                else {
                    me.mainview = view;
                    var btnFile = Ext.create('Ext.form.field.FileButton', { renderTo: Ext.getBody(), hidden: true, accept: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel' });
                    btnFile.on("change", me.buttonChange, me);
                    btnFile.fileInputEl.dom.click();
                }
            }
        });
    },
});