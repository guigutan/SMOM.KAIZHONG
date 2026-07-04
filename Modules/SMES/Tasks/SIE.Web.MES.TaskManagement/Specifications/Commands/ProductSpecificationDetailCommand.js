SIE.defineCommand('SIE.Web.MES.TaskManagement.Specifications.Commands.ProductSpecificationDetailCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    onSuccessImported: function (me, res) {
        Ext.MessageBox.close();
        if (res.Result.LoadSucessQty === 0 && res.Result.LoadFailQty === 0)
            res.Result.MessageList.push({ RowNum: 0, MsgType: 1, Message: "导入的文件为空！" })
        me.showInfoWin(res.Result);
    }
});