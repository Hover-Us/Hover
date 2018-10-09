'use strict';

//  Main script for https://Hover.Us

// The menu scroll could be changed to be draggable... but would anyone?

// Defer-loads style sheets
// scrollIt: Does smooth scrolling to #id or anywhere
// show_pos: updates the scrollbar on header.menu
// TouchEnd: swipe < or > to jump to next menu content

// Changes, add to bottom. This will be minified so put in detail
// Date yyyy-mm-dd,What changes
// 2018/07/27, 


// Work in progress: Bug- 

// To-do:
// facebook login, show comments


    // defer_load_this
    var dl = document.createElement('link'); // defer-load, Kalam font
    dl.rel = 'stylesheet';
    dl.href = 'https://fonts.googleapis.com/css?family=Kalam';
    dl.type = 'text/css';
    document.head.appendChild(dl);
    dl = null;

    //dl = document.createElement('link'); // defer-load, keep ATF styles on the html <head><style>
    //dl.rel = 'stylesheet';
    //dl.href = 'css/normalize.css';
    //dl.type = 'text/css';
    //document.head.appendChild(dl);
    //dl = null;


    // SHOULD NOT SEE ME IN PRODUCTION :-O
  // MINIMIZE ME PLEASE

    var hH = 30; // header height
    // Because the header is "position:fixed;top:0" allways at the top, .._Header(subtractHeader) makes this adjustment
    function dH(subtractHeader = false) { // NOTE: hH is not adjusted out by default, dH() = documentHeight
        return Math.max(document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight, document.documentElement.offsetHeight)-(subtractHeader ? hH : 0);
}
    function wH(subtractHeader = false) {
        return (window.innerHeight || document.documentElement.clientHeight || document.getElementsByTagName('body')[0].clientHeight)-(subtractHeader ? hH : 0);
}

var sim = 0; // Still In Motion: Keep the menu button "mouseover" background while being scrolled
    function scrollIt(event, destination, duration = 200) {
        // scroll the page to the destination. Keep out of History, Thanks to pawelgrzybek.com/
        // Any JS error should result in the default anchor action when used in anchor onclick

        if (typeof destination == 'undefined') // use href="#home" to set destination = "home"
        {
        destination = document.getElementById(event.currentTarget.hash.substr(1)).offsetTop;
    // event.currentTarget.blur(); // Seems to work with button:not(:focus):hover fixing mobile stuck hover- still has :hover
    // event.currentTarget.style['pointer-events'] = "none"; not good, still :hover but can't click again
    // event.currentTarget.parentNode.click();
    // setTimeout(document.getElementById("history").click(), 2000);
    //  event.currentTarget.style.backgroundColor = "red";


    const currentTarget = event.currentTarget; // needed for setTimeout

   //  console.warn('sim=' + sim + ', currentTarget.matches(:hover)=' + currentTarget.matches(':hover'));
    // do not remove backgroundColor if mouse is still here... & it's not mobile, because :hover on mobile sucks
    setTimeout(function () { if (!sim || !currentTarget.matches(':hover') || 'ontouchstart' in document.documentElement || 'ontouchstart' in window || window.DocumentTouch && document instanceof DocumentTouch) currentTarget.style.backgroundColor = "";  sim = 0}, duration+50);
}

/*    const easings = { // many more functions at pawelgrzybek.com
        linear(t) { return t; },
            easeInOutQuad(t) { return t < 0.5 ? 2 * t * t : (4 - 2 * t) * t - 1; } // start and finish soft
};
*/
const pageYOffset_ = window.pageYOffset;
const startTime = 'now' in window.performance ? performance.now() : new Date().getTime();

var destinationYNoHeader = (typeof destination == 'number' ? destination : (typeof destination == 'string' ? document.getElementById(destination).offsetTop : destination.offsetTop)) - hH;
        if (destinationYNoHeader < 0) destinationYNoHeader = 0;
        const destinationY = (dH() - destinationYNoHeader < wH() ? dH() - wH() : destinationYNoHeader);

        if ('requestAnimationFrame' in window == false) {
        window.scroll(0, destinationY);
    return;
}

        function scroll() {
            const now = 'now' in window.performance ? performance.now() : new Date().getTime();
    const t = Math.min(1, ((now - startTime) / duration));
            window.scroll(0, Math.ceil((t < 0.5 ? 2 * t * t : (4 - 2 * t) * t - 1) * (destinationY - pageYOffset_)) + pageYOffset_);

            if (Math.abs(window.pageYOffset - destinationY) < 1 || now > startTime + 3 * duration)
        return; // either finished or ran 3x too long

    requestAnimationFrame(scroll);
}
scroll();
}

var mL = 1.99, mR = 1.99; // Margin for header buttons, set dynamically below, here <-- Does the parseFloat ever fail? Is this useful?


