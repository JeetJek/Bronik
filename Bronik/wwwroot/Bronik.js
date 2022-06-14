const popupLinks = document.querySelectorAll('.popup_link');
const body = document.querySelector('body');

let unlock = true;

const timeout = 800;

if (popupLinks.length > 0) {
    for (let index = 0; index < popupLinks.length; index++) {
        const popup_link = popupLinks[index];
        popup_link.addEventListener("click", function (e) {
            const popupName = popup_link.getAttribute('href').replace('#', '');
            const curentPopup = document.getElementById(popupName);
            popupOpen(curentPopup);
            e.preventDefault();
        });
    }
}
const popupCloseIcon = document.querySelectorAll('.close-popup');
if (popupCloseIcon.length > 0) {
    for (let index = 0; index < popupCloseIcon.length; index++) {
        const el = popupCloseIcon[index];
        el.addEventListener('click', function (e) {
            popupClose(el.closest('.popup'));
            e.preventDefault();
        });
    }
}

function popupOpen(curentPopup) {
    if (curentPopup && unlock) {
        const popupActive = document.querySelector('.popup.open');
    }
    curentPopup.classList.add('open');
    curentPopup.addEventListener("click", function (e) {
        if (!e.target.closest('.infowindow_content')) {
            popupClose(e.target.closest('.popup'));
        }
    });
}

function popupClose(popupActive) {
    if (unlock) {
        popupActive.classList.remove('open')
    }
}
