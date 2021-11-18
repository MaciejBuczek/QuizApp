const quizId = 1;
const ratingUrl = "/Rating/Add";
const starEmpty = "<i class='far fa-star' onclick='onClick(this)' onmouseover='onHover(this)' onmouseout='onHoverExit(this)' value=></i>";
const startFull = "<i class='fas fa-star' onclick='onClick(this)' onmouseover='onHover(this)' onmouseout='onHoverExit(this)' value=></i>";
let lockedStars = [false, false, false, false, false];

function onHover(element) {
    fillPreviousStars(element, false);
}

function onHoverExit(element) {
    let stars = document.querySelectorAll("i.far, i.fas");

    for (var i = 0; i < stars.length; i++) {
        if (!lockedStars[i] && stars[i].classList.contains("fas")) {
            stars[i].classList.remove("fas");
            stars[i].classList.add("far");
        }
    } 
}

function onClick(element) {
    let value = parseInt(element.getAttribute("value"));

    fillPreviousStars(element, true);
    alert(value);
    sendRating(value)
}

function fillPreviousStars(element, lock) {
    let value = parseInt(element.getAttribute("value"));
    let stars = document.querySelectorAll("i.far, i.fas");
    for (var i = 0; i < stars.length; i++) {     

        if (parseInt(stars[i].getAttribute("value")) <= value) {
            if (lock)
                lockedStars[i] = true;

            if (stars[i].classList.contains("far")) {
                stars[i].classList.remove("far");
                stars[i].classList.add("fas");
            }
        } else {
            if (lock)
                lockedStars[i] = false;

            if ((lock || !lockedStars[i]) && stars[i].classList.contains("fas")) {
                stars[i].classList.remove("fas");
                stars[i].classList.add("far");
            }
        }
    } 
}

function sendRating(ratingValue) {
    $.ajax({
        url: ratingUrl,
        type: "POST",
        dataType: "json",
        async: false,
        data: { rating: ratingValue, quizId: quizId },
        success: function (result) {
            Swal.fire(
                'Success',
                'You rating has been saved',
                'success'
            );
        },
        error: function (result) {
            Swal.fire(
                'Error',
                'Something went wrong. Your rating cannot be saved',
                'error'
            );
        }
    });
}

function renderRatings(element)
{
    let search = "value=";
    element.innerHtml = '';
    for (var i = 0; i < 5; i++) {

        let starString = starEmpty;
        var index = starString.indexOf(search);

        var starValueString = starString.slice(0, index + search.length) + i + starString.slice(index + search.length);

        element.innerHTML += starValueString;
    }
}