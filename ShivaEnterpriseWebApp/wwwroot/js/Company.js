$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteCompany(Company) {
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Company.Company_Name} ?`,
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
            deleteCompanyAJAX(Company.Company_Id);
        }
    });
}

//AJAX call for delete organization
function deleteCompanyAJAX(companyId) {
    $.ajax({
        url: `/Company/RemoveCompany`,
        type: 'POST',
        data: { companyId: companyId },
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

//Load Company Detail partial view

function modalclose() {
    window.location.reload();
}
function loadPartial(companyId) {
    debugger;
    $.ajax({
        url: `/Company/Details?companyId=${companyId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            $('#CompanyDetailDiv').html(res)
            useJQueryNoConflict();
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