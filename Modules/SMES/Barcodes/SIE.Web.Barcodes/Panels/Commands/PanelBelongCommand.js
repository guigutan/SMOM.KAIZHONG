SIE.defineCommand('SIE.Web.Barcodes.Panels.Commands.PanelBelongCommand', {
    meta: { text: "拼板码归属", group: "edit" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        var selecteditems = view.getSelection();
        if (selecteditems != null && selecteditems.length > 1)
            return false;
        if (entity !== null && entity.data && entity.data.IsScrap === false)
            return true;
        return false;
    },
    execute: function (view, source) {
        var me = this;
        me.view = view;
        var indata = view.getCurrent().getData();
        SIE.AutoUI.getMeta({
            model: "SIE.Barcodes.Panels.ViewModels.PanelBelongViewModel",
            module: 'SIE.Barcodes.Panels.PanelWorkOrder,SIE.Barcodes',
            ingoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.generateAggtControl(res);
                var entity = new SIE.Barcodes.Panels.ViewModels.PanelBelongViewModel();
                entity.setOrgWorkOrderId(indata.WorkOrderId);
                entity.setOrgWorkOrderNo(indata.WONo);
                entity.setSn(indata.Code);
                entity.setPanelId(indata.Id);
                detailView._view.setData(entity);
                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "拼板码归属".t(),
                    width: 370,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var currentView = detailView._view;
                            var barBelongVM = currentView.getCurrent().getData();
                            var result = me.validateData(indata, barBelongVM);
                            if (!result)
                                return false;

                            me.view.execute({
                                data: barBelongVM,
                                success: function (res) {
                                    var errMsg = res.Result;
                                    if (errMsg == '拼板码归属成功'.t())
                                        view.getParent().reloadData();   
                                    SIE.Msg.showMessage(errMsg);
                                }
                            });
                        }
                    }
                });
            }
        });
    },

    /**
   * validateData 验证拼板码归属是否合法
   * @method validateData
   * @param {me} me 当前页面
   */
    validateData: function (indata, barBelongVM) {
        if (indata.IsScrap) {
            SIE.Msg.showError("拼板码已报废，拼板码归属失败！".t());
            return false;
        }       
        if (barBelongVM.WorkOrderId <= 0) {
            SIE.Msg.showError("归属工单必填，拼板码归属失败！".t());
            return false;
        }

        var restQty = barBelongVM.PlanQty - barBelongVM.PanelPrintQty;
        if (restQty <= 0) {
            SIE.Msg.showError("归属工单的剩余数量为0，拼板码归属失败！".t());
            return false;
        }
        return true;
    },
})