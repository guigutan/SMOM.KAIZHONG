Ext.define('SIE.Web.LES.MaterialReceptions.Behaviors.TabBehavior', {
    onViewReady: function (view) {
        view.getControl().up().hide();
    },
});
