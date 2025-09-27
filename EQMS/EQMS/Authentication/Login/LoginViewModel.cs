using EQMS.Authentication.Login;
using EQMS.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EQMS.Authentication
{
    public class LoginViewModel
    {
        public readonly ILoginDAO loginDAO;
        public readonly dbm db = new dbm();

        public LoginViewModel()
        {
            loginDAO = new LoginDAO(db.connectionString);
        }


        public bool Login(string user, string pass)
        {
            if (checkID(user) && checkPass(user, pass))
            {
                return true;
            }
            else
            {   
                return false;
            }
        }

        private bool checkID(string user)
        {
            if (user != "")
            {
                if (!loginDAO.IDNumExist(user))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private bool checkPass(string user, string pass)
        {
            if (pass != "")
            {
                if (!loginDAO.Authenticate(user, pass))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public string UserErrorText(string user)
        {
            if (user != "")
            {
                if (!loginDAO.IDNumExist(user))
                {
                    return "ID number not found.";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "Please enter your ID number";
            }
        }

        public string PassErrorText(string user, string pass)
        {
            if (pass != "")
            {
                if (!loginDAO.Authenticate(user, pass))
                {
                    return "Password is incorrect.";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "Please enter your password";
            }
        }
    }
}
