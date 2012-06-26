using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using ApiBridge.Commands;
using ApiBridge.Contracts;
using ApiBridge.Web.Interfaces;
using ApiBridge.Web.MVC.ControlPanel.Models;
using AutoMapper;

namespace ApiBridge.Web.MVC.ControlPanel.Controllers
{
    public class CommentsController : BaseController
    {
        ICommentManagementService commentService;

        public CommentsController(ICommentManagementService commentService)
        {
            this.commentService = commentService;
        }

        public ActionResult Index()
        {
            List<Comment> model = commentService.GetAllComments().ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult PostComment(PostCommentModel model)
        {
            if (ModelState.IsValid)
            {
                // Map model to entity
                Comment comment = GetComment(model);
                commentService.Save(comment);

                // Business Rule:
                // Only syndicate new comment when syndication checkbox is checked
                if (!String.IsNullOrEmpty(model.SyndicationFlag) && model.SyndicationFlag.Equals("1"))
                {
                    PublicCommentEvent(comment);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteComment(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                commentService.Delete(id);

                PublicCommentDeletedEvent(id);
            }

            return RedirectToAction("Index");
        }


        /// <summary>
        /// Map PostCommentModel to a contract entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        static Comment GetComment(PostCommentModel model)
        {
            Mapper.CreateMap<PostCommentModel, Comment>();
            return Mapper.Map<PostCommentModel, Comment>(model);
        }

        /// <summary>
        /// Syndicates the comment event.
        /// </summary>
        /// <param name="comment">The comment.</param>
        void PublicCommentEvent(Comment comment)
        {
            try
            {
                CommentEvent newCommentEvent = new CommentEvent();
                newCommentEvent.Comment = comment;

                BaseController.ServiceBus.PublishAsync<CommentEvent>(newCommentEvent, (result) => { });
            }
            catch
            {

            }
        }

        void PublicCommentDeletedEvent(string commentId)
        {
            try
            {
                CommentDeletedEvent deletedCommentEvent = new CommentDeletedEvent();
                deletedCommentEvent.Id = commentId;

                BaseController.ServiceBus.PublishAsync<CommentDeletedEvent>(deletedCommentEvent, (result) => { });
            }
            catch
            {

            }
        }
    }
}
