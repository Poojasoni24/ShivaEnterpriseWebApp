var ProductId, BrandId;
let updatedPOs = []
$(document).ready(function () {
    var counter = 0;
    $("#tblPurchaseOrderDetail tbody tr").each(function () {
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

    //$("#tblPurchaseOrderDetail tfoot tr td").each(function () {
    //    if ($('#enable_disable').prop('checked')) {
    //        $('#btnAdd').attr("disabled", true);
    //    }
    //    else {
    //        $('#btnAdd').attr("disabled", false);
    //    }
    //});
});

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


    //Add PurchaseOrderDetail cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add PurchaseOrder cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add CreatedBy cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add Createddate cell.
    var cell = $(row.insertCell(-1));
    cell.addClass("d-none");

    //Add ProductId cell.

    var cell = $(row.insertCell(-1));
    cell.html(ProductId);
    cell.addClass("d-none");


    //Add BrandId cell.
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

    //Add Discount cell.
    cell = $(row.insertCell(-1));
    cell.html(txtDiscount.val());

    //Add NetTotal cell.
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

$("#podetaildiv").on("click", "#btnupdate", function () {

    var purchaseOrderDetailIdcell = $(this).closest("tr").find(".purchaseOrderDetailId-cell");
    var purchaseOrderIdcell = $(this).closest("tr").find(".purchaseOrderId-cell");
    var productIdcell = $(this).closest("tr").find(".productId-cell");
    var BrandIdcell = $(this).closest("tr").find(".brandId-cell");
    var qtycell = $(this).closest("tr").find(".qty-cell");
    var unitpricecell = $(this).closest("tr").find(".unitprice-cell");
    var discountcell = $(this).closest("tr").find(".discount-cell");
    var netTotalcell = $(this).closest("tr").find(".nettotal-cell");
    var CreatedBy = $(this).closest("tr").find(".createdby-cell");
    var CreatedDt = $(this).closest("tr").find(".createddate-cell");

    var purchaseOrderDetailIdContent = purchaseOrderDetailIdcell.text();
    var purchaseOrderIdContent = purchaseOrderIdcell.text();
    var productIdContent = productIdcell.text();
    var brandIdContent = BrandIdcell.text();
    var createdByContent = CreatedBy.text();
    var createdDtContent = CreatedDt.text();
    var parts = createdDtContent.split(/[- :]/);
    // Note: Month is 0-based, so subtract 1 from the month value
    var dateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);

    var qtyContent = qtycell.find("input[type='text']").val();
    qtycell.html(qtyContent);

    var unitpriceContent = unitpricecell.find("input[type='text']").val();
    unitpricecell.html(unitpriceContent);

    var discountContent = discountcell.find("input[type='text']").val();
    discountcell.html(discountContent);

    var netTotalContent = netTotalcell.text();

    let PurchaseOrderDetail = {
        PurchaseOrderDetailId: purchaseOrderDetailIdContent,
        PurchaseOrderId:purchaseOrderIdContent,
        ProductId: productIdContent,
        BrandId: brandIdContent,
        Quantity: qtyContent,
        Discount: discountContent,
        UnitPrice: unitpriceContent,
        NetTotal: netTotalContent,
        CreatedBy: createdByContent,
        CreatedDateTime: dateTimeObject,
    }
    updatedPOs.push(PurchaseOrderDetail);
});
function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(2).html();
    if (confirm("Do you want to delete: " + name)) {
        //Get the reference of the Table.
        var table = $("#tblPurchaseOrderDetail")[0];

        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};

$("#btnSave").on("click", function () {
    //Loop through the Table rows and build a JSON array.
    let Podetails = [];
    let PoHeaders = [];
    var createdDtContent = $('#PurchaseOrder_CreatedDateTime').val();
    var parts = createdDtContent.split(/[- :]/);
    // Note: Month is 0-based, so subtract 1 from the month value
    var dateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);

    $("#tblPurchaseOrderDetail TBODY TR").each(function () {
        debugger;
        var row = $(this);
        var pOCreatedDt = row.find("TD").eq(5).html();
        var parts = pOCreatedDt.split(/[- :]/);
        // Note: Month is 0-based, so subtract 1 from the month value
        var POdetaildateTimeObject = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], parts[5]);
        let PODetail = {
            PurchaseOrderDetailId: row.find("TD").eq(0).html(),
            PurchaseOrderId: row.find("TD").eq(1).html(),
            ProductId: row.find("TD").eq(2).html(),
            BrandId: row.find("TD").eq(3).html(),
            CreatedBy: row.find("TD").eq(4).html(),
            CreatedDateTime:POdetaildateTimeObject,
            Quantity: row.find("TD").eq(8).html(),
            UnitPrice: row.find("TD").eq(9).html(),
            Discount: row.find("TD").eq(10).html(),
            NetTotal: row.find("TD").eq(11).html(),
        };
        //PODetail.Brand = row.find("TD").eq(1).html();
        Podetails.push(PODetail);
    });
    
    let POHeader = {
        PurchaseOrderId: $('#PurchaseOrder_PurchaseOrderId').val(),
        VendorID: $('#vendorID').val(),
        OrderDate: $('#orderDate').val(),
        DeliveryDate: $('#deliveryDate').val(),
        TotalAmount: $('#totalNumber').val(),
        Doc_No: $('#docNo').val(),
        Tax_Percentage: $('#tax').val(),
        CreatedBy: $('#PurchaseOrder_CreatedBy').val(),
        CreatedDateTime: dateTimeObject,
    }

    let PurchaseOrderViewModel = {
        PurchaseOrder: POHeader,
        PODetail: Podetails,
        UpdatedPODetail: updatedPOs
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