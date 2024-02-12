

//#region DELETE

//On Click of delete organization.
function onDeleteModeOfPayment(ModeOfPayment) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${ModeOfPayment.MODName} ?`,
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
            deleteModeOfPaymentAJAX(ModeOfPayment.MODId);
        }
    });
}

//AJAX call for delete organization
function deleteModeOfPaymentAJAX(modId) {
    debugger;
    $.ajax({
        url: `/ModeOfPayment/RemoveModeofPayment`,
        type: 'POST',
        data: { modId: modId },
        success: function (res) {
            if (res.success) {
                debugger;
                Snackbar.show({ text: res.message, textColor: "#FF0000", pos: "bottom-center", showAction: false, backgroundColor: "#F6F2F5" });
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
function loadModeOfPaymentPartial(modId) {
    $.ajax({
        url: `/ModeOfPayment/ModeofPaymentDetail?modId=${modId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#modeofPaymentDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
        },
        async: true,
        error: function (err) {
        }
    });

}
function useJQueryNoConflict() {
    jQuery.noConflict();
}