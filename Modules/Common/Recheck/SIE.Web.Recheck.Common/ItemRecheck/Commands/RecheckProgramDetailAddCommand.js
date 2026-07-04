SIE.defineCommand('SIE.Web.Recheck.Common.ItemRecheck.Commands.RecheckProgramDetailAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var sort = this.view.getData().data.items.max(function (p) { return p.data.Sort });
            if (sort > 0) {
                entity.setSort(sort + 1);
            }
            else
                entity.setSort(1);
            entity.setRecheckSort("第" + model.Sort + "次");
        }
    },
});