// Reference: http://www.html5rocks.com/en/tutorials/speed/animations/
    function show_pos() {
        // Style the header.menu to reflect what is shown below it (the viewport - the header)
        // adjust slider position, button color, and button text shadow
        var Dheader = document.querySelectorAll('header')[0];
    const pageYOffset_ = window.pageYOffset;
    var StartX = null, FinishX = null; // Start & Finish (X) of menu slider
    var menuElements = Dheader.getElementsByClassName("button"); // Menu elements to style
    var menuShadowX = Dheader.getElementsByClassName("butx"); // Highlight (shadow) nemu button text when content is on screen
    hH = Dheader.clientHeight;

   // console.info('marginL=' + mL + ', marginR=' + mR);

    // EACH HEADER .button NEEDS TO HAVE A .butx, or rewrite this: KIS!
        for (var i = 0; i < menuElements.length; i++) {
            var x = document.getElementById(menuElements[i].hash.substr(1)); // x is the page content (hash) for this nemu menuElements[i]
            if (!x) console.info(menuElements[i].hash.substr(1) + ' not found in show_pos');
            else {
                var Taget_idY = x.offsetTop;
                var nextTaget_idY = (i < menuElements.length - 1 ? document.getElementById(menuElements[i + 1].hash.substr(1)).offsetTop : dH());
                if (pageYOffset_ + wH() <= Taget_idY) {// this menu item's page content is below the viewport
                    menuElements[i].style.color = "#ddd";
                    menuShadowX[i].style.display = "none";
                }
                else if (pageYOffset_ + hH >= nextTaget_idY) {// Already scrolled past the content
                    menuElements[i].style.color = "#777";
                    menuShadowX[i].style.display = "none";
                }
                else { // Content is on screen. StartX and FinishX must be here. Display menuShadowX (butx class) only here
                    menuElements[i].style.color = "#a5c";
                    menuShadowX[i].style.display = "";

                    var menuButtonRectangle = menuElements[i].getBoundingClientRect(); // menu button rectangle to get position
                    var StartShadowX = null, FinishShadowX = null; // like StartX and FinishX for butx: whole menu has 1 StartX but each displayed menuShadowX needs it's own set
                    // Get each button's margins
                    mL = parseFloat(window.getComputedStyle(menuElements[i], null).getPropertyValue("margin-left"), mL);
                    mR = parseFloat(window.getComputedStyle(menuElements[i], null).getPropertyValue("margin-right"), mR);

                    if (StartX == null) { // First menuShadowX displayed
                        // percent of distance past Taget_idY, on the way to the next: Page (viewport) %, used to set menu display
                        var percentStart = (pageYOffset_ + (i == 0 ? 0 : hH) - Taget_idY) / (nextTaget_idY - Taget_idY - (i == 0 ? hH : 0));

                        // the first button has header padding/margin to left
                        if (i == 0) {
                            StartX = (menuButtonRectangle.right + mR) * percentStart;
                            // might not be clipped because of spacing
                            if (StartX >= menuButtonRectangle.left)
                                StartShadowX = StartX - menuButtonRectangle.left - 1;
                        }
                        else {
                            StartX = (menuButtonRectangle.left - mL) + (menuElements[i].clientWidth + mL + mR) * percentStart;
                            if (StartX >= menuButtonRectangle.left)
                                StartShadowX = StartX - menuButtonRectangle.left - 1;
                        }

                    }

                    if (pageYOffset_ + wH(true) <= nextTaget_idY) {
                        // cannot see Next Content yet, so end the slider in this menu item
                        var percentFinish = Math.min(1, (pageYOffset_ + wH() - Taget_idY) / (nextTaget_idY - Taget_idY)); // percent of distance past Taget_idY, on way to the next

                        // the last button has header padding/margin to right
                        if (i == menuElements.length - 1) {
                            //               
                            FinishX = (menuButtonRectangle.left - mL) + (document.documentElement.clientWidth - (menuButtonRectangle.left - mL)) * percentFinish;
                            if (FinishX < menuButtonRectangle.right) FinishShadowX = FinishX - menuButtonRectangle.left - 1;
                        }
                        else {
                            FinishX = (menuButtonRectangle.left - mL) + (menuElements[i].clientWidth + mL + mR) * percentFinish;
                            if (FinishX < menuButtonRectangle.right) FinishShadowX = FinishX - menuButtonRectangle.left - 1;
                        }
                    }
                    //   document.getElementById("help").innerText = ' i=' + i + ' \n percentFinish=' + percentFinish + ' \n FinishX=' + FinishX + ' \n menuButtonRectangle.left=' + menuButtonRectangle.left + '\n FinishShadowX='+FinishShadowX + '\n StartX=' + StartX;

                    menuShadowX[i].style.clip = "rect(0, " + (FinishShadowX == null ? "auto" : FinishShadowX + "px") + ", auto, " + (StartShadowX == null ? "auto" : StartShadowX + "px") + ")";
                }
            }
        }

var show_sldr = document.getElementById("show_sldr"); // display slider
show_sldr.style.left = (StartX == null ? 0 : StartX) + 'px';
show_sldr.style.width = (FinishX - StartX) + 'px';
}


