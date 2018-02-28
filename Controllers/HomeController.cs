using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using belt2.Factory;
using belt2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace belt2.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory userfactory;
        private readonly IdeaFactory ideafactory;
        public HomeController(UserFactory usersf, IdeaFactory ideaf){
            userfactory = usersf;
            ideafactory = ideaf;
        }
        
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("User/Register")]
        public IActionResult AddUser(User user){
            if(ModelState.IsValid){
                userfactory.AddNewUser(user);
                return RedirectToAction("Success");
            }
            return View("Index");
        }

        public IActionResult Success(){
            return View();
        }

        [HttpPost]
        [Route("logattempt")]
        public IActionResult LogTry(string email, string pass){
            var myQuery = userfactory.GetUsers();
            foreach(var x in myQuery){
                if( x.email.ToString() == email && x.password.ToString() == pass){
                    HttpContext.Session.SetInt32("id", x.id);
                    HttpContext.Session.SetString("user", x.first_name);
                    return RedirectToAction("LoggedIn");
                }
            }
            ViewBag.errors = "invalid email/password combination";
            return View("index");
        }

        [HttpGet]
        [Route("Home")]
        public IActionResult LoggedIn(){
            if(HttpContext.Session.GetInt32("id") > 0){
                int? thisid = HttpContext.Session.GetInt32("id");
                string thisuser = HttpContext.Session.GetString("user");
                ViewBag.sessionid = thisid;
                ViewBag.sessionuser = thisuser;
                ViewBag.ideas = ideafactory.GetAllIdeas();
                return View();
            }
            return RedirectToAction("Index");
        }

        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }

        [Route("NewIdea")]
        [HttpPost]
        public IActionResult AddIdea(Idea idea){
            ideafactory.AddIdea(idea);
            return RedirectToAction("LoggedIn");
        
        }
        [Route("users/{id}")]
        public IActionResult UserInfo(int id){
            ViewBag.user = userfactory.GetUserById(id);
            return View("UserInfo");
        }

        [Route("/idea/{id}")]
        public IActionResult IdeaInfo(int id){
            ViewBag.idea = ideafactory.GetIdeaById(id);
            ViewBag.likedby = ideafactory.GetLikersByID(id);
            return View("IdeaInfo");
        }

        [Route("/users/{poster}")]
        public IActionResult UserInfoByPoster(string poster){
            ViewBag.user = userfactory.GetUserById(2);
            return View("UserInfo");
        }

        [Route("/idea/{idea_id}/like/{user_id}")]
        public IActionResult LikeIdea(int idea_id, int user_id){
            ideafactory.LikeIdea(idea_id, user_id);
            return RedirectToAction("LoggedIn");
        }
        [Route("/idea/{idea_id}/unlike/{user_id}")]
        public IActionResult UnlikeIdea(int idea_id, int user_id){
            ideafactory.UnlikeIdea(idea_id, user_id);
            return RedirectToAction("LoggedIn");
        }
        [Route("/idea/{idea_id}/delete")]
        public IActionResult DeleteIdea(int idea_id){
            ideafactory.DeleteIdeaByID(idea_id);
            return RedirectToAction("LoggedIn");
        }
    }
}