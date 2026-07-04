SIE.defineCommand('SIE.Web.ERPInterface.InventoryControl.Commands.InventoryControlQueryCommand', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    /**
     * @property {Boolean}
     * 是否允许查询，反正恶意查询 
     */
    allow: true,

    /**
     * @property {Boolean}
     * 是否已经注册数据加载完成事件
     */
    register: false,
    canExecute: function (view, source) {
        var current = view.getCurrent();
        return this.allow && current != null;
    },

    /**
     * 判断查询方法能否执行
     * @param view 查询逻辑视图
     * @returns 能执行返回true，否则返回false
     */
    execute: function (view) {
        var me = this;
        setTimeout(function () {
            SIE.Msg.wait("查询数据中".L10N());
        }, 0)
        try {
            me.allow = false;
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
            var mainView = view.getResultView();
            if (mainView) {
                var layout = me.view;
                if (layout) {
                    //layout.loadReportData(record.data, mainView.token);
                    me.allow = true;
                    var filter = {
                        Method: 'SearchInventoryControlData',
                        Parameters: [record.getItemCode(), record.getItemName(), record.getErpLotCode(), record.getWarehouseCode(), record.getErpWarehouseCode(), record.getIsShowDifferent(), record.getIsShowZero()]
                    };
                    filter = Ext.encode(filter);
                    setTimeout(function () {
                        SIE.invokeDataQuery({
                            async: false,
                            type: "SIE.Web.ERPInterface.InventoryControl.DataQueryer.InventoryControlDataQueryer",
                            method: 'SearchInventoryControlData',
                            token: mainView.token,
                            params: [record.getItemCode(), record.getItemName(), record.getErpLotCode(), record.getWarehouseCode(), record.getErpWarehouseCode(), record.getIsShowDifferent(), record.getIsShowZero()],
                            callback: function (res) {
                                if (res.Success) {
                                    //console.log(res.Result);
                                    SIE.Web.ERPInterface.InventoryControlLayout.setAllData(res.Result.ParentListData, res.Result.DetailListData, res.Result.ErpListData);
                                    SIE.Web.ERPInterface.InventoryControlLayout.setListData(view);
                                    SIE.Web.ERPInterface.InventoryControlLayout.ClearListData(view);
                                    //setTimeout(function () {
                                    //    SIE.Msg.hide();
                                    //},100)
                                    SIE.Msg.hide();
                                }
                            }
                        });
                    },100)
                    
                }
            }
        } catch (e) {
            me.allow = true;
            SIE.Msg.hide();
            throw e;
        }
    }
});