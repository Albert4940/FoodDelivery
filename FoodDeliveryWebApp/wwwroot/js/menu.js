jQuery(document).ready(function () {
   
    function showFoods() {
        $("#card-group .card").each(function () {

            $(this).removeClass("hide-card-food")

        })
    }
    showFoods();
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