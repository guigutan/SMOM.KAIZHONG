SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.AddStoreDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    /**
    * 是否允许执行
    * @param {any} view 当前视图
    */
    canExecute: function (view) {
        if (view.getParent()._selection != null && view.getParent()._selection.length == 1 && view.getParent()._selection[0].getStoreStatus()==0) return true; else return false;//选定行才显示添加属性按钮
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: [],
                success: function (res) {
                    entity.setBatchNumber(res.Result);
                }
            }, me.view);
        }
    },
});