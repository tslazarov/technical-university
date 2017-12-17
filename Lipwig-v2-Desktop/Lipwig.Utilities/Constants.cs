namespace Lipwig.Utilities
{
    public static class Constants
    {
        public static string BaseUri = "http://localhost:6500";
        public static string MediaType = "application/json";

        #region Api paths

        // Acount
        public static string ApiLogin = "api/account/login";
        public static string ApiRegister = "api/account/register";

        // Currencies
        public static string ApiCurrencies = "api/currencies";

        // Expenses
        public static string ApiExpenses = "api/expenses/";
        public static string ApiExpensesUpdate = "api/expenses/update";

        // Incomes
        public static string ApiIncomes = "api/incomes/";
        public static string ApiIncomesUpdate = "api/incomes/update";

        // Users
        public static string ApiUsers = "api/users/";
        public static string ApiUsersByEmail = "api/users/";
        public static string ApiUsersUpdate = "api/users/update";
        public static string ApiUsersUpdatePassword = "api/users/updatePassword";
        public static string ApiUsersExpensesCreate = "api/users/addExpense";
        public static string ApiUsersIncomesCreate = "api/users/addIncome";

        #endregion

        #region Messages

        public static string MessagePositiveColor = "#2CB144";
        public static string MessageNegativeColor = "#FFD50000";

        // Login
        public static string InvalidEmailOrPasswordMessage = "Invalid email or password";

        // Registration
        public static string AlreadyExistingUserMessage = "A user with this email exists";
        public static string UnsuccessfulRegistration = "Registration was unsuccessful";

        // Expenses
        public static string SuccessfulExpenseUpdate = "Expense update was successful";
        public static string UnsuccessfulExpenseUpdate = "Expense update was unsuccessful";
        public static string SuccessfulfulExpenseCreation = "Expense creation was successful";
        public static string UnsuccessfulExpenseCreation = "Expense creation was unsuccessful";

        // Incomes
        public static string SuccessfulIncomeUpdate = "Income update was successful";
        public static string UnsuccessfulIncomeUpdate = "Income update was unsuccessful";
        public static string SuccessfulfulIncomeCreation = "Income creation was successful";
        public static string UnsuccessfulIncomeCreation = "Income creation was unsuccessful";

        // Settings
        public static string SuccessfulUserDetailsUpdate = "User details update save was successful";
        public static string UnsuccessfulUserDetailsUpdate = "User details update was unsuccessful";
        public static string SuccessfulUserPasswordUpdate = "User password update save was successful";
        public static string UnsuccessfulUserPasswordUpdate = "User password update was unsuccessful";
        public static string IncorrectOldPassword = "Old password was incorrect";
        #endregion
    }
}
