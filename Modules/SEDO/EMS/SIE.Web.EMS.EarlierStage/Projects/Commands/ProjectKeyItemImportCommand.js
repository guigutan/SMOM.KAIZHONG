SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectKeyItemImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    onSuccessImported: function (me, res) {

        if (!me.mainview.getParent()) {
            me.mainview.reloadData();
        }
        else if (me.mainview != null && me.mainview.getParent() != null) {
            //项目事项导入时，会更新项目的预算总额，所以要刷新项目列表
            //me.mainview.loadChildData(true);
            me.mainview.getParent().reloadData();
        }

        Ext.MessageBox.close();
        me.showInfoWin(res.Result);
    },
});