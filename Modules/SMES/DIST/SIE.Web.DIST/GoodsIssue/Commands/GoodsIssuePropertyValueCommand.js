SIE.defineCommand('SIE.Web.DIST.GoodsIssuePropertyValueCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getParent().getCurrent() == null) {//选中项修改
            return false;
        }
        var entity = view.getParent().getCurrent().data;
        if (entity.ItemId == null) {//选中项的配送数量小于等于0才能修改
            return false;
        }
        return true;
       
    },
    onItemCreated: function (entity) {
        var itemId = this.view.getParent().getCurrent().data.ItemId;
        var parentId = this.view.getParent().getCurrent().data.Id;
        entity.setParentId(parentId);
        entity.setItemId(itemId);
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                success: function (res) {
                    //var data = res.Result;
                    //entity.setCode(data.Code);
                    //res.data.set  ParentId
                    //ItemId
                }
            }, me.view);
        }
    }
})