using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPweb.Models
{

    public class Authorization
    {
        private const string _loginA = "Testiko";
        private const string _passwordA = "Testiko";

        public string LoginA
        { get; set; } 
        public string PasswordA
        { get; set; }

        public bool AutProc()
        {
            return CheckAutData(LoginA, PasswordA);
        }

        private bool CheckAutData(string l, string p)
        {
            if ((l == "") | (p == ""))
            {
                return false;
            }
            else
            {
                return ((l == _loginA) && (p == _passwordA));
            }
        }
    }
}