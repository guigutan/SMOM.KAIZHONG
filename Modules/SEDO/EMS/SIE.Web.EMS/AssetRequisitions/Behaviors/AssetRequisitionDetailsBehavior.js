Ext.define('SIE.Web.EMS.AssetRequisitions.Behaviors.AssetRequisitionDetailsBehavior',
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
                    method: 'GetAssetRequisitionNo',
                    params: [],
                    async: false,
                    action: 'queryer',
                    type: 'SIE.Web.EMS.AssetRequisitions.DataQueryer.AssetRequisitionDataQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            entity.setRequisitionNo(res.Result);
                            entity.setEmployeeId(userInfo.EmployeeId);
                            entity.setEmployeeId_Display(userInfo.Name);
                            entity.setApplyDate(new Date());
                            entity.setApprovalStatus(10);
                            entity.setIssueStatus(10);
                            entity.setReturnStatus(10);
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
            view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);

            var equipChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionEquipment');
            var fixtureChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionFixture');
            var externalChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisition');
            var attachmentChildView = view.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionAttachment');

            var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
            var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
            var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
            var externalTab = externalChildView.getControl().ownerLayout.owner.tab;
            var attachmentTab = attachmentChildView.getControl().ownerLayout.owner.tab;

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
                    tabPanel.setActiveTab(tabPanel.items.keys.indexOf(attachmentTab.card.id));
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
        onEntityPropertyChanged: function (e) {

            if (e.property == 'AssetObject' || e.property == 'External') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionEquipment');
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionFixture');
                var externalChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisition');
                var attachmentChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionAttachment');
                var equipChildStore = equipChildView.getData();
                var fixtureChildStore = fixtureChildView.getData();

                var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
                var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
                var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
                var externalTab = externalChildView.getControl().ownerLayout.owner.tab;
                var attachmentTab = attachmentChildView.getControl().ownerLayout.owner.tab;

                if (e.property == 'AssetObject') {

                    var store = {};
                    if (e.entity.data.AssetObject == 10) {
                        equipTab.show();
                        fixtureTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                        store = equipChildStore;
                    }
                    if (e.entity.data.AssetObject == 20) {
                        fixtureTab.show();
                        equipTab.hide();
                        tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                        store = fixtureChildStore;
                    }

                    var amountList = store.getData().items.where(function (p) { return !Ext.isEmpty(p.getEstimatedAmount()); })
                        .select(function (p) { return parseFloat(p.getEstimatedAmount()); });
                    var amount = amountList.sum();

                    e.entity.setAmount(amount);
                }
                
                if (e.property == 'External') {
                    if (e.entity.data.External) {
                        externalTab.show();
                    }
                    else {
                        externalTab.hide();
                        if (tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id) == tabPanel.items.keys.indexOf(externalTab.card.id)) {

                            if (e.entity.data.AssetObject == 0 || e.entity.data.AssetObject == null) {
                                attachmentTab.show();
                                equipTab.hide();
                                fixtureTab.hide();
                                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(attachmentTab.card.id));
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
            }

            if (e.property == 'FactoryId' || e.property == 'LendingDepartmentId' || e.property == 'WarehouseId' || e.property == 'AssetObject') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionEquipment');
                var equipChildStore = equipChildView.getData();
                equipChildStore.removeAll();
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetRequisitions.AssetRequisitionFixture');
                var fixtureChildStore = fixtureChildView.getData();
                fixtureChildStore.removeAll();
                e.entity.setAmount(0);
            }
        }
    });
