SIE.defineCommand('SIE.Web.Tech.Processs.Commands.AddProcessCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    /**
           * 实体数据创建后处理
           * @param entity 当前实体      
           */
    onItemCreated: function (entity) {
        var me = this;
        entity.mon(entity, 'propertyChanged', SIE.Web.Tech.ProcessCommonFun.ProcessPropertyChanged, me);
        if (me.view.getChildren().length <= 0) return;
        var tabPanel = me.view.getChildren()[0].getControl().up('tabpanel');
        var ppView = me.view.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessParameter' });
        if (ppView) { tabPanel.setActiveItem(ppView.getControl().up());}
        entity.setType(0);
    },
});
