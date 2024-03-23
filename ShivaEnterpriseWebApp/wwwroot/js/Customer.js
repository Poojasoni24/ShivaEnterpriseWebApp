
//#region DELETE

//On Click of delete organization.
function onDeleteCustomer(Customer) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Customer.AccontName} ?`,
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
            deleteBranchAJAX(Customer.CustomerId);
        }
    });
}

//AJAX call for delete organization
function deleteBranchAJAX(customerId) {
    debugger;
    $.ajax({
        url: `/Customer/RemoveCustomer`,
        type: 'POST',
        data: { customerId: customerId },
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
function loadCustomerPartial(customerId) {
    $.ajax({
        url: `/Customer/CustomerDetail?customerId=${customerId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#customerModalLong').modal('show')
            useJQueryNoConflict();
            $('#customerDetailDiv').html(res)
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