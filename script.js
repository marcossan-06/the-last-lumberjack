document.addEventListener('DOMContentLoaded', function () {
      // Menú móvil
      const mobileToggle = document.querySelector('.mobile-toggle');
      const navMenu = document.querySelector('.nav-menu');

      if (mobileToggle) {
        mobileToggle.addEventListener('click', function () {
          navMenu.classList.toggle('active');
          this.classList.toggle('active');
        });
      }

      // Smooth scroll
      document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
          if (this.getAttribute('href') === '#') return;

          e.preventDefault();
          const targetId = this.getAttribute('href');
          const targetElement = document.querySelector(targetId);

          if (targetElement) {
            window.scrollTo({
              top: targetElement.offsetTop - 80,
              behavior: 'smooth'
            });

            // Cerrar menú móvil
            if (navMenu.classList.contains('active')) {
              navMenu.classList.remove('active');
              mobileToggle.classList.remove('active');
            }
          }
        });
      });

      // Detectar dispositivo y mostrar botón correspondiente
      function detectDevice() {
        const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        const pcDownload = document.querySelector('a[href$=".zip"]');
        const androidDownload = document.querySelector('a[href$=".apk"]');

        if (isMobile && androidDownload) {
          androidDownload.style.display = 'flex';
          if (pcDownload) pcDownload.style.display = 'none';
        } else if (!isMobile && pcDownload) {
          pcDownload.style.display = 'flex';
          if (androidDownload) androidDownload.style.display = 'none';
        }
      }

      // Llamar a la detección de dispositivo
      detectDevice();

      // Animación de aparición de elementos al hacer scroll
      const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
      };

      const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
          if (entry.isIntersecting) {
            entry.target.classList.add('animate-in');
          }
        });
      }, observerOptions);

      // Observar elementos para animación
      document.querySelectorAll('.story-paragraph, .enemy-item, .progression-feature').forEach(el => {
        observer.observe(el);
      });
    });