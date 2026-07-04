SIE.defineCommand('SIE.Web.Kit.MES.Storages.Commands.StorageAreaCopyCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    isCopy: true,
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        return true;
    },   
    execute: function (view, source) {
        var me = this;
        var view = this.view;
        var c = view.getCurrent();
        var model = SIE.getModel('SIE.Kit.MES.Storages.StorageArea');
        var newModel = new model();
        newModel.setCode(c.data.Code + "-复制");
        newModel.setName(c.data.Name + "-复制");
        newModel.setType(c.data.Type);
        newModel.setWarehouseId(c.data.WarehouseId);
        newModel.setWarehouseId_Display(c.data.WarehouseId_Display);
        newModel.setWarehouseName(c.data.WarehouseName);
        var editmode = view.editMode;
        if (editmode === SIE.viewMeta.editMode.INLINE) {
            view.getData().insert(0, newModel);
        }
        newModel.isCopy = true;
        return view.newModel;
    },
});