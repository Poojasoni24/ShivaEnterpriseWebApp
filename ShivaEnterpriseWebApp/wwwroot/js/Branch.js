$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteBranch(Branch) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${Branch.Branch_Name} ?`,
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
            deleteBranchAJAX(Branch.Branch_Id);
        }
    });
}

//AJAX call for delete organization
function deleteBranchAJAX(branchId) {
    debugger;
    $.ajax({
        url: `/Branch/RemoveBranch`,
        type: 'POST',
        data: { branchId: branchId },
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
function loadBranchPartial(branchId) {
    $.ajax({
        url: `/Branch/BranchDetails?branchId=${branchId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            useJQueryNoConflict();
            $('#branchDetailDiv').html(res)
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