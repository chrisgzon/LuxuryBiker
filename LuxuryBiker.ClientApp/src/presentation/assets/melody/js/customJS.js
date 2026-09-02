var melodyJs = {
  formpickers: () => {
    (function ($) {
      "use strict";
      if ($("#timepicker-example").length) {
        $("#timepicker-example").datetimepicker({
          format: "LT",
        });
      }
      if ($(".color-picker").length) {
        $(".color-picker").asColorPicker();
      }
      if ($("#datepicker-popup").length) {
        $("#datepicker-popup").datepicker({
          enableOnReadonly: true,
          todayHighlight: true,
        });
      }
      if ($("#inline-datepicker").length) {
        $("#inline-datepicker").datepicker({
          enableOnReadonly: true,
          todayHighlight: true,
        });
      }
      if ($(".datepicker-autoclose").length) {
        $(".datepicker-autoclose").datepicker({
          autoclose: true,
        });
      }
      if ($('input[name="date-range"]').length) {
        $('input[name="date-range"]').daterangepicker();
      }
      if ($('input[name="date-time-range"]').length) {
        $('input[name="date-time-range"]').daterangepicker({
          timePicker: true,
          timePickerIncrement: 30,
          locale: {
            format: "MM/DD/YYYY h:mm A",
          },
        });
      }
    })(jQuery);
  },
  misc: () => {
    (function ($) {
      "use strict";
      $(document).ready(function () {
        var body = $("body");
        var sidebar = $(".sidebar");

        //Close other submenu in sidebar on opening any
        sidebar.on("show.bs.collapse", ".collapse", function () {
          sidebar.find(".collapse.show").collapse("hide");
        });

        //Change sidebar and content-wrapper height
        applyStyles();

        function applyStyles() {
          //Applying perfect scrollbar
          if (!body.hasClass("rtl")) {
            if (
              $(".settings-panel .tab-content .tab-pane.scroll-wrapper").length
            ) {
              const settingsPanelScroll = new PerfectScrollbar(
                ".settings-panel .tab-content .tab-pane.scroll-wrapper"
              );
            }
            if ($(".chats").length) {
              const chatsScroll = new PerfectScrollbar(".chats");
            }
            if (body.hasClass("sidebar-fixed")) {
              var fixedSidebarScroll = new PerfectScrollbar("#sidebar .nav");
            }
          }
        }

        $('[data-toggle="minimize"]').on("click", function () {
          if (
            body.hasClass("sidebar-toggle-display") ||
            body.hasClass("sidebar-absolute")
          ) {
            body.toggleClass("sidebar-hidden");
          } else {
            body.toggleClass("sidebar-icon-only");
          }
        });

        //checkbox and radios
        $(".form-check label,.form-radio label").append(
          '<i class="input-helper"></i>'
        );

        //fullscreen
        $("#fullscreen-button").on("click", function toggleFullScreen() {
          if (
            (document.fullScreenElement !== undefined &&
              document.fullScreenElement === null) ||
            (document.msFullscreenElement !== undefined &&
              document.msFullscreenElement === null) ||
            (document.mozFullScreen !== undefined && !document.mozFullScreen) ||
            (document.webkitIsFullScreen !== undefined &&
              !document.webkitIsFullScreen)
          ) {
            if (document.documentElement.requestFullScreen) {
              document.documentElement.requestFullScreen();
            } else if (document.documentElement.mozRequestFullScreen) {
              document.documentElement.mozRequestFullScreen();
            } else if (document.documentElement.webkitRequestFullScreen) {
              document.documentElement.webkitRequestFullScreen(
                Element.ALLOW_KEYBOARD_INPUT
              );
            } else if (document.documentElement.msRequestFullscreen) {
              document.documentElement.msRequestFullscreen();
            }
          } else {
            if (document.cancelFullScreen) {
              document.cancelFullScreen();
            } else if (document.mozCancelFullScreen) {
              document.mozCancelFullScreen();
            } else if (document.webkitCancelFullScreen) {
              document.webkitCancelFullScreen();
            } else if (document.msExitFullscreen) {
              document.msExitFullscreen();
            }
          }
        });
      });
    })(jQuery);
  },
  offCanvas: () => {
    (function ($) {
      "use strict";
      $(function () {
        $('[data-toggle="offcanvas"]').on("click", function () {
          $(".sidebar-offcanvas").toggleClass("active");
        });
      });
    })(jQuery);
  },
  settings: () => {
    (function ($) {
      "use strict";
      $(function () {
        $(".nav-settings").on("click", function () {
          $("#right-sidebar").toggleClass("open");
        });
        $(".settings-close").on("click", function () {
          $("#right-sidebar,#theme-settings").removeClass("open");
        });

        $("#settings-trigger").on("click", function () {
          $("#theme-settings").toggleClass("open");
        });

        //background constants
        var navbar_classes =
          "navbar-danger navbar-success navbar-warning navbar-dark navbar-light navbar-primary navbar-info navbar-pink";
        var sidebar_classes = "sidebar-light sidebar-dark";
        var $body = $("body");

        //sidebar backgrounds
        $("#sidebar-light-theme").on("click", function () {
          $body.removeClass(sidebar_classes);
          $body.addClass("sidebar-light");
          $(".sidebar-bg-options").removeClass("selected");
          $(this).addClass("selected");
        });
        $("#sidebar-dark-theme").on("click", function () {
          $body.removeClass(sidebar_classes);
          $body.addClass("sidebar-dark");
          $(".sidebar-bg-options").removeClass("selected");
          $(this).addClass("selected");
        });

        //Navbar Backgrounds
        $(".tiles.primary").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-primary");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.success").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-success");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.warning").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-warning");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.danger").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-danger");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.light").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-light");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.info").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-info");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.dark").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".navbar").addClass("navbar-dark");
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
        $(".tiles.default").on("click", function () {
          $(".navbar").removeClass(navbar_classes);
          $(".tiles").removeClass("selected");
          $(this).addClass("selected");
        });
      });
    })(jQuery);
  },
  select2: () => {
    (function ($) {
      "use strict";

      if ($(".js-example-basic-single").length) {
        $(".js-example-basic-single").select2();
      }
      if ($(".js-example-basic-multiple").length) {
        $(".js-example-basic-multiple").select2();
      }
    })(jQuery);
  },
};
