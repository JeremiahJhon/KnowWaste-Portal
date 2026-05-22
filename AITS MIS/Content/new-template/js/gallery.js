const articleGallerySwiper = new Swiper(".article-gallery-swiper", {

    loop:true,

    speed:1200,

    effect:"fade",

    fadeEffect:{
        crossFade:true
    },

    autoplay:{
        delay:4500,
        disableOnInteraction:false,
        pauseOnMouseEnter:true
    },

    navigation:{
        nextEl:".article-gallery-next",
        prevEl:".article-gallery-prev"
    },

    slidesPerView:1

});