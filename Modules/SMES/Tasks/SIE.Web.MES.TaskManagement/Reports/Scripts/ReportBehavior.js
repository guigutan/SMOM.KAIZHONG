/**
 * 生命周期，添加图库颜色提示
 * @class SIE.Web.MES.TaskManagement.Reports.ReportBehavior
 * @constructor
 */
Ext.define('SIE.Web.MES.TaskManagement.Reports.ReportBehavior', {
    /**
         * view生命周期函数--view生成前
         * @param {*} meta 实体视图元数据
         * @param {*} curEntity 当前操作实体(可空)
         */
    beforeCreate: function (meta, curEntity) {
        meta.gridConfig.dockedItems[1].items.push({
            xtype: 'tbtext',
            style: 'background: #66FF99; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;',
            text: '合格'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: #FF0000; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;',
            text: '不合格'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: #EDEDED; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;',
            text: '待报工'.t()
        });
    }
});