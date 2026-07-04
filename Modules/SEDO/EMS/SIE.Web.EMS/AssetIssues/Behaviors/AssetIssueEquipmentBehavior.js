Ext.define('SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueEquipmentBehavior',
    {
        /**
         * view生命周期函数--数据加载后
         * @param {any} view
         */
        onDataLoaded: function (view) {
            var me = this;
            var equipChildStore = view.getData();
            var formEntity = view.getParent().getCurrent();

            for (var i = 0; i < equipChildStore.getCount(); i++) {
                var equipRecord = equipChildStore.getAt(i);
                equipRecord.setFactoryId(formEntity.data.FactoryId);
                equipRecord.setLendingDepartmentId(formEntity.data.LendingDepartmentId);

                if (equipRecord.data.EquipAccountId != null) {
                    view.getControl().getSelectionModel().select(equipRecord, true);
                }
            }
            view.mon(equipChildStore, 'propertyChanged', me.onEntityPropertyChanged, view);
        },
        /**
         * 属性变更事件
         * @param {any} e
         */
        onEntityPropertyChanged: function (e) {
            var me = this;
            if (e.property == 'EquipAccountId') {

                if (e.entity.data.EquipAccountId == null) {
                    SIE.invokeDataQuery({
                        method: 'GetAssetIssueEquipmentsByReqEquipId',
                        params: [e.entity.data.AssetRequisitionEquipmentId],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                        token: me.token,
                        success: function (res) {

                            if (res.Success) {

                                setTimeout(function () {
                                    var equipRecord = res.Result.data.items[0];
                                    e.entity.setEquipAccountId_Display(equipRecord.data.EquipAccountCode);
                                    e.entity.setEquipAccountCode(equipRecord.data.EquipAccountCode);
                                    e.entity.setEquipAccountId(equipRecord.data.EquipAccountId);
                                    e.entity.setEquipAccountName(equipRecord.data.EquipAccountName);
                                    e.entity.setUseState(equipRecord.data.UseState);
                                    e.entity.setAlias(equipRecord.data.Alias);
                                    e.entity.setEquipModelCode(equipRecord.data.EquipModelCode);
                                    e.entity.setEquipModelName(equipRecord.data.EquipModelName);
                                    e.entity.setSpecifications(equipRecord.data.Specifications);
                                    e.entity.setEquipTypeCode(equipRecord.data.EquipTypeCode);
                                    e.entity.setEquipTypeName(equipRecord.data.EquipTypeName);

                                    me.getControl().getSelectionModel().deselect(e.entity);
                                }, 0);
                            }
                        }
                    });
                }
                else {
                    SIE.invokeDataQuery({
                        method: 'GetAssetIssueEquipmentsByEquipId',
                        params: [e.entity.data.EquipAccountId],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                        token: me.token,
                        success: function (res) {

                            if (res.Success) {

                                setTimeout(function () {
                                    var equipRecord = res.Result.data.items[0];
                                    e.entity.setEquipAccountCode(equipRecord.data.EquipAccountCode);
                                    e.entity.setEquipAccountName(equipRecord.data.EquipAccountName);
                                    e.entity.setUseState(equipRecord.data.UseState);
                                    e.entity.setAlias(equipRecord.data.Alias);
                                    e.entity.setEquipModelCode(equipRecord.data.EquipModelCode);
                                    e.entity.setEquipModelName(equipRecord.data.EquipModelName);
                                    e.entity.setSpecifications(equipRecord.data.Specifications);
                                    e.entity.setEquipTypeCode(equipRecord.data.EquipTypeCode);
                                    e.entity.setEquipTypeName(equipRecord.data.EquipTypeName);

                                    me.getControl().getSelectionModel().select(e.entity, true);
                                }, 0);
                            }
                        }
                    });
                }
            }
        }
    });