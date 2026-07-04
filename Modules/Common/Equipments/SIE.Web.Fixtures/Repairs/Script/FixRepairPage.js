Ext.define('SIE.Web.Fixtures.Repairs.Script.FixRepairPage', {
    extend: 'SIE.Page',
    beforeLoad: function (args) {
        this.isCustomize = true;
    },
    onLoad: function () {
        var me = this;
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.No = params.No;
            me.RepairState = params.RepairState;
            me.ApplyById = params.ApplyById;
            me.ApplyByName = params.ApplyByName;
            me.ApplyDate = params.ApplyDate;
            me.RepairById = params.RepairById;
            me.RepairByName = params.RepairByName;
            me.RepairDate = params.RepairDate;
            me.RepairId = params.RepairId;
            me.token = params.token;
            me.tabId = params.tabId;
        }

        var meta = null;
        SIE.AutoUI.getMeta({
            model: 'SIE.Fixtures.Repairs.FixtureRepair',
            viewGroup: 'RepairDetails',
            isDetail: true,
            isAggt: true,
            ignoreQuery: true,
            async: false,
            callback: function (res) {
                meta = res;
                me.handlerLayout(meta);
                me.genaralUI(meta);
            }
        });
    },
    genaralUI: function (meta) {
        var me = this;
        var ui = SIE.AutoUI.generateAggtControl(meta);
        me.ui = ui;
        if (me.RepairRecordLayout) {
            var uiBottom = SIE.AutoUI.generateAggtControl(me.RepairRecordLayout);
            var frdView = ui.getView().findChild('SIE.Fixtures.Repairs.FixtureRepairDetail');
            uiBottom.getView()._setParent(frdView);
            uiBottom.getView().childLayoutType = 1;
            uiBottom.getView()._childProperty = me.RepairRecordLayout.childProperty;
            me.ui.getView().uiBottom = uiBottom;
            me.uiBottom = uiBottom;
            me.ui.getView().tabId = me.tabId;
            frdView.on('currentChanged', function (oldValue, value) {
                me.uiBottom.getView().syncCmdState(me.uiBottom.getView(), true);
            });
        }
        
        var entity = Ext.create(ui.getView().model);
        entity.setNo(me.No);
        entity.setRepairState(me.RepairState);
        entity.setApplyByName(me.ApplyByName);
        entity.setApplyDate(me.ApplyDate);
        entity.setRepairByName(me.RepairByName);
        entity.setRepairDate(me.RepairDate);
        entity.setId(me.RepairId);
        ui.getView().setData(entity);
        ui.getView().getData().dirty = false;
        ui.getView().getCurrent().phantom = false;

        me.detailView = me.ui.getView().findChild('SIE.Fixtures.Repairs.FixtureRepairDetail');
        if (me.detailView)
            me.detailView.loadData();
        

        ui.getControl().setRegion('center');
        ui.getControl().items.items[0].setHeight('30%');
        ui.getControl().items.items[1].setHeight('70%');

        var form = Ext.create('Ext.panel.Panel', {
            layout: 'border',
            items: [
                ui.getControl(),
                {
                    xtype: 'panel',
                    region: 'south',
                    height: 250,
                    layout: 'fit',
                    title: '维修记录'.t(),
                    items: uiBottom.getControl()
                }
            ]
        });
        Ext.create('Ext.container.Viewport', {
            layout: {
                type: 'border'
            },
            border: 0,
            defaults: {
                layout: 'fit'
            },
            view: me.ui.getView(),
            items: {
                region: 'center',
                items: form
            },
            renderTo: Ext.getBody()
        });
    },
    handlerLayout: function (meta) {
        var me = this;
        me.RepairDetailLayout = Ext.Array.findBy(meta.children, function (item) {
            if (item.mainBlock.model == 'SIE.Fixtures.Repairs.FixtureRepairDetail' && item.mainBlock.viewGroup == 'FixtureRepairDetail') { return true; }
        });

        if (me.RepairDetailLayout) {
            me.RepairRecordLayout = Ext.Array.findBy(me.RepairDetailLayout.children, function (item) {
                if (item.mainBlock.model == 'SIE.Fixtures.Repairs.FixtureRepairRecord' && item.mainBlock.viewGroup == 'FixtureRepairDetail') { return true; }
            });

            //剪切
            if (me.RepairRecordLayout) {
                var posIndex = me.RepairDetailLayout.children.indexOf(me.RepairRecordLayout);
                me.RepairDetailLayout.children.splice(posIndex, 1);
            }
        }
    },
});