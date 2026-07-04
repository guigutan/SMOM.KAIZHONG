Ext.define('SIE.Web.EMS.AssetDisposals.Behaviors.AssetDisposalDetailsBehavior', {
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
                method: 'GetAssetDisposalNo',
                params: [],
                async: false,
                action: 'queryer',
                type: 'SIE.Web.EMS.AssetDisposals.DataQueryer.AssetDisposalDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        entity.setNo(res.Result);
                        entity.setApplicantId(userInfo.EmployeeId);
                        entity.setApplicantId_Display(userInfo.Name);
                        entity.setApplyDate(new Date());
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
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);

        var equipChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalEquipment');
        var fixtureChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalFixture');
        var sparePartChildView = view.findChild('SIE.EMS.AssetDisposals.AssetDisposalSparePart');

        var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
        var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
        var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;
        var sparePartTab = sparePartChildView.getControl().ownerLayout.owner.tab;

        if (entity.data.AssetObject == 0 || entity.data.AssetObject == null) {
            equipTab.hide();
            fixtureTab.hide();
            tabPanel.setActiveTab(tabPanel.items.keys.indexOf(sparePartTab.card.id));
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

        if (e.property == 'AssetObject') {

            var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetDisposals.AssetDisposalEquipment');
            var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetDisposals.AssetDisposalFixture');
            var equipChildStore = equipChildView.getData();
            var fixtureChildStore = fixtureChildView.getData();

            var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
            var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
            var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;

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

            var amountList = store.getData().items.where(function (p) { return !Ext.isEmpty(p.getNetValue()); })
                .select(function (p) { return parseFloat(p.getNetValue()); });
            var amount = amountList.sum();

            e.entity.setAmount(amount);
        }

        if (e.property == 'FactoryId' || e.property == 'UseDeptId' || e.property == 'ManageDeptId' || e.property == 'IsFixAsset' || e.property == 'WarehouseId') {

            if (e.entity.data.AssetObject == 10) {
                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.AssetDisposals.AssetDisposalEquipment');
                var equipChildStore = equipChildView.getData();
                equipChildStore.removeAll();
            }

            if (e.property == 'WarehouseId' && e.entity.data.AssetObject == 20) {
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.AssetDisposals.AssetDisposalFixture');
                var fixtureChildStore = fixtureChildView.getData();
                fixtureChildStore.removeAll();
            }
        }
    }
});
