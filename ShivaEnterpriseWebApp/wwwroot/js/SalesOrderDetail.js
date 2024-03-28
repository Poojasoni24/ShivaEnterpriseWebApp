var ProductId, BrandId;
$("#sodetaildiv").on("click", "#btnAdd", function () {
    debugger;
    //Reference the Name and Country TextBoxes.
    var txtsoproduct = $("#ProductId").find(":selected").text();
    ProductId = $("#ProductId").find(":selected").val();
    var txtsoBrand = $("#BrandId").find(":selected").text();
    BrandId = $("#BrandId").find(":selected").val();
    var txtsoQty = $("#txtsoQty");
    var txtsoUnitprice = $("#txtsoUnitprice");
    var txtsoDiscount = $("#txtsoDiscount");
    var txtsoNetTotal = $("#txtsonetTotal");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblSalesOrderDetail > TBODY")[0];

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
    cell.html(txtsoproduct);


    //Add Brand cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtsoBrand);


    //Add Qty cell.
    var cell = $(row.insertCell(-1));
    cell.html(txtsoQty.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoUnitprice.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoDiscount.val());

    //Add unitprice cell.
    cell = $(row.insertCell(-1));
    cell.html(txtsoNetTotal.val());

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    txtsoQty.val("");
    txtsoUnitprice.val("");
    txtsoDiscount.val("");
    txtsoNetTotal.val("");
    ProductId.val(0);
    BrandId.val(0);
});

function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblSalesOrderDetail")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("#btnSave").on("click", function () {
    //Loop through the Table rows and build a JSON array.
    let Podetails = [];
    $("#tblSalesOrderDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        let PODetail = {
            ProductId: row.find("TD").eq(0).html(),
            BrandId: row.find("TD").eq(1).html(),
            Quantity: row.find("TD").eq(4).html(),
            UnitPrice: row.find("TD").eq(5).html(),
            Discount: row.find("TD").eq(6).html(),
            NetTotal: row.find("TD").eq(7).html(),
        };
        //PODetail.Brand = row.find("TD").eq(1).html();
        Sodetails.push(SODetail);
    });

    //Send the JSON array to Controller using AJAX.
    $.ajax({
        url: "/SalesOrder/AddOrEditSalesOrder",
        type: "POST",
        contentType: 'application/json',
        data: JSON.stringify(Sodetails),
        success: function (r) {
            alert(r + " record(s) inserted.");
        }
    });
});

$('#txtsoUnitprice').on('change', function () {
    var qty = $("#txtsoQty").val();
    var unitprice = $("#txtsoUnitprice").val();
    var netPrice = qty * unitprice;
    $("#txtsonetTotal").val(netPrice);
});

$("#txtsoDiscount").on('change', function () {
    debugger;
    var discount = $("#txtsoDiscount").val();
    var netPrice = $("#txtsonetTotal").val();
    var discountAmount = netPrice * (discount / 100);
    $("#txtsonetTotal").val(netPrice - discountAmount);
});