﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function () {

    let cart = localStorage.getItem('cart')
        ? JSON.parse(localStorage.getItem('cart'))
        : { cartItems: [], shippingAddress: "", paymentMethod: "PayPal" };


    updateBadge(cart)

    if (cart.cartItems.length > 0) {
        $.post("/Cart/Index", cart).done(function (data) {
            console.log("Successfully : ",data)
        }).fail(function (error) {
            console.error("Failed  : ",error)
        })
    }

    listCart(cart);

    
    $('.btn-add').click(function (e) {
        var btn = $(this);
        
        let idFood = btn.attr('id');
       
        $.get('/Cart/AddToCart/?id=' + idFood).done(
            function (data) {
            
               cart =  addToCart(cart, data)
               updateCart(cart);
               updateBadge(cart);

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

    })

    function liCreator(data) {
        let liElt = "<li class='list-group-item'>"
            liElt += "<div class='row'>"
                liElt += "<div class='col-md-2'>"
                     liElt += "<img src='~/img/c1.png'/>" 
                liElt += "</div>"
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
            $('#cart-list').html(liCreator())
        }


        /*$('#cart-alert').css('display', 'block')
        $('.list-group').css('display', 'block')*/
    }
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
            cart.cartItems = cart.cartItems.map(x => x._id === existItem.id ? item : x)
        } else {
            cart.cartItems = [...cart.cartItems,item]
        }
       
        return cart;
    }
})