

//#region DELETE

//On Click of delete organization.
function onDeleteTransport(Transport) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Transport.TransportName} ?`,
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
            deleteTransportAJAX(Transport.TransportId);
        }
    });
}

//AJAX call for delete organization
function deleteTransportAJAX(transportId) {
    debugger;
    $.ajax({
        url: `/Transport/RemoveTransport`,
        type: 'POST',
        data: { transportId: transportId },
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
function loadTransportPartial(transportId) {
    $.ajax({
        url: `/Transport/TransportDetail?transportId=${transportId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#TransportDetailDiv').html(res)
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