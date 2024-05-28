using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Hotel_Reservation.Pages.Staff
{
    [Authorize(Policy = "RequireAdminRole")]
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public StaffInputModel Staff { get; set; }
        public void OnGet()
        {
            // Check if the logged-in user is an admin
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return;
            }
            else
            {
                // Redirect non-admin users
                RedirectToPage("/Index");
            }
        }
        public IActionResult OnPost()
        {
            // Check if the logged-in user is an admin
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToPage("/Index");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string hashedPassword = HashPassword(Staff.Password);

            using (SqlConnection conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True"))
            {
                conn.Open();
                string sql = "INSERT INTO Staff (username, password, fname, lname, email, phone, role) VALUES (@Username, @Password, @Fname, @Lname, @Email, @Phone, @Role)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", Staff.Username);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Fname", Staff.Fname);
                cmd.Parameters.AddWithValue("@Lname", Staff.Lname);
                cmd.Parameters.AddWithValue("@Email", Staff.Email);
                cmd.Parameters.AddWithValue("@Phone", Staff.Phone);
                cmd.Parameters.AddWithValue("@Role", Staff.Role);
                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("/Index");
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

        public class StaffInputModel
        {
            [Required]
            public string Username { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required]
            public string Fname { get; set; }
            [Required]
            public string Lname { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            [Phone]
            public string Phone { get; set; }
            [Required]
            public string Role { get; set; }
        }
    }
}
