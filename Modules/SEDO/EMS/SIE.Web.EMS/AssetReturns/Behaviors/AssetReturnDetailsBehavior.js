Ext.define('SIE.Web.EMS.AssetReturns.Behaviors.AssetReturnDetailsBehavior',
    {
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {

            var entity = CRT.Context.PageContext.getCurrentRecord();
            var userInfo = CRT.Context.GlobalContext.getContext('userInfo');

            if (!entity) {
                entity = new view._model();
            }

            if (entity.data.CreateDate == null) {
                SIE.invokeDataQuery({
                    method: 'GetAssetReturnNo',
                    params: [],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            entity.setReturnNo(res.Result);
                            entity.setApprovalStatus(10);
                            entity.setEmployeeId(userInfo.EmployeeId);
                            entity.setEmployeeId_Display(userInfo.Name);
                            entity.setApplyDate(new Date());
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

            var equipChildView = view.findChild('SIE.EMS.AssetReturns.AssetReturnEquipment');
            var fixtureChildView = view.findChild('SIE.EMS.AssetReturns.AssetReturnFixture');
            var attachmentChildView = view.findChild('SIE.EMS.AssetReturns.AssetReturnAttachment');
            var existEquipChildView = view.getChildren().first(function (e) {
                return e.viewGroup === "ExistAssetReturnEquipmentViewGroup";
            });
            var existFixtureChildView = view.getChildren().first(function (e) {
                return e.viewGroup === "ExistAssetReturnFixtureViewGroup";
            });

            var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
            var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
            var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
            var attachmentTab = attachmentChildView.getControl().ownerLayout.owner.tab;
            var existEquipTab = existEquipChildView.getControl().ownerLayout.owner.tab;
            var existFixtureTab = existFixtureChildView.getControl().ownerLayout.owner.tab;

            if (entity.data.AssetObject == 0 || entity.data.AssetObject == null) {
                equipTab.hide();
                existEquipTab.hide();
                fixtureTab.hide();
                existFixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(attachmentTab.card.id));
            }
            if (entity.data.AssetObject == 10) {
                fixtureTab.hide();
                existFixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            if (entity.data.AssetObject == 20) {
                equipTab.hide();
                existEquipTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }
        },
        onEquipStorePropertyChanged: function (e) {
            var me = this;

            if (e.property == 'ReturnType') {

                if (e.entity.data.ReturnType == null || e.entity.data.ReturnType == 0) {

                    me.getControl().getSelectionModel().deselect(e.entity);
                }
                else {
                    me.getControl().getSelectionModel().select(e.entity, true);
                }
            }
        },
        onFixtureStorePropertyChanged: function (e) {
            var me = this;

            if (e.property == 'ReturnType' && e.entity.data.ManageMode == 5) {

                e.entity.setQty((e.entity.data.ReturnType == null || e.entity.data.ReturnType == 0) ? 0 : 1);
            }

            if (e.property == 'Qty') {

                if (e.entity.data.Qty > 0) {

                    me.getControl().getSelectionModel().select(e.entity, true);
                }
                else {
                    me.getControl().getSelectionModel().deselect(e.entity);
                }
            }
        },
        onEntityPropertyChanged: function (e) {

            var me = this;

            if (e.property == 'AssetRequisitionId' || e.property == 'AssetObject') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnEquipment');
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnFixture');
                var attachmentChildView = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnAttachment');
                var existEquipChildView = e.entity.belongsView.getChildren().first(function (e) {
                    return e.viewGroup === "ExistAssetReturnEquipmentViewGroup";
                });
                var existFixtureChildView = e.entity.belongsView.getChildren().first(function (e) {
                    return e.viewGroup === "ExistAssetReturnFixtureViewGroup";
                });
                
                var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
                var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
                var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
                var attachmentTab = attachmentChildView.getControl().ownerLayout.owner.tab;
                var existEquipTab = existEquipChildView.getControl().ownerLayout.owner.tab;
                var existFixtureTab = existFixtureChildView.getControl().ownerLayout.owner.tab;

                if (e.property == 'AssetObject') {

                    if (e.entity.data.AssetObject == 0 || e.entity.data.AssetObject == null) {
                        equipTab.hide();
                        existEquipTab.hide();
                        fixtureTab.hide();
                        existFixtureTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(attachmentTab.card.id));
                    }
                    if (e.entity.data.AssetObject == 10) {
                        equipTab.show();
                        existEquipTab.show();
                        fixtureTab.hide();
                        existFixtureTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(existEquipTab.card.id));
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                    }
                    if (e.entity.data.AssetObject == 20) {
                        fixtureTab.show();
                        existFixtureTab.show();
                        equipTab.hide();
                        existEquipTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(existFixtureTab.card.id));
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                    }
                }

                if (e.property == 'AssetRequisitionId') {

                    setTimeout(function () {

                        if (e.entity.data.AssetRequisitionId != null) {
                            if (e.entity.data.AssetObject == 10) {

                                SIE.invokeDataQuery({
                                    method: 'GetAssetReturnEquipmentsById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {

                                        if (res.Success) {
                                            var equipChildStore = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnEquipment').getData();
                                            equipChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                var equipRecord = res.Result.data.items[i];

                                                equipRecord.setEquipAccountId(equipRecord.data.EquipAccountId);
                                                equipRecord.setEquipAccountId_Display(equipRecord.data.EquipAccountCode);
                                                equipRecord.setFactoryId(e.entity.data.FactoryId);
                                                equipChildStore.add(equipRecord);
                                                equipChildView.mon(equipRecord, 'propertyChanged', me.onEquipStorePropertyChanged, equipChildView);

                                                if (equipRecord.data.ReturnType != null && equipRecord.data.ReturnType != 0) {
                                                    equipChildView.getControl().getSelectionModel().select(equipRecord, true);
                                                }

                                            }
                                        }
                                    }
                                });

                                SIE.invokeDataQuery({
                                    method: 'GetExistAssetReturnEquipmentsById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {

                                        if (res.Success) {
                                            var existEquipChildStore = existEquipChildView.getData();
                                            existEquipChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                existEquipChildStore.add(res.Result.data.items[i]);
                                            }
                                        }
                                    }
                                });
                            }
                            if (e.entity.data.AssetObject == 20) {

                                SIE.invokeDataQuery({
                                    method: 'GetAssetReturnFixturesById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {
                                        if (res.Success) {
                                            var fixtureChildStore = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnFixture').getData();
                                            fixtureChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                var fixtureRecord = res.Result.data.items[i];
                                                fixtureChildStore.add(fixtureRecord);
                                                fixtureChildView.mon(fixtureRecord, 'propertyChanged', me.onFixtureStorePropertyChanged, fixtureChildView);

                                                if (fixtureRecord.data.Qty > 0) {
                                                    fixtureChildView.getControl().getSelectionModel().select(fixtureRecord, true);
                                                }
                                            }
                                        }
                                    }
                                });

                                SIE.invokeDataQuery({
                                    method: 'GetExistAssetReturnFixturesById',
                                    params: [0, e.entity.data.AssetRequisitionId],
                                    async: false,
                                    action: 'queryer',
                                    type: 'SIE.Web.EMS.AssetReturns.DataQueryer.AssetReturnDataQueryer',
                                    token: e.entity.belongsView.token,
                                    success: function (res) {
                                        if (res.Success) {
                                            var existFixtureChildStore = existFixtureChildView.getData();
                                            existFixtureChildStore.removeAll();
                                            for (var i = 0; i < res.Result.data.items.length; i++) {
                                                existFixtureChildStore.add(res.Result.data.items[i]);
                                            }
                                        }
                                    }
                                });
                            }
                        }
                    }, 0);
                }
            }

            if (e.property == 'FactoryId') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetReturns.AssetReturnEquipment');
                var equipChildStore = equipChildView.getData();
                for (var i = 0; i < equipChildStore.getCount(); i++) {
                    var equipRecord = equipChildStore.getAt(i);
                    equipRecord.setFactoryId(e.entity.data.FactoryId);
                }
            }
        }
    });
