using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Configuration;
using PPweb.Utils;
using System.Web.Mvc;

namespace PPweb.Models
{
    public class FilterCust
    {
        [RegularExpression(@"^[0-9]{13}|[0-9]{10}$", ErrorMessage = "Некорректное значение")]
        public string fCardNumber
        {
            get;
            set;
        }

        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string fLastName
        {
            get; set;
        }

        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string fFirstName
        {
            get; set;
        }

        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string fMiddleName
        {
            get; set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote("ValidFilterBirthDate","Passport")]
        public string fBirthDate
        {
            get; set;
        }

        [ValidPassSeria]
        [RegularExpression(@"^[0-9]{4}|[_]{4}$", ErrorMessage = "Введите корректное значение")]
        public string fPassSeria
        {
            get; set;
        }

        [ValidPassNumber]
        [RegularExpression(@"^[0-9]{6}|[_]{6}$", ErrorMessage = "Введите корректное значение")]
        public string fPassNumber
        {
            get; set;
        }

        public DataTable resultCust
        {
            get; set;
        }
        public DataTable resultCustPassport
        {
            get; set;
        }
        public DataTable resultCustAddress
        {
            get; set;
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
            string text = string.Empty;
            if (strDate.IndexOf("-") == 4)
            {
                text = strDate.Substring(8, 2) + "." + strDate.Substring(5, 2) + "." + strDate.Substring(0, 4);
            }
            else if (strDate.IndexOf("-") == 2)
            {
                text = strDate.Substring(0, 2) + "." + strDate.Substring(3, 2) + "." + strDate.Substring(6, 4);
            }
            else
            {
                if (strDate.Length > 10)
                {
                    text = strDate.Remove(11);
                }
                else
                {
                    text = strDate;
                }
            }

            if (strDate.Length > 10)
            {
                text += strDate.Substring(11);
            }
            return text;
        }
        public string fPhone { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidPassSeria: ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var filterCust = validationContext.ObjectInstance as FilterCust;
            if(value == null && filterCust.fPassNumber != null)
                return new ValidationResult("Поле является обязательным для заполнения");
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule();
            mvr.ErrorMessage = "Поле является обязательным для заполнения";
            mvr.ValidationType = "validpassseria";
            return new[] { mvr };
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidPassNumber : ValidationAttribute, IClientValidatable
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var filterCust = validationContext.ObjectInstance as FilterCust;
            if (value == null && filterCust.fPassSeria != null)
                return new ValidationResult("Поле является обязательным для заполнения");
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule();
            mvr.ErrorMessage = "Поле является обязательным для заполнения";
            mvr.ValidationType = "validpassnumber";
            return new[] { mvr };
        }
    }

