using SqlToMdxLib.DAL;
using System.Text.RegularExpressions;

namespace SqlToMdxLib
{
    public class SqlToMdx
    {
        public void replaceAll()
        {
            UsersLineContext db = new UsersLineContext();
            foreach (var item in db.UsersLinePs)
            {
                item.MDX = replaceIn(item.MDX, "BG", "BG");
                item.MDX = replaceIn(item.MDX, "SalesP", "SalesP");
                item.MDX = replaceIn(item.MDX, "office", "Office");
                item.MDX = replaceIn(item.MDX, "ProfitCenter", "ProfitCenter");
                item.MDX = replaceIn(item.MDX, @"[End Customer]", "EndCustomerName");
                item.MDX = replaceLike(item.MDX, "ProfitCenter", "ProfitCenter");
                item.MDX = replaceStartWith(item.MDX, "ProfitCenter", "ProfitCenter");
                item.MDX = replaceStartWith(item.MDX, "[End Customer]", "EndCustomerName");
            }
            db.SaveChanges();
        }

        string replaceIn(string condition, string oldField, string newField)
        {
            string pattern;
            if (oldField.StartsWith("[") && oldField.EndsWith("]"))
            {
                pattern = @"\{0}\ *IN *\(.*?\)";
            }
            else
            {
                pattern = @"\b{0}\b *IN *\(.*?\)";
            }
            pattern = string.Format(pattern, oldField);
            string subPattern = @"\(.*?\)";
            string pre = string.Format(@" or [DimFilter].[{0}].CurrentMember.name = ", newField);
            string oldValue = null;
            string newValue = null;
            string members = null;
            foreach (Match result in Regex.Matches(condition, pattern, RegexOptions.IgnoreCase))
            {
                oldValue = result.Value;//BG IN ('IABG','MPBG')
                members = Regex.Match(oldValue, subPattern).Value;//('IABG','MPBG')
                members = members.TrimStart('(');
                members = members.TrimEnd(')');
                foreach (string member in members.Split(','))
                {
                    newValue += pre + member.Trim();
                }
                newValue = newValue.TrimStart(" or ".ToCharArray());
                newValue = "(" + newValue + ")";
                condition = condition.Replace(oldValue, newValue);
            }
            return condition;
        }

        string replaceLike(string condition, string oldField, string newField)//ProfitCenter
        {
            string pattern = @"\b{0}\b *like *'%.*?%'";
            pattern = string.Format(pattern, oldField);
            string subPattern = @"'%.*?%'";
            string pre = @"instr([DimFilter].[{0}].CurrentMember.name, '{1}')>=1";
            string oldValue = null;
            string newValue = null;
            string partString = null;
            foreach (Match result in Regex.Matches(condition, pattern, RegexOptions.IgnoreCase))
            {
                oldValue = result.Value;//ProfitCenter like '%VTN%'
                partString = Regex.Match(oldValue, subPattern).Value;//'%VTN%'
                partString = partString.TrimStart(@"'%".ToCharArray());
                partString = partString.TrimEnd(@"%'".ToCharArray());
                newValue = string.Format(pre, newField, partString);
                condition = condition.Replace(oldValue, newValue);
            }
            return condition;
        }

        string replaceStartWith(string condition, string oldField, string newField)//ProfitCenter
        {
            string pattern = @"\b{0}\b *like *'[^%].*?%'";
            if (oldField.StartsWith("[") && oldField.EndsWith("]"))
            {
                pattern = @"\{0}\ *like *'[^%].*?%'";
            }
            else
            {
                pattern = @"\b{0}\b *like *'[^%].*?%'";
            }
            pattern = string.Format(pattern, oldField);
            string subPattern = @"'.*?%'";
            string pre = @"instr([DimFilter].[{0}].CurrentMember.name, '{1}')=1";
            string oldValue = null;
            string newValue = null;
            string partString = null;
            foreach (Match result in Regex.Matches(condition, pattern, RegexOptions.IgnoreCase))
            {
                oldValue = result.Value;//ProfitCenter like '%VTN%'
                partString = Regex.Match(oldValue, subPattern).Value;//'%VTN%'
                partString = partString.TrimStart(@"'".ToCharArray());
                partString = partString.TrimEnd(@"%'".ToCharArray());
                newValue = string.Format(pre, newField, partString);
                condition = condition.Replace(oldValue, newValue);
            }
            return condition;
        }
    }
}
