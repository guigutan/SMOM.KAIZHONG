SIE.defineCommand('SIE.Web.EMS.AssetReturns.Commands.CopyAssetReturnFixtureCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && view.getSelection()[0].data.ManageMode == 10;
    },
    execute: function (view, source) {

        var me = this;
        var entity = view.getCurrent();
        var store = view.getData();
        var copyEntity = view.copyEntity(entity);

        copyEntity.data.Qty = 0;
        copyEntity.data.QualityStatus = null;
        copyEntity.data.ReturnType = null;
        copyEntity.data.CreateBy = 0;
        copyEntity.data.CreateDate = null;
        copyEntity.data.UpdateBy = 0;
        copyEntity.data.UpdateDate = null;

        store.insert(store.indexOf(entity) + 1, copyEntity);

        view.mon(copyEntity, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;

        if (e.property == 'Qty') {

            if (e.entity.data.Qty > 0) {

                me.getControl().getSelectionModel().select(e.entity, true);
            }
            else {
                me.getControl().getSelectionModel().deselect(e.entity);
            }
        }
    }
});