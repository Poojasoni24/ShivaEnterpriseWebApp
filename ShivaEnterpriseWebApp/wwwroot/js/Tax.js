$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteTax(Tax) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Tax.TaxName} ?`,
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
            deleteTaxAJAX(Tax.TaxId);
        }
    });
}

//AJAX call for delete organization
function deleteTaxAJAX(ModId) {
    debugger;
    $.ajax({
        url: `/Tax/RemoveTax`,
        type: 'POST',
        data: { taxId: taxId },
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
function loadTaxPartial(taxId) {
    $.ajax({
        url: `/Tax/TaxDetail?taxId=${taxId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#TaxDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
        },
        async: true,
        error: function (err) {
        }
    });

}