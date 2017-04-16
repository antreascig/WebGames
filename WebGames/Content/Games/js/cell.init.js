var isPaused = false;
var timeRemaining = null;
var item = null;
var itemClue = null;
var stage = $('.game').data('stage');
var level = $('.game').data('level');
var cVal = null;
var gameTime = null;
var enteredCode = null;
var unlockCode = null;
var score = null;
var levelScore = null;

if ($('.game').data('level') == 'level1') {
	levelScore = 30;
} else {
	levelScore = 10;
}
$.custom.Server.GetGameScore(
	function (res) { // success
	    if (res && res.success && res.score) { // to time einai remaining seconds
	    	score = res.score;
	    }
	    else { // fail

		}
}, function () { // fail
	 
});
$.custom.Server.Get_Escape_Solution( 
	function (solution) { // success
		unlockCode = solution;
}, function () { // fail
	
});
function startTimer(duration, display, redirUrl) {
    var timer = duration, minutes, seconds;

    setInterval(function () {
    	if(!isPaused) {	
	        minutes = parseInt(timer / 60, 10)
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

function endGame() {
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

// callback that will be run once images are ready 
loader.addCompletionListener(function() {
var time = null;
$.custom.Server.GetGameTime(
	function (res) { // success
	    if (res && res.success && res.time) { // to time einai remaining seconds
	    	time = res.time;
		    display = $('#time');
		    startTimer(time, display);
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

				$('#preloader').fadeOut(1000);
				$('.wrapper').fadeIn(2000);
				music.play();
			}
	    }
	    else { // fail

		}
}, function () { // fail
	 
});

}); 

		
$(window).on('resize', function(){
	$('.game').center();
});

$(document).ready(function() {
	$('.exit').click(function(event) {
		/* Act on the event */
		isPaused = false;
		$('#explainer').show();
	});
	$('.pause').click(function(event) {
		/* Act on the event */
		isPaused = true;
		$('#explainer').hide();
	});
	$('.wrapper').fadeIn(2000);
	$('.codenum').easyAudioEffects({
	   ogg : "/music/number.ogg",
	   mp3 : "/music/number.mp3",
	   eventType : "click" // or "click"

	});
	$('.item').easyAudioEffects({
	   ogg : "/music/click.ogg",
	   mp3 : "/music/click.mp3",
	   eventType : "click" // or "click"

	});
	$('.close').easyAudioEffects({
	   ogg : "/music/close.ogg",
	   mp3 : "/music/close.mp3",
	   eventType : "click" // or "click"

	});
	$('.game').center();

	$('.gamewrap').on('click', '.item', function(event) {
		item = $(this);
		itemClue = item.data('clue');
		event.preventDefault();
		/* Act on the event */
		$('.zoom').empty();
		$('.zoom').append('<img src="/images/cells/' + stage + '/' + level + '/' + itemClue + '.jpg" />');
		$('.popup').fadeIn(500);
	});
	$('body').on('click', '.close', function(event) {
		event.preventDefault();
		$(this).parent().fadeOut(500);
		$('.overlay').fadeOut(500);
		/* Act on the event */
	});
	$('.popup .close').click(function(event) {
		/* Act on the event */
		$('.popup').fadeOut('500', function() {
			$('.zoom').empty();
		});
	});
	$('#lock').click(function(event) {
		/* Act on the event */
		$('.overlay, .unlock').fadeIn(500);
	});
	$('.codenum').click(function(event) {
		/* Act on the event */
		cVal = parseInt($(this).html());
		if (cVal < 9) {
			cVal++;
			$(this).html(cVal);
		} else {
			cVal = 0;
			$(this).html(cVal);
		}
	});
	$('.item').hover(function() {
		$(this).glow({ radius: "8", color:"#fffb3d"});
	}, function() {
		$(this).glow({ disable:true }); 
	});

		$('#go').click(function(event) {
			/* Act on the event */
			 
			enteredCode = parseInt($('#c1').html() + $('#c2').html() + $('#c3').html());
 

			if (enteredCode == unlockCode) {
				// alert('woohooo');
			    success.play();
				isPaused = true;
				score += levelScore;
	    		$.custom.Server.SaveScoreGame(score, 
					function (res) { // success
						if (res && res.success) {
							if (res.IsCorrect) {
				    			setTimeout(function(){
						    		$('.wrapper').fadeOut(2000, function() {
						    			setTimeout(function(){
						    				location.reload();
						    			},500);
						    		});
				    			},2000);
							}
						} 
						else { // fail 

						}
				}, function () { // fail
					
				});

			} else {
				$(this).toggleClass('wrong');
				setTimeout(function() {
					$('#go').toggleClass('wrong');
				}, 1000);
			}
		});
});