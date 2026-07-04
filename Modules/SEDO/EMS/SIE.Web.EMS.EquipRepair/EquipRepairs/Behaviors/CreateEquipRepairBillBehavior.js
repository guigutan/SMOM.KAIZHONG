Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.CreateEquipRepairBillBehavior',
    {
        onViewReady: function (view) {
            var me = this;
            var entity = view.getCurrent();

            //要上传附件，所以必须先生成表ID
            if (!entity.getId() || entity.getId() == 0) {
                entity.generateId();
            }

            var params = CRT.Context.PageContext.getParams();
            // 20241028传入后台状态赋值成新
            entity.data.PersistenceStatus = 2;
            entity.setApplyRepairEmployeeId(CurUserStateHelper.getSessionUser().EmployeeId);
            entity.setApplyRepairEmployeeId_Display(CurUserStateHelper.getSessionUser().Name);
            entity.setApplyRepairDate(new Date);

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

            //有传设备ID,获取设备信息
            if (params && entity && params.equipmentAccountID && params.sourceNo) {

                //设备台账Id
                var equipmentAccountID = params.equipmentAccountID;
                var sourceNo = params.sourceNo;
                var sourceType = 2;
                if (params.sourceType != null) {
                    sourceType = params.sourceType
                }

                SIE.invokeDataQuery({
                    type: "SIE.Web.EMS.EquipRepair.EquipRepairs.DataQuerys.EquipRepairDataQuery",
                    method: "GetEquipAccountById",
                    params: [equipmentAccountID],
                    async: false,
                    token: view.token,
                    callback: function (resGetEquipmentAccount) {
                        if (resGetEquipmentAccount.Result != null) {
                            var result = resGetEquipmentAccount.Result.getData().items[0];
                            entity.setSourceNo(sourceNo)
                            entity.setEquipAccountId(equipmentAccountID);
                            entity.setEquipAccountId_Display(result.getCode());
                            entity.setEquipAccountName(result.getName());
                            entity.setEquipAccountMode(result.getModelName());
                            entity.setEquipAccountTypeName(result.getEquipTypeName());
                            entity.setResourceName(result.getResourceName());
                            entity.setUseDepartment(result.getUseDepartmentName());
                            entity.setInstallationLocation(result.getInstallationLocation());
                            entity.setProcessName(result.getProcessName());
                            entity.setWorkShopName(result.getWorkShopName());
                            entity.setSourceType(sourceType);

                        }
                    }
                });
            }
        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {

            var me = this;
            //获取故障信息，绑定属性变更事件 
            var faultInofView = view.getChildren().first(function (e) {
                return e.viewConfig === "SIE.Web.EMS.EquipRepair.EquipRepairs.EquipRepairViewConfig";
            });

            if (faultInofView) {
                var records = faultInofView.getData();
                if (records) {
                    faultInofView.mon(records, 'propertyChanged', me.onPropertyChanged, me);
                }
            }
            view._children[0].getData().setEquipAccountId(view.getCurrent().getEquipAccountId());
            view._children[0].getData().setRepairType(view.getCurrent().getRepairType());
        },
        onPropertyChanged: function (e) {
            if (e.property.length > 0) {
                if (e.property === 'ProduceState') {
                    if (e.entity.getProduceState() == 0) {
                        e.entity.setUrgentDegree(0)
                    } else {
                        e.entity.setUrgentDegree(2)
                    }
                }
            }
        }
    });
