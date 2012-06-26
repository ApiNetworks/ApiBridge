using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Web.Interfaces;
using ApiBridge.Contracts;
using Raven.Client;

namespace ApiBridge.Web.Services
{
    public class CommentManagementService : ICommentManagementService
    {
        private readonly IDocumentSession session;

        public CommentManagementService(IDocumentSession session)
        {
            this.session = session;
        }

        public List<Comment> GetAllComments()
        {
            return session.Query<Comment>().ToList();
        }

        public void Save(Comment comment)
        {
            session.Store(comment);
            session.SaveChanges();
        }

        public void Delete(string id)
        {
            Comment comment = session.Load<Comment>(id);
            if (comment != null)
            {
                session.Delete<Comment>(comment);
                session.SaveChanges();
            }
        }
    }
}
