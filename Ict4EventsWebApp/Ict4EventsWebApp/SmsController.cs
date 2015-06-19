using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Oracle.ManagedDataAccess.Client;

namespace Ict4EventsWebApp
{
    public class SmsConnect
    {
        private static SmsConnect _instance;

        public static SmsConnect Instance
        {
            get { return _instance ?? (_instance = new SmsConnect()); }
        }

        private SmsConnect()
        {
            UsernameToken = new Dictionary<string, string>();
            highAutherized = new List<string>();
        }

        public string AddToken(string username, bool isHighAutherized = false)
        {
            var token = Guid.NewGuid().ToString();
            UsernameToken.Add(username, token);
            if (isHighAutherized) highAutherized.Add(username);
            return token;
        }

        public int CheckUser(string username, string token)
        {
            var r = 0;
            if (UsernameToken.ContainsKey(username))
            {
                if (UsernameToken[username] == token)
                {
                    r = 1;
                }
            }
            if (highAutherized.Contains(username) && r == 1)
                r = 2;
            return r;

        }

        public void RemoveToken(string username)
        {
            UsernameToken.Remove(username);
        }

        private Dictionary<string, string> UsernameToken;
        private List<String> highAutherized;
    }

    public class SmsController : ApiController
    {

        [HttpGet, Route("api/sms/postsOfCategorie")]
        public IHttpActionResult GetPostOfCategorie(int id, string username, string token)
        {
            var o = new
            {
                auterized = SmsConnect.Instance.CheckUser(username, token),
                
                topCategories = new List<object>(),
                categorieTrace = new List<object>(),
                categories = new List<object>(),
                posts = new List<object>()
            };

            if ((o.auterized < 1))
            {
                return Json(o);//quit if the user isn't autherized
            }

            var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            con.Open();

            //load top categories
            using (var com = con.CreateCommand())
            {
                com.CommandText = "SELECT bijdrage_id AS id, naam as name " +
                                  "FROM categorie " +
                                  "WHERE categorie_id IS NULL";
                var r = com.ExecuteReader();
                while (r.Read())
                {
                    o.topCategories.Add(new { id = Convert.ToInt32(r["id"]), title = (string)r["name"] });
                }
            }

            using (var com = con.CreateCommand())
            {

                //get an 7 layer deep breadcrum trail
                com.CommandText =
    @"SELECT t1.bijdrage_id AS b1id, t1.naam AS b1name, 
       t2.bijdrage_id AS b2id, t2.naam as b2name, 
       t3.bijdrage_id AS b3id, t3.naam as b3name, 
       t4.bijdrage_id AS b4id, t4.naam as b4name, 
       t5.bijdrage_id AS b5id, t5.naam as b5name, 
       t6.bijdrage_id AS b6id, t6.naam as b6name, 
       t7.bijdrage_id AS b7id, t7.naam as b7name
FROM categorie t1
LEFT JOIN categorie  t2 ON t1.CATEGORIE_ID = t2.bijdrage_id
LEFT JOIN categorie  t3 ON t2.CATEGORIE_ID = t3.bijdrage_id
LEFT JOIN categorie  t4 ON t3.CATEGORIE_ID = t4.bijdrage_id
LEFT JOIN categorie  t5 ON t4.CATEGORIE_ID = t5.bijdrage_id
LEFT JOIN categorie  t6 ON t5.CATEGORIE_ID = t6.bijdrage_id
LEFT JOIN categorie  t7 ON t6.CATEGORIE_ID = t7.bijdrage_id
WHERE t7.bijdrage_id = :bid
OR t6.bijdrage_id = :bid
OR t5.bijdrage_id = :bid
OR t4.bijdrage_id = :bid
OR t3.bijdrage_id = :bid
OR t2.bijdrage_id = :bid
OR t1.bijdrage_id = :bid
ORDER BY t1.bijdrage_id";

                //add the parameter
                var pBid = com.CreateParameter();
                pBid.DbType = DbType.Int32;
                pBid.Direction = ParameterDirection.Input;
                pBid.ParameterName = "bid";
                pBid.Value = id;
                com.Parameters.Add(pBid);

                var r = com.ExecuteReader();
                r.Read();
                //get the breadcrumb trace
                if(r.HasRows)
                    GetBreadCrumbTrace(o.categorieTrace,r);
            }
            
            //all categories in categorie
            using (var com = con.CreateCommand())
            {
                com.CommandText =
    @"SELECT bijdrage_id as bijid, naam
FROM categorie
WHERE categorie_id=:bid";
                var pBid = com.CreateParameter();
                pBid.DbType = DbType.Int32;
                pBid.Direction = ParameterDirection.Input;
                pBid.ParameterName = "bid";
                pBid.Value = id;
                com.Parameters.Add(pBid);

                var r = com.ExecuteReader();
                while (r.Read())
                {
                    o.categories.Add(new { id = Convert.ToInt32(r["bijid"]), title = (string)r["naam"] });
                }
            }

            //all posts in categorie
            using (var com = con.CreateCommand())
            {
                com.CommandText =
@"SELECT b.id AS id, b.soort as soort, be.BESTANDSLOCATIE AS url, bi.TITEL AS title, bi.INHOUD AS TEXT, NVL(lf.likes,0) AS likes, NVL(lf.flags,0) AS flags, a.GEBRUIKERSNAAM AS username
FROM bijdrage b LEFT JOIN bestand be ON b.ID = be.BIJDRAGE_ID 
  LEFT JOIN bericht bi ON b.ID = bi.BIJDRAGE_ID 
  LEFT JOIN bijdrage_bericht bb ON bi.BIJDRAGE_ID = bb.bericht_id 
  LEFT JOIN (SELECT bijdrage_id, SUM(likepost) AS likes, SUM(ongewenst) AS flags FROM account_bijdrage GROUP BY bijdrage_id) lf ON lf.bijdrage_id = b.ID 
  JOIN account a ON b.ACCOUNT_ID=a.id
WHERE be.CATEGORIE_ID = :bid
OR bb.BIJDRAGE_ID = :bid";

                var pBid = com.CreateParameter();
                pBid.DbType = DbType.Int32;
                pBid.Direction = ParameterDirection.Input;
                pBid.ParameterName = "bid";
                pBid.Value = id;
                com.Parameters.Add(pBid);

                var r = com.ExecuteReader();

                while (r.Read())
                {
                    //get an post item
                    o.posts.Add(GetPostItem(r));
                }
            }
            con.Dispose();
            //return all the data in json format
            return Json(o);
        }

