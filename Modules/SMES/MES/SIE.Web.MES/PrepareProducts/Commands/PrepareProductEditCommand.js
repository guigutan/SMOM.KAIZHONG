SIE.defineCommand('SIE.Web.MES.PrepareProducts.Commands.PrepareProductEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this.onEntityPropertyChanged, this);
        }
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property == 'ProductFamilyId') {
            var child = e.entity.belongsView.findChild('SIE.MES.PrepareProducts.PrepareProductDetail');
            var store = child.getData();
            store.getData().items.forEach(item => {
                item.setProFamiliyId(e.entity.getProductFamilyId());
            });
        }
    },
});
