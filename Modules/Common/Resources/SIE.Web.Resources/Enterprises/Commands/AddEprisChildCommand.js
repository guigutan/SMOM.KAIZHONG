SIE.defineCommand('SIE.Web.Resources.Enterprises.Commands.AddChildCommand', {
    extend: 'SIE.Web.Resources.Commands.AddChildCommand',
    meta: { text: "添加子", group: "edit" },
    execute: function (view, source) {
        var childNode = view.insertNewChild();
        childNode.generateId();
        view.getControl().setSelection(childNode);
        //if (childNode.parentNode) {
        //    childNode.setLevelId(childNode.parentNode.data.LevelId);
        //}

        //实体创建后更改库存组织ID
        childNode.setInvOrgId(CRT.Context.GlobalContext.getContext('orgInfo').Code);
        view.setCurrent(childNode);
    }
});