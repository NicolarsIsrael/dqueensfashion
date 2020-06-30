
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

function AddToCartCustomReadyMade(productId) {

    $.ajax({
        url: "/Cart/AddToCartCustomReadyMade/" + productId,
        dataType: "html",
        data: {},
        success: function (result) {
            $("#sidebarbody").html(result);
            openNav();
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });

}

function AddToCartCustomReadyMadePost() {

    var cartModel = {
        ProductId: $('#ProductId').val(),
        Quantity: $('#Quantity').val(),

        ShoulderValue: $('#ShoulderValue').val(),
        ArmHoleValue: $('#ArmHoleValue').val(),
        BurstValue: $('#BurstValue').val(),
        WaistValue: $('#WaistValue').val(),
        HipsValue: $('#HipsValue').val(),
        ThighValue: $('#ThighValue').val(),
        FullBodyLengthValue: $('#FullBodyLengthValue').val(),
        KneeGarmentLengthValue: $('#KneeGarmentLengthValue').val(),
        TopLengthValue: $('#TopLengthValue').val(),
        TrousersLengthValue: $('#TrousersLengthValue').val(),
        RoundAnkleValue: $('#RoundAnkleValue').val(),
        NipNipValue: $('#NipNipValue').val(),
        SleeveLengthValue: $('#SleeveLengthValue').val(),
    }

    $.ajax({
        url: "/Cart/AddToCartCustomReadyMade",
        data: JSON.stringify(cartModel),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "html",
        success: function (result) {
            $("#navbarCartNumber").html(result);
            closeNav();
            alertify.success("Product added to cart");
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

function openNav() {
    document.getElementById("sidenav").style.width = "300px";
}

function closeNav() {
    document.getElementById("sidenav").style.width = "0";
}



function SubscribeToMailingList() {
    var subscribeEmail = document.getElementById("subscribeEmail");

    if (subscribeEmail.value != "" && subscribeEmail.value != null) {
        $.ajax({
            url: "/MailingList/Subscribe?email=" + subscribeEmail.value,
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (result) {
                if (result == "Success") {
                    subscribeEmail.value = "";
                    alertify.success("Thank you for subscribing");
                }
                else if (result == "Invalid") {
                    alertify.error("Invalid email address");
                }
                else {
                    alertify.error("Error");
                }
            },
            error: function (xhr, status, error) {
                alertify.error("Error");
            }
        });
    } else {
        alertify.error("Enter email address");
    }

}

function Unsubscribe() {

    var unsubscribeEmail = document.getElementById("unsubscribeEmail");
    $.ajax({
        url: "/MailingList/Unsubscribe?email=" + unsubscribeEmail.value,
        type: "POST",
        contentType: "application/json;charset=utf-8",
        success: function (result) {
            if (result == "Success") {
                location.reload();
                alertify.success("You have successfully unsubscribe");
            }
            else {
                alertify.error("Error");
            }
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}
