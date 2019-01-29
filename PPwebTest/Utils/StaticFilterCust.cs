using PPweb.Models;
using System.Data;

namespace PPweb.Utils
{
    public static class StaticFilterCust
    {
        public static string fCardNumber
        {
            get;
            set;
        }

        public static string fLastName
        {
            get; set;
        }

        public static string fFirstName
        {
            get; set;
        }

        public static string fMiddleName
        {
            get; set;
        }

        public static string fBirthDate
        {
            get; set;
        }


        public static string fPassSeria
        {
            get; set;
        }

        public static string fPassNumber
        {
            get; set;
        }

        public static DataTable resultCust
        {
            get; set;
        }
        public static DataTable resultCustPassport
        {
            get; set;
        }
        public static DataTable resultCustAddress
        {
            get; set;
        }
        public static DataTable resultCard
        {
            get; set;
        }
        public static DataTable resultCardAction
        {
            get; set;
        }
    }  
    public static class StaticEditClient
        {
            public static string ID
            {
                get;
                set;
            }
            
            public static string BirthDate
            {
                get;
                set;
            }
            
            public static string FirstName
            {
                get;
                set;
            }
            public static string LastName
            {
                get;
                set;
            }
            public static string MiddleName
            {
                get;
                set;
            }

            public static string Operator
            {
                get;
                set;
            }
            
            public static string MobilePhone
            {
                get;
                set;
            }
            public static string Phone
            {
                get;
                set;
            }
            public static string EMail
            {
                get;
                set;
            }

            public static string Sex
            {
                get;
                set;
            }

            public static int ReceivePension
            {
                get;
                set;
            }

            public static string ShopNumber
            {
                get;
                set;
            }

            public static string OperatorName
            {
                get;
                set;
            }
            public static string SnilsNumber
            {
                get;
                set;
            }
            public static string FiscalNumber
            {
                get;
                set;
            }

            public static bool? SendBy
            {
                get;
                set;
            }

            public static string CreationDate
            {
                get;
                set;
            }
            public static string Appartment
            {
                get;
                set;
            }

            public static string Building
            {
                get;
                set;
            }

            public static string Corp
            {
                get;
                set;
            }
            public static string City
            {
                get;
                set;
            }
            public static string District
            {
                get;
                set;
            }

            public static string DistrictArea
            {
                get;
                set;
            }

            public static string House
            {
                get;
                set;
            }

            public static string Other
            {
                get;
                set;
            }

            public static string Region
            {
                get;
                set;
            }
            public static string Street
            {
                get;
                set;
            }
            public static string Zip
            {
                get;
                set;
            }
            public static string Delivery
            {
                get;
                set;
            }
            public static string DeliveryDate
            {
                get;
                set;
            }

            public static string DepartmentCode
            {
                get;
                set;
            }
            
            public static string Number
            {
                get;
                set;
            }
            public static string Serie
            {
                get;
                set;
            }
            public static DataTable resultCard
            {
                get; set;
            }
            public static DataTable resultCardAction
            {
                get; set;
            }
        }
    public static class StaticOpsInfo
    {
        public static string OpsIndex
        {
            get;
            set;
        }

        public static string OpsAddress
        {
            get;
            set;
        }

        public static string OpsWorkTime
        {
            get;
            set;
        }

        public static string OpsFreeDays
        {
            get;
            set;
        }

        public static string OpsTimeOff
        {
            get;
            set;
        }

        public static string OpsPhone
        {
            get;
            set;
        }
        public static string OpsOperatorId //Id оператора
        {
            get;
            set;
        }
        public static string OpsShopId //Id терминала (окна)
        {
            get;
            set;
        }
        public static string OpsOperatorName //Оператор ФИО (сокращенное)
        {
            get;
            set;
        }
        public static string OpsOperatorFullName //Оператор ФИО (полное)
        {
            get;
            set;
        }
    }
    public static class StaticModifyCard
    {
        public static string CustId
        {
            get; set;
        }
        public static string oldId
        {
            get; set;
        }
        public static string oldNumber
        {
            get; set;
        }
        public static CClientCard.CCardBlockingReason Reason
        {
            get; set;
        }
        public static string newNumber
        {
            get; set;
        }
        public static string Comment
        {
            get; set;
        }
        public static DataTable resultNewCard
        {
            get; set;
        }
        public static DataTable resultNewCardAction
        {
            get; set;
        }
    }
    public static class StaticTempCust
    {
        public static string ID
        {
            get;
            set;
        }
        
        public static string BirthDate
        {
            get;
            set;
        }
        
        public static string FirstName
        {
            get;
            set;
        }
        
        public static string LastName
        {
            get;
            set;
        }
        
        public static string MiddleName
        {
            get;
            set;
        }

        public static string Operator
        {
            get;
            set;
        }
        
        public static string MobilePhone
        {
            get;
            set;
        }
        
        public static string Phone
        {
            get;
            set;
        }
        
        public static string EMail
        {
            get;
            set;
        }

        public static string Sex
        {
            get;
            set;
        }

        public static int ReceivePension
        {
            get;
            set;
        }

        public static string ShopNumber
        {
            get;
            set;
        }

        public static string OperatorName
        {
            get;
            set;
        }

        public static string SnilsNumber
        {
            get;
            set;
        }

        public static string FiscalNumber
        {
            get;
            set;
        }

        public static bool? SendBy
        {
            get;
            set;
        }

        public static string CreationDate
        {
            get;
            set;
        }
        public static string Appartment
        {
            get;
            set;
        }

        public static string Building
        {
            get;
            set;
        }

        public static string Corp
        {
            get;
            set;
        }

        public static string City
        {
            get;
            set;
        }

        public static string District
        {
            get;
            set;
        }

        public static string DistrictArea
        {
            get;
            set;
        }

        public static string House
        {
            get;
            set;
        }

        public static string Other
        {
            get;
            set;
        }

        public static string Region
        {
            get;
            set;
        }

        public static string Street
        {
            get;
            set;
        }

        public static string Zip
        {
            get;
            set;
        }
        public static string Delivery
        {
            get;
            set;
        }

        public static string DeliveryDate
        {
            get;
            set;
        }

        public static string DepartmentCode
        {
            get;
            set;
        }

        public static string Number
        {
            get;
            set;
        }

        public static string Serie
        {
            get;
            set;
        }
        
        public static string CardNumber
        {
            get; set;
        }
        public static bool CardChecked
        {
            get; set;
        }
        public static bool CardPersonalized
        {
            get; set;
        }
        public static string CardTypeName
        {
            get; set;
        }
        public static string CardStatus
        {
            get; set;
        }
    }
}