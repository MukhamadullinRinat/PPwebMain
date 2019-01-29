using PPweb.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace PPweb.Models
{
    public class CClientCard
    {

        public class CClient
        {
            public string ID
            {
                get;
                set;
            }

            public string BirthDate
            {
                get;
                set;
            }

            public string FirstName
            {
                get;
                set;
            }

            public string LastName
            {
                get;
                set;
            }

            public string MiddleName
            {
                get;
                set;
            }

            public string Operator
            {
                get;
                set;
            }

            public string MobilePhone
            {
                get;
                set;
            }

            public string Phone
            {
                get;
                set;
            }

            public string EMail
            {
                get;
                set;
            }

            public string Sex
            {
                get;
                set;
            }

            public int ReceivePension
            {
                get;
                set;
            }

            public string ShopNumber
            {
                get;
                set;
            }

            public string OperatorName
            {
                get;
                set;
            }

            public string SnilsNumber
            {
                get;
                set;
            }

            public string FiscalNumber
            {
                get;
                set;
            }

            public bool? SendBy
            {
                get;
                set;
            }

            public string CreationDate
            {
                get;
                set;
            }

            public CAddress ClientAddress
            {
                get;
                set;
            }

            public CDocument Passport
            {
                get;
                set;
            }

            public CClient()
            {
                this.ClientAddress = new CAddress();
                this.Passport = new CDocument();
                this.SendBy = false;
            }
        }
        public class CAddress
        {
            public string Appartment
            {
                get;
                set;
            }

            public string Building
            {
                get;
                set;
            }

            public string Corp
            {
                get;
                set;
            }

            public string City
            {
                get;
                set;
            }

            public string District
            {
                get;
                set;
            }

            public string DistrictArea
            {
                get;
                set;
            }

            public string House
            {
                get;
                set;
            }

            public string Other
            {
                get;
                set;
            }

            public string Region
            {
                get;
                set;
            }

            public string Street
            {
                get;
                set;
            }

            public string Zip
            {
                get;
                set;
            }

            private bool IsFederalCity
            {
                get
                {
                    if (this.Region.Contains("МОСКВА") || this.Region.Contains("САНКТ-ПЕТЕРБУРГ") || this.Region.Contains("СЕВАСТОПОЛЬ"))
                    {
                        return true;
                    }
                    return false;
                }
            }

            public bool isCityEqualsRegion
            {
                get
                {
                    if (City == null)
                        City = string.Empty;
                    if (this.IsFederalCity && this.Region.ToUpper().Equals(this.City.Trim().ToUpper()))
                    {
                        goto IL_006c;
                    }
                    if (this.IsFederalCity && this.City.ToUpper().Equals("СПБ"))
                    {
                        goto IL_006c;
                    }
                    int result = (this.IsFederalCity && this.City.ToUpper().Equals("МСК")) ? 1 : 0;
                    goto IL_006d;
                    IL_006c:
                    result = 1;
                    goto IL_006d;
                    IL_006d:
                    return (byte)result != 0;
                }
            }

            public static bool StatusFederalCity(string region)
            {
                CAddress cAddress = new CAddress();
                cAddress.Region = region;
                CAddress cAddress2 = cAddress;
                return cAddress2.IsFederalCity;
            }
        }
        public class CDocument
        {
            public string Delivery
            {
                get;
                set;
            }

            public string DeliveryDate
            {
                get;
                set;
            }

            public string DepartmentCode
            {
                get;
                set;
            }

            public string Number
            {
                get;
                set;
            }

            public string Serie
            {
                get;
                set;
            }
        }
        public class CCard
        {
            public string CardNumber
            {
                get;
                set;
            }
            public string CardId
            {
                get;
                set;
            }
        }
        public class CCardBlockingReason
        {
            public string Value
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public CCardBlockingReason(string value, string name)
            {
                this.Value = value;
                this.Name = name;
            }
        }
        public class CCardBlockingReasonList : List<CCardBlockingReason>
        {
            public CCardBlockingReasonList()
            {
                Add(new CCardBlockingReason("ROBBERY", "Отказ от использования"));
                Add(new CCardBlockingReason("LOSS", "Карта утеряна"));
                Add(new CCardBlockingReason("REFUSAL", "Карта повреждена"));
                Add(new CCardBlockingReason("ANOTHER", "Другая причина"));
            }
        }
        public class CCardFilter
        {
            public string ClientID
            {
                get;
                set;
            }

            public string Number
            {
                get;
                set;
            }

            public string LastName
            {
                get;
                set;
            }

            public string FirstName
            {
                get;
                set;
            }

            public string MiddleName
            {
                get;
                set;
            }

            public string BirthDate
            {
                get;
                set;
            }

            public string PassSerie
            {
                get;
                set;
            }

            public string PassNumber
            {
                get;
                set;
            }
        }
        
        public class ValidPP
        {
            public string LetValidSome(CClient client)
            {
                string err = string.Empty;
                if (!validateClient(client, out err))
                    { return err; }
                else
                {
                    if (!validCAddress(client, out err))
                    { return err; }
                    else
                    {
                        if (!validateCPass(client, out err))
                        {
                            return err;
                        }
                        else return err;
                    }
                }
            }
            private bool validateClient(CClient client, out string errMessage)
            {
                bool result = true;
                errMessage = "";
                /*if (client.BirthDate == null)
                    client.BirthDate = string.Empty;*/
                /*if (client.BirthDate == null)
                {
                    errMessage = errMessage + "Не заполнена дата рождения." + Environment.NewLine;
                    result = false;
                }*/
                /*if (String.IsNullOrEmpty(client.FirstName))
                {
                    errMessage = errMessage + "Не заполнено имя клиента." + Environment.NewLine;
                    result = false;
                }
                if (String.IsNullOrEmpty(client.LastName))
                {
                    errMessage = errMessage + "Не заполнена фамилия клиента" + Environment.NewLine;
                    result = false;
                }*/
                /*if (errMessage.Length > 1)
                {
                    errMessage = errMessage.Substring(0, errMessage.Length - 2);
                }*/
                return result;
            }
            private bool validCAddress(CClient client, out string errMessage)
            {
                bool result = true;
                errMessage = "";
                /*if (client.ClientAddress.Zip.Trim().Length != 6)
                {
                    errMessage = errMessage + "Неверно заполнен индекс в адресе клиента." + Environment.NewLine;
                    result = false;
                }*/
                /*if (String.IsNullOrEmpty(client.ClientAddress.Region))
                {
                    errMessage = errMessage + "Неверно заполнен регион в адресе клиента." + Environment.NewLine;
                    result = false;
                }*/
                if (!CClientCard.CAddress.StatusFederalCity(client.ClientAddress.Region))
                {
                    if (client.ClientAddress.City.Trim().Equals(string.Empty))
                    {
                        errMessage = errMessage + "Неверно заполнен населенный пункт в адресе клиента." + Environment.NewLine;
                        result = false;
                    }
                }
                else if (client.ClientAddress.isCityEqualsRegion)
                {
                    errMessage = errMessage + "Поле населённый пункт и регион должны отличаться" + Environment.NewLine;
                    result = false;
                }
                if (errMessage.Length > 1)
                {
                    errMessage = errMessage.Substring(0, errMessage.Length - 2);
                }
                return result;
            }
            private bool validateCPass(CClient client, out string errMessage)
            {
                bool result = true;
                errMessage = "";
                /*if (String.IsNullOrEmpty(client.Passport.Delivery))
                {
                    errMessage = errMessage + "Неверно заполнено поле кем выдан паспорт." + Environment.NewLine;
                    result = false;
                }
                if (client.Passport.Number.Trim().Length != 6)
                {
                    errMessage = errMessage + "Неверно заполнен номер паспорта." + Environment.NewLine;
                    result = false;
                }
                if (client.Passport.Serie.Trim().Length != 4)
                {
                    errMessage = errMessage + "Неверно заполнена серия паспорта." + Environment.NewLine;
                    result = false;
                }*/
                /*if (client.Passport.DeliveryDate.Trim().Length != 10)
                {
                    errMessage = errMessage + "Неверно заполнена дата выдачи паспорта." + Environment.NewLine;
                    result = false;
                }*/
                /*if (client.Passport.DepartmentCode.Trim().Length < 6)
                {
                    errMessage = errMessage + "Неверно заполнен код подразделения паспорта." + Environment.NewLine;
                    result = false;
                }
                if (errMessage.Length > 1)
                {
                    errMessage = errMessage.Substring(0, errMessage.Length - 2);
                }*/
                return result;
            }
            private bool PassUniq(CClient client, out string errMessage)
            {
                bool result = true;
                errMessage = "";
                if (!this.checkClient(client))
                {
                    errMessage = errMessage + "Клиент с такими паспортными данными уже существует." + Environment.NewLine;
                    result = false;
                }
                if (errMessage.Length > 1)
                {
                    errMessage = errMessage.Substring(0, errMessage.Length - 2);
                }
                return result;
            }
            private bool checkClient(CClient client)
            {
                LoyaltyService.RequestGenerator rg = new LoyaltyService.RequestGenerator();
                if (client.ID == null || client.ID.Length == 0)
                {
                    try
                    {

                        DataSet cardByFilter = rg.getCardByFilter(new CCardFilter
                        {
                            PassNumber = client.Passport.Number,
                            PassSerie = client.Passport.Serie
                        });
                        if (cardByFilter.Tables.Count > 0 && cardByFilter.Tables.Contains("ClientVO") && cardByFilter.Tables["ClientVO"].Rows.Count > 0)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    try
                    {
                        DataSet cardByFilter = rg.getCardByFilter(new CCardFilter
                        {
                            PassNumber = client.Passport.Number,
                            PassSerie = client.Passport.Serie
                        });
                        if (cardByFilter.Tables.Contains("ClientVO"))
                        {
                            foreach (DataRow row in cardByFilter.Tables["ClientVO"].Rows)
                            {
                                if (row["ID"].ToString() != client.ID)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
    public class OpsInfo
    {
        public class COpsInfo
        {
            private string pOpsShopId = string.Empty;
            private string pOpsOperatorId = string.Empty;
            private string pOpsOperatorName = string.Empty;
            private string pOpsOperatorFName = string.Empty;
            public string OpsIndex
            {
                get;
                set;
            }

            public string OpsAddress
            {
                get;
                set;
            }

            public string OpsWorkTime
            {
                get;
                set;
            }

            public string OpsFreeDays
            {
                get;
                set;
            }

            public string OpsTimeOff
            {
                get;
                set;
            }

            public string OpsPhone
            {
                get;
                set;
            }
            public string OpsOperatorId //Id оператора
            {
                get
                {
                    return pOpsOperatorId;
                }
                set
                {
                    pOpsOperatorId = value;
                }
            }
            public string OpsShopId //Id терминала (окна)
            {
                get
                {
                    return pOpsShopId;
                }
                set
                {
                    pOpsShopId = value;
                }
            }
            public string OpsOperatorName //Оператор ФИО (сокращенное)
            {
                get
                {
                    return pOpsOperatorName;
                }
                set
                {
                    pOpsOperatorName = value;
                }
            }
            public string OpsOperatorFullName //Оператор ФИО (полное)
            {
                get
                {
                    return pOpsOperatorFName;
                }
                set
                {
                    pOpsOperatorFName = value;
                }
            }

        }
        
    }

}