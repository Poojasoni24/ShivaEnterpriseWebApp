

//#region DELETE

//On Click of delete organization.
function onDeleteSalesmanAgent(SalesmanAgent) {
    debugger
    swal({
        title: "Are you sure?",
        text: `Are you sure to delete ${SalesmanAgent.Salesman_Name} ?`,
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
            deleteSalesmanAgentAJAX(SalesmanAgent.SalesmanAgentID);
        }
    });
}

//AJAX call for delete organization
function deleteSalesmanAgentAJAX(salemanAgentId) {
    debugger;
    $.ajax({
        url: `/SalesmanAgent/RemovesalesmanAgent`,
        type: 'POST',
        data: { salemanAgentId: salemanAgentId },
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
function loadSalesmanAgentPartial(salemanAgentId) {
    $.ajax({
        url: `/SalesmanAgent/SalesmanAgentDetail?salemanAgentId=${salemanAgentId}`,
        type: 'GET',
        success: function (res) {
            debugger;
            $('#salesmanagentModalLong').modal('show')
            useJQueryNoConflict();
            $('#salesmanagentDetailDiv').html(res)
            //document.getElementById("CompanyDetailDiv")
            //    .innerHTML += res
        },
        async: true,
        error: function (err) {
        }
    });

    //Jquery No conflict 
    function useJQueryNoConflict() {
        jQuery.noConflict();
    }




}