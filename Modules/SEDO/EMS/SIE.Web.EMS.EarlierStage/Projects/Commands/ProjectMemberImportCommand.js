SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.ProjectMemberImportCommand', {
    extend: 'SIE.Web.Common.Import.Commands.ImportExcelCommand',
    meta: { text: "导入", group: "business", iconCls: "icon-Upload icon-green" },
    onSuccessImported: function (me, res) {
        if (!me.view.getParent()) {
            me.view.reloadData();
        }
        else if (me._ownerView != null && me.view.getParent().getCurrent() != null) {
            me._ownerView.loadChildData(true);
        }
        Ext.MessageBox.close();
        SIE.each(res.Result.MessageList, function (list) {
            if (list.Message.indexOf("读取列[项目编码]失败，找不到值为") === 0) {
                list.Message = "用户没有此项目权限或者此项目不存在".t();
            }
        });
        me.showInfoWin(res.Result);
    }
});