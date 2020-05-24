function AddToCart(productId) {

    $.ajax({
        url: '/Cart/addtocart/' + productId,
        dataType: "html",
        data: { id: productId},
        success: function (result) {
            $("#cartCount").html(result);
        },
        error: function (xhr, status, error) {
            alert("Error");
        }
    });
}

function GetCart() {

    $.ajax({
        url: '/Cart/getcart/',
        dataType: "html",
        data: { },
        success: function (result) {
            $("#cartCount").html(result);
        },
        error: function (xhr, status, error) {
            alert("Error");
        }
    });
}

function RemoveFromCart(productId) {

    $.ajax({
        url: '/Cart/removefromcart/' + productId,
        dataType: "html",
        data: { id: productId },
        success: function (result) {
            $("#cartCount").html(result);
        },
        error: function (xhr, status, error) {
            alert("Error");
        }
    });
}


window.onload = GetCart;