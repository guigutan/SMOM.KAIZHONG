
Ext.define('SIE.Web.ERPInterface.Scripts.StateColorStyle', {
    extend: 'SIE.grid.column.ComboBox',
    alias: 'widget.stateColorStyle',
    renderer: function (value, meta) {
        if (value == 2) {
            meta.tdCls = "icon-red";
            return "失败".t();
        }
        if (value == 3) {
            meta.tdCls = "icon-green";
            return "重试".t();
        }
        if (value == 0) {
          
            return "未处理".t();
        }
        if (value == 1) {
           
            return "已处理".t();
        }
        if (value == 4) {
            
            return "处理中".t();
        }
        if (value == 5) {
            
            return "放弃".t();
        }
         
    }
});