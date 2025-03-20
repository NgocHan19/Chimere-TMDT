import Swiper from 'swiper';
import 'sưiper/css';

var swiper = new Swiper(".swiper", {
    loop: true,
    navigation: {
        nextEl: ".swiper-button-next",
        prevEl: ".swiper-button-prev",
    },
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
    autoplay: {
        delay: 3000,
        disableOnInteraction: false,
    },
});


document.addEventListener("DOMContentLoaded", function () {
    const thumbnailsContainer = document.querySelector(".thumbnails");
    const prevBtn = document.querySelector(".prev");
    const nextBtn = document.querySelector(".next");

    const step = 65; // Khoảng cách cuộn (60px hình + 5px gap)

    function checkButtons() {
        prevBtn.style.display = thumbnailsContainer.scrollLeft <= 0 ? "none" : "block";
        nextBtn.style.display = thumbnailsContainer.scrollLeft >= (thumbnailsContainer.scrollWidth - thumbnailsContainer.clientWidth) ? "none" : "block";
    }

    nextBtn.addEventListener("click", function () {
        thumbnailsContainer.scrollBy({ left: step, behavior: "smooth" });
        setTimeout(checkButtons, 300); // Kiểm tra lại sau khi cuộn
    });

    prevBtn.addEventListener("click", function () {
        thumbnailsContainer.scrollBy({ left: -step, behavior: "smooth" });
        setTimeout(checkButtons, 300);
    });

    checkButtons();
});
