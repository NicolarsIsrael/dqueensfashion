
function AddToCart(productId, _quantity) {
    
    $.ajax({
        url: '/Cart/addtocart/',
        dataType: "html",
        data: { id: productId, quantity:_quantity },
        success: function (result) {
            $("#navbarCartNumber").html(result);
            ShowSnackbarSuccess("Item added to cart successfully");
        },
        error: function (xhr, status, error) {
            ShowSnackbarError("Oops, sorry! Error");
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
            ShowSnackbarError("Oops, sorry! Error");
        }
    });

}

function AddToCartCustomReadyMadePost() {

    var cartModel = {
        ProductId: $('#ProductId').val(),
        Quantity: $('#Quantity').val(),

        ShoulderValue: $('#ShoulderValue').val(),
        ArmHoleValue: $('#ArmHoleValue').val(),
        BustValue: $('#BustValue').val(),
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
            ShowSnackbarSuccess("Item added to cart successfully");
        },
        error: function (xhr, status, error) {
            ShowSnackbarError("Oops, sorry! Error");
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
            ShowSnackbarError("Oops, sorry! Error");
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
                ShowSnackbarSuccess("Item added to wishlist successfully");
            } else if (result == "login") {
                ShowSnackbarMessage("Login or register to add item to wish list");
            }
        },
        error: function (xhr, status, error) {
            ShowSnackbarError("Oops, sorry! Error");
        }
    });

}


function ShopByCategory(categoryId) {
    window.location.href = "/Product/Shop?categoryId=" + categoryId;
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
                    ShowSnackbarSuccess("Thank you for subscribing");
                }
                else if (result == "Invalid") {
                    ShowSnackbarError("Invalid email address");
                }
                else {
                    ShowSnackbarError("Oops, sorry! Error");
                }
            },
            error: function (xhr, status, error) {
                ShowSnackbarError("Oops, sorry! Error");
            }
        });
    } else {
        ShowSnackbarMessage("Enter email address");
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
                ShowSnackbarSuccess("You have successfully unsubscribe");
            }
            else {
                ShowSnackbarError("Oops, sorry! Error");
            }
        },
        error: function (xhr, status, error) {
            ShowSnackbarError("Oops, sorry! Error");
        }
    });
}

function ShowSnackbarSuccess(message) {
    RemoveAllSnack();
    var x = document.getElementById("snackbar-success");
    x.innerHTML = message;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 5000);
    console.log(message);
}

function ShowSnackbarError(message) {
    RemoveAllSnack();
    var x = document.getElementById("snackbar-error");
    x.innerHTML = message;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 5000);
}

function ShowSnackbarMessage(message) {
    RemoveAllSnack();
    var x = document.getElementById("snackbar-message");
    x.innerHTML = message;
    x.className = "show";
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 5000);
}

function RemoveAllSnack() {
    var snackbarSuccess = document.getElementById("snackbar-success");
    var snackbarError = document.getElementById("snackbar-error");
    var snackbarMessage = document.getElementById("snackbar-message");

    snackbarSuccess.className = snackbarSuccess.className.replace("show", "");
    snackbarError.className = snackbarError.className.replace("show", "");
    snackbarMessage.className = snackbarMessage.className.replace("show", "");
}