const loader = document.getElementById("loader");
const loaderText = document.getElementById("loader-text");
const loaderBox = document.getElementById("loader-box");

function showLoader(text) {

    loader.style.display = "block"

    if(text != null)
        loaderText.innerText = text;
}

function hideLoader() {
    loader.style.display = "none"
    loaderText.innerText = "";
}

function moveLoader(target) {
    target.appendChild(loaderBox);
}