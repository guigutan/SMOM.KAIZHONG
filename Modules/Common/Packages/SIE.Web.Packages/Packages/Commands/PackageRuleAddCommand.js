SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleAddCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },

    onItemCreated: function (entity) {
        var model = entity.data;
        var me = this;
        var masterUnit;
        var indata = {};
        indata.data = Ext.encode({ Code: '', Name: '', Description: '' });
        this.view.execute({
            data: indata,
            isSubmmit: false,
            success: function (res) {
                var masterUnit = res.Result;
                if (masterUnit) {
                    var parent = me.view.getCurrent();
                    var childrenView = me.view.getChildren()[0];
                    var store = childrenView.getData();
                    //自己创建一个单位行吧-_-!
                    var val = childrenView.addNew();
                    val.data.INDEX_ = -1;  //框架不给力，主单位的排序临时方案赋值-1
                    val.data.PackageUnit = masterUnit;
                    val.data.PackageUnitId = masterUnit.Id;
                    val.data.PackageUnitId_Display = masterUnit.Code;
                    val.data.Qty = 1;
                    val.data.LevelQty = 1;
                    val.data.PackageUnitName = masterUnit.Name;
                    val.data.IsMasterUnit = true;
                    val.data.PackageRuleId = parent.getId();
                    store.add(val);
                }
                
            }
        }, me.view);
    }
});