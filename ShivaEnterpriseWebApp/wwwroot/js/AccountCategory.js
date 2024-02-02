$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteAccountCategory(AccountCategory) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${AccountCategory.AccountCategoryName} ?`,
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
            deleteBranchAJAX(AccountCategory.AccountCategoryId);
        }
    });
}

//AJAX call for delete organization
function deleteAccountCategoryAJAX(accountCategoryId) {
    debugger;
    $.ajax({
        url: `/AccountCategory/RemoveAccountCatgory`,
        type: 'POST',
        data: { accountCategoryId: accountCategoryId },
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
function loadAccountCategoryPartial(accountCategoryId) {
    $.ajax({
        url: `/AccountCategory/AccountCategoryDetail?accountCategoryId=${accountCategoryId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#AccountCategoryDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
        },
        async: true,
        error: function (err) {
        }
    });

}