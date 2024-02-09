
//#region DELETE

//On Click of delete organization.
function onDeleteAccount(Account) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Account.AccountName} ?`,
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
            deleteBranchAJAX(Account.AccountId);
        }
    });
}

//AJAX call for delete organization
function deleteBranchAJAX(accountId) {
    debugger;
    $.ajax({
        url: `/Account/RemoveAccount`,
        type: 'POST',
        data: { accountId: accountId },
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
function loadAccountPartial(accountId) {
    $.ajax({
        url: `/Account/AccountDetail?accountId=${accountId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#accountModalLong').modal('show')
            useJQueryNoConflict();
            $('#accountDetailDiv').html(res)
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