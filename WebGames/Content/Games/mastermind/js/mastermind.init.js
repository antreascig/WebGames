var isPaused = false;
var timeRemaining = null;
var cVal = null;
var gameTime = null;
function startTimer(duration, display) {
    var timer = duration, minutes, seconds;

    setInterval(function () {
        if (!isPaused) {
            minutes = parseInt(timer / 60, 10);
            seconds = parseInt(timer % 60, 10);
            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            display.text(minutes + ":" + seconds);
            if (--timer <= 0) {
                isPaused = true;
                endGame();
                // timer = duration;
            }
            timeRemaining = timer;
        } else {
            timeRemaining = timer;
        }
    }, 1000);
}
var success = $("#mysoundclip")[0];

function endGame() {
    $('.wrapper').fadeOut(2000, function () {
        window.location.replace("/Games/ActiveGame");
    });
}
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
        $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
        $(window).scrollLeft()) + "px");
    return this;
}
$(window).on('resize', function () {
    $('.game').center();
});

$(document).ready(function () {
    $('.wrapper').fadeIn(2000);


    $('.game').center();

    $('.gamewrap').on('click', '.item', function (event) {
        item = $(this);
        itemClue = item.data('clue');
        event.preventDefault();
        /* Act on the event */
    });


});