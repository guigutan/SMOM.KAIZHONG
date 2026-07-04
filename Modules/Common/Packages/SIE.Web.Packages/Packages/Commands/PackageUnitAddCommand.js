SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageUnitAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "新增主单位", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    viewtoken: null,
    canExecute: function (view) {
        this.viewtoken = view.token;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.invokeDataQuery({
            method: 'IsExistMasterUnit',
            action: 'queryer',
            async: false,
            token: this.viewtoken,
            type: 'SIE.Web.Packages.Packages.PackageUnitDataQuery',
            success: function (res) {
                if (res.Result) {
                    Ext.Msg.alert('提示', '已存在主单位');
                    return false;
                }
                var editEntity = me.getEditEntity();
                me.onEditting(editEntity);
                me.edit(editEntity);
                me.onEdited(editEntity);
            }
        });
    },
    onItemCreated: function (entity) {//添加主单位
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {//成功就给添加的新行赋值
                    var data = res.Result;
                    entity.setCode(data.Code);
                    entity.setName(data.Name);
                    entity.setDescription(data.Description);
                    entity.setIsMasterUnit(data.IsMasterUnit);
                },
                error: function () {
                    me.view.reloadData();
                }
            }, me.view);
        }
    },
});

