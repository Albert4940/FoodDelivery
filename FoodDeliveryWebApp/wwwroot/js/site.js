// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function () {


    /*let cart = localStorage.getItem('cart')
        ? JSON.parse(localStorage.getItem('cart'))
        : { cartItems: [], shippingAddress: "", paymentMethod: "PayPal" };*/

    //let cart = getCartData();


    updateBadge()
    
    //if
   // listCart(cart);

    
    /*$('.btn-add').click(function (e) {
        var btn = $(this);
        
        let idFood = btn.attr('id');
       
        $.get('/Cart/AddToCart/?id=' + idFood).done(
            function (data) {
            
               //cart =  addToCart(cart, data)
               //updateCart(cart);
               //updateBadge(cart);

            }).fail(
                function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status == 401) {
                        window.location.href = "/User/Index";
                    } else {
                        console.log("Failed with Status Code : ")
                        console.log("Error : " + errorThrown);
                        console.log(textStatus);
                    }
                   
                }
            );

    })*/
    function liCreator(data) {
        let ImageURL =  data.imageURL;
        let liElt = "<li class='list-group-item'>"

        liElt += "<div class='row'>"

        liElt += "<div class='col-md-2'>"
        //liElt += "<img src='@Url.Content(" + ImageURL + ")'/>" 
        liElt += "<img src='" + ImageURL + "'/>" 
        liElt += "</div>"

        liElt += "<div class='col-md-3'>"
        liElt += data.title
        liElt += "</div >"

        liElt += "</div>"

        liElt += "</li>"
        return liElt;
    }
    function listCart(cart) {
        //cart.cartItems.length !== 0 ? $('.alert').toggle('block') : $('.list-group').toggle('display','block'); 
        if (cart.cartItems.length === 0)
            $('#cart-alert').css('display', 'block')
        else {
            $('#cart-list').css('display', 'block')
            $('#cart-list').html(liCreator(cart.cartItems[0]))
        }


        /*$('#cart-alert').css('display', 'block')
        $('.list-group').css('display', 'block')*/
    }
    function updateCart(cart) {
        sessionStorage.setItem('cart', JSON.stringify(cart));
    }

    function getCartData() {
        let cart = sessionStorage.getItem('cart')
            ? JSON.parse(sessionStorage.getItem('cart'))
            : { cartItems: [], shippingAddress: "", paymentMethod: "PayPal" };
        return cart;
    }
    function updateBadge() {
        $.get("/Cart/CountItems/").done(function (data) {
            $(".badge").text(data);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.status);
        })
    }
    function addToCart(cart,item) {
        //let newCart = []
        var existItem = cart.cartItems.find(x => x.id === item.id);
        
        if (existItem) {
            cart.cartItems = cart.cartItems.map(x => x._id === existItem.id ? item : x)
        } else {
            cart.cartItems = [...cart.cartItems,item]
        }
       
        return cart;
    }

})