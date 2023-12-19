using EVOKETASK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using MimeKit;


namespace EVOKETASK.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult EmpDetails()
        {
            string connection = _configuration.GetConnectionString("DBconnection");
            var employee = new List<EmpList>();
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("GETALLDETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader s = cmd.ExecuteReader();
                    while (s.Read())
                    {
                        EmpList emp = new EmpList
                        {
                            ID = (Int32)s["ID"],
                            NAME = s["NAME"].ToString(),
                            EMAIL = s["EMAIL"].ToString(),
                            PHONENUMBER = (Int64)s["PHONENUMBER"],
                            DEPTNAME = s["DEPTNAME"].ToString(),
                            GNAME = s["GNAME"].ToString()
                        };
                        employee.Add(emp);
                    }
                }
            }
            return View(employee);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateEmployee()
        {
            Employee emp = new Employee();
            return View(emp);
        }
        [HttpPost]
        public IActionResult CreateEmployee(EmpList emp)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Employee employee = new Employee();
                    using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand("CreateEmployee", con);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@NAME", emp.NAME);
                        command.Parameters.AddWithValue("@EMAIL", emp.EMAIL);
                        command.Parameters.AddWithValue("@PHONENUMBER", emp.PHONENUMBER);
                        command.Parameters.AddWithValue("@DEPTNAME", emp.DEPTNAME);
                        command.Parameters.AddWithValue("@GName", emp.GNAME);
                        int result = command.ExecuteNonQuery();
                        con.Close();
                   }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                // Handle the exception or redirect to an error page
            }

            return RedirectToAction("EmpDetails");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditView(int id)
        {
            EmpList emp = new EmpList();
            SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234");
            con.Open();
            SqlCommand command = new SqlCommand("Getbyid", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    emp.ID = (Int32)reader["ID"];
                    emp.NAME = reader["NAME"].ToString();
                    emp.EMAIL = reader["EMAIL"].ToString();
                    emp.PHONENUMBER = (Int64)reader["PHONENUMBER"];
                    emp.DEPTNAME = reader["DEPTNAME"].ToString();
                    emp.GNAME = reader["GNAME"].ToString();

                }
            }
            return View(emp);
        }


        [HttpPost]
        public IActionResult EditView(EmpList emp)
        {
            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UpdateEmployee", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", emp.ID);
                cmd.Parameters.AddWithValue("@NAME", emp.NAME);
                cmd.Parameters.AddWithValue("@EMAIL", emp.EMAIL);
                cmd.Parameters.AddWithValue("@PHONENUMBER", emp.PHONENUMBER);
                cmd.Parameters.AddWithValue("@DEPTNAME", emp.DEPTNAME);
                cmd.Parameters.AddWithValue("@GNAME", emp.GNAME);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("EmpDetails");
        }

        [HttpGet]
        public IActionResult DeleteView(int id)
        {
            EmpList Emplist = new EmpList();
            SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234");
            con.Open();
            SqlCommand command = new SqlCommand("Getbyid", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ID", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Emplist.ID = (Int32)reader["ID"];

                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult DeleteView(EmpList em, int id)
        {
            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("EmpDetails");
        }
        [Authorize]
        public IActionResult AboutUs()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult contact(contact emp)
        {
            if (!ModelState.IsValid)
            {

                return View(emp);
                

            }
            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
            {
                con.Open();
                SqlCommand command = new SqlCommand("InsertContact", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", emp.FirstName);
                command.Parameters.AddWithValue("@LastName", emp.LastName);
                command.Parameters.AddWithValue("@Email", emp.Email);
                command.Parameters.AddWithValue("@Phone", emp.Phone);
                command.Parameters.AddWithValue("@Message", emp.Message);
                command.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }

        private IList<SelectListItem> GetEmployees()
        {
            List<SelectListItem> DeptList = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234;"))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("GetEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        DeptList.Add(new SelectListItem
                        {
                            Value = dr["DEPTID"].ToString(),
                            Text = dr["DEPTNAME"].ToString()
                        });
                    }
                }
            }
            return (DeptList);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                bool isValidUser = ValidateUser(login.Email, login.Password);

                if (isValidUser)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.Email),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)).Wait();
                    return RedirectToAction("EmpDetails");
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                

            }

            return View("invalid");
        }

        private bool ValidateUser(string email, string password)
        {
            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234;"))
            {
                con.Open();

                using (SqlCommand command = new SqlCommand("ValidateUser", con))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        return reader.HasRows;
                    }

                }

            }
            
        }
        [HttpGet]
        public ActionResult RegisterView()
        {
            RegisterModel registerModel = new RegisterModel();
            return View(registerModel);
        }

        [HttpPost]
        public ActionResult RegisterView(RegisterModel registerModel)
        {
            if (EmailExists(registerModel.Email))
            {
                ModelState.AddModelError("Email", "Email is already registered. Please use a different email.");
            }
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                return View(registerModel);
            }

            if (ModelState.IsValid)
            {


                // Generate a unique confirmation token
                string confirmationToken = GenerateConfirmationToken(registerModel.Email);

                //Generate a unique confirmation token
                //        string confirmationToken = GenerateConfirmationToken(registerModel.Email);

                // Call the stored procedure to insert registration data
                using (SqlConnection connection = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234;"))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("sp_InsertRegistration", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FirstName", registerModel.FirstName);
                        command.Parameters.AddWithValue("@LastName", registerModel.LastName);
                        command.Parameters.AddWithValue("@Email", registerModel.Email);
                        command.Parameters.AddWithValue("@PhoneNumber", registerModel.PhoneNumber);
                        command.Parameters.AddWithValue("@Password", registerModel.Password);
                        command.Parameters.AddWithValue("@Country", registerModel.Country);
                        command.Parameters.AddWithValue("@State", registerModel.State);
                        command.Parameters.AddWithValue("@ConfirmationToken", confirmationToken);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }

                    // Save the confirmation token in the user's record (you may need to adjust your data model)
                    registerModel.ConfirmationToken = confirmationToken;

                    // Save the user details to the database (you may need to adjust your data model)
                    InsertRegistrationData(registerModel);

                    // Send confirmation email with the generated token
                    string confirmationLink = Url.Action("ConfirmEmail", "Home", new { confirmationToken }, Request.Scheme);
                    SendConfirmationEmail(registerModel.Email, confirmationLink);

                    //return RedirectToAction("Login");
                }

                return View(registerModel);
            }

            return RedirectToAction("RegisterView");
        }

        private void SendConfirmationEmail(string toEmail, string token)
        {
            // Replace these values with your email server details
            string smtpServer = "smtp.outlook.com";
            int smtpPort = 587;
            string smtpUsername = "chittisrija16@outlook.com";
            string smtpPassword = "Srija@1234";
            var confirmationToken = GenerateConfirmationToken(toEmail);
            string confirmationLink = Url.Action("ConfirmEmail", "Home", new { confirmationToken }, Request.Scheme);
          // Create the email message using MimeKit
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Chitti Srija", "chittisrija16@outlook.com"));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Account Confirmation";

            var builder = new BodyBuilder();
            builder.TextBody = $"Thank you for registering! Click the following link to confirm your account: {confirmationLink}";

            message.Body = builder.ToMessageBody();

            // Set up the SmtpClient from MailKit
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpServer, smtpPort, false);

                //SMTP server requires authentication provide credentials
                client.Authenticate(smtpUsername, smtpPassword);

                // Send the email
                client.Send(message);

                // Disconnect from the SMTP server
                client.Disconnect(true);
            }
        }
        private string GenerateConfirmationToken(string email)
        {
            // Generate a unique confirmation token based on the email
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(email + DateTime.UtcNow.ToString()));
                string confirmationToken = BitConverter.ToString(hashedBytes).Replace("-", "");

                // Store the confirmation token in the database
                using (SqlConnection connection = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UPDATE RegistrationTables SET ConfirmationToken = @ConfirmationToken, IsConfirmed = 0 WHERE Email = @Email", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@ConfirmationToken", confirmationToken);
                        command.ExecuteNonQuery();
                    }
                }

                return confirmationToken;
            }
        }



        private bool EmailExists(string email)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE Email = @Email", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        [HttpGet]
        public ActionResult ConfirmEmail(string confirmationToken)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234;"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("UPDATE RegistrationTables SET IsConfirmed = 1 WHERE ConfirmationToken = @ConfirmationToken AND IsConfirmed = 0", connection))
                {
                    command.Parameters.AddWithValue("@ConfirmationToken", confirmationToken);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Update successful, redirect to a page indicating successful confirmation
                        return View("ConfirmationSuccess");
                    }
                    else
                    {
                        // No rows affected or email already confirmed, redirect to a page indicating unsuccessful confirmation
                        return View("ConfirmationFailure");
                    }
                }
            }
        }
        private void InsertRegistrationData(RegisterModel registerViewModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //EXCEPTION LOGGING

        private void LogException(Exception ex)
        {
            using (SqlConnection con = new SqlConnection("Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("InsertErrorLog", con)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace);
                    cmd.Parameters.AddWithValue("@Source", ex.Source);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                    // Execute the stored procedure
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}