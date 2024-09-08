// JavaScript source code

//On Click of delete organization.
function onDeletePOR(PurchaseOrderReturn) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${PurchaseOrderReturn.PurchaseOrder.Doc_No} ?`,
        //type: "warning",
        buttons: {
            yes: {
                text: "Delete",
                value: true
            },
            no: {
                text: "Cancel",
                value: false
            }
        },
        showCancelButton: false,
        confirmButtonColor: '#c10909'
    }).then(res => {
        if (res) {
            deletePORAJAX(PurchaseOrderReturn.PurchaseReturnId);
        }
    });
}

//AJAX call for delete organization
function deletePORAJAX(purchaseReturnId) {
    $.ajax({
        url: `/PurchaseOrderReturn/DeletePurchaseReturn`,
        type: 'POST',
        data: { PurchaseReturnId: purchaseReturnId },
        success: function (res) {
            if (res.success) {
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
            else {
                Snackbar.show({ text: "something went wrong", textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
                setTimeout(() => { window.location.reload() }, 1500);
            }
        },
        async: true,
        error: function (err) {
        }
    });
}