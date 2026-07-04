SIE.defineCommand("SIE.Web.LES.LinesideWarehouses.Commands.LinesideWarehouseAddCommand", {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    /*
     * 编辑的处理方法
     */
    onItemCreated: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },
    _onEntityPropertyChanged: function (e) {
        var data = e.entity;
        if (e.property == "WipResouceId") {
            SIE.invokeDataQuery({
                type: "SIE.Web.LES.LinesideWarehouses.LinesideWarehouseDataQueryer",
                method: "GetWipResourceInfo",
                params: [e.value],
                async: false,
                token: this.view.token,
                callback: function (res) {
                    if (res.Success) {
                        var info = res.Result;
                        if (info != null) {
                            data.setFactoryId_Display(info.FactoryName);
                            data.setFactoryId(info.FactoryId);
                            data.setWorkShopId_Display(info.WorkShopName);
                            data.setWorkShopId(info.WorkShopId);
                        }
                    }
                }
            })
        }
        else if (e.property == "WorkShopId" && e.entity.getWipResouceId() == null) {
            SIE.invokeDataQuery({
                type: "SIE.Web.LES.LinesideWarehouses.LinesideWarehouseDataQueryer",
                method: "GetWorkShopInfo",
                params: [e.value],
                async: false,
                token: this.view.token,
                callback: function (res) {
                    if (res.Success) {
                        var info = res.Result;
                        if (info != null) {
                            data.setFactoryId_Display(info.FactoryName);
                            data.setFactoryId(info.FactoryId);
                        }
                    }
                }
            })
        }
    }
});