        [HttpGet, Route("api/sms/postComments")]
        public IHttpActionResult GetPostComments(int id, string username, string token)
        {
            var auterized = SmsConnect.Instance.CheckUser(username, token);
            if (auterized < 1)
            {
                return Json(new { autherized = 0 });
            }

            var o =
               new
               {
                   comments = new List<object>()
               };
            
            //load all the data
            var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            con.Open();

            //load top categories
            using (var com = con.CreateCommand())
            {
                //get all comments on a post
                com.CommandText =
                    "SELECT b.bijdrage_id AS id, a.GEBRUIKERSNAAM AS username, bij.DATUM AS datetime, inhoud AS content, NVL(likes,0) AS likes, NVL(flags,0) AS flags " +
                    "FROM bericht b, bijdrage_bericht bb, account a, bijdrage bij LEFT JOIN " +
                    "  (SELECT SUM(likepost) AS likes, SUM(ongewenst) AS flags, bijdrage_id " +
                    "  FROM account_bijdrage " +
                    "  GROUP BY bijdrage_id) lf ON lf.bijdrage_id = bij.id " +
                    "WHERE b.BIJDRAGE_ID = bb.BERICHT_ID " +
                    "AND b.BIJDRAGE_ID = bij.id " +
                    "AND bij.ACCOUNT_ID = a.ID " +
                    "AND bb.BIJDRAGE_ID = :bid";
                var pBid = com.CreateParameter();
                pBid.DbType=DbType.Int32;
                pBid.Direction=ParameterDirection.Input;
                pBid.ParameterName = "bid";
                pBid.Value = id;
                com.Parameters.Add(pBid);

                var r = com.ExecuteReader();
                while (r.Read())
                {
                    //add a comment
                    o.comments.Add(
                        new
                        {
                            id = r["id"],
                            content = r["content"],
                            username = r["username"],
                            date = r["datetime"],
                            likes = r["likes"],
                            flags = r["flags"]
                        });
                }
            }

            //close connection
            con.Close();
            con.Dispose();

            //return as json object
            return Json(o);
        }

