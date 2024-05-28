using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Reservation.Pages.Staff
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Login { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (SqlConnection conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True"))
            {
                conn.Open();
                string sql = "SELECT password, role FROM Staff WHERE username = @Username";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", Login.Username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHashedPassword = reader.GetString(0);
                        string role = reader.GetString(1);

                        if (VerifyPassword(Login.Password, storedHashedPassword))
                        {
                            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Login.Username),
                            new Claim(ClaimTypes.Role, role)
                        };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                            return RedirectToPage("/Index");
                        }
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        private bool VerifyPassword(string password, string storedHashedPassword)
        {
            string hashedPassword = HashPassword(password);
            return hashedPassword == storedHashedPassword;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
