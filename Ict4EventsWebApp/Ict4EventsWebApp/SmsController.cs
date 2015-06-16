using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ict4EventsWebApp
{
    public class SmsController : ApiController
    {
        [HttpGet, Route("api/sms/newPosts")]
        public IHttpActionResult GetNewPosts()
        {
            var o = new {posts = new List<Post>()};
            var p = new Post();
            p.Comments.Add(new KeyValuePair<string, string>("Rick","Wahahahaha"));
            p.Comments.Add(new KeyValuePair<string, string>("Rick","FU"));
            p.Content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.";
            p.Likes = 5;
            p.Flags = 0;
            p.Uploader = "Jim";
            p.Title = "Le me, Le latijn!";
            o.posts.Add(p);
            p = new Post();
            p.Comments.Add(new KeyValuePair<string, string>("Rick", "Wahahahaha"));
            p.Comments.Add(new KeyValuePair<string, string>("Rick2", "FU"));
            p.Content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.";
            p.Likes = 5;
            p.Flags = 0;
            p.Uploader = "Jim";
            p.Title = "Le me, Le latijn!";
            o.posts.Add(p);
            return Json(o);
        }

        [HttpGet, Route("api/sms/postsOfCategorie")]
        public IHttpActionResult GetPostOfCategorie(int id)
        {
            var o = new
            {
                categorieTrace = new List<object>(),
                categories = new List<object>(),
                posts = new List<object>()
            };

            o.categorieTrace.Add(new {id = 0, title = "aapje", username = "aapje", date = DateTime.Now, likes = 0, flags = 0, comments = 0});
            o.categorieTrace.Add(new { id = 0, title = "aapje", username = "aapje", date = DateTime.Now, likes = 0, flags = 0, comments = 0 });

            o.categories.Add(new {id = 0, title = "aapje", username = "aapje", date = DateTime.Now, likes = 0, flags = 0, comments = 0});
            o.categories.Add(new { id = 0, title = "aapje", username = "aapje", date = DateTime.Now, likes = 0, flags = 0, comments = 0 });

            o.posts.Add(new {id = 0, type = "file/file", url = "aapje",size = 0, likes = 0, flags = 0, comments = 0});
            o.posts.Add(new { id = 0, type = "file/image", url = "aapje", size = 0, likes = 0, flags = 0, comments = 0 });
            o.posts.Add(new { id = 0, type = "file/video", subType = "mp4", url = "aapje", size = 0, likes = 0, flags = 0, comments = 0 });
            o.posts.Add(new {id = 0, type = "text", title = "aapje", text = "ik wil meer aapjes", likes = 0, flags = 0, comments = 0});

            return Json(o);
        }

        [HttpGet, Route("api/sms/postComments")]
        public IHttpActionResult GetPostComments(int id)
        {
            var o =
                new
                {
                    id = 0,
                    title = "aapje",
                    username = "aapje",
                    date = DateTime.Now,
                    likes = 0,
                    flags = 0,
                    commentCnt = 2,
                    comments = new List<object>()
                };
            o.comments.Add(
                new
                {
                    id = 0,
                    title = "aapje",
                    username = "aapje",
                    date = DateTime.Now,
                    likes = 0,
                    flags = 0,
                    commentCnt = 0,
                    comments = new List<object>()
                });
            o.comments.Add(
                new
                {
                    id = 0,
                    title = "aapje",
                    username = "aapje",
                    date = DateTime.Now,
                    likes = 0,
                    flags = 0,
                    commentCnt = 0,
                    comments = new List<object>()
                });
            return Json(o);
        }
    }
}
