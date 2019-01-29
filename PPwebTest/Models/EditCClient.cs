using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PPweb.Models
{
    public class EditCClient : Client
    {
        public EditCClient()
        {
            Header = "Редактирование данных клиента";
            ModalBoxType = "EditClient";
        }

        public DataTable resultCard
        {
            get; set;
        }
        public DataTable resultCardAction
        {
            get; set;
        }
        public string DateToDocFormat2(string strDate)
        {
            string text = "";
            text = strDate.Substring(8, 2) + "." + strDate.Substring(5, 2) + "." + strDate.Substring(0, 4);
            return text;
        }

        public string NormalizingDate(string date)
        {
            var notFormat = "[0-9]{4}\\-[0-9]{2}\\-[0-9]{2}";
            var normalFormat = "[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}";
            var isNotFormat = Regex.IsMatch(date, notFormat);
            if (isNotFormat)
            {
                var dateList = Regex.Match(date, notFormat).ToString().Split('-').Reverse().ToList();
                var normalizeDate = "";
                for (var i = 0; i < dateList.Count; i++)
                {
                    normalizeDate += dateList[i];
                    if (i + 1 < dateList.Count)
                    {
                        normalizeDate += ".";
                    }
                }
                return normalizeDate;
            }
            else return Regex.Match(date, normalFormat).ToString();
        }
    }
}