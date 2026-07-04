SIE.defineCommand('SIE.Web.Resources.Employees.Commands.ForemanCommand', {
    meta: { text: "设为班组长", group: "edit", iconCls: "icon-PeopleSetting icon-blue" },
    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems.length === 0) {
            return false;
        }
        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.getEmployeeType() == 2) {
                return false;
            }
        }
        return true;
    },
    execute: function (view, source) {
        var entityList = this.view.getSelection();
        var list = [];
        if (entityList.length > 0) {
            for (var i = 0; i < entityList.length; i++) {
                list[i] = entityList[i].data;
            }
            var msg = Ext.String.format('确定设为班组长吗？'.t());
            SIE.Msg.askQuestion(msg, function () {
                view.execute({
                    data: list,
                    success: function (res) {
                        view.loadChildData(true);
                    }
                }, view);
            });
        }
    }
});