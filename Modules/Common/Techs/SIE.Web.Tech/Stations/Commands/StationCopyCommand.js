SIE.defineCommand('SIE.Web.Tech.Stations.Commands.StationCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-AddEntity icon-green" },
    getEditEntity: function () {
        var view = this.view;
        var c = view.getCurrent();
        view.copyEntityData = view.copyEntity(c);
        var copyEntity = view.copyEntityData;
        this._setCopyEntity(copyEntity.data);
        var editmode = view.editMode;
        if (editmode === SIE.viewMeta.editMode.INLINE) {
            view.getData().insert(0, copyEntity);
        }
        copyEntity.isCopy = true;
        copyEntity.setCode(c.data.Code + '-复制'.t());
        return view.copyEntityData;
    },
});