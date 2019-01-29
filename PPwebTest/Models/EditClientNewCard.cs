using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPweb.Models
{
    public class EditClientNewCard : EditCClient
    {
        public EditClientNewCard()
        {
            Header = "Оформление карты \"" + SelectedCardType + "\"";
            ModalBoxType = "NewCard";
        }

        public string SelectedCardType { get; set; }
    }
}