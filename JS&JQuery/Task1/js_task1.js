$(document).ready(function(){
		$(".pager li").click(function(){
			$(".selected").removeClass("selected");
			$(this).addClass("selected");			
		});
	});