var active = false;
    ['scroll', 'resize', 'load'].forEach(function (e) {
        window.addEventListener(e, function (e) {
            //     var last_pos = 0; last_pos = window.scrollY;
            if (!active) {
                active = true;
                window.requestAnimationFrame(function () {
                    show_pos();
                    active = false;
                });
            }
        });
    });

// Detect Left or Right swipe to page-menu down or up. Let normal scroll handle up/down swipes
var xDown = null;
var xDiff = 0;
document.getElementById('bodyTouch').addEventListener('touchstart', function (e) {xDown = e.touches[0].clientX}, true);
document.getElementById('bodyTouch').addEventListener('touchmove', function (e){if (!xDown) return; xDiff = xDown - e.touches[0].clientX }, true);
document.getElementById('bodyTouch').addEventListener('touchend', TouchEnd, true);

// NOTE BUG- if no bodyTouch but document.addEventListener('touchend',... after a swipe, pressing a menu button could have ScrollIt move like a swipe
        
function TouchEnd(evt) { // Scroll to next (prior) menu item on page
    var pageY2go = Math.max(0, window.pageYOffset + xDiff); // give it a start
    
        // "sim" Still In Motion is only needed for mouse, set it to 0 to avoid stuck mobile
        sim = 0;
        var Dheader = document.querySelectorAll('header')[0];
        var menuElements = Dheader.getElementsByClassName("button"); // Menu elements, determine which (page content) to scroll to
    
         // HEADER .button elements need same order as content on page:
        if (Math.abs(xDiff) > 120) /* significant */
        for (var i = 0; i < menuElements.length; i++) {
            var x = document.getElementById(menuElements[i].hash.substr(1)); // x is the page content (hash) for this nemu menuElements[i]
            if (!x) console.info(menuElements[i].hash.substr(1) + ' not found TouchEnd');
            else {
                var Taget_idY = x.offsetTop; // Content Top, the anchor Y position
                var nextTaget_idY = (i < menuElements.length - 1 ? document.getElementById(menuElements[i + 1].hash.substr(1)).offsetTop : dH(true));
                if (pageY2go + wH() <= Taget_idY) {// this menu item's page content is below the viewport
        } else if (pageY2go + hH >= nextTaget_idY) {// Already scrolled past the content
        } else // Got It, either Taget_idY or nextTaget_idY
                {
                    if (xDiff > 0) // left swipe, scroll down, Y increasing
            pageY2go = nextTaget_idY;
        else // right swipe ...
            pageY2go = Taget_idY;
    
        scrollIt(null, pageY2go);
        return;
    }
}
}

/* reset */
xDown = null; 
}


function clean(t) {
        t.value = t.value.toString().replace(/[^a-zA-Z\- 0-9\n\r]+/g, '').replace(/[ ]{2,}/g, ' ').trim();
    }

    function signupcheck(token) {
        if (typeof (token) == 'undefined') token = grecaptcha.getResponse();
    document.getElementById('recaptcha').style.display = "";
    var signupDisabled = false; // set to false below if so
    if (document.getElementById('firstname').validity.valid)
        document.getElementById('firstname').style.borderColor = "";
        else {
        document.getElementById('firstname').style.borderColor = "red";
    signupDisabled = true;
}

if (document.getElementById('email').validity.valid)
    document.getElementById('email').style.borderColor = "";
        else {
        document.getElementById('email').style.borderColor = "red";
    signupDisabled = true;
}
document.getElementById('signUpButton').disabled = signupDisabled || token == '';
}


function signup() {
        document.getElementById("update").innerHTML = "";
    var token = grecaptcha.getResponse();
    if (token == '') {
        document.getElementById('recaptcha').style.display = ""; return;
}

    var payload = JSON.stringify({"name": document.getElementById('firstname').value, "email": document.getElementById('email').value, "captcha": token });

    document.getElementById('recaptcha').style.display = "none";

    var oReq = new XMLHttpRequest();
    oReq.onreadystatechange = function() {
        if (this.readyState == 4) {
        document.getElementById("update").innerHTML = this.responseText;
    if (this.responseText.indexOf("Error:") == 0) {
        document.getElementById('recaptcha').style.display = "";
    if (grecaptcha.getResponse() == '') grecaptcha.reset();
}
}
};

oReq.open('POST', 'https://localhost:44309/user?');
oReq.send(payload);
grecaptcha.reset();
}
