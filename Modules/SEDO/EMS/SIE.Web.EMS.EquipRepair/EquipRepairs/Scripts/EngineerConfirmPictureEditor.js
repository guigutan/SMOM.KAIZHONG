Ext.define('SIE.Web.EMS.EquipRepair.EquipRepairs.Scripts.EngineerConfirmPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'engineerPictureFieldContainer',
    alias: 'widget.engineerPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'engineerPicture',
            beforeRender: function () {
                this.src = "";
                this.width = 300;
                this.height = 300;
            },
            listeners: {
                el: {
                    click: 'onClick'
                }
            },
            onClick: function () {
            }
        }
    ],
});