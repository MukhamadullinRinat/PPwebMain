using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using PPweb.Utils;

namespace PPweb.Models
{
    public class ModifyCard
    {
        public string CustId
        {
            get;set;
        }
        public string oldId
        {
            get;set;
        }
        public string oldNumber
        {
            get; set;
        }
        public CClientCard.CCardBlockingReason Reason
        {
            get;set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Допустимы только цифры")]
        public string newNumber
        {
            get; set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string Comment
        {
            get; set;
        }
        public DataTable resultNewCard
        {
            get; set;
        }
        public DataTable resultNewCardAction
        {
            get;set;
        } 
    }
    public class FindData
    {
        public DataTable FindCard(string nNumber, out string err)
        {
            err = string.Empty;
            DataTable tempCard = new DataTable();
            FindProcess fp = new FindProcess();
            tempCard = fp.checkCardNumber(nNumber, out err);

            return tempCard;

        }
        public string ChangeCard(ref ModifyCard mCard, OpsInfo.COpsInfo opsInfo, out bool result)
        {
            result = false;
            string msg = string.Empty;
            LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
            string IndexOps = string.IsNullOrEmpty(opsInfo.OpsIndex) ? "" : opsInfo.OpsIndex;
            string OperOps = string.IsNullOrEmpty(opsInfo.OpsOperatorName) ? "" : opsInfo.OpsOperatorName;
            DataSet dataSet = new DataSet();
            dataSet = ls.changeCard(mCard.oldId, mCard.oldNumber, mCard.newNumber, OperOps, IndexOps);
            if (dataSet.Tables.Contains("return"))
            {
                //foreach (DataRow row in mCard.resultNewCard.Rows)
                //{
                //    row["CustId"] = mCard.CustId;
                //}
                result = true;
                return msg = "Карта успешно заменена!";
            }
            else
            {
                try
                {
                    string text = dataSet.Tables["Fault"].Rows[0]["faultString"].ToString();
                    string text2 = text;
                    if (dataSet.Tables.Contains("CardsException"))
                    {
                        string text3 = dataSet.Tables["CardsException"].Rows[0]["type"].ToString();
                        switch (text3)
                        {
                            case "INVALID_CATEGORY":
                                text2 = "Тип заменяемой карты отличается от типа новой. Замена невозможна.";
                                break;
                            case "CARD_IS_BLOCKED":
                                text2 = "Новая карта заблокирована";
                                break;
                            case "CARD_BELONG_ANOTHER_CLIENT":
                                text2 = "Новая карта выдана другому клиенту";
                                break;
                            case "CANT_USE_ITSELF":
                                text2 = "Попытка замена на саму себя";
                                break;
                        }
                        msg = text2;
                    }
                    msg = "Ошибка замены";
                }
                catch
                {
                    msg = "Неизвестная ошибка замены карты";
                }
                result = false;
                return msg;
            }
        }
        public string BlockCard(ModifyCard mCard, out bool result, OpsInfo.COpsInfo _ops)
        {
            string msg = string.Empty;
            result = false;
            string operName = string.Empty;
            string shopId = string.Empty;
            if (_ops != null)
            {
                operName = _ops.OpsOperatorName;
                shopId = _ops.OpsIndex;
            }
            LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
            DataSet dataSet = ls.blockCard(mCard.oldNumber, mCard.Reason, mCard.Comment, operName, shopId);
            if (dataSet.Tables.Contains("return"))
            {
                DataTable dataTable = dataSet.Tables["return"];
                if (dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    result = true;
                    msg = "Карта " + mCard.oldNumber + " заблокирована";

                }
            }

            return msg;
        }
    }
}