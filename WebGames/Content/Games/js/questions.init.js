var isPaused = false;
var timeRemaining = null;
var time = null;
var music = $("#music")[0];
var correct = $("#correct")[0];
var wrong = $("#wrong")[0];
var questions = null;
var qcount = null;
var tries = 8;
var currentQ = null;
var randomQ = null;
var correctAnswer = null;
function startTimer(duration, display, redirUrl) {
    var timer = duration, minutes, seconds;

    setInterval(function () {
    	if(!isPaused) {	
	        minutes = parseInt(timer / 60, 10);
	        seconds = parseInt(timer % 60, 10);
	        minutes = minutes < 10 ? "0" + minutes : minutes;
	        seconds = seconds < 10 ? "0" + seconds : seconds;

	        display.text(minutes + ":" + seconds);
	        if (--timer <= 0) {
	        	isPaused = true;
                endGame(redirUrl);
	            // timer = duration;
	        }
        	timeRemaining = timer;
        } else {
        	timeRemaining = timer;
        }
    }, 1000);
}
var success = $("#mysoundclip")[0];

function endGame(redirUrl) {
    $('.wrapper').fadeOut(2000, function() {
        window.location.replace(redirUrl);
    });
}
jQuery.fn.center = function () {
    this.css("position","absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) + 
                                                $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) + 
                                                $(window).scrollLeft()) + "px");
    return this;
}
$(window).on('resize', function(){
	$('.game').center();
});



		loader.addCompletionListener(function() {
		var time = null;
		$.custom.Server.GetGameTime(
			function (res) { // success
			    if (res && res.success && res.time) { // to time einai remaining seconds
			    	time = res.time;
				    display = $('#time');
				    startTimer(time, display);

			    }
			    else { // fail

				}
		}, function () { // fail
			 
		});
			if (isMobile == true) {
				$('body').addClass('mobile');
				$('.windows8').hide();	
				$('#letsgo').addClass('show');
				$('#letsgo').click(function(event) {
					$('#preloader').fadeOut(1000);
					$('.wrapper').fadeIn(2000);
					music.play();
				});
					
			} else {
				$('.game').center();
				$('#preloader').fadeOut(1000);
				$('.wrapper').fadeIn(2000);
				music.play();
			}
		}); 


$(document).ready(function() {
 	loader.start(); 
	$('.gamewrap').on('click', '.item', function(event) {
		item = $(this);
		itemClue = item.data('clue');
		event.preventDefault();
		/* Act on the event */
	});

setInterval(function(){
	$.custom.Server.SaveGameTime(timeRemaining, 
		function (res) { // success
			if (res && res.success) {
				console.log('Playtime updated');
			} 
			else { // fail
				console.log('Playtime not updated!');

			}
	}, function () { // fail
		
	});
},5000);

});