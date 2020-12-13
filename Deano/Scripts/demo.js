

var slideshowElement = $('#slideshow'); // for demonstration

$( '#showNext' ).click(function(){
  slideshow.show( 'next' );
  // or slideshowElement.slideshow( 'show', 'next' );
});

$( '#showPrevious' ).click(function(){
  slideshow.show( 'previous' );
  // or slideshowElement.slideshow( 'show', 'previous' );
  // etc.
});

$( '#showIndex' ).click(function(){
  slideshow.show( 2 );
});

$( '#showWithOptions' ).click(function(){
  slideshow.show( 0, {
    transition: 'fadeThroughBackground',
    duration: 2000
  });
});

$( '#play' ).click(function(){
  slideshow.play( true ); // true plays and shows the next slide immediate rather than waiting for the timer
  // or slideshowElement.slideshow( 'play' );
});

$( '#stop' ).click(function(){
  slideshow.stop();
  // or slideshowElement.slideshow( 'stop' );
});

// create custom transitions like so...
jQuery.rf.slideshow.defineTransition( 'custom', function ( params, arg ){
  // there is no arg, but if you wanted to pass an arg in, you'd
  // set the transition option to "custom(dude)"
  // and then `arg` would be `dude`, just like push(left), etc.
  var half = params.duration / 2;
  params.previous.hide();
  params.next.hide();
  for (var i = 0, l = 3; i < l; i++) {
    setTimeout(function(){
      params.previous.show();
      setTimeout(function(){
        params.previous.hide();
      }, half / 6);
    }, half / 3);
  }

  setTimeout(function(){
    params.next.show();
    for (var i = 0, l = 3; i < l; i++) {
      setTimeout(function(){
        params.next.hide();
        setTimeout(function(){
          params.next.show();
        }, half / 6);
      }, half / 3);
    }
  }, half);
});

$('#transition').bind('change', function(){

  slideshow.options.transition = this.value;
});