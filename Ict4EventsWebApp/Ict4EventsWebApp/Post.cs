using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ict4EventsWebApp
{
    public class Post
    {
        /*
         {
	"posts":[{
		"title": "TestPost",
		"uploader": "Test Uploader",
		"likes":0,
		"flags":0,
		"content":"Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu."}]
}
         */
        public String Title { get; set; }
        public String Uploader { get; set; }
        public int Likes { get; set; }
        public int Flags { get; set; }
        public String Content { get; set; }
        public Dictionary<String, String> Comments { get; set; }

        public Post()
        {
            Comments=new Dictionary<string, string>();
        }
    }
}