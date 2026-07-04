/**
 * 配送管理属性值添加命令
 */
SIE.defineCommand('SIE.Web.DIST.GoodsIssue.Commands.GoodsIssuePropertyValueAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parent = this.view.getParent().getCurrent();
        if (parent && parent.data && parent.data.ItemId !== null) {
            return true;
        }
        return false;
    },
    /**
     * 添加按钮错误验证
     * @param {any} view
     * @param {any} source
     */
    execute: function (view, source) {
        var me = this;
        var itemId = this.view.getParent().getCurrent().data.ItemId;
        //if (this.view.getParent().getCurrent().data.ItemId == null) {
        //    Ext.Msg.alert('提示', '未选择物料');
        //    return false;
        //}

        SIE.invokeDataQuery({
            method: 'IsExistProperty',
            action: 'queryer',
            async: false,
            token: me.view.token,
            params: [itemId],
            type: 'SIE.Web.Items.ViewModels.PropertyValueDataQueryer',
            success: function (res) {
                if (res.Result) {
                    Ext.Msg.alert('提示', '当前物料没有配置物料属性，请先在物料中配置');
                    return false;
                }
                var editEntity = me.getEditEntity();
                me.onEditting(editEntity);
                me.edit(editEntity);
                me.onEdited(editEntity);
            }
        });

    },
    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var productBomId = this.view.getParent().getCurrent().data.Id;
        entity.ParentId = productBomId;
        entity.data.ItemId = this.view.getParent().getCurrent().data.ItemId;
    }
});