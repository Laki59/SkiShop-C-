/*Uzima sve stvari koje imaju klasu section */
const sections = document.querySelectorAll('.section');
//handscroll se poziva svkai put kad user scrolluje
function handleScroll() {
    sections.forEach(section => {
        const rect = section.getBoundingClientRect();
        //pitamo se da li je vrh sekcije u ili iznad view window-a,a da bottom nije kompletno van
        if (rect.top < window.innerHeight && rect.bottom >= 0) {
            section.classList.add('show');
        }
    });
}

window.addEventListener('scroll', handleScroll);
document.addEventListener('DOMContentLoaded', handleScroll);