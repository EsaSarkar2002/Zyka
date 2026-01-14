//document.addEventListener('DOMContentLoaded', function () {
//    let authValue = false;
//    const body = document.body;
//    if (body && body.dataset && typeof body.dataset.authenticated !== 'undefined') {
//        const val = body.dataset.authenticated;
//        authValue = val === 'true';
//    }
//    if (!authValue) {
//        openLoginPopup();
//    }
//});

function openLoginPopup() {
    const overlay = document.getElementById("loginOverlay");
    if (!overlay) return;
    overlay.classList.add("active");
}

function closeLoginPopup() {
    const overlay = document.getElementById("loginOverlay");
    if (!overlay) return;
    overlay.classList.remove("active");
    //overlay.style.display = "none";
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