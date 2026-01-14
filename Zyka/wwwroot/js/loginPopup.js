document.addEventListener("DOMContentLoaded", function () {
    openLoginPopup();
});

function openLoginPopup() {
    document.getElementById("loginOverlay").classList.add("active");
}

function closeLoginPopup() {
    document.getElementById("loginOverlay")
        .classList.remove("active");
}

function showSignup() {
    const loginForm = document.getElementById("loginForm");
    const signupForm = document.getElementById("signupForm");

    if (!loginForm || !signupForm) return;

    loginForm.style.display = "none";
    signupForm.style.display = "block";
}

function showLogin() {
    const loginForm = document.getElementById("loginForm");
    const signupForm = document.getElementById("signupForm");

    if (!loginForm || !signupForm) return;

    signupForm.style.display = "none";
    loginForm.style.display = "block";
}