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


window.onload = GetCart;