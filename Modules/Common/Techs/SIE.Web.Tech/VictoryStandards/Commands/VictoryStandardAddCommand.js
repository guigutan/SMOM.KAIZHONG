SIE.defineCommand('SIE.Web.Tech.VictoryStandards.Commands.VictoryStandardAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit" },
    onItemCreated: function (entity) {
        entity.setMaxTestQty(1);
        entity.setState(0);
    }
});