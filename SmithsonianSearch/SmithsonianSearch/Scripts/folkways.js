




/*


 ____|       |  |                                        ___|              _)         |         
 |     _ \   |  |  / \ \  \   /  _` |  |   |   __|     \___ \    __|   __|  |  __ \   __|   __| 
 __|  (   |  |    <   \ \  \ /  (   |  |   | \__ \           |  (     |     |  |   |  |   \__ \ 
_|   \___/  _| _|\_\   \_/\_/  \__,_| \__, | ____/     _____/  \___| _|    _|  .__/  \__| ____/ 
                                      ____/                                   _|                


*/


$(document).ready(function () {

/***************************
* General
* defined in styleguide.folklife.si.edu/code/assets/common/js/common-scripts.js
***************************/

/*********************
* Hero Carousel
 Uses slick.js from Ken Wheeler at http://kenwheeler.github.io/slick/
*********************/

	$bannerCarousel.slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		arrows: false,
		pauseOnHover: false,
		mobileFirst: 'true',
		asNavFor: '.banner-nav',
		autoplay: true, // pause for testing
		autoplaySpeed: 4000,
		responsive: [
			{
				breakpoint: 1480,
				settings: {
					autoplaySpeed: 6000,
				}
			}
	   	]
	});

	// mobile captions
	$('.mobile-banner-nav').slick({
		slidesToShow: 1,
		slidesToScroll: 1,
		mobileFirst: 'true',
		asNavFor: '#banner-carousel',
		dots: true,
		appendDots: '.mobile-dots',
	  	focusOnSelect: true,
		adaptiveHeight: true,
		prevArrow: false,
		nextArrow: false,
			responsive: [
				{
					breakpoint: 992,
					settings: {
						dots: false
					}
				}
		   	]
	});

	// pop-up captions
	$('.desktop-banner-nav').slick({
		slidesToShow: 4,
		slidesToScroll: 1,
		asNavFor: '#banner-carousel',
		dots: true,
	  	focusOnSelect: true,
		adaptiveHeight: true,
	});

	/****************
	* Banner tab vertical alignment so can peek from bottom
	* needed b/c iOS is inconsistent in vh
	* uses viewport-units-buggyfill, from https://github.com/rodneyrehm/viewport-units-buggyfill
	******************/
	window.viewportUnitsBuggyfill.init();

/**********
* Match Heights
* from https://github.com/liabru/jquery-match-height
* site-wide rules in common-scripts.js
**********/

	// Homepage banner text boxes
	// $('.mh-fw-banner-textboxes .textbox-inner').matchHeight();

	// Folkways homepage magazine feature and featurettes
	$('#mh-fw-homepage-magazine-details .details-inner').matchHeight();

	// Folkways homepage learn section
	$('.mh-fw-homepage-learn-section .details-inner').matchHeight();

	// Member support banner
	$('.mh-member-support').matchHeight({
		target: $('#member-image')
	});	

	// Membership page - membership details
	$('.membership-card').matchHeight();

	// Magazine Homepage
	$('.fw-mg-feature-thirds .details-inner').matchHeight();
	$('.fw-mg-feature-halfs .details-inner').matchHeight();
	$('.mh-fwmg-half-section').matchHeight();
	$('.back-issues-column').matchHeight(); // for right border

/**********
* Magazine article sidebar retrofit
* making conversion of old site to new one easier
* only run if #magazine-article-sidebar-js is present
**********/
	if(document.getElementById('magazine-article-sidebar-js')) {
		console.log('sidebar exists');
		var $sidebar = $('.sidebar');

		/*****
		* Video Player
		*****/
			$sidebar['video'] = {
				element : $('#magazine-article-sidebar-js img[alt="Click to watch video"]'),
			}
			$sidebar['video']['wrapper'] = $sidebar.video.element.closest('a'); // video anchor tag wrapper
			$sidebar['video']['vimeoID'] = $sidebar.video.wrapper.attr('href').match(/\d{9}/g); // look for 9 numbers in a row to find Vimeo ID, from http://stackoverflow.com/a/4975676
			$sidebar['video']['title'] = $sidebar.video.wrapper.data('lightview-title');	// get video title

			$sidebar.video.element.unwrap(); // remove wrapper because will interfere with video
			// put this before <div class="tag">Video</div>

			$sidebar.video.element.wrap('<div class="video-card"><div class="video sidebar-video-js" data-id="' + $sidebar.video.vimeoID + '" data-title="' + $sidebar.video.title + '" data-description="Lorem ipsum... <a>more</a>"><div class="center-box"><div class="modal-trigger"></div><div class="icon-play-button"></div></div></div>');
			// now get video card for adding tag
			// $sidebar['video']['card'] = $sidebar.element.parent('.video-card');

			// connect video popup to remodal and playing script
			site.videoPopupBuilder($('.sidebar-video-js'));
		
		/*****
		*Sidebar Captions, remove captions
		* go through all .captions and remove anything that matches
		*****/
			$sidebar['captions'] = $sidebar.find('.caption'); // video anchor tag wrapper
			$sidebar['captions']['oldTags'] = ['Video: ','Lesson plan: ','Audio: ','Slideshow: '];
			$sidebar.captions.each(function() {
				$this = $(this); // cache var
				var thisCaption = $this.html(); // get caption including any anchor tags
				// look for oldTags to remove
				// console.log(thisCaption);
				for($i=0;$i<$sidebar.captions.oldTags.length;$i++) {
					// console.log($sidebar.captions.oldTags[$i]);
					if (thisCaption.includes($sidebar.captions.oldTags[$i])) {
						// console.log('we have a match!',thisCaption);
						thisCaption = thisCaption.replace($sidebar.captions.oldTags[$i], ''); // remove old caption tag
						$this.html(thisCaption); // rewrite on page
					}
				}
			});

		/*****
		* Single Image Popup
		*****/
			// get image with lightvew class
			$sidebar['image'] = {
				element : $('a.lightview[data-lightview-type!="iframe"] img'),
			}

			console.log($sidebar.image.element);

			// continue if it exists
			if($sidebar['image']) {
				console.log('a.lightview does exist');
				// $sidebar['video']['wrapper'] = $sidebar.video.element.closest('a'); // video anchor tag wrapper
				// $sidebar['video']['vimeoID'] = $sidebar.video.wrapper.attr('href').match(/\d{9}/g); // look for 9 numbers in a row to find Vimeo ID, from http://stackoverflow.com/a/4975676
				// $sidebar['video']['title'] = $sidebar.video.wrapper.data('lightview-title');	// get video title

				// $sidebar.video.element.unwrap(); // remove wrapper because will interfere with video
				// // put this before <div class="tag">Video</div>

				// $sidebar.video.element.wrap('<div class="video-card"><div class="video sidebar-video-js" data-id="' + $sidebar.video.vimeoID + '" data-title="' + $sidebar.video.title + '" data-description="Lorem ipsum... <a>more</a>"><div class="center-box"><div class="modal-trigger"></div><div class="icon-play-button"></div></div></div>');
				// // now get video card for adding tag
				// // $sidebar['video']['card'] = $sidebar.element.parent('.video-card');

				// // connect video popup to remodal and playing script
				// site.videoPopupBuilder($('.sidebar-video-js'));
			} else {
				console.log('a.lightview does not exist');
			}

		/*****
		* Sidebar Headers
		*****/
			$sidebar['titles'] = {
				img : $('img[alt="Audio"]'),
				video : $sidebar.find('.video-card')
			}
			$sidebar.titles.img.replaceWith('<div class="tag">Audio</div>');
			$sidebar.titles.video.prepend('<div class="tag">Video</div>');
	}
});