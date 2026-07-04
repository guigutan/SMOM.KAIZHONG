Ext.define('SIE.Web.EMS.Equipments.Scripts.EquipAccountTabBehavior',
    {
        onViewReady: function (view) {
            view.getControl().up().hide();
        }
    });