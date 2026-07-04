SIE.defineCommand('SIE.Web.Packages.Packages.Commands.ItemPackageDefautlRuleCommand', {
    meta: { text: "设为缺省", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var rule = view.getCurrent();
        if (rule == null) return false;
        return rule != null && !rule.getIsDefault();
    },
    execute: function (view, source) {
        var me = this;
        view.execute({
            data: view.getSelectionIds(),
            success: function (res) { //回调
                var curRule = view.getCurrent();

                var itemPackageRuleData = view.getData();
                if (itemPackageRuleData != null) {
                    if (itemPackageRuleData.getData().items.length > 0) {
                        for (var i = 0; i < itemPackageRuleData.getData().items.length; i++) {
                            var packageRuleData = itemPackageRuleData.getData().items[i];
                            if (packageRuleData.getId() === curRule.getId()) {
                                packageRuleData.setIsDefault(true);
                            } else if (packageRuleData.getIsDefault()) {
                                packageRuleData.setIsDefault(false);
                            }
                        }
                    }
                }

                view.reloadData();
            }
        });
    }
});