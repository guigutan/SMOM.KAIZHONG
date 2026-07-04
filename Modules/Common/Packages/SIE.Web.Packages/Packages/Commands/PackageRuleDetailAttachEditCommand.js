/*
 ** 包装规则单位附加信息的编辑命令（编辑一行让所有行变脏，方便验证数据）
 *   @class SIE.Web.Packages.Packages.Commands.PackageRuleDetailAttachEditCommand
 */
 
SIE.defineCommand('SIE.Web.Packages.Packages.Commands.PackageRuleDetailAttachEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length == 0) {
            return false;
        }
        return true;
    },
    
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            var me = this;
            var store = me.view.getData();
            for (var i = 0; i < store.data.length; i++) {
                store.data.items[i].dirty = true;
            }
        }
    }
});