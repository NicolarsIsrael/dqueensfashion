
$.ajax({
    url: '/home/getcategories',
    dataType: "html",
    data: {},
    success: function (result) {
        $("#navbarCategories").html(result);
    },
    error: function (xhr, status, error) {
        alertify.error("Error occured");
    }
});

$.ajax({
    url: '/Cart/getcart/',
    dataType: "html",
    data: {},
    success: function (result) {
        $("#navbarCart").html(result);
    },
    error: function (xhr, status, error) {
        alertify.error("Error occured");
    }
});


function AddToCart(productId, _quantity) {
    
    $.ajax({
        url: '/Cart/addtocart/',
        dataType: "html",
        data: { id: productId, quantity:_quantity },
        success: function (result) {
            $("#navbarCart").html(result);
            alertify.success("Product added to cart");
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}

function GetCart() {

    $.ajax({
        url: '/Cart/getcart/',
        dataType: "html",
        data: { },
        success: function (result) {
            $("#navbarCart").html(result);
        },
        error: function (xhr, status, error) {
            alertify.error("Error");
        }
    });
}

function RemoveFromCart(productId) {

    $.ajax({
        url: '/Cart/removefromcart/' + productId,
        dataType: "html",
        data: { id: productId },
        success: function (result) {
            $("#navbarCart").html(result);
            alertify.success("Product removed from cart");
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