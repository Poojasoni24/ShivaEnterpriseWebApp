$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteAccountType(AccountType) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${AccountType.AccountTypeName} ?`,
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
            deleteBranchAJAX(AccountType.AccountTypeId);
        }
    });
}

//AJAX call for delete organization
function deleteAccountTypeAJAX(accountTypeId) {
    debugger;
    $.ajax({
        url: `/AccountType/RemoveAccountType`,
        type: 'POST',
        data: { accountTypeId: accountTypeId },
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
function loadAccountTypePartial(accountTypeId) {
    $.ajax({
        url: `/AccountType/AccountTypeDetail?accountTypeId=${accountTypeId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#AccountTypeDetailDiv').html(res)
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