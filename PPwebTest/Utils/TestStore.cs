using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

namespace PPweb.Utils
{
    public class TestStore
    {
        public class FillTempTables
        {
            private static DataTable CustT = new DataTable("clientVO");
            private static DataTable AddressT = new DataTable("clientAddress");
            private static DataTable PassportT = new DataTable("passport");
            private static DataTable CardT = new DataTable("Cards");
            private static DataTable CardActionT = new DataTable("cardAction");

            public DataTable fillCust()
            {
                if (CustT.Columns.Count == 0)
                {
                    DataColumn idCol = new DataColumn("id", Type.GetType("System.String"));
                    idCol.Unique = true; // столбец будет иметь уникальное значение
                    idCol.AllowDBNull = false; // не может принимать null

                    DataColumn lastName = new DataColumn("lastName", Type.GetType("System.String"));
                    DataColumn firstName = new DataColumn("firstName", Type.GetType("System.String"));
                    DataColumn middleName = new DataColumn("middleName", Type.GetType("System.String"));
                    DataColumn birthDate = new DataColumn("birthDate", Type.GetType("System.String"));
                    DataColumn email = new DataColumn("email", Type.GetType("System.String"));
                    DataColumn phone = new DataColumn("phone", Type.GetType("System.String"));
                    DataColumn mobilephone = new DataColumn("mobilePhone", Type.GetType("System.String"));
                    DataColumn mobileOperator = new DataColumn("mobileOperator", Type.GetType("System.String"));
                    DataColumn creationDate = new DataColumn("creationDate", Type.GetType("System.String"));
                    DataColumn fiscalNumber = new DataColumn("fiscalNumber", Type.GetType("System.String"));
                    DataColumn snilsNumber = new DataColumn("snilsNumber", Type.GetType("System.String"));
                    DataColumn recivePension = new DataColumn("recivePension", Type.GetType("System.String"));
                    DataColumn sex = new DataColumn("sex", Type.GetType("System.String"));
                    DataColumn sendCatalog = new DataColumn("sendCatalog", Type.GetType("System.String"));
                    DataColumn cards = new DataColumn("cards", Type.GetType("System.String"));

                    if(!CustT.Columns.Contains("id"))
                    {
                        CustT.Columns.Add(idCol);
                        CustT.PrimaryKey = new DataColumn[] { CustT.Columns["id"] };
                    }
                    if (!CustT.Columns.Contains("lastName"))
                        CustT.Columns.Add(lastName);
                    if (!CustT.Columns.Contains("firstName"))
                        CustT.Columns.Add(firstName);
                    if (!CustT.Columns.Contains("middleName"))
                        CustT.Columns.Add(middleName);
                    if (!CustT.Columns.Contains("birthDate"))
                        CustT.Columns.Add(birthDate);
                    if (!CustT.Columns.Contains("email"))
                        CustT.Columns.Add(email);
                    if (!CustT.Columns.Contains("phone"))
                        CustT.Columns.Add(phone);
                    if (!CustT.Columns.Contains("mobilephone"))
                        CustT.Columns.Add(mobilephone);
                    if (!CustT.Columns.Contains("mobileOperator"))
                        CustT.Columns.Add(mobileOperator);
                    if (!CustT.Columns.Contains("creationDate"))
                        CustT.Columns.Add(creationDate);
                    if (!CustT.Columns.Contains("fiscalNumber"))
                        CustT.Columns.Add(fiscalNumber);
                    if (!CustT.Columns.Contains("snilsNumber"))
                        CustT.Columns.Add(snilsNumber);
                    if (!CustT.Columns.Contains("recivePension"))
                        CustT.Columns.Add(recivePension);
                    if (!CustT.Columns.Contains("sex"))
                        CustT.Columns.Add(sex);
                    if (!CustT.Columns.Contains("sendCatalog"))
                        CustT.Columns.Add(sendCatalog);
                    if (!CustT.Columns.Contains("cards"))
                        CustT.Columns.Add(cards);
                }
                if (CustT.Rows.Count == 0)
                {

                    CustT.Rows.Add(new object[] { "1", "Иванов", "Петр", "Сергеевич", DateTime.Now.Date, "ips@test.te", "495-1112233",
                                   "918-1112233", "918", DateTime.Today.Date, "123456789012", "123-123-123 45", "0", "Male", "true"});
                    CustT.Rows.Add(new object[] { "2", "Смирнова", "Людмила", "Ивановна", DateTime.Now.Date, "sli@test.te", "495-1114533",
                                   "910-1112233", "910", DateTime.Today.Date, "343456789034", "123-123-133 45", "0", "Female", "false"});
                    CustT.Rows.Add(new object[] { "3", "Петров", "Олег", "Викторович", DateTime.Now.Date, "pov@test.te", "495-6712233",
                                   "926-1153233", "926", DateTime.Today.Date, "190456789012", "123-144-123 48", "0", "Male", "true"});
                }
                return CustT;
            }
            public DataTable fillCust(bool emptyTB)
            {
                DataTable tmpTB = new DataTable();
                tmpTB = fillCust();
                tmpTB.Clear();
                return tmpTB;
            }
            public DataTable fillCustAddress()
            {
                if (AddressT.Columns.Count == 0)
                {
                    DataColumn idCol = new DataColumn("CustId", Type.GetType("System.String"));
                    idCol.Unique = true; // столбец будет иметь уникальное значение
                    idCol.AllowDBNull = false; // не может принимать null
                    
                    DataColumn zip = new DataColumn("zip", Type.GetType("System.String"));
                    DataColumn region = new DataColumn("region", Type.GetType("System.String"));
                    DataColumn district = new DataColumn("district", Type.GetType("System.String"));
                    DataColumn districtArea = new DataColumn("districtArea", Type.GetType("System.String"));
                    DataColumn street = new DataColumn("street", Type.GetType("System.String"));
                    DataColumn house = new DataColumn("house", Type.GetType("System.String"));
                    DataColumn city = new DataColumn("city", Type.GetType("System.String"));
                    DataColumn bilding = new DataColumn("building", Type.GetType("System.String"));
                    DataColumn appartment = new DataColumn("appartment", Type.GetType("System.String"));
                    DataColumn corp = new DataColumn("corp", Type.GetType("System.String"));
                    DataColumn others = new DataColumn("other", Type.GetType("System.String"));

                    if (!AddressT.Columns.Contains("CustId"))
                    {
                        AddressT.Columns.Add(idCol);
                        AddressT.PrimaryKey = new DataColumn[] { CustT.Columns["CustId"] };
                    }
                    if (!AddressT.Columns.Contains("zip"))
                        AddressT.Columns.Add(zip);
                    if (!AddressT.Columns.Contains("region"))
                        AddressT.Columns.Add(region);
                    if (!AddressT.Columns.Contains("district"))
                        AddressT.Columns.Add(district);
                    if (!AddressT.Columns.Contains("districtArea"))
                        AddressT.Columns.Add(districtArea);
                    if (!AddressT.Columns.Contains("street"))
                        AddressT.Columns.Add(street);
                    if (!AddressT.Columns.Contains("house"))
                        AddressT.Columns.Add(house);
                    if (!AddressT.Columns.Contains("city"))
                        AddressT.Columns.Add(city);
                    if (!AddressT.Columns.Contains("bilding"))
                        AddressT.Columns.Add(bilding);
                    if (!AddressT.Columns.Contains("appartment"))
                        AddressT.Columns.Add(appartment);
                    if (!AddressT.Columns.Contains("corp"))
                        AddressT.Columns.Add(corp);
                    if (!AddressT.Columns.Contains("others"))
                        AddressT.Columns.Add(others);
                }
                if (AddressT.Rows.Count == 0)
                {

                    AddressT.Rows.Add(new object[] { "1", "112233", "Республика Адыгея", "Охтеевский", "", "ул. Лесная", "22", "д. Горки",
                                   "", "1","", "" });
                    AddressT.Rows.Add(new object[] {"2", "119033", "", "Ясногорский", "Тульская область", "ул. Маяковского", "12", "Чупики",
                                   "", "15","2", "" });
                    AddressT.Rows.Add(new object[] { "3", "782233", "Республика Тува", "Чащи", "", "ул. Мирская", "2", "Ливы",
                                   "", "11","", "" });
                }

                return AddressT;
            }
            public DataTable fillCustAddress(bool emptyTB)
            {
                DataTable tmpTB = new DataTable();
                tmpTB = fillCustAddress();
                tmpTB.Clear();
                return tmpTB;
            }
            public DataTable fillCustPassport()
            {
                if (PassportT.Rows.Count == 0)
                {
                    DataColumn idCol = new DataColumn("CustId", Type.GetType("System.String"));
                    idCol.Unique = true; // столбец будет иметь уникальное значение
                    idCol.AllowDBNull = false; // не может принимать null

                    DataColumn passSerie = new DataColumn("passSerie", Type.GetType("System.String"));
                    DataColumn passNumber = new DataColumn("passNumber", Type.GetType("System.String"));
                    DataColumn delivery = new DataColumn("delivery", Type.GetType("System.String"));
                    DataColumn departamentCode = new DataColumn("departmentCode", Type.GetType("System.String"));
                    DataColumn deliveryDate = new DataColumn("deliveryDate", Type.GetType("System.String"));

                    if (!PassportT.Columns.Contains("CustId"))
                    {
                        PassportT.Columns.Add(idCol);
                        PassportT.PrimaryKey = new DataColumn[] { CustT.Columns["CustId"] };
                    }
                    if (!PassportT.Columns.Contains("passSerie"))
                        PassportT.Columns.Add(passSerie);
                    if (!PassportT.Columns.Contains("passNumber"))
                        PassportT.Columns.Add(passNumber);
                    if (!PassportT.Columns.Contains("delivery"))
                        PassportT.Columns.Add(delivery);
                    if (!PassportT.Columns.Contains("departmentCode"))
                        PassportT.Columns.Add(departamentCode);
                    if (!PassportT.Columns.Contains("deliveryDate"))
                        PassportT.Columns.Add(deliveryDate);

                    PassportT.Rows.Add(new object[] { "1", "1122", "123456", "ОВД Тавридоский", "756-34", DateTime.Today.ToString() });
                    PassportT.Rows.Add(new object[] { "2", "1932", "168456", "ОВД Лимановского района", "790-34", DateTime.Today.ToString() });
                    PassportT.Rows.Add(new object[] { "3", "7805", "582745", "ОВД Республики Тува", "110-63", DateTime.Today.ToString() });
                }

                return PassportT;
            }
            public DataTable fillCustPassport(bool emptyTB)
            {
                DataTable tmpTB = new DataTable();
                tmpTB = fillCustPassport();
                tmpTB.Clear();
                return tmpTB;
            }
            public DataTable fillCard()
            {
                if (CardT.Columns.Count == 0)
                {
                    DataColumn idCol = new DataColumn("Id", Type.GetType("System.String"));
                    idCol.Unique = true; // столбец будет иметь уникальное значение
                    idCol.AllowDBNull = false;

                    if (!CardT.Columns.Contains("Id"))
                    {
                        CardT.Columns.Add(idCol);
                        CardT.PrimaryKey = new DataColumn[] { CardT.Columns["Id"] };
                    }
                    if (!CardT.Columns.Contains("CustId"))
                        CardT.Columns.Add("CustId");
                    if (!CardT.Columns.Contains("number"))
                        CardT.Columns.Add("number");
                    if (!CardT.Columns.Contains("activationDate"))
                        CardT.Columns.Add("activationDate");
                    if (!CardT.Columns.Contains("blockDate"))
                        CardT.Columns.Add("blockDate");
                    if (!CardT.Columns.Contains("blocked"))
                        CardT.Columns.Add("blocked");
                    if (!CardT.Columns.Contains("createDate"))
                        CardT.Columns.Add("createDate");
                    if (!CardT.Columns.Contains("deleted"))
                        CardT.Columns.Add("deleted");
                    if (!CardT.Columns.Contains("status"))
                        CardT.Columns.Add("status");
                    if (!CardT.Columns.Contains("statusDescription"))
                        CardT.Columns.Add("statusDescription");
                    //------------------из тб cardTypeVO
                    if (!CardT.Columns.Contains("classType"))
                        CardT.Columns.Add("classType");
                    if (!CardT.Columns.Contains("name"))
                        CardT.Columns.Add("name");
                    if (!CardT.Columns.Contains("personalized"))
                        CardT.Columns.Add("personalized");
                    //------------------из тб color
                    if (!CardT.Columns.Contains("blue"))
                        CardT.Columns.Add("blue");
                    if (!CardT.Columns.Contains("green"))
                        CardT.Columns.Add("green");
                    if (!CardT.Columns.Contains("red"))
                        CardT.Columns.Add("red");

                }
                if (CardT.Rows.Count == 0)
                {
                    CardT.Rows.Add(new object[] { "1", "1", "1122334455", DateTime.Today.ToString(), "", "false", "2018-05-25T13:32:48.379+04:00","false", "Активна"
                        ,"false", "InternalCardsVO", "Почтовый паспорт", "true", "239", "166", "197" });
                    CardT.Rows.Add(new object[] { "2", "2", "5485462842", DateTime.Today.ToString(), DateTime.Today.ToString(), "true", "2018-05-25T13:32:48.379+04:00","false", "Заблокирована"
                        , "false", "InternalCardsVO", "Любимый клиент", "true", "106", "106", "248" });
                    CardT.Rows.Add(new object[] { "3", "2", "5485466758", DateTime.Today.ToString(), "", "false", "2018-05-25T13:32:48.379+04:00","false", "Активна"
                        , "false", "InternalCardsVO", "Почтовый паспорт", "true", "239", "166", "197" });
                    CardT.Rows.Add(new object[] { "4", "3", "6837537473", DateTime.Today.ToString(), "", "false", "2018-05-25T13:32:48.379+04:00","false", "Активна"
                        , "false", "InternalCardsVO", "Почтовый паспорт", "true", "239", "166", "197" });
                }
                return CardT;
            }
            public DataTable fillCard(bool emptyTB)
            {
                DataTable tmpTB = new DataTable();
                tmpTB = fillCard();
                tmpTB.Clear();
                return tmpTB;
            }
            public DataTable fillCardAction()
            {
                
                if (CardActionT.Columns.Count == 0)
                {
                    DataColumn idCol = new DataColumn("Id", Type.GetType("System.Int32"));
                    idCol.Unique = true; // столбец будет иметь уникальное значение
                    idCol.AllowDBNull = false; // не может принимать null
                    idCol.AutoIncrement = true; // будет автоинкрементироваться
                    idCol.AutoIncrementSeed = 1; // начальное значение
                    idCol.AutoIncrementStep = 1; // приращении при добавлении новой строки

                    DataColumn idCard = new DataColumn("CardId", Type.GetType("System.String"));
                    DataColumn action = new DataColumn("cardAction", Type.GetType("System.String"));
                    DataColumn date = new DataColumn("date", Type.GetType("System.String"));
                    DataColumn userName = new DataColumn("userName", Type.GetType("System.String"));
                    DataColumn shop = new DataColumn("shop", Type.GetType("System.String"));

                    if (!CardActionT.Columns.Contains("Id"))
                    {
                        CardActionT.Columns.Add(idCol);
                        CardActionT.PrimaryKey = new DataColumn[] { CardActionT.Columns["Id"] };
                    }
                    if (!CardActionT.Columns.Contains("CardId"))
                        CardActionT.Columns.Add(idCard);
                    if (!CardActionT.Columns.Contains("cardAction"))
                        CardActionT.Columns.Add(action);
                    if (!CardActionT.Columns.Contains("date"))
                        CardActionT.Columns.Add(date);
                    if (!CardActionT.Columns.Contains("userName"))
                        CardActionT.Columns.Add(userName);
                    if (!CardActionT.Columns.Contains("shop"))
                        CardActionT.Columns.Add(shop);
                }
                if (CardActionT.Rows.Count == 0)
                {

                    CardActionT.Rows.Add(new object[] { null, 1, "Create", DateTime.Today.ToString(), "Оператор Т1", "112233" });
                    CardActionT.Rows.Add(new object[] { null, 2, "Create", DateTime.Today.ToString(), "Оператор Т2", "112233" });
                    CardActionT.Rows.Add(new object[] { null, 2, "Blocked", DateTime.Today.ToString(), "Оператор Т3", "113344" });
                    CardActionT.Rows.Add(new object[] { null, 3, "Create", DateTime.Today.ToString(), "Оператор Т3", "113344" });
                    CardActionT.Rows.Add(new object[] { null, 4, "Create", DateTime.Today.ToString(), "Оператор Т7", "113344" });
                }

                return CardActionT;
            }
            public DataTable fillCardAction(bool emptyTB)
            {
                DataTable tmpTB = new DataTable();
                tmpTB = fillCardAction();
                tmpTB.Clear();
                return tmpTB;
            }
        }
    }
}