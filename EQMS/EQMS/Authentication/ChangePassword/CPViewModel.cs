using EQMS.Authentication.Login;
using EQMS.DatabaseManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EQMS.Authentication.ChangePassword
{
    internal class CPViewModel
    {
        public readonly ICPDAO CPDAO;
        public readonly dbm db = new dbm();

        public CPViewModel()
        {
            CPDAO = new CPDAO(db.connectionString);
        }

        public bool ChangePassword(string user, string pass, string confirmPass)
        {
            if (checkID(user) && checkPass(pass, confirmPass))
            {
                CPDAO.ChangePassDB(user, pass);
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
                if (CPDAO.IDNumExist(user))
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
        public bool checkPass(string pass, string confirmPass)
        {
            if (pass != "")
            {
                if (pass.Length >= 8)
                {
                    if (pass.Equals(confirmPass))
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
            else
            {
                return false;
            }
        }

        public string UserErrorText(string user)
        {
            if (user != "")
            {
                if (!CPDAO.IDNumExist(user))
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
        public string ConfirmPassErrorText(string pass, string confirmPass)
        {
            if (confirmPass != "")
            {
                if (pass.Equals(confirmPass))
                {
                    return "";
                }
                else
                {
                    return "Password does not match.";
                }
            }
            else
            {
                return "Please re-enter your password.";
            }
        }
    }
}
