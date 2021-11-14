function generateSwal(icon, title, text) {
    Swal.fire({
        icon: icon,
        title: title,
        text: text,
    });
}

function generateSwalRedirect(icon, title, text, redirect) {
    Swal.fire({
        icon: icon,
        title: title,
        text: text,
    }).then(() => {
        window.location.replace(redirect)
    });
}