    public class FindProcess
    {
        public bool letCheckPassportUniq(ref FilterCust fCust, out string err)
        {
            err = string.Empty;
            bool checkStatus = false;
            bool hasResult = false;
            string _controlID = fCust.fCardNumber;

            DataSet tempData = new DataSet();
            DataTable resultTable = new DataTable();
            LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
            CClientCard.CCardFilter filter = new CClientCard.CCardFilter { PassSerie = fCust.fPassSeria, PassNumber = fCust.fPassNumber };

            tempData = ls.getCardByFilter(filter);
            if (tempData.Tables.Contains("clientVO") && tempData.Tables["clientVO"].Rows.Count > 0)
            {
                hasResult = true;
                DataTable dataTable = tempData.Tables["return"];
                dataTable.Columns.Add("CustId");
                tempData.Tables["clientVO"].Columns.Add("flag");
                foreach (DataRow row in dataTable.Rows)
                {
                    string empty = string.Empty;
                    DataRow[] childRows = row.GetChildRows(tempData.Relations["return_clientVO"]);
                    string value = childRows[0]["id"].ToString();
                    row["CustId"] = value;
                    string text = "F";
                    foreach (DataRow row2 in tempData.Tables["clientVO"].Rows)
                    {
                        if (row2["id"].ToString().Equals(value))
                        {
                            row2["flag"] = text;
                            if (text.Equals("F"))
                            {
                                text = "D";
                            }
                        }

                    }
                }
                dataTable.AcceptChanges();
                tempData.Tables["clientVO"].AcceptChanges();
                foreach (DataRow row3 in tempData.Tables["clientVO"].Rows)
                {
                    if (row3["flag"].Equals("D"))
                    {
                        row3.Delete();
                    }
                }
                dataTable.AcceptChanges();
                tempData.Tables["clientVO"].AcceptChanges();
            }
            if (hasResult)
            {
                resultTable = tempData.Tables["clientVO"].Copy();
                if (resultTable.Rows.Count > 1)
                {
                    checkStatus = false;
                    err = "Клиент с такими паспортными данными уже существует!";
                }
                else
                {
                    if (_controlID == "new")
                    {
                        checkStatus = false;
                        err = "Клиент с такими паспортными данными уже существует!";
                    }
                    else
                    {
                        DataRow row = resultTable.Rows[0];
                        if (row["id"].ToString().Equals(_controlID))
                        {
                            checkStatus = true;
                        }
                        else
                        {
                            checkStatus = false;
                            err = "Клиент с такими паспортными данными уже существует!";
                        }

                    }
                }
            }
            else
            {
                checkStatus = true;
            }
            return checkStatus;
        }
        public void letFindSomeData(ref FilterCust fCust, out string err)
        {
            err = string.Empty; ;
            if (ConfigurationManager.AppSettings["TestStore"] == "true")
            {
                TestStore.FillTempTables fillTest = new TestStore.FillTempTables();
                fCust.resultCust = fillTest.fillCust();
                fCust.resultCard = fillTest.fillCard();
                fCust.resultCardAction = fillTest.fillCardAction();
                fCust.resultCustPassport = fillTest.fillCustPassport();
                fCust.resultCustAddress = fillTest.fillCustAddress();
            }
            else
            {
                findFunction(fCust, out err);
                //if (err != string.Empty)
                //{
                //    TestStore.FillTempTables fillTest = new TestStore.FillTempTables();
                //    fCust.resultCust = fillTest.fillCust(true);
                //    fCust.resultCard = fillTest.fillCard(true);
                //    fCust.resultCardAction = fillTest.fillCardAction(true);
                //    fCust.resultCustPassport = fillTest.fillCustPassport(true);
                //    fCust.resultCustAddress = fillTest.fillCustAddress(true);
                //}
            }


        }
        public DataTable checkCardNumber(string CardN, out string err)
        {
            err = "";
            CClientCard.CCardFilter _filter = new CClientCard.CCardFilter();
            _filter.Number = CardN;
            DataTable cardInfo = new DataTable();
            cardInfo = null;
            cardInfo = findCardFunction(_filter, out err);
            return cardInfo;
        }
        private DataTable findCardFunction(CClientCard.CCardFilter _filter, out string err)
        {
            err = "";
            DataSet tempData = new DataSet();
            LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
            DataTable tempCard = new DataTable();
            DataTable cardInfo = new DataTable();
            cardInfo.TableName = "Cards";
            cardInfo.Columns.Add("Id");
            cardInfo.PrimaryKey = new DataColumn[] { cardInfo.Columns["Id"] };
            cardInfo.Columns.Add("CustId");
            cardInfo.Columns.Add("number");
            cardInfo.Columns.Add("activationDate");
            cardInfo.Columns.Add("blockDate");
            cardInfo.Columns.Add("blocked");
            cardInfo.Columns.Add("createDate");
            cardInfo.Columns.Add("deleted");
            cardInfo.Columns.Add("status");
            cardInfo.Columns.Add("statusDescription");
            //------------------из тб cardTypeVO
            cardInfo.Columns.Add("classType");
            cardInfo.Columns.Add("name");
            cardInfo.Columns.Add("personalized");
            //------------------из тб color
            cardInfo.Columns.Add("blue");
            cardInfo.Columns.Add("green");
            cardInfo.Columns.Add("red");

            try
            {
                tempData = ls.getCardByFilter(_filter);
                if (!tempData.Tables.Contains("clientVO"))
                {
                    if (!tempData.Tables.Contains("cardTypeVO"))
                    {
                        err = "Карта c указанным номером не существует";
                    }
                    else
                    {
                        tempCard = tempData.Tables["return"];
                        foreach (DataRow rowA in tempCard.Rows)
                        {
                            DataRow[] childRowsT = rowA.GetChildRows(tempData.Relations["return_cardTypeVO"]);
                            DataTable cardType = tempData.Tables["cardTypeVO"];
                            DataTable cardColor = tempData.Tables["color"];
                            string rowId = childRowsT[0]["return_Id"].ToString();
                            string classType = string.Empty;
                            string name = string.Empty;
                            string personalized = string.Empty;
                            string blue = string.Empty;
                            string green = string.Empty;
                            string red = string.Empty;
                            foreach (DataRow rowT in cardType.Rows)
                            {
                                if (rowT["return_Id"].ToString().Equals(rowId))
                                {
                                    if (cardType.Columns.Contains("classType"))
                                    {
                                        classType = rowT["classType"].ToString();
                                    }
                                    if (cardType.Columns.Contains("name"))
                                    {
                                        name = rowT["name"].ToString();
                                    }
                                    if (cardType.Columns.Contains("personalized"))
                                    {
                                        personalized = rowT["personalized"].ToString();
                                    }
                                }
                            }
                            foreach (DataRow rowC in cardColor.Rows)
                            {
                                if (rowC["cardTypeVO_Id"].ToString().Equals(rowId))
                                {
                                    if (cardColor.Columns.Contains("blue"))
                                    {
                                        blue = rowC["blue"].ToString();
                                    }
                                    if (cardColor.Columns.Contains("green"))
                                    {
                                        green = rowC["green"].ToString();
                                    }
                                    if (cardColor.Columns.Contains("red"))
                                    {
                                        red = rowC["red"].ToString();
                                    }
                                }
                            }
                            string cid = string.Empty;
                            if (tempCard.Columns.Contains("id"))
                            {
                                cid = rowA["id"].ToString();
                            }
                            string custId = string.Empty;
                            string number = string.Empty;
                            if (tempCard.Columns.Contains("number"))
                            {
                                number = rowA["number"].ToString();
                            }
                            string activationDate = string.Empty;
                            if (tempCard.Columns.Contains("activationDate"))
                            {
                                activationDate = rowA["activationDate"].ToString();
                            }
                            string blockDate = string.Empty;
                            if (tempCard.Columns.Contains("blockDate"))
                            {
                                blockDate = rowA["blockDate"].ToString();
                            }
                            string blocked = string.Empty;
                            if (tempCard.Columns.Contains("blocked"))
                            {
                                blocked = rowA["blocked"].ToString();
                            }
                            string createDate = string.Empty;
                            if (tempCard.Columns.Contains("createDate"))
                            {
                                createDate = rowA["createDate"].ToString();
                            }
                            string deleted = string.Empty;
                            if (tempCard.Columns.Contains("deleted"))
                            {
                                deleted = rowA["deleted"].ToString();
                            }
                            string status = string.Empty;
                            if (tempCard.Columns.Contains("status"))
                            {
                                status = ConvertCardAction(rowA["status"].ToString());
                            }
                            string statusDescription = string.Empty;
                            if (tempCard.Columns.Contains("statusDescription"))
                            {
                                statusDescription = rowA["statusDescription"].ToString();
                            }
                            cardInfo.Rows.Add(new object[] { cid, custId, number, activationDate, blockDate, blocked, createDate,
                            deleted, status, statusDescription, classType, name, personalized, blue, green, red});
                        }
                        cardInfo.AcceptChanges();
                    }
                }
                else
                {
                    err = "Карта выдана другому клиенту";
                }
            }
            catch
            {
                err = "Ошибка поиска карты";
            }
            return cardInfo;
        }
        private FilterCust findFunction(FilterCust fCust, out string errMess)
        {
            errMess = "";
            int parCustRows = ClientInResult();
            DataSet tempData = new DataSet();
            LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
            CClientCard.CCardFilter filter = new CClientCard.CCardFilter();
            string DateFilter = string.Empty;
            if (fCust.fBirthDate != null)
            {
                filter.BirthDate = ConvertDateFormat(fCust.fBirthDate);
                DateFilter = ConvertDateFormat(fCust.fBirthDate);
            }
            else filter.BirthDate = string.Empty;
            filter.FirstName = fCust.fFirstName;
            filter.LastName = fCust.fLastName;
            filter.MiddleName = fCust.fMiddleName;
            bool fullDataList = ((!string.IsNullOrEmpty(filter.FirstName)) && (!string.IsNullOrEmpty(filter.LastName)) &&
                (!string.IsNullOrEmpty(filter.MiddleName)) && (!string.IsNullOrEmpty(filter.BirthDate)));
            
            filter.PassNumber = fCust.fPassNumber;
            filter.PassSerie = fCust.fPassSeria;
            if (fCust.fCardNumber != null)
            { filter.Number = fCust.fCardNumber.Trim(); }
            else
            { filter.Number = null; }

            if (filter.Number != null && filter.Number.Length > 0)
            {
                tempData = ls.getCardByFilter(filter);
                if (tempData.Tables.Contains("clientVO") && tempData.Tables["clientVO"].Rows.Count > 0)
                {
                    string clientID = tempData.Tables["clientVO"].Rows[0]["id"].ToString();
                    filter = new CClientCard.CCardFilter();
                    filter.ClientID = clientID;
                    tempData = new DataSet();
                }
            }
            tempData = ls.getCardByFilter(filter);
            if (tempData.Tables.Contains("clientVO") && tempData.Tables["clientVO"].Rows.Count > 0)
            {
                DataTable dataTable = tempData.Tables["return"];
                dataTable.Columns.Add("CustId");
                dataTable.Columns.Add("dateFlag");
                tempData.Tables["clientVO"].Columns.Add("flag");
                foreach (DataRow row in dataTable.Rows)
                {
                    string empty = string.Empty;
                    DataRow[] childRows = row.GetChildRows(tempData.Relations["return_clientVO"]);
                    string value = childRows[0]["id"].ToString();
                    row["CustId"] = value;
                    string text = "F";
                    foreach (DataRow row2 in tempData.Tables["clientVO"].Rows)
                    {
                        if (row2["id"].ToString().Equals(value))
                        {
                            row2["flag"] = text;
                            if (text.Equals("F"))
                            {
                                text = "D";
                            }
                            if (row2["flag"].ToString().Equals("F"))
                            {
                                if (tempData.Tables["clientVO"].Columns.Contains("sex"))
                                {
                                    string sex = row2["sex"].ToString();
                                    if (sex != "0" && sex != "1")
                                    {
                                        if (sex == "Male")
                                        {
                                            row2["sex"] = "0";
                                        }
                                        else if (sex == "Female")
                                        {
                                            row2["sex"] = "1";
                                        }
                                        else
                                        { row2["sex"] = "2"; }
                                    }
                                }
                            }

                            if (DateFilter != string.Empty)
                            {
                                if (tempData.Tables["clientVO"].Columns.Contains("birthDate"))
                                {
                                    string dateTemp = ConvertDateFormat(row2["birthDate"].ToString());
                                    if (!dateTemp.Equals(DateFilter))
                                    {
                                        row2["flag"] = "D";
                                        row["dateFlag"] = "D";
                                    }
                                }
                            }
                        }

                    }
                }
                dataTable.AcceptChanges();
                tempData.Tables["clientVO"].AcceptChanges();
                foreach (DataRow row3 in tempData.Tables["clientVO"].Rows)
                {
                    if (row3["flag"].Equals("D"))
                    {
                        row3.Delete();
                    }
                }
                foreach (DataRow row3 in dataTable.Rows)
                {
                    if (row3["dateFlag"].Equals("D"))
                    {
                        row3.Delete();
                    }
                }
                dataTable.AcceptChanges();
                tempData.Tables["clientVO"].Columns.Add("cards");
                tempData.Tables["clientVO"].AcceptChanges();
                if ((!fullDataList) && (tempData.Tables["clientVO"].Rows.Count > parCustRows))
                {
                    errMess = "Слишком много результатов поиска. Просьба уточнить критерии и повторить поиск.";
                }
                else
                {
                    if (tempData.Tables["clientVO"].Rows.Count > 0)
                    {
                        fCust.resultCust = tempData.Tables["clientVO"];

                        DataTable tableAction = new DataTable();
                        tableAction.TableName = "cardsActions";
                        tableAction.Columns.Add("cardAction");
                        tableAction.Columns.Add("date");
                        tableAction.Columns.Add("userName");
                        tableAction.Columns.Add("shop");
                        tableAction.Columns.Add("CardId");
                        foreach (DataRow row4 in tempData.Tables["return"].Rows)
                        {
                            DataSet tAction = new DataSet();
                            tAction = ls.getCardAction(row4["id"].ToString());
                            if (tAction.Tables.Contains("return"))
                            {
                                foreach (DataRow rowA in tAction.Tables["return"].Rows)
                                {
                                    string cAct = string.Empty;
                                    if (tAction.Tables["return"].Columns.Contains("cardAction"))
                                    {
                                        cAct = ConvertCardAction(rowA["cardAction"].ToString());
                                    }
                                    string date = string.Empty;
                                    if (tAction.Tables["return"].Columns.Contains("date"))
                                    {
                                        date = rowA["date"].ToString();
                                    }
                                    string uName = string.Empty;
                                    if (tAction.Tables["return"].Columns.Contains("userName"))
                                    {
                                        uName = rowA["userName"].ToString();
                                    }
                                    string shop = string.Empty;
                                    if (tAction.Tables["return"].Columns.Contains("shop"))
                                    {
                                        shop = rowA["shop"].ToString();
                                    }
                                    string cardId = string.Empty;
                                    if (cAct != string.Empty)
                                    {
                                        cardId = row4["id"].ToString();
                                    }
                                    tableAction.Rows.Add(new object[] { cAct, date, uName, shop, cardId });
                                }
                            }
                            tableAction.AcceptChanges();
                            fCust.resultCardAction = tableAction;
                        }

                        //-------------------------------------------------------------------------таблица Карт
                        DataTable tempCard = new DataTable();
                        tempCard = tempData.Tables["return"];
                        DataTable readyCard = new DataTable();
                        readyCard.TableName = "Cards";
                        readyCard.Columns.Add("Id");
                        readyCard.Columns.Add("CustId");
                        readyCard.Columns.Add("number");
                        readyCard.Columns.Add("activationDate");
                        readyCard.Columns.Add("blockDate");
                        readyCard.Columns.Add("blocked");
                        readyCard.Columns.Add("createDate");
                        readyCard.Columns.Add("deleted");
                        readyCard.Columns.Add("status");
                        readyCard.Columns.Add("statusDescription");
                        //------------------из тб cardTypeVO
                        readyCard.Columns.Add("classType");
                        readyCard.Columns.Add("name");
                        readyCard.Columns.Add("personalized");
                        //------------------из тб color
                        readyCard.Columns.Add("blue");
                        readyCard.Columns.Add("green");
                        readyCard.Columns.Add("red");
                        readyCard.Columns.Add("classId");
                        readyCard.Columns.Add("classOrderId");
                        foreach (DataRow rowA in tempCard.Rows)
                        {
                            DataRow[] childRowsT = rowA.GetChildRows(tempData.Relations["return_cardTypeVO"]);
                            DataTable cardType = tempData.Tables["cardTypeVO"];
                            DataTable cardColor = tempData.Tables["color"];
                            string rowId = childRowsT[0]["return_Id"].ToString();
                            string classType = string.Empty;
                            string name = string.Empty;
                            string personalized = string.Empty;
                            string blue = string.Empty;
                            string green = string.Empty;
                            string red = string.Empty;
                            string classId = string.Empty;
                            string classOrderId = string.Empty;
                            foreach (DataRow rowT in cardType.Rows)
                            {
                                if (rowT["return_Id"].ToString().Equals(rowId))
                                {
                                    if (cardType.Columns.Contains("classType"))
                                    {
                                        classType = rowT["classType"].ToString();
                                    }
                                    if (cardType.Columns.Contains("name"))
                                    {
                                        name = rowT["name"].ToString();
                                    }
                                    if (cardType.Columns.Contains("personalized"))
                                    {
                                        personalized = rowT["personalized"].ToString();
                                    }
                                    if (cardType.Columns.Contains("id"))
                                    {
                                        classId = rowT["id"].ToString();
                                        classOrderId = ConvertClassOrder(rowT["id"].ToString());
                                    }
                                }
                            }
                            foreach (DataRow rowC in cardColor.Rows)
                            {
                                if (rowC["cardTypeVO_Id"].ToString().Equals(rowId))
                                {
                                    if (cardColor.Columns.Contains("blue"))
                                    {
                                        blue = rowC["blue"].ToString();
                                    }
                                    if (cardColor.Columns.Contains("green"))
                                    {
                                        green = rowC["green"].ToString();
                                    }
                                    if (cardColor.Columns.Contains("red"))
                                    {
                                        red = rowC["red"].ToString();
                                    }
                                }
                            }
                            string cid = string.Empty;
                            if (tempCard.Columns.Contains("id"))
                            {
                                cid = rowA["id"].ToString();
                            }
                            string custId = string.Empty;
                            if (tempCard.Columns.Contains("CustId"))
                            {
                                custId = rowA["CustId"].ToString();
                            }
                            string number = string.Empty;
                            if (tempCard.Columns.Contains("number"))
                            {
                                number = rowA["number"].ToString();
                            }
                            string activationDate = string.Empty;
                            if (tempCard.Columns.Contains("activationDate"))
                            {
                                activationDate = rowA["activationDate"].ToString();
                            }
                            string blockDate = string.Empty;
                            if (tempCard.Columns.Contains("blockDate"))
                            {
                                blockDate = rowA["blockDate"].ToString();
                            }
                            string blocked = string.Empty;
                            if (tempCard.Columns.Contains("blocked"))
                            {
                                blocked = rowA["blocked"].ToString();
                            }
                            string createDate = string.Empty;
                            if (tempCard.Columns.Contains("createDate"))
                            {
                                createDate = rowA["createDate"].ToString();
                            }
                            string deleted = string.Empty;
                            if (tempCard.Columns.Contains("deleted"))
                            {
                                deleted = rowA["deleted"].ToString();
                            }
                            string status = string.Empty;
                            if (tempCard.Columns.Contains("status"))
                            {
                                status = ConvertCardAction(rowA["status"].ToString());
                            }
                            string statusDescription = string.Empty;
                            if (tempCard.Columns.Contains("statusDescription"))
                            {
                                statusDescription = rowA["statusDescription"].ToString();
                            }
                            readyCard.Rows.Add(new object[] { cid, custId, number, activationDate, blockDate, blocked, createDate,
                            deleted, status, statusDescription, classType, name, personalized, blue, green, red, classId, classOrderId});
                        }
                        readyCard.AcceptChanges();
                        fCust.resultCard = readyCard;
                        //----------------------------------------------------------------------------------таблицы Паспорт и Адрес
                        DataTable tempPass = new DataTable();
                        tempPass = tempData.Tables["passport"];
                        DataTable readyPass = new DataTable();
                        readyPass.TableName = "passport";
                        readyPass.Columns.Add("delivery");
                        readyPass.Columns.Add("deliveryDate");
                        readyPass.Columns.Add("departmentCode");
                        readyPass.Columns.Add("passNumber");
                        readyPass.Columns.Add("passSerie");
                        readyPass.Columns.Add("CustId");

                        DataTable tempAddress = new DataTable();
                        tempAddress = tempData.Tables["clientAddress"];
                        DataTable readyAddress = new DataTable();
                        readyAddress.TableName = "clientAddress";
                        readyAddress.Columns.Add("appartment");
                        readyAddress.Columns.Add("building");
                        readyAddress.Columns.Add("city");
                        readyAddress.Columns.Add("district");
                        readyAddress.Columns.Add("districtArea");
                        readyAddress.Columns.Add("house");
                        readyAddress.Columns.Add("other");
                        readyAddress.Columns.Add("region");
                        readyAddress.Columns.Add("street");
                        readyAddress.Columns.Add("zip");
                        readyAddress.Columns.Add("CustId");

                        foreach (DataRow row in tempData.Tables["clientVO"].Rows)
                        {
                            DataRow[] childRowsP = row.GetChildRows(tempData.Relations["clientVO_passport"]);
                            DataRow[] childRowsA = row.GetChildRows(tempData.Relations["clientVO_clientAddress"]);
                            string valueP = childRowsP[0]["clientVO_Id"].ToString();
                            string valueA = childRowsA[0]["clientVO_Id"].ToString();
                            foreach (DataRow rowp in tempData.Tables["passport"].Rows)
                            {
                                if (rowp["clientVO_Id"].ToString().Equals(valueP))
                                {
                                    string delivery = string.Empty;
                                    if (tempData.Tables["passport"].Columns.Contains("delivery"))
                                    {
                                        delivery = rowp["delivery"].ToString();
                                    }
                                    string deliveryDate = string.Empty;
                                    if (tempData.Tables["passport"].Columns.Contains("deliveryDate"))
                                    {
                                        deliveryDate = rowp["deliveryDate"].ToString();
                                    }
                                    string departmentCode = string.Empty;
                                    if (tempData.Tables["passport"].Columns.Contains("departmentCode"))
                                    {
                                        departmentCode = rowp["departmentCode"].ToString();
                                    }
                                    string passNumber = string.Empty;
                                    if (tempData.Tables["passport"].Columns.Contains("passNumber"))
                                    {
                                        passNumber = rowp["passNumber"].ToString();
                                    }
                                    string passSerie = string.Empty;
                                    if (tempData.Tables["passport"].Columns.Contains("passSerie"))
                                    {
                                        passSerie = rowp["passSerie"].ToString();
                                    }
                                    readyPass.Rows.Add(new object[] { delivery, deliveryDate, departmentCode, passNumber, passSerie, row["id"].ToString() });
                                }
                            }
                            foreach (DataRow rowA in tempData.Tables["clientAddress"].Rows)
                            {
                                if (rowA["clientVO_Id"].ToString().Equals(valueA))
                                {
                                    string appartment = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("appartment"))
                                    {
                                        appartment = rowA["appartment"].ToString();
                                    }
                                    string building = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("building"))
                                    {
                                        building = rowA["building"].ToString();
                                    }
                                    string city = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("city"))
                                    {
                                        city = rowA["city"].ToString();
                                    }
                                    string district = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("district"))
                                    {
                                        district = rowA["district"].ToString();
                                    }
                                    string districtArea = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("districtArea"))
                                    {
                                        districtArea = rowA["districtArea"].ToString();
                                    }
                                    string house = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("house"))
                                    {
                                        house = rowA["house"].ToString();
                                    }
                                    string other = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("other"))
                                    {
                                        other = rowA["other"].ToString();
                                    }
                                    string region = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("region"))
                                    {
                                        region = rowA["region"].ToString();
                                    }
                                    string street = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("street"))
                                    {
                                        street = rowA["street"].ToString();
                                    }
                                    string zip = string.Empty;
                                    if (tempData.Tables["clientAddress"].Columns.Contains("zip"))
                                    {
                                        zip = rowA["zip"].ToString();
                                    }
                                    readyAddress.Rows.Add(new object[] { appartment, building, city, district, districtArea, house,
                                    other, region, street, zip, row["id"].ToString() });
                                }
                            }
                        }
                        readyPass.AcceptChanges();
                        readyAddress.AcceptChanges();
                        fCust.resultCustAddress = readyAddress;
                        fCust.resultCustPassport = readyPass;
                    }
                }
            }
            CheckCardActivationDate(fCust);
            AddPPInformRow(fCust);
            ChangeDataSort(fCust);
            return fCust;
        }

        private void AddPPInformRow(FilterCust fCust)
        {
            if (fCust.resultCust != null)
            {
                foreach (DataRow _cust in fCust.resultCust.Rows)
                {
                    string _custId = _cust["id"].ToString();
                    string _filter = "CustId = '" + _custId + "' AND name = 'Почтовый паспорт'";
                    DataRow[] _card = fCust.resultCard.Select(_filter);
                    if (_card.Length == 0)
                    {
                        fCust.resultCard.Rows.Add(new object[] { _custId, _custId, "Отсутствует у клиента", "-", "-", "-", "-",
                            "-", "-", "-", "-", "Почтовый паспорт", "-", "-", "-", "-"});
                    }
                }
            }

        }
        private void CheckCardActivationDate(FilterCust fCust)
        {
            if (fCust.resultCust != null)
            {
                foreach (DataRow _cust in fCust.resultCust.Rows)
                {
                    string _custId = _cust["id"].ToString();
                    string _filter = "CustId = '" + _custId + "'";
                    DataRow[] _cards = fCust.resultCard.Select(_filter);
                    if (_cards.Length != 0)
                    {
                        foreach (DataRow _card in _cards)
                        {
                            string controlDate = _card["activationDate"].ToString();
                            if (string.IsNullOrEmpty(controlDate))
                            {
                                string _filter2 = "CardId = '" + _card["Id"].ToString() + "'";
                                DataRow[] _actions = fCust.resultCardAction.Select(_filter2, "date");
                                if (_actions.Length > 0)
                                {
                                    DataRow temp = _actions[0];
                                    _card["activationDate"] = temp["date"].ToString();
                                    fCust.resultCard.AcceptChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ChangeDataSort(FilterCust fCust)
        {
            if (fCust.resultCust != null)
            {
                DataTable tempCard = fCust.resultCard.Copy();
                fCust.resultCard.Clear();

                #region ChangeCardsSort
                foreach (DataRow _cust in fCust.resultCust.Rows)
                {
                    string _custId = _cust["id"].ToString();
                    string _filter = "CustId = '" + _custId + "'";
                    string _sort = "classOrderId ASC, activationDate DESC";
                    DataRow[] _cards = tempCard.Select(_filter, _sort);
                    if (_cards.Length != 0)
                    {
                        foreach (DataRow _card in _cards)
                        {
                            fCust.resultCard.Rows.Add(_card.ItemArray);
                            fCust.resultCard.AcceptChanges();
                        }
                    }
                }
                #endregion
                #region ChangeCustomersSort

                DataTable tempAddress = fCust.resultCustAddress.Copy();
                fCust.resultCustAddress.Clear();
                DataTable tempPassport = fCust.resultCustPassport.Copy();
                fCust.resultCustPassport.Clear();

                fCust.resultCust.Columns.Add("sortId");
                fCust.resultCust.Columns.Add("cardActivationDate");
                DataTable preCust = fCust.resultCust.Copy();

                DataRow[] _rows = new DataRow[fCust.resultCust.Rows.Count];
                fCust.resultCust.Rows.CopyTo(_rows, 0);
                foreach (DataRow row in _rows)
                {
                    fCust.resultCust.Rows.Remove(row);
                }

                foreach (DataRow _cust in preCust.Rows)
                {
                    string _custId = _cust["id"].ToString();
                    string _filter = "CustId = '" + _custId + "'";
                    string _sort = "activationDate DESC";
                    DataRow[] _cards = fCust.resultCard.Select(_filter, _sort);
                    if (_cards.Length != 0)
                    {
                        DataRow _card = _cards[0];
                        _cust["cardActivationDate"] = _card["activationDate"].ToString();
                    }
                    _filter = "CustId = '" + _custId + "' AND status = 'Активна'";
                    _cards = tempCard.Select(_filter);
                    if (_cards.Length != 0)
                        _cust["sortId"] = "1";
                    else
                        _cust["sortId"] = "2";
                    preCust.AcceptChanges();
                }
                string _sortCust = "sortId ASC, lastChangeDate DESC, cardActivationDate DESC";
                DataRow[] tempCusts = preCust.Select("", _sortCust);
                foreach (DataRow _cust in tempCusts)
                {
                    fCust.resultCust.Rows.Add(_cust.ItemArray);
                    fCust.resultCust.AcceptChanges();
                }
                //fCust.resultCust.Columns.Remove("cardActivationDate");
                //fCust.resultCust.Columns.Remove("sortId");
                fCust.resultCustPassport = tempPassport.Copy();
                fCust.resultCustAddress = tempAddress.Copy();
                #endregion
            }
        }
        private string ConvertDateFormat(string strDate)
        {
            string text = string.Empty;
            if (strDate.IndexOf("-") == 4)
            {
                text = strDate.Substring(8, 2) + "." + strDate.Substring(5, 2) + "." + strDate.Substring(0, 4);
            }
            else if (strDate.IndexOf("-") == 2)
            {
                text = strDate.Substring(0, 2) + "." + strDate.Substring(3, 2) + "." + strDate.Substring(6, 4);
            }
            else
            {
                if (strDate.Length > 10)
                {
                    text = strDate.Remove(11, strDate.Length - 10);
                }
                else
                {
                    text = strDate;
                }
            }

            if (strDate.Length > 10)
            {
                text += strDate.Substring(11);
            }
            return text;
        }
        private int ClientInResult()
        {
            int rows = 3;
            int.TryParse(ConfigurationManager.AppSettings["ClientInResult"], out rows);
            return rows;
        }
        private string ConvertCardAction(string EnAction)
        {
            string RuAction = string.Empty;
            switch (EnAction)
            {
                case "Blocked":
                    RuAction = "Заблокирована";
                    break;
                case "Create":
                    RuAction = "Активна";
                    break;
                case "Active":
                    RuAction = "Активна";
                    break;
                case "Changed":
                    RuAction = "Заменена";
                    break;
                default:
                    RuAction = EnAction;
                    break;
            }
            return RuAction;
        }
        private string ConvertClassOrder(string _classId)
        {
            string newOrder = string.Empty;
            switch (_classId)
            {
                case "708911":
                    {
                        newOrder = "1";
                        break;
                    }
                case "718713":
                    {
                        newOrder = "2";
                        break;
                    }
                case "5":
                    {
                        newOrder = "3";
                        break;
                    }
                default:
                    {
                        newOrder = "10";
                        break;
                    }
            }
            return newOrder;
        }
    }

}