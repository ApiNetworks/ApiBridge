using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiBridge.Contracts;

namespace ApiBridge.Web.Interfaces
{
    public interface ICommentManagementService
    {
        List<Comment> GetAllComments();
        void Save(Comment comment);
        void Delete(string id);
    }
}
