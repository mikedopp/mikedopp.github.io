// Rack behaviour: whole-U unit heights, numbered rails, slide-out service views,
// and a link LED on the nav port for the section in view. The page works without it.
(function () {
  "use strict";

  var rack = document.querySelector("[data-rack]");
  if (!rack) return;

  var bay = rack.querySelector(".bay");
  var rails = rack.querySelectorAll(".rail");
  var heightLabel = document.querySelector("[data-rack-height]");
  var units = Array.prototype.slice.call(bay.querySelectorAll(".unit"));
  var sliding = 0;

  function pitch() {
    var probe = getComputedStyle(document.documentElement).getPropertyValue("--u");
    return parseFloat(probe) || 48;
  }

  // Round every unit up to a whole number of U, then number the rails to match.
  function snap() {
    if (sliding) return;
    var u = pitch();
    units.forEach(function (el) { el.style.minHeight = ""; });
    var heights = units.map(function (el) { return el.getBoundingClientRect().height; });
    units.forEach(function (el, i) {
      var count = Math.max(1, Math.ceil((heights[i] - 0.5) / u));
      el.style.minHeight = count * u + "px";
      el.dataset.u = count;
    });
    number(Math.round(bay.getBoundingClientRect().height / u));
  }

  var lastCount = -1;
  function number(count) {
    if (heightLabel) heightLabel.textContent = count + "U";
    if (count === lastCount) return;
    lastCount = count;
    var u = pitch();
    rails.forEach(function (rail) {
      var frag = document.createDocumentFragment();
      for (var i = 0; i < count; i++) {
        var n = document.createElement("span");
        n.className = "rail-num";
        n.style.top = i * u + "px";
        n.textContent = count - i;
        frag.appendChild(n);
      }
      rail.replaceChildren(frag);
    });
  }

  // Slide-out service views
  bay.querySelectorAll("[data-service-unit]").forEach(function (unit) {
    var handle = unit.querySelector(".handle");
    var panel = unit.querySelector(".service");
    if (!handle || !panel) return;

    function set(open) {
      unit.classList.toggle("is-open", open);
      handle.setAttribute("aria-expanded", String(open));
      panel.inert = !open;
    }

    set(false);

    handle.addEventListener("click", function () {
      var open = !unit.classList.contains("is-open");
      var reduce = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
      unit.style.minHeight = "";
      if (!reduce) sliding++;
      set(open);
      if (reduce) { snap(); return; }
      var done = false;
      function finish() {
        if (done) return;
        done = true;
        sliding = Math.max(0, sliding - 1);
        snap();
      }
      panel.addEventListener("transitionend", function onEnd(e) {
        if (e.target !== panel) return;
        panel.removeEventListener("transitionend", onEnd);
        finish();
      });
      setTimeout(finish, 900);
    });
  });

  // Nav port LED follows the section in view
  var ports = {};
  document.querySelectorAll(".port[data-port]").forEach(function (p) { ports[p.dataset.port] = p; });
  if ("IntersectionObserver" in window) {
    var io = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        var p = ports[e.target.id];
        if (p) p.classList.toggle("is-active", e.isIntersecting);
      });
    }, { rootMargin: "-35% 0px -55% 0px" });
    Object.keys(ports).forEach(function (id) {
      var section = document.getElementById(id);
      if (section) io.observe(section);
    });
  }

  snap();
  if (document.fonts && document.fonts.ready) document.fonts.ready.then(snap);
  window.addEventListener("load", snap);

  var resizeTimer;
  window.addEventListener("resize", function () {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(snap, 120);
  });
})();