        [HttpGet, Route("api/sms/LikeFlag")]
        public IHttpActionResult LikeFlag(int id, string action, string username, string token)
        {
            if (SmsConnect.Instance.CheckUser(username, token) < 1)
            {
                return Json(new { succes = false, errormessage = "Not Authenticated", likes = 0, flags = 0});
            }
            if (!(action == "L" || action == "F"))
            {
                return Json(new { succes = false, errormessage = "Action not L or F", likes = 0, flags = 0 });
            }

            //load all the data
            var con = OracleClientFactory.Instance.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            con.Open();

            int _likes, _flags;

            using (var com = con.CreateCommand())
            {
                //use a stored procedure to add a like or flag
                com.CommandType=CommandType.StoredProcedure;
                com.CommandText = "AddLikeFlag";

                //parameter creation
                var pUsrn = com.CreateParameter();
                var pPost = com.CreateParameter();
                var pAction = com.CreateParameter();
                var pLikes = com.CreateParameter();
                var pFlags = com.CreateParameter();
                
                //names
                pUsrn.ParameterName = "username";
                pPost.ParameterName = "postId";
                pAction.ParameterName = "action";
                pLikes.ParameterName = "likes";
                pFlags.ParameterName = "flags";
                
                //directions
                pUsrn.Direction = ParameterDirection.Input;
                pPost.Direction = ParameterDirection.Input;
                pAction.Direction = ParameterDirection.Input;
                pLikes.Direction = ParameterDirection.Output;
                pFlags.Direction = ParameterDirection.Output;
                
                //types
                pUsrn.DbType = DbType.String;
                pPost.DbType = DbType.Int32;
                pAction.DbType = DbType.String;
                pLikes.DbType=DbType.Int32;
                pFlags.DbType=DbType.Int32;
                
                //values
                pUsrn.Value = username;
                pPost.Value = id;
                pAction.Value = action;

                com.Parameters.Add(pUsrn);
                com.Parameters.Add(pPost);
                com.Parameters.Add(pAction);
                com.Parameters.Add(pLikes);
                com.Parameters.Add(pFlags);

                try
                {
                    com.ExecuteNonQuery();

                    _likes = Convert.ToInt32(pLikes.Value);
                    _flags = Convert.ToInt32(pFlags.Value);
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 20000)
                    {
                        return Json(new { succes = false, errormessage = "Action not L or F, Oracle", likes = 0, flags = 0 });
                    }
                    throw;
                }
            }

            con.Close();
            con.Dispose();

            return Json(new {succes = true, errormessage = "N/A", likes = _likes, flags = _flags});
        }

