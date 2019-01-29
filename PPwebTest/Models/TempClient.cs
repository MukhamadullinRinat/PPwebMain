using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace PPweb.Models
{
    public class TempClient : Client
    {
        public TempClient()
        {
            Header = "Регистрация нового клиента";
            ModalBoxType = "NewClient";
        }

        public bool CardChecked
        {
            get; set;
        }
        public bool CardPersonalized
        {
            get; set;
        }
        public string CardTypeName
        {
            get; set;
        }
        public string CardStatus
        {
            get; set;
        }
    }
}