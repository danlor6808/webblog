/*!
 * Start Bootstrap - Creative Bootstrap Theme (http://startbootstrap.com)
 * Code licensed under the Apache License v2.0.
 * For details, see http://www.apache.org/licenses/LICENSE-2.0.
 */
(function($) {
    "use strict"; // Start of use strict
    //Preloader
    var progress = setInterval(function () {
        var $bar = $("#bar");

        if ($bar.width() >= 600) {
            clearInterval(progress);
        } else {
            $bar.width($bar.width() + 60);
        }
        $bar.text($bar.width() / 6 + "%");
        if ($bar.width() / 6 == 100) {
            $bar.text("Still working ... " + $bar.width() / 6 + "%");
        }
    }, 800);

    $(window).load(function () {
        $("#bar").width(600);
        $(".loader").fadeOut(1000);
    });

    // jQuery for page scrolling feature - requires jQuery Easing plugin
    $('a.page-scroll').bind('click', function(event) {
        var $anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: ($($anchor.attr('href')).offset().top - 50)
        }, 1250, 'easeInOutExpo');
        event.preventDefault();
    });

    // Highlight the top nav as scrolling occurs
    $('body').scrollspy({
        target: '.navbar-fixed-top',
        offset: 51
    })

    $('.ignore').css({ 'visibility': 'hidden', 'width': '0%' });

    // Closes the Responsive Menu on Menu Item Click
    $('.navbar-collapse ul li a').click(function() {
        $('.navbar-toggle:visible').click();
    });

    // Fit Text Plugin for Main Header
    $("h1").fitText(
        1.2, {
            minFontSize: '35px',
            maxFontSize: '65px'
        }
    );

    // Offset for Main Navigation
    $('#mainNav').affix({
        offset: {
            top: 10
        }
    });

    // fade in .navbar
    $(function () {
        $(window).scroll(function () {

            // set distance user needs to scroll before we start fadeIn
            if ($(this).scrollTop() > 350) {
                $('#popupSearch').fadeIn();
            } else {
                $('#popupSearch').fadeOut(100);
            }
        });
    });

    var fileInput = document.getElementById('fileUpload');
    var fileDisplayArea = document.getElementById('fileDisplay');

    //File Reader Function for displaying images
    fileInput.addEventListener('change', function (e) {
        var file = fileInput.files[0];
        var imageType = /image.*/;

        if (file.type.match(imageType)) {
            var reader = new FileReader();

            reader.onload = function (e) {
                fileDisplayArea.innerHTML = "";

                // Create a new image.
                var img = new Image();
                // Set the img src property using the data URL.
                img.src = reader.result;

                // Add the image to the page.
                fileDisplayArea.appendChild(img);
            }

            reader.readAsDataURL(file);
        } else {
            fileDisplayArea.innerHTML = "File not supported!";
        }
    });
    // Initialize WOW.js Scrolling Animations
    new WOW().init();

})(jQuery); // End of use strict
