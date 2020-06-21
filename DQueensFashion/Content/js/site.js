
function AddToCart(productId, _quantity) {
    
    $.ajax({
        url: '/Cart/addtocart/',
        dataType: "html",
        data: { id: productId, quantity:_quantity },
        success: function (result) {
            $("#navbarCartNumber").html(result);
            alertify.success("Product added to cart");
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}

function AddToCartCustomMade(productId) {

    var url = "/Cart/AddToCartCustomMade/" + productId;

    $("#addToCartCustomMadeBody").load(url, function () {
        $("#addToCartCustomMade").modal("show");

    })

}

function AddToCartCustomMadePost() {
    var cartModel = {
        ProductId: $('#ProductId').val(),
        Quantity: $('#Quantity').val(),
        BurstSizeValue: $('#BurstSizeValue').val(),
        ShoulderLengthValue: $('#ShoulderLengthValue').val(),
        WaistLengthValue: $('#WaistLengthValue').val(),
    }

    $.ajax({
        url: "/Cart/AddToCartCustomMade",
        data: JSON.stringify(cartModel),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "html",
        success: function (result) {
            $("#navbarCartNumber").html(result);
            $('#addToCartCustomMade').modal('hide');
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}

function EditCartCustomMade(productId) {
    var url = "/Cart/EditCartCustomMade/" + productId;

    $("#addToCartCustomMadeBody").load(url, function () {
        $("#addToCartCustomMade").modal("show");

    })
}

function EditCartCustomMadePost() {
    var cartModel = {
        ProductId: $('#ProductId').val(),
        Quantity: $('#Quantity').val(),
        BurstSizeValue: $('#BurstSizeValue').val(),
        ShoulderLengthValue: $('#ShoulderLengthValue').val(),
        WaistLengthValue: $('#WaistLengthValue').val(),
    }

    $.ajax({
        url: "/Cart/EditCartCustomMade",
        data: JSON.stringify(cartModel),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "html",
        success: function (result) {
            UpdateCartNumber();
            $(".viewCartTable").html(result);
            $('#addToCartCustomMade').modal('hide');
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}

function UpdateCartNumber() {

    $.ajax({
        url: '/Cart/getcart/',
        dataType: "html",
        data: { },
        success: function (result) {
            $("#navbarCartNumber").html(result);
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}


function AddToWishList(productId) {

    $.ajax({
        url: '/wishlist/addtowishlist/' + productId,
        data: { id: productId },
        success: function (result) {
            console.log(result);
            if (result == "success") {
                alertify.success("Item successfully added");
            } else if (result == "login") {
                alertify.success("Login or register to add item to wish list");
            }
        },
        error: function (xhr, status, error) {
            alertify.error("An error occured");
        }
    });

}


function ShopByCategory(categoryId) {
    window.location.href = "/Product/Index?categoryId=" + categoryId;
}

function NavigateToProductDetails(productId){
    window.location.href = '/Product/ProductDetails/' + productId;
}

function productQuickView(productId) {
    var url = "/Product/ProductQuickView/" + productId;

    $("#productQuickViewBody").load(url, function () {
        $("#productQuickView").modal("show");

    })
}

function SetProductTotalPrice(price,discount) {
    price = Number(price);
    discount = Number(discount);

    var totalPrice = price * (1 - discount / 100);
    if (totalPrice > price)
        totalPrice = price;

    if (totalPrice < 0)
        totalPrice = 0;

    document.getElementById("product-total-price").value = totalPrice;
}

function CalculateTotalQuantity() {
    return Number(xsQty.value) + Number(smQty.value)
                    + Number(mQty.value) + Number(lQty.value)
                    + Number(xlQty.value);
}