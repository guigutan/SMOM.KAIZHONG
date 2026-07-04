SIE.defineCommand('SIE.Web.Resources.Commands.AddChildCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加子", group: "edit" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var childNode = view.insertNewChild();
        childNode.generateId();
        view.getControl().setSelection(childNode);

        //实体创建后更改库存组织ID
        childNode.setInvOrgId(CRT.Context.GlobalContext.getContext('orgInfo').Code);
    }
});