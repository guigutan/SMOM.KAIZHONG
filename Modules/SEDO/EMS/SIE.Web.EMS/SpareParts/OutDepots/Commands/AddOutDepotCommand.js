SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.AddOutDepotCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        return true;
    },
    createNewItem: function () {
        var item = this.view.createNewItem()//新增时，生成实体
        var token = this.view.token;//获取页面的token 
        var _view = this.view;
        SIE.invokeDataQuery({
            //请求的方法
            method: 'GetNo',
            //参数
            params: [],
            action: 'queryer',
            //后台的命名空间及类
            type: 'SIE.Web.EMS.SpareParts.OutDepots.DataQuerys.OutDepotViewDataQuery',
            //必须带上token
            token: token,
            //调用成功的回调函数
            success: function (res) {
                //把返回后台生成的编码，设置到实体上
                item.setNo(res.Result);
                //刷新页面信息（如果更新后，页面处理编辑状态，对应的字段必须要刷新，否则编辑中的控件不会有值）
                _view.refresh();
            }
        });
        return item;
    },
});