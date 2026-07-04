Ext.define('SIE.Web.Core.UserAgreementController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.useragreement',

    /**
     * 获取协议
     * @param {any} agreementId
     */
    getFile: function (agreementId, token, callback) {
        SIE.invokeDataQuery({
            action: 'queryer',
            type: 'SIE.Web.Core.UserAgreements.DataQueryers.UserAgreementDataQueryer',
            method: 'GetUserAgreementAttach',
            params: [agreementId],
            token: token,
            success: function (res) {
                if (res.Result) {
                    var data = res.Result;
                    callback(data);
                }
            }
        });
    }
});