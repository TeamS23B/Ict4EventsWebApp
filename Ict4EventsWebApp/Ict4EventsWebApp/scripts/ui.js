/// <reference path="../typings/jquery/jquery.d.ts"/>
/// <reference path="jquery-1.11.2.min.js"/>
var leftbar;

$(document).ready(function(){
	leftbar = $("#leftBar");
	$("#menuButton").click(function(){
		if(leftbar.css("margin-left")=="-250px"){
			leftbar.css("margin-left","0px");
		}else{
			leftbar.css("margin-left","-250px");
		}
	});
});