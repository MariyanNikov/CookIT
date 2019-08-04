// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    var Accordion = function (el, multiple) {
        this.el = el || {};
        this.multiple = multiple || false;

        // Variables privadas
        var links = this.el.find('.link');
        // Evento
        links.on('click', { el: this.el, multiple: this.multiple }, this.dropdown)
    }

    Accordion.prototype.dropdown = function (e) {
        var $el = e.data.el;
        $this = $(this),
            $next = $this.next();

        $next.slideToggle();
        $this.parent().toggleClass('open');

        if (!e.data.multiple) {
            $el.find('.submenu').not($next).slideUp().parent().removeClass('open');
        };
    }


    var accordion = new Accordion($('#accordion'), false);

});


$(function () {
    var url = window.location.pathname;

    $('#accordion a').each(function () {
        var myHref = $(this).attr('href');
        if (url.match(myHref)) {
            $(this).addClass('activeClassNameForCSSHighlight')
            $(this).closest('ul').show();
        }
    });
})

let disableButtons = function () {
    let numberOfIngredients = $('#numberOfIngredients').val();

    for (var i = 0; i < numberOfIngredients; i++) {
        let weightId = `ingredientsWeight${i}`;
        let countId = `ingredientsCount${i}`;
        $(`#ingredientsCount${i}`).on('propertychange input', function (e) {
            document.getElementById(weightId).disabled = this.value != ""
        });
        $(`#ingredientsWeight${i}`).on('propertychange input', function (e) {
            document.getElementById(countId).disabled = this.value != ""
        });
        if (document.getElementById(weightId).value != "") {
            document.getElementById(countId).disabled = true;
        } else if (document.getElementById(countId).value != "") {
            document.getElementById(weightId).disabled = true;
        }
    }
}


$(function () {
    $('#errorMessage').first().hide();

    disableButtons();

    $('#addIngredient').click(function (event) {
        event.preventDefault();

        let numberOfIngredients = $('#numberOfIngredients').val();
        if (numberOfIngredients >= 10) {

            $('#errorMessage').first().show();
            return;
        }
        $('#errorMessage').first().hide();
        let container = $('#ingredientsContainer');

        let ingredientRow = $('#ingredientRow').clone();


        ingredientRow.children()[0].setAttribute("name", `InputModel.Ingredients[${numberOfIngredients}].IngredientId`);
        //ingredientRow.children()[1].setAttribute('data-valmsg-for', `InputModel.Ingredients[${numberOfIngredients}].IngredientId`);
        ingredientRow.children()[1].setAttribute("name", `InputModel.Ingredients[${numberOfIngredients}].Count`);
        ingredientRow.children()[1].setAttribute("id", `ingredientsCount${numberOfIngredients}`);
        //ingredientRow.children()[3].setAttribute('data-valmsg-for', `InputModel.Ingredients[${numberOfIngredients}].Count`);
        ingredientRow.children()[2].setAttribute("name", `InputModel.Ingredients[${numberOfIngredients}].Weight`);
        ingredientRow.children()[2].setAttribute("id", `ingredientsWeight${numberOfIngredients}`);
        //ingredientRow.children()[5].setAttribute('data-valmsg-for', `InputModel.Ingredients[${numberOfIngredients}].Weight`);


        $('#numberOfIngredients').val(Number(numberOfIngredients) + 1);
        container.append(ingredientRow);

        $('#ingredientsContainer div:last-child :input').eq(0).val(0);
        $('#ingredientsContainer div:last-child :input').eq(1).val('');
        $('#ingredientsContainer div:last-child :input').last().val('');
        $('#ingredientsContainer div:last-child :input').prop('disabled', false);

        $(`#ingredientsCount${numberOfIngredients}`).on('propertychange input', function (e) {
            document.getElementById(`ingredientsWeight${numberOfIngredients}`).disabled = this.value != ""
        });
        $(`#ingredientsWeight${numberOfIngredients}`).on('propertychange input', function (e) {
            document.getElementById(`ingredientsCount${numberOfIngredients}`).disabled = this.value != ""
        });

    });

    $('#removeIngredient').click(function (event) {
        event.preventDefault();
        let numberOfIngredients = $('#numberOfIngredients').val();
        $('#errorMessage').first().hide();
        if (numberOfIngredients <= 1) {
            $('#errorMessage').first().show();
            return;
        }
        let container = $('#ingredientsContainer div:last-child');

        container.remove();

        $('#numberOfIngredients').val(Number(numberOfIngredients) - 1);
    });

});

$(function () {
    $('#imageInputField').hide();

    $('#changeBtnId').click(function (event) {
        event.preventDefault();

        $("#pictureId").hide();
        $("#changeBtnId").hide();

        $('#imageInputField').show();
    });
});

$(function () {
    var $star_rating = $('.star-rating .fa');

    var SetRatingStar = function () {
        return $star_rating.each(function () {
            if (parseInt($star_rating.siblings('input.rating-value').val()) >= parseInt($(this).data('rating'))) {
                return $(this).addClass('reviewStarClicked');
            } else {
                return $(this).removeClass('reviewStarClicked');
            }
        });
    };

    $star_rating.on('click', function () {
        $star_rating.siblings('#starsValue').val($(this).data('rating'));

        return SetRatingStar();
    });

    SetRatingStar();
    $(document).ready(function () {

    });

});