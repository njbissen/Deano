(function($) {
	$.fn.watermark = function(c, t) {
		return this.each(function() {
			var i = $(this);
			i.focus(function() {
				i.hasClass(c) && i.removeClass(c).val('');
			})
			.blur(function() {
				!i.val() && i.addClass(c).val(t);
			})
			.change(function() {
				i.hasClass(c) && i.removeClass(c);
				!i.val() && i.addClass(c).val(t);
			})
			.closest('form').submit(function() {
				i.removeWatermark(c);
			});
			i.blur();
		});
	};
	$.fn.removeWatermark = function(c) {
		return this.each(function() {
			$(this).hasClass(c) && $(this).val('');
		});
	};
})(jQuery);