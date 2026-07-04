SIE.defineCommand('SIE.Web.Elec.MES.TurnoverTools.Commands.TurnoverToolCopyCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "icon-ContentCopy icon-green" },
    getEditEntity: function () {//需重写，不然会复制子列表
        var view = this.view;
        var c = view.getCurrent();
        var copyEntity = new view._model();
        copyEntity.setCode(c.getCode() + '-复制'.t());
        copyEntity.setName(c.getName() + '-复制'.t());
        copyEntity.setToolType(c.getToolType()); 
        copyEntity.setModelId(c.getModelId());
        copyEntity.setModelId_Display(c.getModelId_Display());
        copyEntity.setState(5);
        view.getData().insert(0, copyEntity);
        copyEntity.isCopy = true;
        return copyEntity;
    }
});