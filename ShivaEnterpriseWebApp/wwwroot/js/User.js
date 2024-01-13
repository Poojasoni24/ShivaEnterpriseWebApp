
//#region DELETE

//On Click of delete organization.
function onDeleteUser(data) {
    debugger;
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${data.UserName} ?`,
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
            deleteUserAJAX(data.Id);
        }
    });
}

//AJAX call for delete organization
function deleteUserAJAX(userId) {
    $.ajax({
        url: `/User/RemoveUser`,
        type: 'POST',
        data: { userId: userId },
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
function loadUserPartial(userId) {
    debugger;
    $.ajax({
        url: `/User/UserDetails?userId=${userId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#exampleModalLong').modal('show')
            $('#UserDetailDiv').html(res)
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