Ext.define('SIE.Web.EMS.AssetIssues.Behaviors.AssetIssueDetailsBehavior',
    {
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {

            var entity = CRT.Context.PageContext.getCurrentRecord();

            if (!entity) {
                entity = new view._model();
            }

            if (entity.data.CreateDate == null) {
                SIE.invokeDataQuery({
                    method: 'GetAssetIssueNo',
                    params: [],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            entity.setIssueNo(res.Result);
                            entity.setApprovalStatus(10);
                        }
                    }
                });
            }
        },
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {

            var me = this;
            var entity = view.getCurrent();
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);

            var equipChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueEquipment');
            var fixtureChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssueFixture');
            var externalChildView = view.findChild('SIE.EMS.AssetIssues.AssetIssue');

            var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
            var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
            var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
            var externalTab = externalChildView.getControl().ownerLayout.owner.tab;

            if (!entity.data.External) {
                externalTab.hide();
            }

            if (entity.data.AssetObject == 0 || entity.data.AssetObject == null) {
                equipTab.hide();
                fixtureTab.hide();
                if (entity.data.External) {
                    tabPanel.setActiveTab(tabPanel.items.keys.indexOf(externalTab.card.id));
                }
                else {
                    equipChildView.getControl().up().up().setVisible(false);
                }
            }
            if (entity.data.AssetObject == 10) {
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            if (entity.data.AssetObject == 20) {
                equipTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }
        },
        onStorePropertyChanged: function (e) {
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

            if (e.property == 'FixtureAccountId') {

                e.entity.setQty(e.entity.data.FixtureAccountId == null ? 0 : 1);
            }

            if (e.property == 'Qty') {

                if (e.entity.data.Qty > 0) {

                    me.getControl().getSelectionModel().select(e.entity, true);
                }
                else {
                    me.getControl().getSelectionModel().deselect(e.entity);
                }
            }

            if (e.property == 'FixtureEncodeId' || e.property == 'StorageLocationId' || e.property == 'QualityStatus') {

                var warehouseId = me.getParent().getCurrent().data.WarehouseId;
                if (e.entity.data.FixtureEncodeId != null && warehouseId != null) {
                    SIE.invokeDataQuery({
                        method: 'GetCanUseNumByWarehouseId',
                        params: [warehouseId, e.entity.data.FixtureEncodeId, e.entity.data.StorageLocationId, e.entity.data.QualityStatus],
                        async: false,
                        action: 'queryer',
                        type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                        token: me.token,
                        success: function (res) {
                            if (res.Success) {
                                e.entity.setStoreUsableQty(res.Result);
                            }
                        }
                    });
                }
                else {
                    e.entity.setStoreUsableQty(0);
                }
            }
        },
        onEntityPropertyChanged: function (e) {

            var me = this;
            
            if (e.property == 'AssetRequisitionId' || e.property == 'AssetObject' || e.property == 'External') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssueEquipment');
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssueFixture');
                var externalChildView = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssue');
                var equipChildStore = equipChildView.getData();

                var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
                var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
                var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
                var externalTab = externalChildView.getControl().ownerLayout.owner.tab;

                equipChildView.getControl().up().up().setVisible(true);

                if (e.property == 'AssetObject') {

                    if (e.entity.data.AssetObject == 0 || e.entity.data.AssetObject == null) {
                        equipTab.hide();
                        fixtureTab.hide();
                        externalTab.hide();
                        equipChildView.getControl().up().up().setVisible(false);
                    }
                    if (e.entity.data.AssetObject == 10) {
                        equipTab.show();
                        fixtureTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                    }
                    if (e.entity.data.AssetObject == 20) {
                        fixtureTab.show();
                        equipTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                    }
                }

                if (e.property == 'External') {
                    if (e.entity.data.External) {
                        externalTab.show();
                    }
                    else if (e.entity.data.AssetObject != 0 && e.entity.data.AssetObject != null) {
                        externalTab.hide();
                        if (tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(externalTab.card.id)) {

                            if (e.entity.data.AssetObject == 0 || e.entity.data.AssetObject == null) {
                                equipTab.hide();
                                fixtureTab.hide();
                                externalTab.hide();
                                equipChildView.getControl().up().up().setVisible(false);
                            }
                            if (e.entity.data.AssetObject == 10) {
                                equipTab.show();
                                fixtureTab.hide();
                                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                            }
                            if (e.entity.data.AssetObject == 20) {
                                fixtureTab.show();
                                equipTab.hide();
                                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                            }
                        }
                    }
                } 

                if (e.property == 'AssetRequisitionId') {
                    
                    setTimeout(function () {
                        if (e.entity.data.AssetRequisitionId == null) {
                            equipChildView.getControl().up().up().setVisible(false);
                        }
                        else {

                            if (e.entity.data.AssetObject == 10) {

                                SIE.invokeDataQuery({
                                    method: 'GetAssetIssueEquipmentsById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {
                                        
                                        if (res.Success) {
                                            equipChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                var equipRecord = res.Result.data.items[i];
                                                equipRecord.setEquipAccountId_Display(equipRecord.data.EquipAccountCode);
                                                equipRecord.setFactoryId(e.entity.data.FactoryId);
                                                equipRecord.setLendingDepartmentId(e.entity.data.LendingDepartmentId);
                                                equipChildStore.add(equipRecord);
                                                equipChildView.mon(equipRecord, 'propertyChanged', me.onStorePropertyChanged, equipChildView);
                                                
                                                if (equipRecord.data.EquipAccountId != null) {
                                                    equipChildView.getControl().getSelectionModel().select(equipRecord,true);
                                                }
                                                
                                            }
                                        }
                                    }
                                });
                            }
                            if (e.entity.data.AssetObject == 20) {
                                
                                SIE.invokeDataQuery({
                                    method: 'GetAssetIssueFixturesById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetIssues.DataQueryer.AssetIssueDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {
                                        if (res.Success) {
                                            var fixtureChildStore = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssueFixture').getData();
                                            fixtureChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                var fixtureRecord = res.Result.data.items[i];
                                                fixtureRecord.setWarehouseId(e.entity.data.WarehouseId);
                                                fixtureChildStore.add(fixtureRecord);
                                                fixtureChildView.mon(fixtureRecord, 'propertyChanged', me.onStorePropertyChanged, fixtureChildView);

                                                if (fixtureRecord.data.Qty > 0) {
                                                    fixtureChildView.getControl().getSelectionModel().select(fixtureRecord, true);
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                        }

                        if (e.entity.data.External) {

                            externalTab.show();
                            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(externalTab.card.id));

                            if (e.entity.data.AssetObject == 10) {
                                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                            }
                            if (e.entity.data.AssetObject == 20) {
                                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                            }

                            var externalEntity = externalChildView.getCurrent();
                            if (externalEntity) {

                                externalEntity.setExternalType(e.entity.data.ExternalType);
                                externalEntity.setSupplierCode(e.entity.data.SupplierCode);
                                externalEntity.setSupplierName(e.entity.data.SupplierName);
                                externalEntity.setCustomerCode(e.entity.data.CustomerCode);
                                externalEntity.setCustomerName(e.entity.data.CustomerName);
                                externalEntity.setDestination(e.entity.data.Destination);
                                externalEntity.setContactPerson(e.entity.data.ContactPerson);
                                externalEntity.setContactInformation(e.entity.data.ContactInformation);
                                externalEntity.setAddress(e.entity.data.Address);
                                externalEntity.setDeliveryWay(e.entity.data.DeliveryWay);
                                externalEntity.setTrackNo(e.entity.data.TrackNo);
                            }
                        }
                    }, 0);
                }
            }

            if (e.property == 'FactoryId' || e.property == 'LendingDepartmentId') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssueEquipment');
                var equipChildStore = equipChildView.getData();
                for (var i = 0; i < equipChildStore.getCount(); i++) {
                    var equipRecord = equipChildStore.getAt(i);
                    equipRecord.setFactoryId(e.entity.data.FactoryId);
                    equipRecord.setLendingDepartmentId(e.entity.data.LendingDepartmentId);
                }
            }

            if (e.property == 'WarehouseId') {

                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetIssues.AssetIssueFixture');
                var fixtureChildStore = fixtureChildView.getData();

                for (var i = 0; i < fixtureChildStore.getCount(); i++) {
                    var fixtureRecord = fixtureChildStore.getAt(i);
                    fixtureRecord.setWarehouseId(e.entity.data.WarehouseId);
                }
            }
        }
    });
