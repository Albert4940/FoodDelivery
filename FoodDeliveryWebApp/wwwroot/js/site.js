// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function () {

    let cart = localStorage.getItem('cart')
        ? JSON.parse(localStorage.getItem('cart'))
        : { cartItems: [], shippingAddress: "", paymentMethod: "PayPal" };

    updateBadge(cart)

    $('.btn-add').click(function (e) {
        var btn = $(this);
        
        let idFood = btn.attr('id');
       
        $.get('/Menu/AddToCart/?id=' + idFood).done(
            function (data) {
                
              /*  if (data == 0) {
                    btn.attr('presentDansListe-val', 'False');
                    btn.html(' + ');
                } else if (data == 1) {
                    btn.attr('presentDansListe-val', 'True');
                    btn.html(' - ');
                }*/
               cart =  addToCart(cart, data)
               updateCart(cart);
               updateBadge(cart);
               //console.log(cart)

            }).fail(
                function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 401) {
                        window.location.href = "/User/Index";
                    }
                    console.log("Failed with Status Code : ")
                    console.log("Error : " + errorThrown);
                    console.log(textStatus);
                }
            );

    })

    function updateCart(cart) {
        localStorage.setItem('cart', JSON.stringify(cart));
    }

    function updateBadge(cart) {
        $(".badge").text(cart.cartItems.length)
    }
    function addToCart(cart,item) {
        //let newCart = []
        var existItem = cart.cartItems.find(x => x.id === item.id);
        
        if (existItem) {
            cart.cartItems = cart.cartItems.map(x => x._id === existItem.Id ? item : x)
        } else {
            cart.cartItems = [...cart.cartItems,item]
        }
       
        return cart;
    }
})