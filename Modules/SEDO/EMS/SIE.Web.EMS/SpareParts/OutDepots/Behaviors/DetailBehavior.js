Ext.define('SIE.Web.EMS.SpareParts.OutDepots.Behaviors.OutDepotBillBehavior',
    {
        onViewReady: function (view) {
            view.mon(view, 'currentChanged', this.currentChanged, view);
        },
        currentChanged: function (e) {
            var view = this;
            var applyChildView = view.findChild('SIE.EMS.SpareParts.OutDepots.Details.OutDepotDetail');
            var outDepotChildView = view.findChild('SIE.EMS.SpareParts.OutDepots.Details.PartOutDepotDetail');

            var tabPanel = applyChildView.getControl().ownerCt.ownerCt;
            var applyChildTab = applyChildView.getControl().ownerLayout.owner.tab;
            var outDepotChildTab = outDepotChildView.getControl().ownerLayout.owner.tab;

            var curEntity = view.getCurrent();
            if (curEntity) {

                console.log(tabPanel.items);
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(applyChildTab.card.id));
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(outDepotChildTab.card.id));
                tabPanel.setActiveTab(tabPanel.items.keys.indexOf(tabPanel.getActiveTab().id));
            }
        },        
    });