        //Errormessages:
        //N/A No error message
        //WIP Work in progress
        //Not Authenticated, username or token don't match
        [HttpGet, Route("api/sms/placeComment")]
        public IHttpActionResult PlaceComment(int id, string comment, string username, string token)
        {
            if (SmsConnect.Instance.CheckUser(username, token) < 1)
            {
                return Json(new {succes = false, errormessage = "Not Authenticated"});
            }

            /*
             * commant
             * INSERT INTO BIJDRAGE values (null,1,sysdate,'bericht');
             * INSERT INTO bericht VALUES ((SELECT MAX(id) FROM bijdrage),null,'aapje');
             * INSERT INTO BIJDRAGE_BERICHT VALUES (1,(SELECT MAX(id) FROM bijdrage));
             */

            //load all the data
            var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            con.Open();
            //insert into bijdrage
            using (var com = con.CreateCommand())
            {
                com.CommandText = "INSERT INTO BIJDRAGE values (null,(SELECT id FROM account WHERE gebruikersnaam = :usrn),sysdate,'bericht')";
                var pUsrn = com.CreateParameter();
                pUsrn.DbType=DbType.String;
                pUsrn.Direction=ParameterDirection.Input;
                pUsrn.ParameterName = "usrn";
                pUsrn.Value = username;
                com.Parameters.Add(pUsrn);

                com.ExecuteNonQuery();
            }

            //insert into bericht
            using (var com = con.CreateCommand())
            {
                com.CommandText = "INSERT INTO bericht VALUES ((SELECT MAX(id) FROM bijdrage),null,:comtxt)";
                var pComtxt = com.CreateParameter();
                pComtxt.DbType = DbType.String;
                pComtxt.Direction = ParameterDirection.Input;
                pComtxt.ParameterName = "comtxt";
                pComtxt.Value = HttpUtility.UrlDecode(comment);
                com.Parameters.Add(pComtxt);

                com.ExecuteNonQuery();
            }

            //insert into bijdrage_bericht
            using (var com = con.CreateCommand())
            {
                com.CommandText = "INSERT INTO BIJDRAGE_BERICHT VALUES (:bid,(SELECT MAX(id) FROM bijdrage))";
                var pBid = com.CreateParameter();
                pBid.DbType = DbType.Int32;
                pBid.Direction = ParameterDirection.Input;
                pBid.ParameterName = "bid";
                pBid.Value = id;
                com.Parameters.Add(pBid);

                com.ExecuteNonQuery();
            }

            con.Close();
            con.Dispose();

            return Json(new {succes = true, errormessage = "N/A"});
        }

        [HttpGet, Route("api/sms/search")]
        public IHttpActionResult Search(string q, string username, string token)
        {
            /*;*/
            
            //check parameters
            if (q.Length < 1)
            {
                return Json(new {succes=false, errormessage="q is empty"});
            }
            if (SmsConnect.Instance.CheckUser(username, token) < 1)
            {
                return Json(new { succes = false, errormessage = "Not Authenticated" });
            }

            var o = new
            {
                categories = new List<object>(),
                posts = new List<object>()
            };

            var regexQ = "(" + q.Replace(" ", ")|(") + ")";//generate an regex statement

            //execute query
            var con = Oracle.ManagedDataAccess.Client.OracleClientFactory.Instance.CreateConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            con.Open();
            //insert into bijdrage
            using (var com = con.CreateCommand())
            {
                com.CommandText = @"SELECT b.id AS id, b.soort as soort, be.BESTANDSLOCATIE AS url, bi.TITEL AS title, bi.INHOUD AS TEXT, c.naam AS catname, NVL(lf.likes,0) AS likes, NVL(lf.flags,0) AS flags, a.GEBRUIKERSNAAM AS username
FROM bijdrage b LEFT JOIN bestand be ON b.ID = be.BIJDRAGE_ID 
  LEFT JOIN bericht bi ON b.ID = bi.BIJDRAGE_ID 
  LEFT JOIN bijdrage_bericht bb ON bi.BIJDRAGE_ID = bb.bericht_id 
  LEFT JOIN CATEGORIE c ON b.ID=c.BIJDRAGE_ID
  LEFT JOIN (SELECT bijdrage_id, SUM(likepost) AS likes, SUM(ongewenst) AS flags FROM account_bijdrage GROUP BY bijdrage_id) lf ON lf.bijdrage_id = b.ID 
  JOIN account a ON b.ACCOUNT_ID=a.id
WHERE (b.soort <> 'bericht' OR bi.titel IS NOT NULL)
AND REGEXP_LIKE(b.id||b.soort||be.bestandslocatie||bi.titel||bi.inhoud||c.NAAM||a.gebruikersnaam,:pQuery,'i')";
                var pQuery = com.CreateParameter();
                pQuery.DbType = DbType.String;
                pQuery.Direction = ParameterDirection.Input;
                pQuery.ParameterName = "pQuery";
                pQuery.Value = regexQ;
                com.Parameters.Add(pQuery);

                var r = com.ExecuteReader();
                while (r.Read())
                {
                    if ((string) (r["soort"] ?? "") == "categorie")
                        //if it is an categorie add it as if it is an categorie
                    {
                        o.categories.Add(new { id = Convert.ToInt32(r["id"]), title = (string)r["catname"] });
                    }
                    else//else add it as an post
                    {
                        o.posts.Add(GetPostItem(r));
                    }

                }
            }
            return Json(o);
        }

