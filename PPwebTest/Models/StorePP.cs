using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace PPweb.Models
{
    namespace StorePP
    {
        [Serializable]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [XmlInclude(typeof(cardVO))]
        [XmlInclude(typeof(clientVO))]
        [XmlInclude(typeof(cardRangeVO))]
        [XmlInclude(typeof(cardTypeVO))]
        [XmlInclude(typeof(presentCardsVO))]
        [XmlInclude(typeof(internalCardsVO))]
        public abstract class baseVO
        {
            private long idField;

            private bool idFieldSpecified;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            [XmlIgnore]
            public bool idSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DesignerCategory("code")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        public class cardVO : baseVO
        {
            private string accountNameField;

            private string accountNumberField;

            private DateTime activationDateField;

            private bool activationDateFieldSpecified;

            private long amountField;

            private bool amountFieldSpecified;

            private string barcodeField;

            private string blockDateField;

            private bool blockedField;

            private cardTypeVO cardTypeVOField;

            private clientVO clientVOField;

            private DateTime createDateField;

            private bool createDateFieldSpecified;

            private bool deletedField;

            private bool deletedFieldSpecified;

            private DateTime expirationDateField;

            private bool expirationDateFieldSpecified;

            private long guidField;

            private bool guidFieldSpecified;

            private cardVO mainCardField;

            private string newCardNumberField;

            private cardTypeVO newCardTypeField;

            private string numberField;

            private string oldCardNumberField;

            private bool signChangeOfCategoryCardField;

            private bool signExtensionCardField;

            private EnumConst.cardStatus statusField;

            private bool statusFieldSpecified;

            private string statusDescriptionField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string accountName
            {
                get
                {
                    return this.accountNameField;
                }
                set
                {
                    this.accountNameField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string accountNumber
            {
                get
                {
                    return this.accountNumberField;
                }
                set
                {
                    this.accountNumberField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime activationDate
            {
                get
                {
                    return this.activationDateField;
                }
                set
                {
                    this.activationDateField = value;
                }
            }

            [XmlIgnore]
            public bool activationDateSpecified
            {
                get
                {
                    return this.activationDateFieldSpecified;
                }
                set
                {
                    this.activationDateFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            [XmlIgnore]
            public bool amountSpecified
            {
                get
                {
                    return this.amountFieldSpecified;
                }
                set
                {
                    this.amountFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string barcode
            {
                get
                {
                    return this.barcodeField;
                }
                set
                {
                    this.barcodeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string blockDate
            {
                get
                {
                    return this.blockDateField;
                }
                set
                {
                    this.blockDateField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool blocked
            {
                get
                {
                    return this.blockedField;
                }
                set
                {
                    this.blockedField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public cardTypeVO cardTypeVO
            {
                get
                {
                    return this.cardTypeVOField;
                }
                set
                {
                    this.cardTypeVOField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public clientVO clientVO
            {
                get
                {
                    return this.clientVOField;
                }
                set
                {
                    this.clientVOField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime createDate
            {
                get
                {
                    return this.createDateField;
                }
                set
                {
                    this.createDateField = value;
                }
            }

            [XmlIgnore]
            public bool createDateSpecified
            {
                get
                {
                    return this.createDateFieldSpecified;
                }
                set
                {
                    this.createDateFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool deleted
            {
                get
                {
                    return this.deletedField;
                }
                set
                {
                    this.deletedField = value;
                }
            }

            [XmlIgnore]
            public bool deletedSpecified
            {
                get
                {
                    return this.deletedFieldSpecified;
                }
                set
                {
                    this.deletedFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime expirationDate
            {
                get
                {
                    return this.expirationDateField;
                }
                set
                {
                    this.expirationDateField = value;
                }
            }

            [XmlIgnore]
            public bool expirationDateSpecified
            {
                get
                {
                    return this.expirationDateFieldSpecified;
                }
                set
                {
                    this.expirationDateFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long guid
            {
                get
                {
                    return this.guidField;
                }
                set
                {
                    this.guidField = value;
                }
            }

            [XmlIgnore]
            public bool guidSpecified
            {
                get
                {
                    return this.guidFieldSpecified;
                }
                set
                {
                    this.guidFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public cardVO mainCard
            {
                get
                {
                    return this.mainCardField;
                }
                set
                {
                    this.mainCardField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string newCardNumber
            {
                get
                {
                    return this.newCardNumberField;
                }
                set
                {
                    this.newCardNumberField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public cardTypeVO newCardType
            {
                get
                {
                    return this.newCardTypeField;
                }
                set
                {
                    this.newCardTypeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string number
            {
                get
                {
                    return this.numberField;
                }
                set
                {
                    this.numberField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string oldCardNumber
            {
                get
                {
                    return this.oldCardNumberField;
                }
                set
                {
                    this.oldCardNumberField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool signChangeOfCategoryCard
            {
                get
                {
                    return this.signChangeOfCategoryCardField;
                }
                set
                {
                    this.signChangeOfCategoryCardField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool signExtensionCard
            {
                get
                {
                    return this.signExtensionCardField;
                }
                set
                {
                    this.signExtensionCardField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.cardStatus status
            {
                get
                {
                    return this.statusField;
                }
                set
                {
                    this.statusField = value;
                }
            }

            [XmlIgnore]
            public bool statusSpecified
            {
                get
                {
                    return this.statusFieldSpecified;
                }
                set
                {
                    this.statusFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string statusDescription
            {
                get
                {
                    return this.statusDescriptionField;
                }
                set
                {
                    this.statusDescriptionField = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DesignerCategory("code")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        public class clientVO : baseVO
        {
            private bool autoField;

            private bool autoFieldSpecified;

            private string birthDateField;

            private string childrenAgeField;

            private clientthis clientthisField;

            private DateTime creationDateField;

            private bool creationDateFieldSpecified;

            private bool deletedField;

            private bool deletedFieldSpecified;

            private string emailField;

            private string firstNameField;

            private long guidField;

            private bool guidFieldSpecified;

            private bool isCompletedField;

            private bool isCompletedFieldSpecified;

            private DateTime lastChangeDateField;

            private bool lastChangeDateFieldSpecified;

            private string lastNameField;

            private bool maritalField;

            private bool maritalFieldSpecified;

            private string middleNameField;

            private string mobileOperatorField;

            private string mobilePhoneField;

            private passportVO passportField;

            private string phoneField;

            private sendBy sendByField;

            private bool sendCatalogField;

            private EnumConst.sex sexField;

            private bool sexFieldSpecified;

            private string shopNumberField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool auto
            {
                get
                {
                    return this.autoField;
                }
                set
                {
                    this.autoField = value;
                }
            }

            [XmlIgnore]
            public bool autoSpecified
            {
                get
                {
                    return this.autoFieldSpecified;
                }
                set
                {
                    this.autoFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string birthDate
            {
                get
                {
                    return this.birthDateField;
                }
                set
                {
                    this.birthDateField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string childrenAge
            {
                get
                {
                    return this.childrenAgeField;
                }
                set
                {
                    this.childrenAgeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public clientthis clientthis
            {
                get
                {
                    return this.clientthisField;
                }
                set
                {
                    this.clientthisField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime creationDate
            {
                get
                {
                    return this.creationDateField;
                }
                set
                {
                    this.creationDateField = value;
                }
            }

            [XmlIgnore]
            public bool creationDateSpecified
            {
                get
                {
                    return this.creationDateFieldSpecified;
                }
                set
                {
                    this.creationDateFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool deleted
            {
                get
                {
                    return this.deletedField;
                }
                set
                {
                    this.deletedField = value;
                }
            }

            [XmlIgnore]
            public bool deletedSpecified
            {
                get
                {
                    return this.deletedFieldSpecified;
                }
                set
                {
                    this.deletedFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string email
            {
                get
                {
                    return this.emailField;
                }
                set
                {
                    this.emailField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string firstName
            {
                get
                {
                    return this.firstNameField;
                }
                set
                {
                    this.firstNameField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long guid
            {
                get
                {
                    return this.guidField;
                }
                set
                {
                    this.guidField = value;
                }
            }

            [XmlIgnore]
            public bool guidSpecified
            {
                get
                {
                    return this.guidFieldSpecified;
                }
                set
                {
                    this.guidFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool isCompleted
            {
                get
                {
                    return this.isCompletedField;
                }
                set
                {
                    this.isCompletedField = value;
                }
            }

            [XmlIgnore]
            public bool isCompletedSpecified
            {
                get
                {
                    return this.isCompletedFieldSpecified;
                }
                set
                {
                    this.isCompletedFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime lastChangeDate
            {
                get
                {
                    return this.lastChangeDateField;
                }
                set
                {
                    this.lastChangeDateField = value;
                }
            }

            [XmlIgnore]
            public bool lastChangeDateSpecified
            {
                get
                {
                    return this.lastChangeDateFieldSpecified;
                }
                set
                {
                    this.lastChangeDateFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string lastName
            {
                get
                {
                    return this.lastNameField;
                }
                set
                {
                    this.lastNameField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool marital
            {
                get
                {
                    return this.maritalField;
                }
                set
                {
                    this.maritalField = value;
                }
            }

            [XmlIgnore]
            public bool maritalSpecified
            {
                get
                {
                    return this.maritalFieldSpecified;
                }
                set
                {
                    this.maritalFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string middleName
            {
                get
                {
                    return this.middleNameField;
                }
                set
                {
                    this.middleNameField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string mobileOperator
            {
                get
                {
                    return this.mobileOperatorField;
                }
                set
                {
                    this.mobileOperatorField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string mobilePhone
            {
                get
                {
                    return this.mobilePhoneField;
                }
                set
                {
                    this.mobilePhoneField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public passportVO passport
            {
                get
                {
                    return this.passportField;
                }
                set
                {
                    this.passportField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string phone
            {
                get
                {
                    return this.phoneField;
                }
                set
                {
                    this.phoneField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public sendBy sendBy
            {
                get
                {
                    return this.sendByField;
                }
                set
                {
                    this.sendByField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool sendCatalog
            {
                get
                {
                    return this.sendCatalogField;
                }
                set
                {
                    this.sendCatalogField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.sex sex
            {
                get
                {
                    return this.sexField;
                }
                set
                {
                    this.sexField = value;
                }
            }

            [XmlIgnore]
            public bool sexSpecified
            {
                get
                {
                    return this.sexFieldSpecified;
                }
                set
                {
                    this.sexFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string shopNumber
            {
                get
                {
                    return this.shopNumberField;
                }
                set
                {
                    this.shopNumberField = value;
                }
            }
        }

        [Serializable]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DesignerCategory("code")]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DebuggerStepThrough]
        public class passportVO
        {
            private string deliveryField;

            private string deliveryDateField;

            private string departmentCodeField;

            private string passNumberField;

            private string passSerieField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string delivery
            {
                get
                {
                    return this.deliveryField;
                }
                set
                {
                    this.deliveryField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string deliveryDate
            {
                get
                {
                    return this.deliveryDateField;
                }
                set
                {
                    this.deliveryDateField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string departmentCode
            {
                get
                {
                    return this.departmentCodeField;
                }
                set
                {
                    this.departmentCodeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string passNumber
            {
                get
                {
                    return this.passNumberField;
                }
                set
                {
                    this.passNumberField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string passSerie
            {
                get
                {
                    return this.passSerieField;
                }
                set
                {
                    this.passSerieField = value;
                }
            }
        }


        [Serializable]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DesignerCategory("code")]
        public class sendBy
        {
            private bool byEMailField;

            private bool byMailField;

            private bool byPhoneField;

            private bool bySMSField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool byEMail
            {
                get
                {
                    return this.byEMailField;
                }
                set
                {
                    this.byEMailField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool byMail
            {
                get
                {
                    return this.byMailField;
                }
                set
                {
                    this.byMailField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool byPhone
            {
                get
                {
                    return this.byPhoneField;
                }
                set
                {
                    this.byPhoneField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool bySMS
            {
                get
                {
                    return this.bySMSField;
                }
                set
                {
                    this.bySMSField = value;
                }
            }
        }

        [Serializable]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        public class clientthis
        {
            private string appartmentField;

            private string buildingField;

            private string cityField;

            private string districtField;

            private string districtAreaField;

            private string houseField;

            private string otherField;

            private string regionField;

            private string streetField;

            private string zipField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string appartment
            {
                get
                {
                    return this.appartmentField;
                }
                set
                {
                    this.appartmentField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string building
            {
                get
                {
                    return this.buildingField;
                }
                set
                {
                    this.buildingField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string city
            {
                get
                {
                    return this.cityField;
                }
                set
                {
                    this.cityField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string district
            {
                get
                {
                    return this.districtField;
                }
                set
                {
                    this.districtField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string districtArea
            {
                get
                {
                    return this.districtAreaField;
                }
                set
                {
                    this.districtAreaField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string house
            {
                get
                {
                    return this.houseField;
                }
                set
                {
                    this.houseField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string other
            {
                get
                {
                    return this.otherField;
                }
                set
                {
                    this.otherField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string region
            {
                get
                {
                    return this.regionField;
                }
                set
                {
                    this.regionField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string street
            {
                get
                {
                    return this.streetField;
                }
                set
                {
                    this.streetField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string zip
            {
                get
                {
                    return this.zipField;
                }
                set
                {
                    this.zipField = value;
                }
            }
        }

        [Serializable]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [XmlInclude(typeof(presentCardsVO))]
        [XmlInclude(typeof(internalCardsVO))]
        [DesignerCategory("code")]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DebuggerStepThrough]
        public class cardTypeVO : baseVO
        {
            private string classTypeField;

            private displayColor colorField;

            private bool deletedField;

            private bool deletedFieldSpecified;

            private long guidField;

            private bool guidFieldSpecified;

            private string nameField;

            private bool personalizedField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string classType
            {
                get
                {
                    return this.classTypeField;
                }
                set
                {
                    this.classTypeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public displayColor color
            {
                get
                {
                    return this.colorField;
                }
                set
                {
                    this.colorField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool deleted
            {
                get
                {
                    return this.deletedField;
                }
                set
                {
                    this.deletedField = value;
                }
            }

            [XmlIgnore]
            public bool deletedSpecified
            {
                get
                {
                    return this.deletedFieldSpecified;
                }
                set
                {
                    this.deletedFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long guid
            {
                get
                {
                    return this.guidField;
                }
                set
                {
                    this.guidField = value;
                }
            }

            [XmlIgnore]
            public bool guidSpecified
            {
                get
                {
                    return this.guidFieldSpecified;
                }
                set
                {
                    this.guidFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool personalized
            {
                get
                {
                    return this.personalizedField;
                }
                set
                {
                    this.personalizedField = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        public class displayColor
        {
            private int blueField;

            private int greenField;

            private int redField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int blue
            {
                get
                {
                    return this.blueField;
                }
                set
                {
                    this.blueField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int green
            {
                get
                {
                    return this.greenField;
                }
                set
                {
                    this.greenField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int red
            {
                get
                {
                    return this.redField;
                }
                set
                {
                    this.redField = value;
                }
            }
        }

        [Serializable]
        [DebuggerStepThrough]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DesignerCategory("code")]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        public class presentCardsVO : cardTypeVO
        {
            private long amountField;

            private bool amountFieldSpecified;

            private string barcodeField;

            private bool fixedAmountField;

            private long maxAmountField;

            private bool maxAmountFieldSpecified;

            private long multiplicityField;

            private bool multiplicityFieldSpecified;

            private bool withoutFinishDateField;

            private timestampPeriod workPeriodRangeField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            [XmlIgnore]
            public bool amountSpecified
            {
                get
                {
                    return this.amountFieldSpecified;
                }
                set
                {
                    this.amountFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string barcode
            {
                get
                {
                    return this.barcodeField;
                }
                set
                {
                    this.barcodeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool fixedAmount
            {
                get
                {
                    return this.fixedAmountField;
                }
                set
                {
                    this.fixedAmountField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long maxAmount
            {
                get
                {
                    return this.maxAmountField;
                }
                set
                {
                    this.maxAmountField = value;
                }
            }

            [XmlIgnore]
            public bool maxAmountSpecified
            {
                get
                {
                    return this.maxAmountFieldSpecified;
                }
                set
                {
                    this.maxAmountFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long multiplicity
            {
                get
                {
                    return this.multiplicityField;
                }
                set
                {
                    this.multiplicityField = value;
                }
            }

            [XmlIgnore]
            public bool multiplicitySpecified
            {
                get
                {
                    return this.multiplicityFieldSpecified;
                }
                set
                {
                    this.multiplicityFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool withoutFinishDate
            {
                get
                {
                    return this.withoutFinishDateField;
                }
                set
                {
                    this.withoutFinishDateField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public timestampPeriod workPeriodRange
            {
                get
                {
                    return this.workPeriodRangeField;
                }
                set
                {
                    this.workPeriodRangeField = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DesignerCategory("code")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        public class internalCardsVO : cardTypeVO
        {
            private bool accumulativeField;

            private bool bonusField;

            private conditionChangeVO[] conditionChangesField;

            private long creditLimitField;

            private bool creditLimitFieldSpecified;

            private bool domesticCreditField;

            private string finishField;

            private long percentageDiscountField;

            private bool percentageDiscountFieldSpecified;

            private string startField;

            private bool withoutFinishDateField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool accumulative
            {
                get
                {
                    return this.accumulativeField;
                }
                set
                {
                    this.accumulativeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool bonus
            {
                get
                {
                    return this.bonusField;
                }
                set
                {
                    this.bonusField = value;
                }
            }

            [XmlElement("conditionChanges", Form = XmlSchemaForm.Unqualified, IsNullable = true)]
            public conditionChangeVO[] conditionChanges
            {
                get
                {
                    return this.conditionChangesField;
                }
                set
                {
                    this.conditionChangesField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long creditLimit
            {
                get
                {
                    return this.creditLimitField;
                }
                set
                {
                    this.creditLimitField = value;
                }
            }

            [XmlIgnore]
            public bool creditLimitSpecified
            {
                get
                {
                    return this.creditLimitFieldSpecified;
                }
                set
                {
                    this.creditLimitFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool domesticCredit
            {
                get
                {
                    return this.domesticCreditField;
                }
                set
                {
                    this.domesticCreditField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string finish
            {
                get
                {
                    return this.finishField;
                }
                set
                {
                    this.finishField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long percentageDiscount
            {
                get
                {
                    return this.percentageDiscountField;
                }
                set
                {
                    this.percentageDiscountField = value;
                }
            }

            [XmlIgnore]
            public bool percentageDiscountSpecified
            {
                get
                {
                    return this.percentageDiscountFieldSpecified;
                }
                set
                {
                    this.percentageDiscountFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool withoutFinishDate
            {
                get
                {
                    return this.withoutFinishDateField;
                }
                set
                {
                    this.withoutFinishDateField = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DesignerCategory("code")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        public class timestampPeriod
        {
            private DateTime finishField;

            private bool finishFieldSpecified;

            private DateTime startField;

            private bool startFieldSpecified;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime finish
            {
                get
                {
                    return this.finishField;
                }
                set
                {
                    this.finishField = value;
                }
            }

            [XmlIgnore]
            public bool finishSpecified
            {
                get
                {
                    return this.finishFieldSpecified;
                }
                set
                {
                    this.finishFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public DateTime start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            [XmlIgnore]
            public bool startSpecified
            {
                get
                {
                    return this.startFieldSpecified;
                }
                set
                {
                    this.startFieldSpecified = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        public class conditionChangeVO
        {
            private long amountField;

            private internalCardsVO cardCategoryField;

            private bool changeCardField;

            private long idField;

            private bool idFieldSpecified;

            private EnumConst.intervalType intervalTypeField;

            private bool intervalTypeFieldSpecified;

            private int monthField;

            private bool monthFieldSpecified;

            private internalCardsVO newCardCategoryField;

            private bool oneCheckField;

            private string operatorMessageField;

            private EnumConst.timeRange timeRangeField;

            private bool timeRangeFieldSpecified;

            private EnumConst.typeOfComparison typeOfComparisonField;

            private bool typeOfComparisonFieldSpecified;

            private timestampPeriod workPeriodRangeField;

            private int worksPeriodField;

            private bool worksPeriodFieldSpecified;

            private EnumConst.periodType worksPeriodTypeField;

            private bool worksPeriodTypeFieldSpecified;

            private int yearField;

            private bool yearFieldSpecified;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long amount
            {
                get
                {
                    return this.amountField;
                }
                set
                {
                    this.amountField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public internalCardsVO cardCategory
            {
                get
                {
                    return this.cardCategoryField;
                }
                set
                {
                    this.cardCategoryField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool changeCard
            {
                get
                {
                    return this.changeCardField;
                }
                set
                {
                    this.changeCardField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            [XmlIgnore]
            public bool idSpecified
            {
                get
                {
                    return this.idFieldSpecified;
                }
                set
                {
                    this.idFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.intervalType intervalType
            {
                get
                {
                    return this.intervalTypeField;
                }
                set
                {
                    this.intervalTypeField = value;
                }
            }

            [XmlIgnore]
            public bool intervalTypeSpecified
            {
                get
                {
                    return this.intervalTypeFieldSpecified;
                }
                set
                {
                    this.intervalTypeFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int month
            {
                get
                {
                    return this.monthField;
                }
                set
                {
                    this.monthField = value;
                }
            }

            [XmlIgnore]
            public bool monthSpecified
            {
                get
                {
                    return this.monthFieldSpecified;
                }
                set
                {
                    this.monthFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public internalCardsVO newCardCategory
            {
                get
                {
                    return this.newCardCategoryField;
                }
                set
                {
                    this.newCardCategoryField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool oneCheck
            {
                get
                {
                    return this.oneCheckField;
                }
                set
                {
                    this.oneCheckField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string operatorMessage
            {
                get
                {
                    return this.operatorMessageField;
                }
                set
                {
                    this.operatorMessageField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.timeRange timeRange
            {
                get
                {
                    return this.timeRangeField;
                }
                set
                {
                    this.timeRangeField = value;
                }
            }

            [XmlIgnore]
            public bool timeRangeSpecified
            {
                get
                {
                    return this.timeRangeFieldSpecified;
                }
                set
                {
                    this.timeRangeFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.typeOfComparison typeOfComparison
            {
                get
                {
                    return this.typeOfComparisonField;
                }
                set
                {
                    this.typeOfComparisonField = value;
                }
            }

            [XmlIgnore]
            public bool typeOfComparisonSpecified
            {
                get
                {
                    return this.typeOfComparisonFieldSpecified;
                }
                set
                {
                    this.typeOfComparisonFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public timestampPeriod workPeriodRange
            {
                get
                {
                    return this.workPeriodRangeField;
                }
                set
                {
                    this.workPeriodRangeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int worksPeriod
            {
                get
                {
                    return this.worksPeriodField;
                }
                set
                {
                    this.worksPeriodField = value;
                }
            }

            [XmlIgnore]
            public bool worksPeriodSpecified
            {
                get
                {
                    return this.worksPeriodFieldSpecified;
                }
                set
                {
                    this.worksPeriodFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public EnumConst.periodType worksPeriodType
            {
                get
                {
                    return this.worksPeriodTypeField;
                }
                set
                {
                    this.worksPeriodTypeField = value;
                }
            }

            [XmlIgnore]
            public bool worksPeriodTypeSpecified
            {
                get
                {
                    return this.worksPeriodTypeFieldSpecified;
                }
                set
                {
                    this.worksPeriodTypeFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public int year
            {
                get
                {
                    return this.yearField;
                }
                set
                {
                    this.yearField = value;
                }
            }

            [XmlIgnore]
            public bool yearSpecified
            {
                get
                {
                    return this.yearFieldSpecified;
                }
                set
                {
                    this.yearFieldSpecified = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = ServiceConst.targetNamespace)]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        public class cardRangeVO : baseVO
        {
            private cardTypeVO cardTypeField;

            private long countField;

            private bool countFieldSpecified;

            private bool deletedField;

            private bool deletedFieldSpecified;

            private string finishField;

            private long guidField;

            private bool guidFieldSpecified;

            private string startField;

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public cardTypeVO cardType
            {
                get
                {
                    return this.cardTypeField;
                }
                set
                {
                    this.cardTypeField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long count
            {
                get
                {
                    return this.countField;
                }
                set
                {
                    this.countField = value;
                }
            }

            [XmlIgnore]
            public bool countSpecified
            {
                get
                {
                    return this.countFieldSpecified;
                }
                set
                {
                    this.countFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public bool deleted
            {
                get
                {
                    return this.deletedField;
                }
                set
                {
                    this.deletedField = value;
                }
            }

            [XmlIgnore]
            public bool deletedSpecified
            {
                get
                {
                    return this.deletedFieldSpecified;
                }
                set
                {
                    this.deletedFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string finish
            {
                get
                {
                    return this.finishField;
                }
                set
                {
                    this.finishField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public long guid
            {
                get
                {
                    return this.guidField;
                }
                set
                {
                    this.guidField = value;
                }
            }

            [XmlIgnore]
            public bool guidSpecified
            {
                get
                {
                    return this.guidFieldSpecified;
                }
                set
                {
                    this.guidFieldSpecified = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }
        }

        [Serializable]
        [XmlType(Namespace = "http://classes.ejb.users.setretailx.crystals.ru/")]
        [GeneratedCode("System.Xml", "4.6.81.0")]
        [DebuggerStepThrough]
        [DesignerCategory("code")]
        public class login
        {
            private string login1Field;

            private string passwordField;

            private string moduleNameField;

            [XmlElement("login", Form = XmlSchemaForm.Unqualified)]
            public string login1
            {
                get
                {
                    return this.login1Field;
                }
                set
                {
                    this.login1Field = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string password
            {
                get
                {
                    return this.passwordField;
                }
                set
                {
                    this.passwordField = value;
                }
            }

            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string moduleName
            {
                get
                {
                    return this.moduleNameField;
                }
                set
                {
                    this.moduleNameField = value;
                }
            }
        }

        //---------------------------------------------------------------------------------
        //Константы
        //---------------------------------------------------------------------------------
        public class EnumConst
        {
            public enum sex
            {
                Male,
                Female,
                Not_Specified
            }
            public enum cardStatus
            {
                Create,
                Active,
                Blocked,
                Inactive,
                Used,
                PreActive,
                PreUsed,
                PreDeactive
            }
            public enum cardTypes
            {
                CardNotFound,
                InternalCard,
                ExternalCard,
                PresentCard,
                BonusCard,
                CardCoupon,
                ChequeCoupon,
                ProcessingCoupon
            }
            public enum cardBlockingReason
            {
                ANOTHER,
                LOSS,
                REFUSAL,
                ROBBERY,
                CALL_SECURITY
            }
            public enum cardActions
            {
                Create,
                Activate,
                Blocked,
                Used,
                Changed,
                Inactive,
                UNDO_USE
            }
            public enum periodType
            {
                NOW,
                MINUTES,
                HOURS,
                DAYS,
                WEEKS,
                MONTHES,
                YEARS
            }
            public enum timeRange
            {
                UPTO,
                WITHIN
            }
            public enum intervalType
            {
                ABSOLUTE,
                RELATIVE
            }
            public enum typeOfComparison
            {
                MORE,
                LESS,
                EQUAL,
                MOREOREQUAL,
                LESSOREQUAL
            }

        }
        public class ServiceConst
        {
            public const string targetNamespace = "http://cardsmanager.cards.poststudio.ru/";

        }
        public class LoginConnection
        {
            public const string clogin = "cto_crystall";
            public const string cpassport = "password";
            public const string cmodule = "Cards";
        }
        public static class SessionPP
        {
            [ThreadStatic]
            public static X509Certificate2 Certificate;
            [ThreadStatic]
            public static string TokenSerialNumber;

        }
    }

}
