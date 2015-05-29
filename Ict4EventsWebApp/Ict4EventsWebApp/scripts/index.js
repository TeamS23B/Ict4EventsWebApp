/// <reference path="../typings/jquery/jquery.d.ts"/>
/// <reference path="jquery-1.11.2.min.js"/>
function loadNewPosts(){
	$.getJSON("newPost.json",function (data,textStatus) {
		console.log(textStatus);
		$.each(data,function(key,value){
			$("#postSpacerTop").after("<div class=\"post\">"+
				"<div class=\"title\">"+value[0].title+"</div>"+
				"<div class=\"username\">"+value[0].uploader+"</div>"+
				"<div class=\"content\">"+value[0].content+"</div>"+
				"<div class=\"likeFlag\">Likes: "+value[0].likes+" flags"+value[0].flags+"</div>"+
			"</div>");
		});
	});
}

$(document).ready(function(){
	$("#btAddPost").click(function () {
		loadNewPosts();
	});
	$(".addComment").click(function(){
		if(!$(this).children("input").length)
		$(this).html('<input type="text" class="commentInput" placeholder="type a comment"/>');
		console.log($(".commentInput"));
		$(".commentInput").focus();
	});
});