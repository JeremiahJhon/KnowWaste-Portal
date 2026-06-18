const expertSwiper = new Swiper(".expertSwiper", {

  slidesPerView: 4,
  slidesPerGroup: 4,   // IMPORTANT
  spaceBetween: 30,

  loop: true,         // IMPORTANT
  speed: 1000,


  autoplay: {
    delay: 5000,
    disableOnInteraction: false,
  },

  pagination: {
    el: ".expertSwiper .swiper-pagination",
    clickable: true,
  },

  breakpoints: {
    0: {
      slidesPerView: 1,
      slidesPerGroup: 1,
    },

    576: {
      slidesPerView: 2,
      slidesPerGroup: 2,
    },

    992: {
      slidesPerView: 4,
      slidesPerGroup: 4,
    }
  }
});