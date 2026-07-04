SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.FileManageQuery', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    _view: null,
    execute: function (view) {
        var me = this;
        me._view = view;
        var record = view.getCurrent();
        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        var istrue = true;
        view.getControl().items.items.forEach(function (item) {
            if (!item.validate()) {
                istrue = false;
            }
        });
        SIE.invokeDataQuery({
            method: 'GetFileManageDatas',
            params: [record.data],
            action: 'queryer',
            type: 'SIE.Web.FMS.FileManageDataQueryer',
            token: view.getToken(),
            success: function (res) {
                var mainView = me._view._relations[0]._target;
                var gridControl = mainView.GridControl;             
                if (gridControl) {                    
                    var resData = res.Result;                     
                    gridControl.initStore(resData.gridData);
                    document.getElementById("topNav").innerHTML = "";
                    mainView.CurFolderId = null;
                }
            }
        });
    }
});