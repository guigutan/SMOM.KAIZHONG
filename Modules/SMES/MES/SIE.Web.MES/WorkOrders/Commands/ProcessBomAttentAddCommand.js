SIE.defineCommand('SIE.Web.MES.WorkOrders.Commands.ProcessBomAttentAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    /**
           * 实体数据创建后处理
           * @param entity 当前实体      
           */
    onItemCreated: function (entity) {
        var me = this;
        var datas = this.view.getData();
        var maxIndex = this.view.getData().data.items.max(function (p) { return p.data.INDEX_; });
        entity.setINDEX_(maxIndex + 1);
    },
});
