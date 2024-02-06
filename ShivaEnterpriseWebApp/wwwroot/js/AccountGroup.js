$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteAccountGroup(AccountGroup) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${AccountGroup.AccountGroupName} ?`,
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
            deleteBranchAJAX(AccountGroup.AccountGroupId);
        }
    });
}

//AJAX call for delete organization
function deleteAccountGroupAJAX(accountGroupId) {
    debugger;
    $.ajax({
        url: `/AccountGroup/RemoveAccountGroup`,
        type: 'POST',
        data: { accountGroupId: accountGroupId },
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
function loadAccountGroupPartial(accountGroupId) {
    $.ajax({
        url: `/AccountGroup/AccountGroupDetail?accountGroupId=${accountGroupId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#accountgroupDetailDiv').html(res)
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