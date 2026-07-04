SIE.defineCommand('SIE.Web.MES.PrepareProducts.Commands.PrepareRecordExportCommand', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    _view: null,
    canExecute: function(view){
        var flag = false;
        if (view._parent.getCurrent()) {
            flag = true;
        }
        return flag;
    },
    execute: function (view) {
        var me = this;
        me._view = view;
        var record = view._parent.getCurrent();
        var istrue = true;
        Ext.MessageBox.show({
            msg: '正在导出数据'.t(),
            progressText: '...',
            width: 300,
            wait: {
                interval: 200
            }
        });
        SIE.invokeDataQuery({
            method: 'ExportPrepareProduct',
            params: [record.data],
            action: 'queryer',
            type: 'SIE.Web.MES.PrepareProducts.DataQuerys.PrepareRecordDataQuery',
            token: view.getToken(),
            success: function (res) {
                var exportData = res.Result;
                if (exportData == "") {
                    SIE.Msg.showMessage("没有可导出的数据".t());
                }
                else {
                    var div = document.createElement("DIV");
                    document.body.appendChild(div);
                    div.innerHTML = exportData;
                    div.style.display = "none";
                    var l = div.children.length;
                    SIE.Web.MES.PrepareProducts.Scripts.CommonFuns.tablesToExcel(div.children, "产前准备记录明细表");
                    document.body.removeChild(div)
                }
            }
        });
    },
});



