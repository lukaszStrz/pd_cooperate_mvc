using System.Text;
namespace Cooperate_mvc.Models
{
    public class UserPrimModel
    {
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Login);
            sb.Append(" (");
            sb.Append(FirstName);
            sb.Append(' ');
            sb.Append(LastName);
            sb.Append(')');
            return sb.ToString();
        }
    }
}