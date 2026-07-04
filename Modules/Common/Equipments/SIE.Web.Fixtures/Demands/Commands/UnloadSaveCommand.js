SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.UnloadSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式

    /**
     * @override 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var result = view.getData().isDirty();
        if (view.getCurrent()) {
            var items = view.getData().data.items;
            if (items != null && items.length > 0) {
                for (var i = 0; i < items.length; i++) {
                    if (items[i].data.IsOld == 0) {
                        return true;
                    }
                }
                return false;
            }
        }
        return result;        
    },

    /**
    * @override 视图数据提交保存回调处理
    * @param view 当前视图
    */
    doSave: function (view) {
        var me = this;
        var data = {};
        data.RestUnloadVMList = [];
        this.view.getData().data.items.forEach(function (item) {
            data.RestUnloadVMList.push(item.getData());
        });

        var parentData = this.view.getParent().getData().data;
        if (parentData) {
            data.DemandId = parentData.Id;
        }

        //电子签名信息
        var commandInfo = {
            command: "SIE.Web.Fixtures.Demands.Commands.UnloadSaveCommand",
            entityType: "SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel",
            parentType: "SIE.Fixtures.FixtureDemands.FixtureDemand",
            moduleName: "工治具需求清单",
            childModuleName: "",
            commandName: "保存",
        }

        var indata = {};
        indata.Data = Ext.encode(data);

        view.execute({
            data: indata,
            logInfo: commandInfo,
            success: function (res) {
                if (res.Result !== '') {
                    SIE.Msg.showError(res.Result);
                    return;
                }
                else {
                    me.onSaved(view, res);
                }

            },
        });
    },
 /**
     * @override 视图数据提交保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        this.callParent(arguments);
        view.getParent().loadChildData;
        var stockView = view.getParent().findChild('SIE.Fixtures.FixtureDemands.ViewModels.UnloadStockViewModel');
        stockView.getData().getData().items.forEach(function (childEntity) {
            childEntity.markSaved();
        });

        view.getData().getData().items.forEach(function (childEntity) {
            childEntity.markSaved();
        });
        view.getParent().getCurrent().dirty = false;
        view.getParent().getCurrent().isPhantom = false;
        view.syncCmdState();
        CRT.Workbench.closeCurrentTab();
        CRT.Event.fire("SIE.Fixtures.FixtureDemands.FixtureDemand_refresh");
    },
});