        private void GetBreadCrumbTrace(List<object> categorieTrace, IDataRecord r)
        {
            
            if (r["b1id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b1id"]), title = (string)r["b1name"] });
            }
            if (r["b2id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b2id"]), title = (string)r["b2name"] });
            }
            if (r["b3id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b3id"]), title = (string)r["b3name"] });
            }
            if (r["b4id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b4id"]), title = (string)r["b4name"] });
            }
            if (r["b5id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b5id"]), title = (string)r["b5name"] });
            }
            if (r["b6id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b6id"]), title = (string)r["b6name"] });
            }
            if (r["b7id"] != DBNull.Value)
            {
                categorieTrace.Add(new { id = Convert.ToInt32(r["b7id"]), title = (string)r["b7name"] });
            }
        }

        private static object GetPostItem(IDataRecord r)
        {
            object o;
            switch ((string)r["soort"])
            {
                case "bestand":
                    var url = (string)r["url"];
                    switch (url.Substring(url.LastIndexOf('.')).ToLower())//switch on file extension
                    {
                        case ".jpg":
                        case ".png":
                        case ".gif":
                        case ".bmp":
                                //return an image type
                            o = new
                            {
                                id = r["id"],
                                type = "file/image",
                                username = r["username"],
                                url = r["url"],
                                likes = r["likes"],
                                flags = r["flags"]
                            };
                            break;
                        case ".mp4":
                            //return an video type
                            o = new
                            {
                                id = r["id"],
                                type = "file/video",
                                username = r["username"],
                                url = r["url"],
                                likes = r["likes"],
                                flags = r["flags"]
                            };
                            break;
                            //audio
                        case ".mp3":
                            o = new
                            {
                                id = r["id"],
                                type = "file/audio",
                                subtype = "mpeg",
                                username = r["username"],
                                url = r["url"],
                                likes = r["likes"],
                                flags = r["flags"]
                            };
                            break;
                        case ".wav":
                            o = new
                            {
                                id = r["id"],
                                type = "file/audio",
                                subtype = "wav",
                                username = r["username"],
                                url = r["url"],
                                likes = r["likes"],
                                flags = r["flags"]
                            };
                            break;;
                        default:
                            //retun a generic file, will apear as a normal file
                            o = new
                                {
                                    id = r["id"],
                                    type = "file/file",
                                    username = r["username"],
                                    url = r["url"],
                                    likes = r["likes"],
                                    flags = r["flags"]
                                };
                            break;
                    }
                    break;
                case "bericht":
                    //if it is a normal post, jus put it in(in database comment on categorie)
                    o = new
                    {
                        id = r["id"],
                        type = "text",
                        title = r["title"],
                        username = r["username"],
                        text = r["text"],
                        likes = r["likes"],
                        flags = r["flags"]
                    };
                    break;
                default:
                    o = new {id = r["id"], type = "unkown"};
                    break;
            }
            return o;
        }
    }
}
