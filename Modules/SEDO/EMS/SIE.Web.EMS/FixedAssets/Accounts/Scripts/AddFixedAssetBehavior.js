Ext.define('SIE.Web.EMS.FixedAssets.Accounts.Scripts.AddFixedAssetBehavior',
    {
        /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
        beforeCreate: function (meta, curEntity) {
            //code here
        },
        /**
        * view生命周期函数--view生成后
        * @param {*} view 生成的view
        */
        onCreated: function (view) {
            var entity = CRT.Context.PageContext.getCurrentRecord();
            var params = CRT.Context.PageContext.getParams();
            if (params) {
                entity.setCode(params.Code);
                entity.setManageStatus(5);
                entity.setAssetsSource(5);
                entity.setResidualValueRatio(3);
                entity.setFixedAssetsTransferDate(new Date());
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

            var equipChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetDeviceBill');
            var sparePartChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetSparePart');
            var fixtureChildView = view.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetFixtureBill');

            var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
            var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
            var sparePartTab = sparePartChildView.getControl().ownerLayout.owner.tab;
            var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;

            if (entity.data.AssetsType == 5) {
                equipTab.show();
                sparePartTab.hide();
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
            }
            else if (entity.data.AssetsType == 10) {
                equipTab.hide();
                sparePartTab.show();
                fixtureTab.hide();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(sparePartTab.card.id));
            }
            else if (entity.data.AssetsType == 15) {
                equipTab.hide();
                sparePartTab.hide();
                fixtureTab.show();
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
            }
            else {
                equipChildView.getControl().up().up().setVisible(false);
            }
        },
        onEntityPropertyChanged: function (e) {

            if (e.property == 'AssetsType') {

                var equipChildView = e.entity.belongsView.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetDeviceBill');
                var sparePartChildView = e.entity.belongsView.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetSparePart');
                var fixtureChildView = e.entity.belongsView.findChild('SIE.EMS.FixedAssets.Accounts.FixedAssetFixtureBill');

                var tabPanel = equipChildView.getControl().ownerCt.ownerCt;
                var equipTab = equipChildView.getControl().ownerLayout.owner.tab;
                var sparePartTab = sparePartChildView.getControl().ownerLayout.owner.tab;
                var fixtureTab = fixtureChildView.getControl().ownerLayout.owner.tab;

                equipChildView.getControl().up().up().setVisible(true);

                if (e.entity.data.AssetsType == 5) {
                    equipTab.show();
                    sparePartTab.hide();
                    fixtureTab.hide();
                    tabPanel.setActiveTab(tabPanel.items.keys.indexOf(equipTab.card.id));
                }
                else if (e.entity.data.AssetsType == 10) {
                    equipTab.hide();
                    sparePartTab.show();
                    fixtureTab.hide();
                    tabPanel.setActiveTab(tabPanel.items.keys.indexOf(sparePartTab.card.id));
                }
                else if (e.entity.data.AssetsType == 15) {
                    equipTab.hide();
                    sparePartTab.hide();
                    fixtureTab.show();
                    tabPanel.setActiveTab(tabPanel.items.keys.indexOf(fixtureTab.card.id));
                }
                else {
                    equipTab.hide();
                    sparePartTab.hide();
                    fixtureTab.hide();
                    equipChildView.getControl().up().up().setVisible(false);
                }
            }

            if (e.property == "ResidualValueRatio") {
                e.entity.setDepreciationResidualValue(e.entity.getOriginalAssetsValue() * (e.entity.getResidualValueRatio() / 100));
                e.entity.setNetAssetValue(e.entity.getOriginalAssetsValue() - e.entity.getDepreciationResidualValue());
            }
            if (e.property == "OriginalAssetsValue" && e.value > 0) {
                e.entity.setDepreciationResidualValue(e.entity.getOriginalAssetsValue() * (e.entity.getResidualValueRatio() / 100));
                e.entity.setNetAssetValue(e.entity.getOriginalAssetsValue() - e.entity.getDepreciationResidualValue());

            }


        },
        /**
         * view生命周期函数--数据加载后
         * @param {any} view 逻辑视图
         */
        onDataLoaded: function (view) {
            //code here
        },
    });
