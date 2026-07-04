Ext.define('SIE.Web.EMS.SpareParts.Behaviors.OrderNumberBehavior', {
    onViewReady: function (view) {
        view.mon(view, 'currentChanged', this.currentChanged, view);
    },
    currentChanged: function (config) {
        if (this.getParent()._selection.length == 1) {
            var id = this.getParent()._selection[0].getSparePartDepotId();
            //this._selection[0].setSparePartDepotId(id);
            if (this._selection[0] != undefined)
            this._selection[0].data.SparePartDepotId = id;
        }

    }
});