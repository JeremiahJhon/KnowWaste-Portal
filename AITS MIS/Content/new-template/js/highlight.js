// hilight js

// ===============================
// HIGHLIGHT SLIDER SCRIPT
// ===============================

document.addEventListener("DOMContentLoaded", function () {

  const DEFAULT_DELAY = 3000;

  const timelineItems = document.querySelectorAll(".highlight-timeline-item");
  const progressBars = document.querySelectorAll(".highlight-timeline-progress");
  const swiperEl = document.querySelector(".highlightSwiper");

  let currentDelay = DEFAULT_DELAY;
  let progressStartTime = 0;
  let remainingTime = DEFAULT_DELAY;

  let forceVideoDuration = false;
  let animationFrameId = null;


  // ===============================
  // CREDIT FUNCTION (NEW)
  // ===============================
  function updateCredit(swiper) {
    const creditBox = document.querySelector('.global-credit');
    if (!creditBox) return;

    const activeSlide = swiper.slides[swiper.activeIndex];
    const credit = activeSlide.getAttribute('data-credit');

    if (credit) {
      creditBox.textContent = credit;
      creditBox.classList.add('show');
    } else {
      creditBox.classList.remove('show');
    }
  }

  // ===============================
  // RESET TIMELINE
  // ===============================
  function resetTimeline() {
    cancelAnimationFrame(animationFrameId);

    progressBars.forEach(bar => {
      bar.style.transition = "none";
      bar.style.width = "0%";
    });
  }

  // ===============================
  // IMAGE TIMELINE (CSS BASED)
  // ===============================
  function startImageTimeline(index, delay) {
    const bar = timelineItems[index].querySelector(".highlight-timeline-progress");

    timelineItems.forEach(item => item.classList.remove("active"));
    timelineItems[index].classList.add("active");

    void bar.offsetWidth;

    bar.style.transition = `width ${delay}ms linear`;
    bar.style.width = "100%";

    progressStartTime = Date.now();
    currentDelay = delay;
    remainingTime = delay;
  }

  // ===============================
  // VIDEO TIMELINE (REAL SYNC)
  // ===============================
  function startVideoTimeline(index, video) {
    const bar = timelineItems[index].querySelector(".highlight-timeline-progress");

    timelineItems.forEach(item => item.classList.remove("active"));
    timelineItems[index].classList.add("active");

    bar.style.transition = "none";

    function update() {
      if (!video.duration) {
        animationFrameId = requestAnimationFrame(update);
        return;
      }

      const progress = (video.currentTime / video.duration) * 100;
      bar.style.width = progress + "%";

      animationFrameId = requestAnimationFrame(update);
    }

    update();
  }

  // ===============================
  // PAUSE / RESUME (IMAGE ONLY)
  // ===============================
  function pauseTimeline(index) {
    const bar = timelineItems[index].querySelector(".highlight-timeline-progress");

    const elapsed = Date.now() - progressStartTime;
    remainingTime = currentDelay - elapsed;

    bar.style.transition = "none";
    bar.style.width = `${(elapsed / currentDelay) * 100}%`;
  }

  function resumeTimeline(index) {
    const bar = timelineItems[index].querySelector(".highlight-timeline-progress");

    void bar.offsetWidth;

    bar.style.transition = `width ${remainingTime}ms linear`;
    bar.style.width = "100%";

    progressStartTime = Date.now();
  }

  // ===============================
  // VIDEO HANDLING
  // ===============================
  function handleVideos(swiper) {
    const slides = swiper.slides;

    slides.forEach(slide => {
      const video = slide.querySelector("video");
      if (video) {
        video.pause();
        video.currentTime = 0;
      }
    });

    const activeSlide = swiper.slides[swiper.activeIndex];
    const activeVideo = activeSlide.querySelector("video");

    if (activeVideo) {
      activeVideo.play().catch(() => {});
      return activeVideo;
    }

    return null;
  }

  // ===============================
  // GET SLIDE DELAY
  // ===============================
  function getSlideDelay(swiper) {
    const activeSlide = swiper.slides[swiper.activeIndex];
    const video = activeSlide.querySelector("video");

    if (forceVideoDuration && video && video.duration) {
      return video.duration * 1000;
    }

    return DEFAULT_DELAY;
  }

  // ===============================
  // INIT SWIPER
  // ===============================
  const swiper = new Swiper(".highlightSwiper", {
    loop: true,
    effect: "fade",
    speed: 600,

    fadeEffect: {
      crossFade: true
    },

    autoplay: {
      delay: DEFAULT_DELAY,
      disableOnInteraction: false,
      pauseOnMouseEnter: false,
      waitForTransition: false
    },

    loopAdditionalSlides: 1,

    navigation: {
      nextEl: ".swiper-button-next",
      prevEl: ".swiper-button-prev",
    },

    on: {
      init() {
		updateCredit(this);
        const delay = getSlideDelay(this);
        this.params.autoplay.delay = delay;

        const video = handleVideos(this);

        if (video) {
          startVideoTimeline(this.realIndex, video);
        } else {
          startImageTimeline(this.realIndex, delay);
        }
      },

      slideChange() {
		updateCredit(this);
        resetTimeline();

        const delay = getSlideDelay(this);
        this.params.autoplay.delay = delay;

        const video = handleVideos(this);

        if (video) {
          startVideoTimeline(this.realIndex, video);
        } else {
          startImageTimeline(this.realIndex, delay);
        }

        forceVideoDuration = false;
      }
    }
  });

  // ===============================
  // HOVER PAUSE
  // ===============================
  //swiperEl.addEventListener("mouseenter", () => {
   // swiper.autoplay.stop();

   // const activeSlide = swiper.slides[swiper.activeIndex];
   // const video = activeSlide.querySelector("video");

   // if (video) {
  //    video.pause();
  //    cancelAnimationFrame(animationFrameId);
  //  } else {
  //    pauseTimeline(swiper.realIndex);
  //  }
 // });

  swiperEl.addEventListener("mouseleave", () => {
    swiper.autoplay.start();

    const activeSlide = swiper.slides[swiper.activeIndex];
    const video = activeSlide.querySelector("video");

    if (video) {
      video.play().catch(() => {});
      startVideoTimeline(swiper.realIndex, video);
    } else {
      resumeTimeline(swiper.realIndex);
    }
  });

  // ===============================
  // TIMELINE CLICK
  // ===============================
  timelineItems.forEach(item => {
    item.addEventListener("click", function () {
      const index = parseInt(this.getAttribute("data-index"), 10);

      forceVideoDuration = true;

      swiper.slideToLoop(index);

      swiper.autoplay.stop();
      swiper.autoplay.start();
    });
  });

});