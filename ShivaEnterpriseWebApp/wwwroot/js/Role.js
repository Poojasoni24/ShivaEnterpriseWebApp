
//#region DELETE

//On Click of delete organization.
function onDeleteRole(data) {
    debugger;
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${data.Name} ?`,
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
            deleteRoleAJAX(data.Id);
        }
    });
}

//AJAX call for delete organization
function deleteRoleAJAX(roleId) {
    $.ajax({
        url: `/Role/RemoveRole`,
        type: 'POST',
        data: { roleId: roleId },
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
function loadUserPartial(roleId) {
    debugger;
    $.ajax({
        url: `/Role/RoleDetails?roleId=${roleId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#roleModalLong').modal('show')
            $('#RoleDetailDiv').html(res)
            useJQueryNoConflict();           
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