using Lipwig.Models;

namespace Lipwig.Web.Models
{
    public class ExpenseCreateViewModel
    {
        public string Email { get; set; }

        public Expense Expense { get; set; }
    }
}