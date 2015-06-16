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

function loadPosts(categorieId) {
    if (loadRunning) return;//fix for event spam :D
    loadRunning = true;
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
            switch (value.type.substr(0, 4)) {
                case "text":
                    posts.append($(String.format('<div class="post" id="post{0} "> ' +
                        '<div class="title">{1}</div>' +
                        '<div class="username">{2}</div>' +
                        '<div class="content">{3}</div> ' +
                        '<div class="stats">Likes: {4} Flags: {5}</div>' +
                        '</div>', value.id, value.title, value.username, value.text, value.likes, value.flags)));
                    break;
                case "file":
                    switch (value.type.substr(5)) {
                        case "file":
                            posts.append($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><a href="{3}">{3}</a></div> ' +
                                '<div class="stats">Likes: {4} Flags: {5} Comments: {6} </div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags, value.comments)));
                            break;
                        case "image":
                            posts.append($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><img src="{3}" alt="{1}"/></div> ' +
                                '<div class="stats">Likes: {4} Flags: {5} Comments: {6} </div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags)));
                            break;
                        case "video":
                            posts.append($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><video width="100%" preload="metadata"><source src="{3}" type="video/mp4"></video></div> ' +
                                '<div class="stats">Likes: {4} Flags: {5}</div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags)));
                            break;
                        case "audio":
                            posts.append($(String.format('<div class="post" id="post{0} "> ' +
                                '<div class="title">{1}</div>' +
                                '<div class="username">{2}</div>' +
                                '<div class="content"><audio width="100%" preload="metadata"><source src="{3}" type="audio/{6}"></audoi></div> ' +
                                '<div class="stats">Likes: {4} Flags: {5} </div>' +
                                '</div>', value.id, value.url, value.username, value.url, value.likes, value.flags, value.subType)));
                            break;
                        default:
                            console.log("Unkown filetype!" + value.id);
                            break;
                    }
                    break;
                default:
                    console.log("Unkown type! id=" + value.id);
                    break;
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
        });


    }).success(function () {
        loadRunning = false;
    }).fail(function () { loadRunning = false; });
}

$("html").click(function () {
    $(".post.top").remove();//remove the top post if the event isn't stopped
});

function onCatClick(event) {//spam click, don't know why
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

$(document).ready(function () {
    $(".post").click(function () {
        var c = $(this).clone();
        c.addClass('top');
        $("#content").append(c);
    });
});