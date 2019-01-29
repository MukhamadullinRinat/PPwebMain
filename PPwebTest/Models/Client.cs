using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPweb.Models
{
    public class Client
    {
        public string ID
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote("ValidBirthDate", "Passport")]
        public string BirthDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string FirstName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string LastName
        {
            get;
            set;
        }

        [RegularExpression(@"^[а-яА-ЯёЁ\-\ ]+$", ErrorMessage = "Допустимы только русские буквы")]
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

        [Remote("ValidMobilePhone", "Passport")]
        [RegularExpression(@"^(\+\d\s\d\d\d\s\d\d\d\s\d\d\s\d\d)|(\+\d\s___\s___\s__\s__)|(\+\d\s)$", ErrorMessage = "Введите корректное значение")]
        public string MobilePhone
        {
            get;
            set;
        }

        //@"^(\(?\d{3,5}\)?[\- ]?)?[\d\- ]{5,10}$"
        public string Phone
        {
            get;
            set;
        }

        [RegularExpression(@"^([a-zA-Z0-9_-]+\.)*[a-zA-Z0-9_-]+@[a-z0-9_-]+(\.[a-z0-9_-]+)*\.[a-z]{2,6}$", ErrorMessage = "Некорректный адрес электронной почты")]
        public string EMail
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
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

        [RegularExpression(@"^([0-9\s\-]{14})|([_\s\-]{14})$", ErrorMessage = "Введите корректное значение")]
        public string SnilsNumber
        {
            get;
            set;
        }

        [RegularExpression(@"^([0-9]{12})|([_]{12})$", ErrorMessage = "Введите корректное значение")]
        public string FiscalNumber
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public bool? SendBy
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public bool? SendByTemp
        {
            get;
            set;
        }

        public string CreationDate
        {
            get;
            set;
        }

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

        //[Required(ErrorMessage = "Поле является обязательным для заполнения")]
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

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
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

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string Region
        {
            get;
            set;
        }

        [RegularExpression(@"^[а-яА-ЯёЁ0-9\-.,\s]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string Street
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^([0-9]{6})|([_]{6})$", ErrorMessage = "Введите корректное значение")]
        public string Zip
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^[а-яА-ЯёЁ0-9\-.,\s]+$", ErrorMessage = "Допустимы только русские буквы")]
        public string Delivery
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Remote("ValidPassportDate", "Passport")]
        public string DeliveryDate
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^([0-9\-]{7})|([_\-]{7})$", ErrorMessage = "Введите корректное значение")]
        public string DepartmentCode
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^([0-9]{6})|([_]{6})$", ErrorMessage = "Введите корректное значение")]
        public string Number
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression(@"^([0-9]{4})|([_]{4})$", ErrorMessage = "Введите корректное значение")]
        public string Serie
        {
            get;
            set;
        }
        public string PassportForValid
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Допустимы только цифры")]
        [MinLength(13, ErrorMessage = "Длина номера должна составлять 13 цифр")]
        [MaxLength(13, ErrorMessage = "Длина номера должна составлять 13 цифр")]
        public string CardNumber
        {
            get; set;
        }

        public string ModalBoxType { get; set; }
        public string Header { get; set; }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string RegionCode { get; set; }
    }
}