document.addEventListener("DOMContentLoaded", function () {

    new Swiper(".networkSwiper", {
  
      loop: true,
  
      slidesPerView: 1,
  
      spaceBetween: 30,
  
      speed: 900,
  
      autoplay: {
        delay: 2500,
        disableOnInteraction: false,
      },
  
      pagination: {
        el: ".network-pagination",
        clickable: true,
      },
  
    });
  
  });