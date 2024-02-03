$(document).ready(function () {
    // Your code here
});

//#region DELETE

//On Click of delete organization.
function onDeleteState(State) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${State.State_Name} ?`,
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
            deleteStateAJAX(State.State_Id);
        }
    });
}

//AJAX call for delete organization
function deleteStateAJAX(stateId) {
    debugger;
    $.ajax({
        url: `/State/RemoveState`,
        type: 'POST',
        data: { stateId: stateId },
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
function loadStatePartial(stateId) {
    $.ajax({
        url: `/State/stateDetail?stateId=${stateId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#stateModalLong').modal('show')
            useJQueryNoConflict();
            $('#stateDetailDiv').html(res)
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