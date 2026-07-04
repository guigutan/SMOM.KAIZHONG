Ext.define('SIE.Web.EMS.AssetTransfers.AssetTransferBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.action = params.action;
            if (params.action === 0) {
                SIE.invokeDataQuery({
                    method: 'GetNewAssetTransfer',
                    params: [],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetTransfers.AssetTransferDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Result) {
                            var info = res.Result.data.items[0].data;
                            entity.setTransferNo(info.TransferNo);
                            entity.setTransferStatus(info.TransferStatus);
                            entity.setApprovalStatus(info.ApprovalStatus);
                            entity.setApplyDate(info.ApplyDate);
                            entity.setApplicantId(info.ApplicantId);
                            entity.setApplicantId_Display(info.ApplicantName);
                        }
                    }
                });
            }
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        var childView = me._children.first(function (p) { return p.model === "SIE.EMS.AssetTransfers.AssetTransferDetail"; });
        if (e.property === 'TransferType') {
            if (e.value == "10") {//工厂内转移
                e.entity.setTargetFactoryId(e.entity.data.SourceFactoryId);
                e.entity.setTargetManageDeptId(e.entity.data.ManageDeptId);
                e.entity.setTargetUseDepartId(e.entity.data.UseDeptId);

                e.entity.setTargetFactoryId_Display(e.entity.data.SourceFactoryId_Display);
                e.entity.setTargetManageDeptId_Display(e.entity.data.ManageDeptId_Display);
                e.entity.setTargetUseDepartId_Display(e.entity.data.UseDeptId_Display);
            } else//跨工厂调拨 时清除清除责任人到保管人的值
            {
                if (childView) {
                    var store = childView.getData();
                    for (var i = 0; i < store.data.items.length; i++) {
                        var it = store.data.items[0];
                        it.setResponsibleId_Display("");
                        it.setResponsibleId(null);
                        it.setWorkshopId_Display("");
                        it.setWorkshopId(null);

                        it.setResourceId_Display("");
                        it.setResourceId(null)
                        it.setLocation("");

                        it.setWarehouseId(null);
                        it.setStorageLocationId();
                        it.setWarehouseId_Display("");

                        it.setStorageLocationId_Display("");
                        it.setKeeperId_Display("");
                        it.setKeeperId(null);
                    }

                }
            }
        }
        if (e.property === "SourceFactoryId_Display" && e.entity.getTransferType() === 10)//修改原厂时且是工厂内转移需要设置目标工厂为修改后的原工厂，清空目标部门和目标使用部门
        {
            e.entity.setTargetFactoryId(e.entity.data.SourceFactoryId);
            e.entity.setTargetManageDeptId(null);
            e.entity.setTargetUseDepartId(null);

            e.entity.setTargetFactoryId_Display(e.value);
            e.entity.setTargetManageDeptId_Display("");
            e.entity.setTargetUseDepartId_Display("");

        }
        //原工厂、原管理部门、原使用部门  是否固定资产时清楚
        if (e.property === 'IsAsset' || e.property === 'SourceFactoryId' || e.property === 'ManageDeptId' || e.property === 'UseDeptId') {
            if (childView) {
                var store = childView.getData();
                store.removeAll();
            }
        }

        if (e.property === 'TargetFactoryId')//目标工厂改变的时候清空子列表的
        {
            if (childView) {
                var store = childView.getData();
                for (var i = 0; i < store.data.items.length; i++) {
                    var it = store.data.items[0];
                    it.setResponsibleId_Display("");
                    it.setResponsibleId(null);
                    it.setWorkshopId_Display("");
                    it.setWorkshopId(null);

                    it.setResourceId_Display("");
                    it.setResourceId(null)
                    it.setLocation("");

                    it.setWarehouseId(null);
                    it.setStorageLocationId();
                    it.setWarehouseId_Display("");

                    it.setStorageLocationId_Display("");
                    it.setKeeperId_Display("");
                    it.setKeeperId(null);
                    it.setParentTargetFactoryId(e.value);

                }
            }

        }
    }

});