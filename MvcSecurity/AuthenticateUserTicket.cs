using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easy.Public.MvcSecurity
{
    [Serializable]
    public class AuthenticateUserTicket
    {
        public AuthenticateUserTicket(String name, String[] roles, Int32 menuPrivType, String userData, DateTime issueDate, DateTime expiration)
        {
            this.Name = name;
            this.UserData = userData;
            this.Expiration = expiration;
            this.IssueDate = issueDate;
            this.Roles = roles;
            this.MenuPrivType = menuPrivType;
        }
        public DateTime Expiration { get; set; }
        public Boolean Expired 
        {
            get
            {
                if (DateTime.Now > this.Expiration)
                {
                    return true;
                }
                return false;
            }
        }
        public String[] Roles { get; set; }
        public DateTime IssueDate { get; set; }
        public String Name { get; set; }
        public String UserData { get; set; }
        public Int32 MenuPrivType { get; set; }

        public override String ToString()
        {
            StringBuilder ticketSb = new StringBuilder();
            ticketSb.AppendFormat("{0}|", this.Name);
            ticketSb.AppendFormat("{0}|", this.IssueDate);
            ticketSb.AppendFormat("{0}|", this.Expiration);
            ticketSb.AppendFormat("{0}", this.UserData);

            return ticketSb.ToString();
        }
    }
}
