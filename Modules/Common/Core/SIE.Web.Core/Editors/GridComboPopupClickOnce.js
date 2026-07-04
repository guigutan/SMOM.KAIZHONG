Ext.define('SIE.Web.Core.Editors.GridComboPopupClickOnce', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.gridcombopopupclickonce',
   
    //进入编辑状态立即弹窗
    onFocusEnter: function (e) {
        this.onTriggerClick(this);
    }
});