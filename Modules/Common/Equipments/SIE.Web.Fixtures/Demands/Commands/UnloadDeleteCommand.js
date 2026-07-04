SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.UnloadDeleteCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit" },

    /**
     * @override 是否可执行
     * @param {any} view
     */
    canExecute: function (view) {
        var selecteditems = view.getSelection();
        var canUse = false;
        if (selecteditems !== null && selecteditems.length > 0) {
            canUse = true;
            selecteditems.forEach(x => {
                if (x.getIsOld() === true) {
                    canUse = false;
                    return canUse;
                }
            });
            return canUse;
        }
        return canUse;
    },

    /**
     * @override 执行删除
     * @param {any} view
     * @param {source} source
     */
    execute: function (view, source) {
        var me = this;
        var sel = view.getSelection();
        SIE.Msg.askQuestion(Ext.String.format('你确定删除选择的{0}条数据吗？'.t(), sel.length), function () {
            view.removeSelection();
            var data = view.getData().data;
            if (data.length > 0) {
                //不选中一行，列表的tbar（如果命令过多）会变更位置
                view.getControl().setSelection(data.items[0]);
                view.setCurrent(data.items[0], true);
            } else {
                view.setCurrent(null, true);
            }

            var data = me.getUiData(view);
            me.deleteUnloads(view, data);
        });
    },

    /**
    * @getUiData 获取界面数据
    * @param {view} view
    */
    getUiData: function (view) {
        var data = {};
        data.DeleteUnloadVMList = [];
        data.RestUnloadVMList = [];
        view.getData().data.items.forEach(function (item) {
            data.RestUnloadVMList.push(item.getData());
        });

        var parentData = view.getParent().getData().data;
        if (parentData) {
            data.WarehouseId = parentData.WarehouseId;
        }

        var detailView = view.getParent().findChild('SIE.Fixtures.FixtureDemands.FixtureDemandDetail');
        if (detailView) {
            data.DemandDetail = detailView.getCurrent().getData();
        }

        return data;
    },

    /**
    * @deleteUnloads 执行删除
    * @param {view} view
    * @param {data} data
    */
    deleteUnloads: function (view, data) {
        //电子签名信息
        var commandInfo = {
            command: "SIE.Web.Fixtures.Demands.Commands.UnloadDeleteCommand",
            entityType: "SIE.Fixtures.FixtureDemands.ViewModels.FixtureUnloadViewModel",
            parentType: "SIE.Fixtures.FixtureDemands.FixtureDemand",
            moduleName: "工治具需求清单",
            childModuleName: "",
            commandName: "删除",
        }

        var indata = {};
        indata.Data = Ext.encode(data);

        view.execute({
            data: indata,
            logInfo: commandInfo,
            success: function (res) {
                var unloadInfo = res.Result;
                if (unloadInfo.ErrMsg !== '') {
                    SIE.Msg.showError(unloadInfo.ErrMsg);
                    return;
                }
                else {
                    
                    SIE.Web.Fixtures.Demands.Scripts.DemandCommonFun.loadStockInfo(view.getParent(), unloadInfo.UnloadStockVMList);
                }
            },
        });
    }
});