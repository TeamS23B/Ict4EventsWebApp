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
        public IHttpActionResult GetNewPosts()
        {
            var o = new {posts = new List<Post>()};
            var p = new Post();
            p.Comments.Add("Rick","Wahahahaha");
            p.Comments.Add("Rick2","FU");
            p.Content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.";
            p.Likes = 5;
            p.Flags = 0;
            p.Uploader = "Jim";
            p.Title = "Le me, Le latijn!";
            o.posts.Add(p);
            p = new Post();
            p.Comments.Add("Rick", "Wahahahaha");
            p.Comments.Add("Rick2", "FU");
            p.Content = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu.";
            p.Likes = 5;
            p.Flags = 0;
            p.Uploader = "Jim";
            p.Title = "Le me, Le latijn!";
            o.posts.Add(p);
            return Json(o);
        }
    }
}
