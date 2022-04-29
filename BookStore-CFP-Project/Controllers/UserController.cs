using BusinessLayer.Inetrface;
using BusinessLayer.Service;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_CFP_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        //For UserResgister Api Method
        [HttpPost("Register")]
        public  IActionResult RegisterUser(UserModel userModel)
        {
            try
            {
                var user = this.userBL.RegisterUser(userModel);
                if (user != null)
                {
                    return this.Ok(new { Success = true, message = "Rgistration Successfull", Response = user });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "User Registration UnSuccessfull" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        //Login Api Method
        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            try
            {
                var login = this.userBL.Login(email, password);
                if (login != null)
                {
                    return this.Ok(new { Success = true, message = "Login Sucessfull", Response = login });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Login Un Successfull" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        //forgot Api Method 
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var forgotPassword = this.userBL.ForgotPassword(email);
                if (forgotPassword != null)
                {
                    return this.Ok(new { Success = true, message = " mail sent is successful", Response = forgotPassword });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Eneter Valid Email!!Try Again" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            try
            {
                var email = User.Claims.FirstOrDefault(e => e.Type == "EmailId").Value.ToString();
                if (this.userBL.ResetPassword(email, newPassword, confirmPassword))
                {
                    return this.Ok(new { Success = true, message = " Password Updated Successfully " });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = " Unsuccessfull Password Updation " });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }

    }
}
