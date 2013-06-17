﻿using Cooperate_mvc.Models;
using Cooperate_mvc.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cooperate_mvc.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private compact_dbEntities db = new compact_dbEntities();

        //
        // GET: /Messages/

        public ActionResult Index()
        {
            return View();
        }

        private List<ChatMessageModel> GetMessages(string withUser)
        {
            return (from m in db.Messages
                    join uf in db.Users on m.User_from equals uf.User_id
                    join ut in db.Users on m.User_to equals ut.User_id
                    where (uf.User_login.Equals(User.Identity.Name) && ut.User_login.Equals(withUser)) || (uf.User_login.Equals(withUser) && ut.User_login.Equals(User.Identity.Name))
                    orderby m.Message_datetimt descending
                    select new ChatMessageModel()
                    {
                        Datetime = m.Message_datetimt,
                        Id = m.Message_id,
                        RecipientLogin = ut.User_login,
                        SenderLogin = uf.User_login,
                        Text = m.Message_text
                    }).ToList();
        }

        //
        // GETL //Messages/Chat/{login}

        public ActionResult Chat(string login = "")
        {
            User user = db.Users.SingleOrDefault(u => u.User_login.Equals(login));

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ChatModel()
            {
                UserLogin = login,
                Messages = GetMessages(login)
            };

            return View(model);
        }

        [HttpPost]
        public void SendMessage(string MessageText, string UserLogin)
        {
            if (MessageText == null || UserLogin == null)
                return;

            var uFrom = db.Users.SingleOrDefault(u => u.User_login.Equals(User.Identity.Name));

            if (uFrom == null)
                return;

            var uTo = db.Users.Single(u => u.User_login.Equals(UserLogin));

            Message msg = new Message()
            {
                Message_datetimt = DateTime.Now,
                Message_text = MessageText,
                User_from = uFrom.User_id,
                User_to = uTo.User_id
            };

            db.Messages.Add(msg);
            db.SaveChanges();

            var model = new ChatMessageModel()
            {
                Datetime = msg.Message_datetimt,
                Id = msg.Message_id,
                RecipientLogin = UserLogin,
                SenderLogin = User.Identity.Name,
                Text = MessageText
            };

            ChatSocket.SendTo(User.Identity.Name, UserLogin, this.PartialViewToString("_Message", model));
        }
    }
}
