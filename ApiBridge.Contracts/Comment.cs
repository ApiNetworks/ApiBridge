using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiBridge.Contracts
{
    public class Comment
    {
        private string _id;

        public string Id
        {
            get
            {
                if (String.IsNullOrEmpty(this._id))
                {
                    this._id = GenerateId();
                }
                return this._id;
            }
            set { this._id = value; }
        }

        public string Text { get; set; }

        private string GenerateId()
        {
            return "comments/" + Guid.NewGuid().ToString();
        }
    }
}
