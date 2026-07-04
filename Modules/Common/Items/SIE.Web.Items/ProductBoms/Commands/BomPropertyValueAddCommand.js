/**
 * 产品BOM属性值添加命令
 */
SIE.defineCommand('SIE.Web.Items.ProductBoms.Commands.BomPropertyValueAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    /**
     * 是否允许执行
     * @param {any} view 当前视图
     */
    canExecute: function (view) {
        if (view.getParent().getSelection().length > 1) return false;//选定行才显示添加属性按钮
        var parent = view.getParent().getCurrent();
        if (parent !== null && !parent.phantom && parent.data.ProductId) { return true; }//父视图选择物料才允许添加属性以及新建的bom要先保存才能添加
        else { return false; }
    },
    /**
     * 初始化实体数据，供下拉关联对应属性类别
     * @param {any} entity 当前实体
     */
    onItemCreated: function (entity) {
        var me = this;
        var productBomId = this.view.getParent().getCurrent().data.Id;
        var itemId = this.view.getParent().getCurrent().data.ProductId;
        entity.ParentId = productBomId;
        entity.ItemId = itemId;
        me.view.getCurrent().data.ParentId = productBomId;
        me.view.getCurrent().data.ItemId = itemId;
    }
});