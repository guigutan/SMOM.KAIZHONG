Ext.define('SIE.Equipments.Enums.ApprovalStatus', {
    statics: {
        Draft: { value: 10, text: 'Draft', label: '待提交' },
        PendingReview: { value: 20, text: 'PendingReview', label: '待审核' },
        UnderReview: { value: 30, text: 'UnderReview', label: '审核中' },
        Audited: { value: 40, text: 'Audited', label: '已审批' },
        Reject: { value: 50, text: 'Reject', label: '驳回' }
    }
});