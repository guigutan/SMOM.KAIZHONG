SIE.defineCommand('SIE.Web.Tech.Stations.Commands.StationItemAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    onItemCreated: function (entity) {//添加主单位
        if (entity) {
            var model = entity.data;
            var me = this;
            entity.setWarning(1);
            entity.setCapacity(1);
        }
    },
});

