SIE.defineCommand('SIE.Web.MES.PrepareProducts.Commands.PrepareProductExportCommand', {
    meta: { text: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    _view: null,
    execute: function (view) {
        var me = this;
        me._view = view;
        var record = view._relations[0]._target.getCurrent();
        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        delete record.data["IS_PHANTOM"];
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
            type: 'SIE.Web.MES.PrepareProducts.DataQuerys.PrepareProductDataQuery',
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
                    SIE.Web.MES.PrepareProducts.Scripts.CommonFuns.tablesToExcel(div.children, "产品产前准备设置表");
                    document.body.removeChild(div)
                }
            }
        });
    },
});


