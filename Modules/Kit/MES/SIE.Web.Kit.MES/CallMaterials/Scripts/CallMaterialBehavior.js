Ext.define('SIE.Web.Kit.MES.CallMaterials.Scripts.CallMaterialBehavior',
    {
        onDataLoaded: function (view) {
            if (view.model === "SIE.Kit.MES.CallMaterials.CallMaterialBill") {
                var store = view.getData();
                if (store && store.isStore) {
                    var pv = view.getParent();
                    if (pv) {
                        var pe = pv.getCurrent();
                        pe.set('ChildNum', store.getCount());
                        pv.setCurrent(pe, true);
                    }
                }
            }
        }
    });