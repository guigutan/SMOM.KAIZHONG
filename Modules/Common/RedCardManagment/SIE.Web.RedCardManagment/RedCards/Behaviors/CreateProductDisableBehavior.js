Ext.define('SIE.Web.RedCardManagment.RedCards.Behaviors.CreateProductDisableBehavior',
{
    //view显示
    onShow: function (view) {
        view.getConditionView().tryExecuteQuery();
    },
})