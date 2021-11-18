const starEmpty = "<i class='far fa-star'></i>";
const startFull = "<i class='fas fa-star'></i>";


function renderRatings(element, score) {
    element.innerHtml = '';
    for (var i = 1; i <= 5; i++) {
        if(i<=score)
            element.innerHTML += startFull;
        else
            element.innerHTML += starEmpty
    }
}