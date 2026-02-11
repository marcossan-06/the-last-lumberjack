// Espero a que todo el HTML se haya cargado antes de ejecutar mi código
document.addEventListener('DOMContentLoaded', function () {
      // Menú móvil
      // Cojo el botón de menú hamburguesa y el menú de navegación
      const mobileToggle = document.querySelector('.mobile-toggle');
      const navMenu = document.querySelector('.nav-menu');

      if (mobileToggle) {
        mobileToggle.addEventListener('click', function () {
          navMenu.classList.toggle('active'); // Muestro u oculto el menú
          this.classList.toggle('active'); // Cambio el estilo del botón para indicar que está activo o no
        });
      }

      // Smooth scroll
      document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
          // Ignorar enlaces que solo apuntan a "#" 
          if (this.getAttribute('href') === '#') return;

          e.preventDefault();
          const targetId = this.getAttribute('href');
          const targetElement = document.querySelector(targetId);

          if (targetElement) {
            // Desplazo la página suavemente hasta el elemento, le meto un offset de 80 fijo
            window.scrollTo({
              top: targetElement.offsetTop - 80,
              behavior: 'smooth' // Animación suave del scroll
            });

            // Cierro el menu móvil si está abierto
            if (navMenu.classList.contains('active')) {
              navMenu.classList.remove('active');
              mobileToggle.classList.remove('active');
            }
          }
        });
      });

      // Detectar dispositivo y mostrar botón correspondiente
      function detectDevice() {
        // Miro dispositivos móviles posibles en el user agent del navegador
        const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
        
        // Enlaces de descarga para PC y Android
        const pcDownload = document.querySelector('a[href$=".zip"]');
        const androidDownload = document.querySelector('a[href$=".apk"]');

        // Muestro u oculto el indicado en caso
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

      // Con el intersectoin observer veo cuando los elementos entran en el viewport y les añado la clase para animar su aparición
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