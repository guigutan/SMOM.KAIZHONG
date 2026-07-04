//SIE:classEnd
Ext.define('SIE.Equipments.Enums.ApprovalStatus', {
    statics: {
        Draft: { value: 10, text: 'Draft', label: '待提交' },
        PendingReview: { value: 20, text: 'PendingReview', label: '待审核' },
        UnderReview: { value: 30, text: 'UnderReview', label: '审核中' },
        Audited: { value: 40, text: 'Audited', label: '已审批' },
        Reject: { value: 50, text: 'Reject', label: '驳回' }
    }
});

Ext.define('SIE.EMS.Enums.InspectionStatus', {
    statics: {
        Pending: { value: 10, text: 'Pending', label: '待检验' },
        Under: { value: 20, text: 'Under', label: '检验中' },
        Calirated: { value: 30, text: 'Calirated', label: '已校验' }
    }
});