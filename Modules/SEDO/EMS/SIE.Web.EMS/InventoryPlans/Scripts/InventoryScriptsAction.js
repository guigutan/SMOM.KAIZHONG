Ext.define("SIE.Web.EMS.InventoryPlans.InventoryScriptsAction", {
    statics: {
        findEditor: function (form, property) {
            /// <summary>
            /// 根据属性名称来查找对象的 ext field 对象,查找盘点范围的基本筛选里的控件
            /// </summary>
            /// <param name="property">属性名称</param>
            var editors = form.items.items;
            for (var i = 0; i < editors.length; i++) {
                debugger;
                var editor = editors[i];
                if (editor.name == property) {
                    return editor;
                }
            }
            return null;
        },
    }
});