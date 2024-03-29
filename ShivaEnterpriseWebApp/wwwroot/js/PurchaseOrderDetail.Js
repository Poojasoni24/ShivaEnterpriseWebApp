var ProductId,BrandId;
$("#podetaildiv").on("click", "#btnAdd", function () {
    debugger;
    //Reference the Name and Country TextBoxes.
    var txtproduct = $("#ProductId").find(":selected").text();
    ProductId = $("#ProductId").find(":selected").val();
    var txtBrand = $("#BrandId").find(":selected").text();
    BrandId = $("#BrandId").find(":selected").val();
    var txtQty = $("#txtQty");
    var txtUnitprice = $("#txtUnitprice");
    var txtDiscount = $("#txtDiscount");
    var txtNetTotal = $("#txtnetTotal");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblPurchaseOrderDetail > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add Product cell.
    var cell = $(row.insertCell(-1));
    cell.html(ProductId);
    cell.addClass("d-none");


    //Add Product cell.
    var cell = $(row.insertCell(-1));
    cell.html(BrandId);
    cell.addClass("d-none");

    //Add Product cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtproduct);


    //Add Brand cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtBrand);


    //Add Qty cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtQty.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtUnitprice.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtDiscount.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtNetTotal.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtQty.val("");
    txtUnitprice.val("");
    txtDiscount.val("");
    txtNetTotal.val("");
    ProductId.val(0);
    BrandId.val(0);
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblPurchaseOrderDetail")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("#btnSave").on("click",function () {
    //Loop through the Table rows and build a JSON array.
    let Podetails = [];
    let PoHeaders = [];
    $("#tblPurchaseOrderDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        let PODetail = {
            ProductId : row.find("TD").eq(0).html(),
            BrandId : row.find("TD").eq(1).html(),
            Quantity: row.find("TD").eq(4).html(),
            UnitPrice: row.find("TD").eq(5).html(),
            Discount : row.find("TD").eq(6).html(),
            NetTotal: row.find("TD").eq(7).html(),
        };
        //PODetail.Brand = row.find("TD").eq(1).html();
        Podetails.push(PODetail);
    });

    let POHeader = {
        VendorID: $('#vendorID').val(),
        OrderDate: $('#orderDate').val(),
        DeliveryDate: $('#deliveryDate').val(),
        TotalAmount: $('#totalNumber').val(),
        Doc_No: $('#orderDate').val(),
        Tax_Percentage: $('#tax').val(),
    }

    let PurchaseOrderViewModel = {
        PurchaseOrder : POHeader,
        PODetail : Podetails
    }
    PoHeaders.push(POHeader);
    //Send the JSON array to Controller using AJAX.
    $.ajax({
        url: "/PurchaseOrder/AddOrEditPurchaseOrder",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(PurchaseOrderViewModel),
        success: function (r) {
            alert(r + " record(s) inserted.");
        }
    });
});

$('#txtUnitprice').on('change', function () {
    var qty = $("#txtQty").val();
    var unitprice = $("#txtUnitprice").val();
    var netPrice = qty * unitprice;
    $("#txtnetTotal").val(netPrice);
});

$("#txtDiscount").on('change', function () {
    debugger;
    var discount = $("#txtDiscount").val();
    var netPrice = $("#txtnetTotal").val();
    var discountAmount = netPrice * (discount / 100);
    $("#txtnetTotal").val(netPrice - discountAmount);
});