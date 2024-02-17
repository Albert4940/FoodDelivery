jQuery(document).ready(function () {
   
    function showFoods() {
        $("#card-group .card").each(function () {

            $(this).removeClass("hide-card-food")

        })
    }
    showFoods();

    function showNoFoodMesage() {
        // alert - no - food
        $(".alert-no-food").hide();

        let CountCard = $("#card-group .card").length;
        let CountCardHide = $("#card-group .card.hide-card-food").length;

        if (CountCard == CountCardHide)
            $(".alert-no-food").show();
    }
    function filterFood(categoryId) {
        
        if (categoryId == 0) {
            showFoods();
        }
        else {
            $("#card-group .card").each(function () {

                let categoryIdCard = $(this).attr("id");

                if (categoryId != categoryIdCard)
                    $(this).addClass("hide-card-food")
                else
                    $(this).removeClass("hide-card-food")
            })
        }

        showNoFoodMesage();

    }

    $(".btn-category").click(function (e) {
        e.preventDefault();

        
        $(".btn-category").removeClass("btn-dark")
        $(this).addClass("btn-dark")

        let btn = $(this);

        let categoryId = btn.attr("id");

        filterFood(categoryId);

    })
})