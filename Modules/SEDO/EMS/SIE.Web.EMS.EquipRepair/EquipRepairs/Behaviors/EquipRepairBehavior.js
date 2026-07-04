Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {

        },
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            if (!entity) {
                entity = new view._model();
            }
            entity.setApplyRepairEmployeeId(CurUserStateHelper.getSessionUser().EmployeeId);
            entity.setApplyRepairEmployeeId_Display(CurUserStateHelper.getSessionUser().Name);
            entity.setApplyRepairDate(new Date);
            //手动创建设置来源类型为手动创建
            entity.setSourceType(2);

            SIE.invokeDataQuery({
                method: 'GetCode',
                params: [],
                async: false,
                action: 'queryer',
                type: 'SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        entity.setRepairNo(res.Result);
                    }
                }
            });

        },
        onViewReady: function (view) {
            var me = this;
            var current = view.getCurrent();
            view.mon(current, "propertyChanged", me.onPropertyChanged, view);
        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {

            var me = this;
            /*view.mon(view, 'propertyChanged', me.onPropertyChanged, view);*/
            //获取保养项目列表，绑定属性变更事件 
            //var projView = view.getChildren().first(function (e) { return e.viewConfig === "SIE.Web.EMS.EquipRepair.EquipRepairs.EquipRepairViewConfig"; });
            //if (projView) {
            //    var records = projView.getData();
            //    if (records) {
            //        projView.mon(records, 'propertyChanged', me.onPropertyChanged, view);
            //    }
            //};
        },
        onPropertyChanged: function (e) {

            var me = this;
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property === 'RepairType') {

                    if (this._children != undefined) {
                        if (entity.getRepairType() == 1) {
                            this._children[0].getData().setApplyRepairEmployeeId(null);
                            this._children[0].getData().setApplyRepairEmployeeId_Display("");
                        }
                        this._children[0].getData().setRepairType(e.entity.getRepairType());
                    }
                    if (e.entity.belongsView !== null) {
                        e.entity.belongsView.findChild("SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill").getCurrent().setDeviceAbnormalId(null);
                    }
                    else {
                        me.findChild("SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill").getCurrent().setDeviceAbnormalId(null);
                    }
                    entity.setSparePartId(null);
                    entity.setEquipAccountId(null);
                    
                }
                if (e.property === 'EquipAccountId') {
                    if (this._children != undefined) {
                        this._children[0].getData().setEquipAccountId(e.entity.getEquipAccountId());
                        this._children[0].getData().setRepairType(e.entity.getRepairType());
                    }
                    me.findChild("SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill").getCurrent().setDeviceAbnormalId(null);
                }
                if (e.property === 'SparePartId') {
                    if (this._children != undefined) {
                        this._children[0].getData().setSparePartId(e.entity.getSparePartId());
                        this._children[0].getData().setRepairType(e.entity.getRepairType());
                    }
                    me.findChild("SIE.EMS.EquipRepair.EquipRepairs.EquipRepairBill").getCurrent().setDeviceAbnormalId(null);
                }
                if (e.property === 'ProduceState') {
                    if (e.entity.getProduceState() == 0) {
                        e.entity.setUrgentDegree(0);
                    } else {
                        e.entity.setUrgentDegree(2);
                    }
                }
            }
        }

    });
