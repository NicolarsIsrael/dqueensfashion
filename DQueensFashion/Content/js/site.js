
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

function AddToCartOutfits(productId) {

    $.ajax({
        url: "/Cart/AddToCartOutfits/" + productId,
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

function AddToCartOutfitsPost() {

    var form = $('#addToCartOutfitsForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

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
        url: "/Cart/AddToCartOutfits",
        data: {
            __RequestVerificationToken: token,
            cartModel: cartModel,//JSON.stringify(cartModel),
        },
        type: "POST",
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
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

function RequestForProduct(productId) {

    $.ajax({
        url: "/Wishlist/RequestForProduct/" + productId,
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

function RequestForProductPost() {

    var customerEmail = $('#customerEmail').val();

    if (customerEmail != "" && customerEmail != null) {
        var form = $('#addToCartOutfitsForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        var productId = $('#ProductId').val();
        var quantity = $('#Quantity').val();

        $.ajax({
            url: "/Wishlist/RequestForProduct",
            data: {
                __RequestVerificationToken: token,
                productId: productId,
                quantity: quantity,
                customerEmail: customerEmail,
            },
            type: "POST",
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            success: function (result) {

                if (result == "success") {
                    ShowSnackbarSuccess("You have successfully requested for the product");
                    closeNav();
                } else if (result == "Invalid") {
                    ShowSnackbarMessage("Enter valid email address");
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

function ShopByBestSelling() {
    window.location.href = "/Product/Shop?sort=2";
}

function ShopByBestDeals() {
    window.location.href = "/Product/Shop?sort=1";
}

function NavigateToProductDetails(url) {
    window.location.href = '/Product/ProductDetails/' + url;
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

//account/login
function ShowPassword() {
    var passwordInput = document.getElementById("passwordInput");
    var passwordLockIcon = document.getElementById("password-lock-icon");

    if (passwordInput.type === "password") {
        passwordInput.type = "text";
    } else {
        passwordInput.type = "password";
    }

    passwordLockIcon.classList.toggle("fa-eye-slash");
}


//account/register
function ShowConfirmPassword() {
    var ConfirmPasswordInput = document.getElementById("ConfirmPasswordInput");
    var ConfirmPasswordLockIcon = document.getElementById("confirm-password-lock-icon")
    if (ConfirmPasswordInput.type === "password") {
        ConfirmPasswordInput.type = "text";
    } else {
        ConfirmPasswordInput.type = "password";
    } 

    ConfirmPasswordLockIcon.classList.toggle("fa-eye-slash");
}

//customer/changepassword
function ShowOldPassword() {
    var OldPasswordInput = document.getElementById("OldPasswordInput");
    var OldPasswordLockIcon = document.getElementById("old-password-lock-icon")
    if (OldPasswordInput.type === "password") {
        OldPasswordInput.type = "text";
    } else {
        OldPasswordInput.type = "password";
    }

    OldPasswordLockIcon.classList.toggle("fa-eye-slash");
}

//faq toggle
function ToggleIcon(plusIconId, minusIconId) {
    var faqTogglePlusIcon = document.getElementById(plusIconId);
    var faqToggleMinusIcon = document.getElementById(minusIconId);

    if (faqTogglePlusIcon.style.display === "none") {
        faqTogglePlusIcon.style.display = "inline-block";
        faqToggleMinusIcon.style.display = "none";
    } else {
        faqTogglePlusIcon.style.display = "none";
        faqToggleMinusIcon.style.display = "inline-block";
    }

}