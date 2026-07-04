Ext.define('SIE.Web.Elec.QMS.Control.EmployeeComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.employeecombopopup',
    /**
    * 确定事件
    * @param btn--
    * @returns
    */
    onpopupWinbtn: function (btn) {
        var me = this;
        if (btn === '确定'.t()) {
            var entity;
            me.setMULTIValue();
            if (!me.up("form"))
                entity = me.up("container").context.record;
            else
                entity = me.up("form").SIEView.getData();
            me._win.hide();
            return true; //阻止窗口关闭，在save中根据返回结果处理
        } else if (btn === '取消'.t()) {
            me.isCanceling = true;
            return true;
        }
    }
});