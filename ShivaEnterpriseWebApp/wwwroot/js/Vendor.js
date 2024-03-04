
//On Click of delete organization.
function onDeleteVendor(Vendor) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Vendor.VendorName} ?`,
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
            deleteVendorAJAX(Vendor.VendorId);
        }
    });
}

//AJAX call for delete organization
function deleteVendorAJAX(vendorId) {
    debugger;
    $.ajax({
        url: `/Vendor/RemoveVendor`,
        type: 'POST',
        data: { vendorId: vendorId },
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
function loadVendorPartial(vendorId) {
    $.ajax({
        url: `/Vendor/VendorDetail?VendorId=${vendorId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#vendorModalLong').modal('show')
            useJQueryNoConflict();
            $('#vendorDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
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