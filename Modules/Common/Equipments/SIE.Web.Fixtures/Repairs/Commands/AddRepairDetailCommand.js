SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.AddRepairDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return true;
        }
        return false;
    },

    /**
     * onItemCreated 创建实体
     * @param {} entity
     * @returns {}
     */
    onItemCreated: function (entity) {
        var me = this;
        if (entity) {
            me.mon(entity, 'propertyChanged', me.onEntityPropertyChanged, me);
        }
    },

    /**
     * onEntityPropertyChanged 属性变更事件
     * @param {} e 参数
     * @returns {}
     */
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property.length > 0) {
            var detail = e.entity;
            var data = e.entity.data;
            me.token = this.view.token;
            if (e.property === 'FixtureAccountId') {
                //管控方式等于Id管控
                SIE.invokeDataQuery({
                    method: 'GetFixtureRepairDetailInfo',
                    params: [e.value],
                    action: 'queryer',
                    type: 'SIE.Web.Fixtures.Repairs.DataQuery.FixtureRepairDataQueryer',
                    token: me.token,
                    success: function (res) {
                        var info = res.Result;
                        if (info) {
                            //报修前状态
                            detail.setRepairBeforeState(info.RepairBeforeState);
                            //工单赋值
                            detail.setWorkOrderId_Display(info.WorkOrderId_Display);
                            detail.setWorkOrderId(info.WorkOrderId);
                            //仓库赋值
                            detail.setFixtureWarehouseId_Display(info.FixtureWarehouseId_Display);
                            detail.setFixtureWarehouseId(info.FixtureWarehouseId);
                            //库位赋值
                            detail.setFixtureStorageLocationId_Display(info.FixtureStorageLocationId_Display);
                            detail.setFixtureStorageLocationId(info.FixtureStorageLocationId);
                            detail.setLocationName(info.FixtureStorageLocationName);

                        }
                    }
                })
                detail.setQty(1);
            }

            if (e.property === 'RepairBeforeState') {
                //如果工治具台账为编码类型且报修前状态为在线，直接默认报修前质量状态为合格
                if (detail.getManageMode() == 10 && e.value == 5) {
                    detail.setRepairBeforeQualityStatus(5);
                } else {
                    detail.setRepairBeforeQualityStatus(null);
                }
            }
        }
    },
});