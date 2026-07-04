SIE.defineCommand('SIE.Web.Tech.Routings.Commands.ImportRouting', {
    extend: 'SIE.Web.Tech.Common.Commands.ImportDataCommonCommand',
    meta: { text: "导入", group: "edit", iconCls: "icon-Upload icon-blue" },    
    /*
    * 导入数据成功--刷新窗体
    * @param   {*} view 列表视图
    * @returns  *  grid
    * 默认执行成功后，刷新当前视图
    * 子类可以根据具体情况覆写1.自行处理指定刷新子列表，2.父列表或者所有tab子视图列表 3.调用自定义命令
    * 如果是视图使用了客制化查询命令，必须重写此方法，然后重新执行客制化命令中的方法
    * 示例：view._relations[0]._target.getCommands().items[0].方法名;
    */
    _importSuccess: function (view) {
        view.reloadData();
        view.routingControl.filterRoutingTreeData(view.routingControl, '');
    },
});