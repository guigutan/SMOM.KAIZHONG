SIE.defineCommand('SIE.Web.EMS.AssetDisposals.Commands.ImportAssetDisposalSparePartCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    onSuccessImported: function (me, res) {
        if (!me.view.getParent()) {
            me.view.reloadData();
        }
        else if (me._ownerView != null && me.view.getParent().getCurrent() != null) {
            me._ownerView.loadChildData(true)
        }
        Ext.MessageBox.close();
        if (res.Result.LoadSucessQty === 0 && res.Result.LoadFailQty === 0)
            res.Result.MessageList.push({ RowNum: 0, MsgType: 1, Message: "导入的文件为空！" })
        me.showInfoWin(res.Result);
    }
});