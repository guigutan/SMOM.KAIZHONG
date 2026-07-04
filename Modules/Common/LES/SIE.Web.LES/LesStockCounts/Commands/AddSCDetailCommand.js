SIE.defineCommand('SIE.Web.LES.LesStockCounts.Commands.AddSCDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "新增盘盈", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var parentCur = view.getParent().getCurrent();
        if (parentCur == null) return false;
        if (parentCur != null) {
            //SIE.Warehouses.CountState.Audit.value(审批)=10
            //SIE.Warehouses.CountState.PartCount.value(部分盘点)=30
            //SIE.Warehouses.CountState.FinishCount.value(已盘点)=40
            if (parentCur.data.State != 10 &&
                parentCur.data.State != 30 &&
                parentCur.data.State != 40)
                return false;
        }
        return true;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var list = me.view.getData().data.items.where(function (p) { return p.getLineNo() != ""; });
            if (list.length > 0) {
                var lineNo = list.select(function (p) { return parseInt(p.getLineNo()); }).max();
                entity.setLineNo((lineNo + 1).toString());
            }
            else {
                entity.setLineNo(1);
            }
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setCountDimension(data.CountDimension);
                    entity.setState(data.State);
                    entity.setQty(data.Qty);
                    entity.setDiffCountQty(data.DiffCountQty);
                    entity.setIsNewInventory(data.IsNewInventory);
                    var employee = CRT.Context.GlobalContext.getContext('userInfo');
                    entity.setCountById_Display(employee.Name);
                    entity.setCountById(employee.EmployeeId);
                    entity.setCountDate(new Date());
                }
            }, me.view);
            this.mon(entity, 'propertyChanged', SIE.Web.LES.LesStockCounts.Scripts.LesStockCountDtlAction.onEntityPropertyChanged, this);
        }
    },
});