Ext.define('SIE.Web.EMS.EarlierStage.Projects.ProjectKeyItemListBehavior', {
    onViewReady: function (view) {
        view.getConditionView().getControl().ownerCt.setHidden(true);
    }
});