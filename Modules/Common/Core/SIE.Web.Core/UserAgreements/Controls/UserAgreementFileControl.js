/**
 * 工艺路线树控件
 * @class SIE.Tech.RoutingTreeControl
 * @constructor
 */
Ext.define('SIE.Core.UserAgreementFileControl', {
    extend: 'Ext.panel.Panel',
    xtype: 'UserAgreementFile',
    id: "agreementFilePanel",
    controller: {
        type: 'useragreement'
    },
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 0,
    defaults: {
        bodyPadding: 0
    },
    region: 'center',
    border: 0,
    viewConfig: {
        stripeRows: true,  //背景间隔色
    },

    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 显示协议
     * @param {any} record
     */
    showAgreement: function (record) {
        var me = this;
        if (!record)
            return;
        var controller = this.controller;
        var token = this.mainView.getToken();
        var callback = function (result) {
            //获取文件后，显示
            me.renderFile(result, record);
        };
        controller.getFile(record.data.Id, token, callback);
    },

    /**
     * 显示
     * @param {any} result
     */
    renderFile: function (result, record) {
        if (this.items.length > 0) {
            this.removeAll();
        }
        var item = Ext.create({
            title: record.parentNode.data.text + record.data.text,
            xtype: 'panel',
            width: "100%",
            height: "100%",
            items: {
                xtype: 'box',
                autoEl: {
                    tag: 'iframe',
                    style: 'height: 100%; width: 100%',
                    src: result.FullFilePath
                }
            },
        });
        this.add(item);

    },

    /**清空内容 */
    clear: function () {
        this.removeAll();
    },

});
