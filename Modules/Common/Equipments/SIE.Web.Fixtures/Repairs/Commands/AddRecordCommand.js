SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.AddRecordCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {
        var parent = view.getParent();
        if (parent != null && parent.getCurrent()&&parent.getCurrent().data.Result==null) {
            return true;
        }
        return false;
    },
});