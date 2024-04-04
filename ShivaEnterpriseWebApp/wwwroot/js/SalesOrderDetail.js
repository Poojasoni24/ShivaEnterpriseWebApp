var ProductId, BrandId;
let updatedPOs = []
$(document).ready(function () {
    var counter = 0;
    $("#tblSalesOrderDetail tbody tr").each(function () {
        counter++;
        var self = $(this);
        self.addClass("row_" + counter);
        var tdCounter = 0;

        self.find("input[value='Remove']").addClass("d-none");
        self.find('th').each(function () {
            self.find(".enable_disable").removeClass("d-none");
        });
        self.find('td').each(function () {
            tdCounter++;
            $(this).addClass("row_" + counter + tdCounter);
            self.find("#enable_disable").removeClass("d-none");
            self.find("#enable_disable").addClass("d-block");
            self.find('#enable_disable').on('change', function () {
                //var selectedCheckboxes = $('#enable_disable:checked');
                var isChecked = $(this).prop('checked');
                var qtycell = $(this).closest("tr").find(".qty-cell");
                var unitpricecell = $(this).closest("tr").find(".unitprice-cell");
                var discountcell = $(this).closest("tr").find(".discount-cell");
                if (isChecked) {

                    var qtycontent = qtycell.text();
                    if (qtycontent != '') {
                        qtycell.html("<input type='text' id='txtQty' value='" + qtycontent.trim() + "' />");
                    }

                    var unitpricecontent = unitpricecell.text();
                    if (unitpricecontent != '') {
                        unitpricecell.html("<input type='text' id='txtUnitprice' value='" + unitpricecontent.trim() + "' />");
                    }

                    var discountcontent = discountcell.text();
                    if (discountcontent != '') {
                        discountcell.html("<input type='text' id='txtDiscount' value='" + discountcontent.trim() + "' />");
                    }

                    self.find("#btnupdate").removeClass("d-none");
                    //self.find("#btnupdate").addClass("d-block");

                    // Now you have the row, you can perform further operations as needed
                }
                else {
                    var qtycontent = qtycell.find("input[type='text']").val();
                    qtycell.html(qtycontent);

                    var unitpricecontent = unitpricecell.find("input[type='text']").val();
                    unitpricecell.html(unitpricecontent);

                    var discountcontent = discountcell.find("input[type='text']").val();
                    discountcell.html(discountcontent);

                    self.find("#btnupdate").addClass("d-none");
                    //self.find("#btnupdate").removeClass("d-block");
                }
            });
        });
    });
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
    $("#sodetaildiv").on("click", "#btnupdate", function () {

        var salesOrderDetailIdcell = $(this).closest("tr").find(".salesOrderDetailId-cell");
        var salesOrderIdcell = $(this).closest("tr").find(".salesOrderId-cell");
        var salesIdcell = $(this).closest("tr").find(".productId-cell");
        var BrandIdcell = $(this).closest("tr").find(".brandId-cell");
        var qtycell = $(this).closest("tr").find(".qty-cell");
        var unitpricecell = $(this).closest("tr").find(".unitprice-cell");
        var discountcell = $(this).closest("tr").find(".discount-cell");
        var netTotalcell = $(this).closest("tr").find(".nettotal-cell");
        //var CreatedBy = $(this).closest("tr").find(".createdby-cell");
        //var CreatedDt = $(this).closest("tr").find(".createddate-cell");

        var salesOrderDetailIdContent = salesOrderDetailIdcell.text();
        var salesOrderIdContent = salesOrderIdcell.text();
        var productIdContent = productIdcell.text();
        var brandIdContent = BrandIdcell.text();
        //var createdByContent = CreatedBy.text();
        //var createdDtContent = CreatedDt.text();

        var qtyContent = qtycell.find("input[type='text']").val();
        qtycell.html(qtyContent);

        var unitpriceContent = unitpricecell.find("input[type='text']").val();
        unitpricecell.html(unitpriceContent);

        var discountContent = discountcell.find("input[type='text']").val();
        discountcell.html(discountContent);

        var netTotalContent = netTotalcell.text();

        let SalesOrderDetail = {
            SalesOrderDetailId: salesOrderDetailIdContent,
            SalesOrderId: salesOrderIdContent,
            ProductId: productIdContent,
            BrandId: brandIdContent,
            Quantity: qtyContent,
            Discount: discountContent,
            UnitPrice: unitpriceContent,
            NetTotal: netTotalContent,
            //CreatedBy: createdByContent,
            //CreatedDateTime: createdDtContent,
        }
        updatedSOs.push(SalesOrderDetail);
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

$("#btnSalesSave").on("click", function () {
    //Loop through the Table rows and build a JSON array.
    let Sodetails = [];
    $("#tblSalesOrderDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        let SODetail = {
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
    Sodetails = Sodetails.filter(val => !updatedSOs.includes(val));
    let SOHeader = {
        VendorID: $('#vendorID').val(),
        OrderDate: $('#orderDate').val(),
        DeliveryDate: $('#deliveryDate').val(),
        TotalAmount: $('#totalNumber').val(),
        Doc_No: $('#docNo').val(),
        Tax_Percentage: $('#tax').val(),
    }

    let SalesOrderViewModel = {
        SalesOrder: SOHeader,
        SODetail: Sodetails,
        UpdatedSODetail: updatedSOs
    }
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