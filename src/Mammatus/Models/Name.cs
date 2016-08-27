using System.Text;

namespace Mammatus.Models
{

    public class Name
    {
        public string Title { get; set; }

        public string First { get; set; }

        public string Middle { get; set; }

        public string Last { get; set; }

        public string Suffix { get; set; }

        public Name()
        {
            Title = string.Empty;
            First = string.Empty;
            Middle = string.Empty;
            Last = string.Empty;
            Suffix = string.Empty;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var property in this.GetType().GetProperties())
            {
                string value = (string)property.GetValue(this, null);
                if (!string.IsNullOrEmpty(value))
                {
                    sb.Append(value);
                    sb.Append(" ");
                }
            }

            return sb.ToString().Trim();
        }
    }

}