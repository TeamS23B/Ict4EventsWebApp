//string.format http://stackoverflow.com/questions/610406/javascript-equivalent-to-printf-string-format
if (!String.format) {
    String.format = function (format) {
        var args = Array.prototype.slice.call(arguments, 1);
        return format.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match
            ;
        });
    };
}

var loadRunning = false;
var loadPostRunning = false;

function loadPosts(categorieId) {
    if (loadRunning) return;//fix for event spam :D
    loadRunning = true;
    window.curId = categorieId;
    $.getJSON("api/sms/PostsOfCategorie?id=" + categorieId + "&username=" + username + "&token=" + token, function (data) {

        var topCategories = $("#leftBarButtons");
        topCategories.empty();

        //load categories

        $.each(data.topCategories, function (id, value) {
            topCategories.append($('<div class="leftBarButton" id="cat' + value.id + '">' + value.title + '</div>')).one("click", onCatClick);

        });

        var catTrace = $("#categorieTrace");
        catTrace.empty();

        //load categorie trace
        $.each(data.categorieTrace.reverse(), function (id, value) {
            catTrace.append(">");
            catTrace.append($('<a href="?id=' + value.id + '">' + value.title + '</a>')).one("click", onCatClick);

        });

        var cats = $("#categories");
        cats.empty();

        //load categories
        $.each(data.categories, function (id, value) {
            cats.append($('<div class="categorie" id="cat' + value.id + '">' + value.title + '</div>')).one("click", onCatClick);
        });

        var posts = $("#posts");
        posts.empty();
        //load posts
        $.each(data.posts, function (id, value) {
            //add an item based off the type it is
            var ctrl;
            switch (value.type.substring(0, 4)) {
                case "text":
                    ctrl = ($(String.format('<div class="post" id="post{0} "> ' +
                        '<div class="title">{1}</div>' +
                        '<div class="username">{2}</div>' +
                        '<div class="content">{3}</div> ' +
                        '<div class="stats"><div id="like{0}">Likes: {4}</div> <div id="flag{0}">Flags: {5}</div></div>' +
                        '</div>', value.id, value.title, value.username, value.text, value.likes, value.flags)));
                    break;
                case "file":
                    switch (value.type.substring(5)) {
                        case "file":
                            ctrl = ($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><a href="{3}">{3}</a></div> ' +
                                '<div class="stats"><div id="like{0}">Likes: {4}</div> <div id="flag{0}">Flags: {5}</div></div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags, value.comments)));
                            break;
                        case "image":
                            ctrl = ($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><img src="{3}" alt="{1}"/></div> ' +
                                '<div class="stats"><div id="like{0}">Likes: {4}</div> <div id="flag{0}">Flags: {5}</div></div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags)));
                            break;
                        case "video":ctrl = posts.append($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><video width="100%" preload="metadata"><source src="{3}" type="video/mp4"></video></div> ' +
                                '<div class="stats"><div id="like{0}">Likes: {4}</div> <div id="flag{0}">Flags: {5}</div></div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags)));
                            break;
                        case "audio":
                            ctrl = ($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><audio preload="metadata" src="{3}" controls="controls" /></div> ' +
                                '<div class="stats"><div id="like{0}">Likes: {4}</div> <div id="flag{0}">Flags: {5}</div></div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags)));
                            break;
                        default:
                            console.log("Unkown filetype!" + value.id);
                            break;
                    }
                    break;
                default:
                    ctrl = null;
                    console.log("Unkown type! id=" + value.id);
                    break;
            }
            if (ctrl != null) {
                posts.append(ctrl);
                ctrl.find("#like" + value.id).click(function(e) {
                    $.getJSON("api/sms/LikeFlag?id=" + value.id + "&action=L&username=" + window.username + "&token=" + window.token,function() {
                        loadPosts(curId);
                    });
                    e.stopPropagation();
                });
                ctrl.find("#flag" + value.id).click(function(e) {
                    $.get("api/sms/LikeFlag?id=" + value.id + "&action=F&username=" + window.username + "&token=" + window.token,function(data) {
                        loadPosts(curId);
                    });
                    e.stopPropagation();
                });
            }

        });

        $(".post").click(function (e) {
            e.stopPropagation(); //stop event, first need to create it
            var c = $(this).clone(); //make a clone and add the top class to it
            //enable controls for video and audio
            $(this).find("video, audio").attr("controls", "controls");
            c.addClass('top');
            $("#content").append(c); //append to #content
            c.click(function (ev) {
                ev.stopPropagation(); //stop it from getting through
            });
            loadCommets(c);
        });


    }).success(function () {
        loadRunning = false;
    }).fail(function () { loadRunning = false; });
}

$("html").click(function () {
    $(".post.top").remove();//remove the top post if the event isn't stopped
});

function onCatClick(event) {//FIXED: spam click, don't know why
    event.stopPropagation();
    var stateObj = { foo: "bar" };
    if ($(event.target).prop("tagName") == 'A') {
        history.pushState(stateObj, "The new page", $(event.target).attr("href"));//change url
        loadPosts(parseInt($(event.target).attr("href").substr(4))); //loads from ?id=...
    } else {
        history.pushState(stateObj, "The new page", "?id=" + $(event.target).attr("id").substr(3));//change url
        loadPosts(parseInt($(event.target).attr("id").substr(3)));//loads from cat...
    }

    return false;
}

function loadCommets(topPost) {
    if (loadPostRunning)
        return;
    var id = topPost.attr("id").substring(4);

    console.log(id);

    var oldCom = topPost.find(".comments");//remove old comments
    if (oldCom != null)
        oldCom.remove();
    
    var comCont = $('<div class="comments"></div>');
    topPost.append(comCont);

    loadPostRunning = true;
    $.getJSON("api/sms/PostComments?id=" + id + "&username=" + window.username + "&token=" + window.token, function (data) {
        $.each(data.comments, function(key, value) {
            var com = $(String.format('<div class="comment"><div class="cusername">{0}</div>' +
                '<div class="ccontent">{1}</div>' +
                '<div class="cstats"><div id="like{4}">Likes: {2}</div> <div id="flag{4}">Flags: {3}</div></div></div>', value.username, value.content, value.likes, value.flags,value.id));
            comCont.append(com);
            com.find("#like" + value.id).click(function(e) {
                $.get("api/sms/LikeFlag?id=" + value.id + "&action=L&username=" + window.username + "&token=" + window.token, function () {
                    loadCommets();
                });
                e.stopPropagation();
            });
            com.find("#flag" + value.id).click(function(e) {
                $.get("api/sms/LikeFlag?id=" + value.id + "&action=F&username=" + window.username + "&token=" + window.token, function() {
                    loadCommets();
                });
                e.stopPropagation();
            });
        });
        comCont.append($('<div id="newComment">' +
            '<textarea id="commentText"/><button id="commentPlace" placeholder="comment">Plaatsen</button>' +
            '</div>'));
        $("#commentPlace").click(function() {
            var comment = $("#commentText").val();
            $.getJSON("api/sms/PlaceComment?id="+id+"&comment="+comment+"&username="+window.username+"&token="+window.token,function(data) {
                if (!data.succes) {
                    $("#commentText").val(data.errormessage);
                } else {
                    var com = $(String.format('<div class="comment"><div class="cusername">{0}</div>' +
                    '<div class="ccontent">{1}</div>' +
                    '<div class="cstats">Likes: {2} Flags: {3}</div></div>', window.username, comment, 0, 0));
                    comCont.append(com);
                    $("#commentText").val("");
                }
            });
        });
    }).success(function () {
        loadPostRunning = false;
    }).fail(function () { loadPostRunning = false; });
}

function Like() {
    
}

function Flag() {
    
}