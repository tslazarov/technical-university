using Lipwig.Models;
using Lipwig.Utilities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lipwig.HttpUtilities
{
    public class HttpRequestHelper
    {
        #region Account

        public static async Task<string> LoginUserRemote(string email, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var user = JsonConvert.SerializeObject(new { Email = email, Password = password });

                var response = await client.PostAsync(Constants.ApiLogin, new StringContent(user.ToString(), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }


        public static async Task<string> RegisterUserRemote(User user, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var registerData = new { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Password = password, Balance = user.Balance, Currency = user.Currency };

                var response = await client.PostAsync(Constants.ApiRegister, new StringContent(JsonConvert.SerializeObject(registerData), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion

        #region Currencies

        public static async Task<string> GetCurrenciesRemote()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.GetAsync(Constants.ApiCurrencies);
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion

        #region Expenses

        public static async Task<string> GetExpenseRemote(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.GetAsync(string.Format("{0}{1}", Constants.ApiExpenses, id.ToString()));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> UpdateExpenseRemote(Expense expense)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PutAsync(Constants.ApiExpensesUpdate, new StringContent(JsonConvert.SerializeObject(expense), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> DeleteExpenseRemote(Expense expense)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.DeleteAsync(string.Format("{0}{1}", Constants.ApiExpenses, expense.Id.ToString()));
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion

        #region Incomes

        public static async Task<string> GetIncomeRemote(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.GetAsync(string.Format("{0}{1}", Constants.ApiIncomes, id.ToString()));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> UpdateIncomeRemote(Income income)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PutAsync(Constants.ApiIncomesUpdate, new StringContent(JsonConvert.SerializeObject(income), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }


        public static async Task<string> DeleteIncomeRemote(Income income)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.DeleteAsync(string.Format("{0}{1}", Constants.ApiIncomes, income.Id.ToString()));
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion

        #region Users

        public static async Task<string> GetUserByEmailRemote(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.GetAsync(string.Format("{0}{1}", Constants.ApiUsersByEmail, StringHelper.EncodeEmailForUrl(email)));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> UpdateUserRemote(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PutAsync(Constants.ApiUsersUpdate, new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> UpdateUserPassword(string email, string oldPassword, string newPassword)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PutAsync(Constants.ApiUsersUpdatePassword, new StringContent(JsonConvert.SerializeObject(new { Email = email, OldPassword = oldPassword, NewPassword = newPassword }), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> CreateExpenseRemote(string email, Expense expense)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PostAsync(Constants.ApiUsersExpensesCreate, new StringContent(JsonConvert.SerializeObject(new { Email = email, Expense = expense }), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> CreateIncomeRemote(string email, Income income)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

                var response = await client.PostAsync(Constants.ApiUsersIncomesCreate, new StringContent(JsonConvert.SerializeObject(new { Email = email, Income = income }), Encoding.UTF8, Constants.MediaType));
                return await response.Content.ReadAsStringAsync();
            }
        }

        #endregion
    }
}
