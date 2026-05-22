document.addEventListener("DOMContentLoaded", function () {

    const carousel = document.querySelector('#carouselExampleCaptions');
    if (!carousel) return;
  
    const timelineItems = document.querySelectorAll('.news-timeline-item');
    const progressBars = document.querySelectorAll('.news-timeline-progress');
  
    const interval = 5000; // MUST match data-bs-interval
  
    // =============================
    // SET ACTIVE TIMELINE
    // =============================
    function setActive(index) {
      timelineItems.forEach(item => item.classList.remove('active'));
      timelineItems[index].classList.add('active');
    }
  
    // =============================
    // RESET ALL BARS
    // =============================
    function resetProgress() {
      progressBars.forEach(bar => {
        bar.style.transition = 'none';
        bar.style.width = '0%';
      });
    }
  
    // =============================
    // ANIMATE CURRENT BAR
    // =============================
    function animateProgress(index) {
      resetProgress();
  
      setTimeout(() => {
        progressBars[index].style.transition = `width ${interval}ms linear`;
        progressBars[index].style.width = '100%';
      }, 50);
    }
  
    // =============================
    // ON SLIDE CHANGE
    // =============================
    carousel.addEventListener('slide.bs.carousel', function (e) {
      setActive(e.to);
      animateProgress(e.to);
    });
  
    // =============================
    // CLICK TIMELINE → GO TO SLIDE
    // =============================
    timelineItems.forEach((item, index) => {
      item.addEventListener('click', () => {
        const bsCarousel = bootstrap.Carousel.getOrCreateInstance(carousel);
        bsCarousel.to(index);
      });
    });
  
    // =============================
    // INIT
    // =============================
    setActive(0);
    animateProgress(0);
  
  });