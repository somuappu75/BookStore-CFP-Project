using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Inetrface
{
   public interface IUserBL
    {
        public UserModel RegisterUser(UserModel userModel);
        public string Login(string email, string password);
        public string ForgotPassword(string email);
        public bool ResetPassword(string email, string newPassword, string confirmPassword);
    }
}
