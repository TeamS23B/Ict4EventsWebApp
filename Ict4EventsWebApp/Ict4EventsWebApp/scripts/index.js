function loadNewPosts(){
    $.getJSON("/api/Sms/NewPosts", function (data, textStatus) {
        console.log(textStatus);
        $.each(data, function (key, value) {
		    console.log(value);
		    var totalHeight = 0;
            keepStadyBegin(0);
		    $.each(value, function (k, post) {
		        var p = $("<div class=\"post\">" +
				"<div class=\"title\">" + post.Title + "</div>" +
				"<div class=\"username\">" + post.Uploader + "</div>" +
				"<div class=\"content\">" + post.Content + "</div>" +
				"<div class=\"likeFlag\">Likes: " + post.Likes + " Flags: " + post.Flags + "</div>" +
			    "</div>");
		        $("#postSpacerTop").after(p);
		        totalHeight += p.height()+32;
		    });
            console.log(totalHeight);
            keepStadyEnd(totalHeight);
        });
    });
}

$(document).ready(function(){
	$("#btAddPost").click(function () {
		loadNewPosts();
	});
	$(".addComment").click(function() {
	    if (!$(this).children("input").length) {
	        $(this).html('<input type="text" class="commentInput" placeholder="type a comment"/>');
	        $(".commentInput").focus();
	    }
	});
});