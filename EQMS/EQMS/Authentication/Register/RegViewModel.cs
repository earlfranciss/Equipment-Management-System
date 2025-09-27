using EQMS.Authentication.ChangePassword;
using EQMS.Authentication.Login;
using EQMS.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EQMS.Authentication.Register
{
    internal class RegViewModel
    {
        public readonly IRegDAO regDAO;
        public readonly dbm db = new dbm();

        public RegViewModel()
        {
            regDAO = new RegDAO(db.connectionString);
        }

        public bool RegisterAcc(string fn, string ln, string email, string phone, string pass)
        {
            if (checkEmail(email) && checkPhone(phone) && FNErrorText(fn).Equals("") && LNErrorText(ln).Equals("") && PassErrorText(pass).Equals(""))
            {
                regDAO.RegisterDB(fn, ln, email, phone, pass);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkEmail(string email)
        {
            if (email != "")
            {
                if (!regDAO.EmailExist(email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool checkPhone(string phone)
        {
            if (phone != "")
            {
                if (!regDAO.PhoneExist(phone))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public string FNErrorText(string fn)
        {
            if (fn == "")
            {
                return "Please enter your first name";
            }
            else
            {
                return "";
            }
        }
        public string LNErrorText(string ln)
        {
            if (ln == "")
            {
                return "Please enter your last name";
            }
            else
            {
                return "";
            }
        }
        public string PassErrorText(string pass)
        {
            if (pass != "")
            {
                if (pass.Length < 8)
                {
                    return "Password too short. Please enter a longer password";
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
        public string EmailErrorText(string email)
        {
            if (email != "")
            {
                if (!regDAO.EmailExist(email))
                {
                    return "";
                }
                else
                {
                    return "Email already exist.";
                }
            }
            else
            {
                return "Please enter your email.";
            }
        }
        public string PhoneErrorText(string phone)
        {
            if (phone != "")
            {
                if (!regDAO.PhoneExist(phone))
                {
                    return "";
                }
                else
                {
                    return "Phone number already exist.";
                }
            }
            else
            {
                return "Please enter your phone number.";
            }
        }

        public string GetID(string fn, string ln)
        {
            return regDAO.GetUserID(fn, ln);
        }
    }
}
