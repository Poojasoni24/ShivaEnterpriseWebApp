



//On Click of delete organization.
function onDeletePO(PurchaseOrder) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${PurchaseOrder.Doc_No} ?`,
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
            deletePOAJAX(PurchaseOrder.PurchaseOrderId);
        }
    });
}

//AJAX call for delete organization
function deletePOAJAX(purchaseOrderId) {
    $.ajax({
        url: `/PurchaseOrder/RemovePurchaseOrder`,
        type: 'POST',
        data: { purchaseOrderId: purchaseOrderId },
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

//#endregion

//Load branch Detail partial view

function modalclose() {
    window.location.reload();
}
function loadPOPartial(purchaseorderId) {
    $.ajax({
        url: `/PurchaseOrder/PurchaseOrderDetail?purchaseorderId=${purchaseorderId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#poModalLong').modal('show')
            useJQueryNoConflict();
            $('#purchaseOrderDetailDiv').html(res)
        },
        async: true,
        error: function (err) {
        }
    });

}

//Jquery No conflict 
function useJQueryNoConflict() {
    jQuery.noConflict();
}

