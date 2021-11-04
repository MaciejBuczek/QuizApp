const loader = document.getElementById("loader");
const loaderText = document.getElementById("loader-text");

function showLoader(text) {

    loader.style.display = "block"

    if(text != null)
        loaderText.innerText = text;
}

function hideLoader() {
    loader.style.display = "none"
    loaderText.innerText = "";
}