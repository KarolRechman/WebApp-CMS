function preLoadImage(img) {
    const src = img.getAttribute("data-src");
    if (!src) {
        return;
    }

    img.src = src;
}

const images = document.querySelectorAll("[data-src]");
const options = {
    threshold: 0
};
const imgObserver = new IntersectionObserver((entries, imgObserver) => {
    entries.forEach(entry => {
        if (!entry.isIntersecting /*isIntersection*/) {
            return;
        } else {
            preLoadImage(entry.target);
            imgObserver.unobserve(entry.target);
        }
    });
}, options);

images.forEach(image => {
    imgObserver.observe(image);
});

