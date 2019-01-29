using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoyaltyAPI;
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Globalization;
using System.Net.Security;
using System.Text;
using System.IO;
using PPweb.Models.StorePP;
using PPweb.Models;
using System.Threading;

namespace PPweb.Utils
{
    public class LoyaltyService
    {
        public static class CEnvironment
        {
            private const int _DEFAULT_REQUEST_RETRY = 10;

            private static Dictionary<string, InputLanguage> cacheInputLanguage = new Dictionary<string, InputLanguage>();

            public static string SessionID
            {
                get;
                set;
            }

            public static bool Connected
            {
                get;
                set;
            }

            public static string ClientID
            {
                get;
                set;
            }

            public static X509Certificate2 Certificate
            {
                get;
                set;
            }


            public static bool SSLEnable
            {
                get
                {
                    return true;
                }
            }

            public static int RequestRetry
            {
                get
                {
                    if (ConfigurationManager.AppSettings["RequestRetry"] != null)
                    {
                        int result = 10;
                        int.TryParse(ConfigurationManager.AppSettings["RequestRetry"], out result);
                        return result;
                    }
                    return 10;
                }
            }


            public static void changeToUSLanguage(string key)
            {
                try
                {
                    if (cacheInputLanguage.ContainsKey(key))
                    {
                        cacheInputLanguage[key] = InputLanguage.CurrentInputLanguage;
                    }
                    else
                    {
                        cacheInputLanguage.Add(key, InputLanguage.CurrentInputLanguage);
                    }
                    InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new CultureInfo("en-US"));
                }
                catch
                {
                }
            }

            public static void restoreLanguage(string key)
            {
                try
                {
                    if (cacheInputLanguage.ContainsKey(key))
                    {
                        InputLanguage.CurrentInputLanguage = cacheInputLanguage[key];
                    }
                }
                catch
                {
                }
            }
            public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }
        }
        public class RequestGenerator
        {
            private HttpWebRequest m_oSOAPRequest = null;
            private string m_sURL = string.Empty;
            [STAThread]
            private DataSet SendRequest(string request)
            {
                Logger.InitLogger();
                DataSet dataSet = new DataSet();
                GetSRUrl();
                string _certError = string.Empty;
                ReadToken _readToken = new ReadToken();
                for (int i = 0; i < CEnvironment.RequestRetry; i++)
                {
                    m_oSOAPRequest = (HttpWebRequest)WebRequest.Create(m_sURL);

                    m_oSOAPRequest.ClientCertificates.Add(_readToken.ReadCertFromStore(out _certError));
                    Logger.Log.Info(_certError);
                    if (m_oSOAPRequest.ClientCertificates != null && m_oSOAPRequest.ClientCertificates.Count > 0)
                    {
                        try
                        {
                            m_oSOAPRequest.AllowAutoRedirect = true;
                            m_oSOAPRequest.KeepAlive = true;
                            m_oSOAPRequest.Method = "POST";
                            m_oSOAPRequest.ContentType = "text/xml; charset=utf-8";
                            if (CEnvironment.SSLEnable)
                            {
                                m_oSOAPRequest.Credentials = CredentialCache.DefaultCredentials;
                                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                                m_oSOAPRequest.ProtocolVersion = HttpVersion.Version11;
                                ServicePointManager.ServerCertificateValidationCallback += CEnvironment.ValidateServerCertificate;
                            }
                            byte[] bytes = Encoding.UTF8.GetBytes(request);
                            Stream requestStream = m_oSOAPRequest.GetRequestStream();
                            requestStream.Write(bytes, 0, bytes.Length);
                            requestStream.Flush();
                            requestStream.Close();

                            Logger.Log.Info(i.ToString() + " - Формирование Запроса");

                            HttpWebResponse httpWebResponse = (HttpWebResponse)m_oSOAPRequest.GetResponse();
                            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), true);
                            if (streamReader != null)
                            {
                                dataSet.ReadXml(streamReader);
                            }
                            try
                            {
                                dataSet.WriteXml("response.xml");
                            }
                            catch
                            {
                                Logger.Log.Info("Ответа нет!");
                            }
                        }
                        catch (Exception ex)
                        {
                            //ServiceProcedure.log.Error(ex.Message);
                            if (ex.InnerException != null)
                            {
                                //ServiceProcedure.log.Error(ex.InnerException.Message);
                            }
                            if (i >= CEnvironment.RequestRetry - 1)
                            {
                                //throw ex;
                            }
                            continue;
                        }
                        finally
                        {
                            ServicePointManager.ServerCertificateValidationCallback = null;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        }
                        break;
                    }
                    else
                    {
                        dataSet = new DataSet();
                        break;
                    }
                }
                Logger.Log.Info(dataSet.Tables.Count.ToString());
                return dataSet;
            }
            public DataSet addClientAndSetToCard(string cardNumber, CClientCard.CClient client)
            {
                client.BirthDate = ServiceProcedure.DateToServerFormat(client.BirthDate);
                client.Passport.DeliveryDate = ServiceProcedure.DateToServerFormat(client.Passport.DeliveryDate);
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{29}\">\r\n   "
                    + "<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <car:addClientAndSetToCard>\r\n         <!--Optional:-->\r\n "
                    + "<sessionId></sessionId>\r\n         <!--Optional:-->\r\n         <card>\r\n            <!--Optional:-->\r\n  "
                    + "<id></id>\r\n            <!--Optional:-->\r\n            <accountName></accountName>\r\n            <!--Optional:-->\r\n  "
                    + "<accountNumber></accountNumber>\r\n            <!--Optional:-->\r\n            <activationDate></activationDate>\r\n "
                    + "<!--Optional:-->\r\n            <amount></amount>\r\n            <!--Optional:-->\r\n            <barcode></barcode>\r\n "
                    + "<!--Optional:-->\r\n            <blockDate></blockDate>\r\n            <blocked>false</blocked>\r\n            <!--Optional:-->\r\n  "
                    + "<cardTypeVO>\r\n               <!--Optional:-->\r\n               <id></id>\r\n               <!--Optional:-->\r\n   "
                    + "<classType></classType>\r\n               <!--Optional:-->\r\n               <color>\r\n                  <blue></blue>\r\n  "
                    + "<green></green>\r\n                  <red></red>\r\n               </color>\r\n               <!--Optional:-->\r\n   "
                    + "<deleted>false</deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n   "
                    + "<name></name>\r\n               <personalized>true</personalized>\r\n            </cardTypeVO>\r\n            <!--Optional:-->\r\n    "
                    + "<clientVO>\r\n               <!--Optional:-->\r\n               <id></id>\r\n               <!--Optional:-->\r\n "
                    + "<auto></auto>\r\n               <!--Optional:-->\r\n               <birthDate></birthDate>\r\n               <!--Optional:-->\r\n    "
                    + "<childrenAge></childrenAge>\r\n               <!--Optional:-->\r\n               <clientAddress>\r\n                  <!--Optional:-->\r\n   "
                    + "<appartment></appartment>\r\n                  <!--Optional:-->\r\n                  <building></building>\r\n   "
                    + "<!--Optional:-->\r\n                  <city></city>\r\n                  <!--Optional:-->\r\n                  <district></district>\r\n "
                    + "<!--Optional:-->\r\n                  <districtArea></districtArea>\r\n                  <!--Optional:-->\r\n    "
                    + "<house></house>\r\n                  <!--Optional:-->\r\n                  <other></other>\r\n                  <!--Optional:-->\r\n "
                    + "<region></region>\r\n                  <!--Optional:-->\r\n                  <street></street>\r\n                  <!--Optional:-->\r\n "
                    + "<zip></zip>\r\n               </clientAddress>\r\n               <!--Optional:-->\r\n               <creationDate></creationDate>\r\n    "
                    + "<!--Optional:-->\r\n               <deleted></deleted>\r\n               <!--Optional:-->\r\n               <email></email>\r\n  "
                    + "<!--Optional:-->\r\n               <firstName></firstName>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n    "
                    + "<!--Optional:-->\r\n               <isCompleted></isCompleted>\r\n               <!--Optional:-->\r\n    "
                    + "<lastChangeDate></lastChangeDate>\r\n               <!--Optional:-->\r\n               <lastName></lastName>\r\n               <!--Optional:-->\r\n  "
                    + "<marital></marital>\r\n               <!--Optional:-->\r\n               <middleName></middleName>\r\n               <!--Optional:-->\r\n    "
                    + "<mobileOperator></mobileOperator>\r\n               <!--Optional:-->\r\n               <mobilePhone></mobilePhone>\r\n   "
                    + "<!--Optional:-->\r\n               <passport>\r\n                  <!--Optional:-->\r\n                  <delivery></delivery>\r\n   "
                    + "<!--Optional:-->\r\n                  <deliveryDate></deliveryDate>\r\n                  <!--Optional:-->\r\n    "
                    + "<departmentCode></departmentCode>\r\n                  <!--Optional:-->\r\n                  <passNumber></passNumber>\r\n   "
                    + "<!--Optional:-->\r\n                  <passSerie></passSerie>\r\n               </passport>\r\n               <!--Optional:-->\r\n   "
                    + "<phone></phone>\r\n               <!--Optional:-->\r\n               <sendBy>\r\n                  <byEMail></byEMail>\r\n   "
                    + "<byMail></byMail>\r\n                  <byPhone></byPhone>\r\n                  <bySMS></bySMS>\r\n               </sendBy>\r\n  "
                    + "<sendCatalog></sendCatalog>\r\n               <!--Optional:-->\r\n               <sex></sex>\r\n               <!--Optional:-->\r\n  "
                    + "<shopNumber></shopNumber>\r\n            </clientVO>\r\n            <!--Optional:-->\r\n            <createDate></createDate>\r\n    "
                    + "<!--Optional:-->\r\n            <deleted>false</deleted>\r\n            <!--Optional:-->\r\n            <expirationDate></expirationDate>\r\n    "
                    + "<!--Optional:-->\r\n            <guid></guid>\r\n            <!--Optional:-->\r\n            <mainCard/>\r\n            <!--Optional:-->\r\n "
                    + "<newCardNumber></newCardNumber>\r\n            <!--Optional:-->\r\n            <newCardType>\r\n               <!--Optional:-->\r\n  "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n               <!--Optional:-->\r\n    "
                    + "<color>\r\n                  <blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n               </color>\r\n  "
                    + "<!--Optional:-->\r\n               <deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n "
                    + "<name></name>\r\n               <personalized></personalized>\r\n            </newCardType>\r\n            <!--Optional:-->\r\n  "
                    + "<number>{0}</number>\r\n            <!--Optional:-->\r\n            <oldCardNumber></oldCardNumber>\r\n  "
                    + "<signChangeOfCategoryCard></signChangeOfCategoryCard>\r\n            <signExtensionCard></signExtensionCard>\r\n            <!--Optional:-->\r\n "
                    + "<status></status>\r\n            <!--Optional:-->\r\n            <statusDescription></statusDescription>\r\n         </card>\r\n         <!--Optional:-->\r\n    "
                    + "<client>\r\n            <!--Optional:-->\r\n            <id></id>\r\n            <!--Optional:-->\r\n            <auto></auto>\r\n   "
                    + "<!--Optional:-->\r\n            <birthDate>{1}</birthDate>\r\n            <!--Optional:-->\r\n            <childrenAge></childrenAge>\r\n    "
                    + "<!--Optional:-->\r\n            <clientAddress>\r\n               <!--Optional:-->\r\n               <appartment>{2}</appartment>\r\n    "
                    + "<!--Optional:-->\r\n               <building>{3}</building>\r\n               <!--Optional:-->\r\n               <city>{4}</city>\r\n    "
                    + "<!--Optional:-->\r\n               <district>{5}</district>\r\n               <!--Optional:-->\r\n               <districtArea>{6}</districtArea>\r\n    "
                    + "<!--Optional:-->\r\n               <house>{7}</house>\r\n               <!--Optional:-->\r\n               <other>{28}</other>\r\n   "
                    + "<!--Optional:-->\r\n               <region>{8}</region>\r\n               <!--Optional:-->\r\n               <street>{9}</street>\r\n    "
                    + "<!--Optional:-->\r\n               <zip>{10}</zip>\r\n            </clientAddress>\r\n            <!--Optional:-->\r\n            <creationDate></creationDate>\r\n  "
                    + "<!--Optional:-->\r\n            <deleted>false</deleted>\r\n            <!--Optional:-->\r\n            <email>{22}</email>\r\n            <!--Optional:-->\r\n  "
                    + "<firstName>{11}</firstName>\r\n            <!--Optional:-->\r\n            <guid></guid>\r\n            <!--Optional:-->\r\n "
                    + "<isCompleted>true</isCompleted>\r\n            <!--Optional:-->\r\n            <lastChangeDate></lastChangeDate>\r\n            <!--Optional:-->\r\n "
                    + "<lastName>{12}</lastName>\r\n            <!--Optional:-->\r\n            <marital></marital>\r\n            <!--Optional:-->\r\n "
                    + "<middleName>{13}</middleName>\r\n            <!--Optional:-->\r\n            <mobileOperator>{24}</mobileOperator>\r\n            <!--Optional:-->\r\n   "
                    + "<mobilePhone>{14}</mobilePhone>\r\n            <!--Optional:-->\r\n            <passport>\r\n               <!--Optional:-->\r\n "
                    + "<delivery>{15}</delivery>\r\n               <!--Optional:-->\r\n               <deliveryDate>{16}</deliveryDate>\r\n               <!--Optional:-->\r\n  "
                    + "<departmentCode>{17}</departmentCode>\r\n               <!--Optional:-->\r\n               <passNumber>{18}</passNumber>\r\n               <!--Optional:-->\r\n  "
                    + "<passSerie>{19}</passSerie>\r\n            </passport>\r\n            <!--Optional:-->\r\n            <phone>{20}</phone>\r\n    "
                    + "<receivePension>0</receivePension>\r\n            <!--Optional:-->\r\n            <sendBy>\r\n               <byEMail></byEMail>\r\n "
                    + "<byMail></byMail>\r\n               <byPhone></byPhone>\r\n               <bySMS></bySMS>\r\n            </sendBy>\r\n            <sendCatalog></sendCatalog>\r\n "
                    + "<!--Optional:-->\r\n            <sex>{21}</sex>\r\n            <!--Optional:-->\r\n            <shopNumber>{23}</shopNumber>\r\n "
                    + "<receivePension>{25}</receivePension>\r\n            <fiscalNumber>{26}</fiscalNumber>\r\n            <snilsNumber>{27}</snilsNumber>\r\n         </client>\r\n  "
                    + "</car:addClientAndSetToCard>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>\r\n ", cardNumber/*0*/, client.BirthDate/*1*/, client.ClientAddress.Appartment/*2*/, client.ClientAddress.Building/*3*/,
                    client.ClientAddress.City/*4*/, client.ClientAddress.District/*5*/, client.ClientAddress.DistrictArea/*6*/, client.ClientAddress.House/*7*/, client.ClientAddress.Region/*8*/, client.ClientAddress.Street/*9*/, client.ClientAddress.Zip/*10*/,
                    client.FirstName/*11*/, client.LastName/*12*/, client.MiddleName/*13*/, client.MobilePhone/*14*/, client.Passport.Delivery/*15*/, client.Passport.DeliveryDate/*16*/, client.Passport.DepartmentCode/*17*/, client.Passport.Number/*18*/,
                    client.Passport.Serie/*19*/, client.Phone/*20*/, client.Sex/*21*/, client.EMail/*22*/, client.ShopNumber/*23*/, client.OperatorName/*24*/, client.ReceivePension/*25*/, client.FiscalNumber/*26*/, client.SnilsNumber/*27*/, client.ClientAddress.Other/*28*/, ServiceConst.targetNamespace/*29*/);
                return this.SendRequest(request);
            }
            public DataSet blockCard(string cardNumber, CClientCard.CCardBlockingReason reason, string comment, string OperName, string ShopId)
            {
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{3}\">\r\n    "
                    + "<soapenv:Header/>\r\n   <soapenv:Body>\r\n      <car:blockCard>\r\n         <!--Optional:-->\r\n         <sessionId></sessionId>\r\n "
                    + "<!--Optional:-->\r\n         <card>\r\n            <!--Optional:-->\r\n            <id></id>\r\n            <!--Optional:-->\r\n "
                    + "<accountName></accountName>\r\n            <!--Optional:-->\r\n            <accountNumber></accountNumber>\r\n   "
                    + "<!--Optional:-->\r\n            <activationDate></activationDate>\r\n            <!--Optional:-->\r\n            <amount></amount>\r\n   "
                    + "<!--Optional:-->\r\n            <barcode></barcode>\r\n            <!--Optional:-->\r\n            <blockDate></blockDate>\r\n   "
                    + "<blocked></blocked>\r\n            <!--Optional:-->\r\n            <cardTypeVO>\r\n               <!--Optional:-->\r\n "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n               <!--Optional:-->\r\n    "
                    + "<color>\r\n                  <blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n "
                    + "</color>\r\n               <!--Optional:-->\r\n               <deleted></deleted>\r\n               <!--Optional:-->\r\n "
                    + "<guid></guid>\r\n               <!--Optional:-->\r\n               <name></name>\r\n               <personalized> </personalized>\r\n    "
                    + "</cardTypeVO>\r\n            <!--Optional:-->\r\n            <clientVO>\r\n               <!--Optional:-->\r\n               <id></id>\r\n   "
                    + "<!--Optional:-->\r\n               <auto></auto>\r\n               <!--Optional:-->\r\n               <birthDate></birthDate>\r\n    "
                    + "<!--Optional:-->\r\n               <childrenAge></childrenAge>\r\n               <!--Optional:-->\r\n               <clientAddress>\r\n  "
                    + "<!--Optional:-->\r\n                  <appartment></appartment>\r\n                  <!--Optional:-->\r\n                  <building></building>\r\n "
                    + "<!--Optional:-->\r\n                  <city></city>\r\n                  <!--Optional:-->\r\n                  <district></district>\r\n "
                    + "<!--Optional:-->\r\n                  <districtArea></districtArea>\r\n                  <!--Optional:-->\r\n                  <house></house>\r\n "
                    + "<!--Optional:-->\r\n                  <other></other>\r\n                  <!--Optional:-->\r\n                  <region></region>\r\n "
                    + "<!--Optional:-->\r\n                  <street></street>\r\n                  <!--Optional:-->\r\n                  <zip></zip>\r\n   "
                    + "</clientAddress>\r\n               <!--Optional:-->\r\n               <creationDate></creationDate>\r\n               <!--Optional:-->\r\n   "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <email></email>\r\n               <!--Optional:-->\r\n  "
                    + "<firstName></firstName>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n    "
                    + "<isCompleted></isCompleted>\r\n               <!--Optional:-->\r\n               <lastChangeDate></lastChangeDate>\r\n   "
                    + "<!--Optional:-->\r\n               <lastName></lastName>\r\n               <!--Optional:-->\r\n               <marital> </marital>\r\n"
                    + "<!--Optional:-->\r\n               <middleName></middleName>\r\n               <!--Optional:-->\r\n               <mobileOperator>{4}</mobileOperator>\r\n  "
                    + "<!--Optional:-->\r\n               <mobilePhone></mobilePhone>\r\n               <!--Optional:-->\r\n               <passport>\r\n   "
                    + "<!--Optional:-->\r\n                  <delivery></delivery>\r\n                  <!--Optional:-->\r\n                  <deliveryDate></deliveryDate>\r\n "
                    + "<!--Optional:-->\r\n                  <departmentCode></departmentCode>\r\n                  <!--Optional:-->\r\n                  <passNumber></passNumber>\r\n "
                    + "<!--Optional:-->\r\n                  <passSerie></passSerie>\r\n               </passport>\r\n               <!--Optional:-->\r\n               <phone></phone>\r\n "
                    + "<!--Optional:-->\r\n               <sendBy>\r\n                  <byEMail></byEMail>\r\n                  <byMail></byMail>\r\n  "
                    + "<byPhone></byPhone>\r\n                  <bySMS></bySMS>\r\n               </sendBy>\r\n               <sendCatalog></sendCatalog>\r\n   "
                    + "<!--Optional:-->\r\n               <sex> </sex>\r\n               <!--Optional:-->\r\n               <shopNumber>{5}</shopNumber>\r\n            </clientVO>\r\n    "
                    + "<!--Optional:-->\r\n            <createDate></createDate>\r\n            <!--Optional:-->\r\n            <deleted></deleted>\r\n            <!--Optional:-->\r\n "
                    + "<expirationDate></expirationDate>\r\n            <!--Optional:-->\r\n            <guid></guid>\r\n            <!--Optional:-->\r\n            <mainCard/>\r\n "
                    + "<!--Optional:-->\r\n            <newCardNumber></newCardNumber>\r\n            <!--Optional:-->\r\n            <newCardType>\r\n               <!--Optional:-->\r\n  "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n               <!--Optional:-->\r\n               <color>\r\n  "
                    + "<blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n               </color>\r\n               <!--Optional:-->\r\n "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n               <name></name>\r\n    "
                    + "<personalized></personalized>\r\n            </newCardType>\r\n            <!--Optional:-->\r\n            <number>{0}</number>\r\n            <!--Optional:-->\r\n  "
                    + "<oldCardNumber></oldCardNumber>\r\n            <signChangeOfCategoryCard></signChangeOfCategoryCard>\r\n            <signExtensionCard></signExtensionCard>\r\n  "
                    + "<!--Optional:-->\r\n            <status></status>\r\n            <!--Optional:-->\r\n            <statusDescription></statusDescription>\r\n         </card>\r\n "
                    + "<!--Optional:-->\r\n         <cause>{1}</cause>\r\n         <!--Optional:-->\r\n         <comment>{2}</comment>\r\n         <!--Optional:-->\r\n         <user>\r\n  "
                    + "<blockedStatus></blockedStatus>\r\n            <!--Optional:-->\r\n            <email></email>\r\n            <!--Optional:-->\r\n            <firstName></firstName>\r\n    "
                    + "<!--Optional:-->\r\n            <id></id>\r\n            <!--Optional:-->\r\n            <lastName></lastName>\r\n            <!--Optional:-->\r\n   "
                    + "<login></login>\r\n            <!--Optional:-->\r\n            <middleName></middleName>\r\n            <!--Optional:-->\r\n            <password></password>\r\n    "
                    + "<!--Optional:-->\r\n            <position></position>\r\n            <!--Zero or more repetitions:-->\r\n            <userRoles>\r\n               <centrum></centrum>\r\n   "
                    + "<!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n               <id></id>\r\n               <!--Zero or more repetitions:-->\r\n   "
                    + "<privilegeVOList>\r\n                  <!--Optional:-->\r\n                  <description></description>\r\n                  <!--Optional:-->\r\n   "
                    + "<id></id>\r\n                  <!--Optional:-->\r\n                  <moduleDescription></moduleDescription>\r\n                  <!--Optional:-->\r\n   "
                    + "<moduleName></moduleName>\r\n                  <!--Optional:-->\r\n                  <name></name>\r\n               </privilegeVOList>\r\n               <!--Optional:-->\r\n   "
                    + "<roleName></roleName>\r\n            </userRoles>\r\n         </user>\r\n      </car:blockCard>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>"
                    , cardNumber/*0*/, reason.Value/*1*/, comment/*2*/, ServiceConst.targetNamespace/*3*/, OperName/*4*/, ShopId/*5*/);
                return this.SendRequest(request);
            }
            public DataSet changeCard(string oldCardID, string oldCardNumber, string newCardNumber, string operatorName, string opsIndex)
            {
                object[] array = new object[7]
                {
                    oldCardID,
                    newCardNumber,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                object[] array2 = array;
                DateTime now = DateTime.Now;
                string str = now.ToShortDateString();
                now = DateTime.Now;
                array[2] = str + " " + now.ToLongTimeString();
                array[3] = ServiceConst.targetNamespace;
                array[4] = oldCardNumber;
                array[5] = operatorName;
                array[6] = opsIndex;
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{3}\">\r\n   <soapenv:Header/>\r\n    "
                    + "<soapenv:Body>\r\n      <car:changeInternalCard>\r\n         <!--Optional:-->\r\n         <sessionId></sessionId>\r\n         <!--Optional:-->\r\n   "
                    + "<oldCard>\r\n            <!--Optional:-->\r\n            <id>{0}</id>\r\n            <!--Optional:-->\r\n            <accountName></accountName>\r\n "
                    + "<!--Optional:-->\r\n            <accountNumber></accountNumber>\r\n            <!--Optional:-->\r\n            <activationDate></activationDate>\r\n "
                    + "<!--Optional:-->\r\n            <amount></amount>\r\n            <!--Optional:-->\r\n            <barcode></barcode>\r\n            <!--Optional:-->\r\n "
                    + "<blockDate></blockDate>\r\n            <blocked></blocked>\r\n            <!--Optional:-->\r\n            <cardTypeVO>\r\n               <!--Optional:-->\r\n    "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n               <!--Optional:-->\r\n               <color>\r\n  "
                    + "<blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n               </color>\r\n               <!--Optional:-->\r\n    "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n               <name></name>\r\n    "
                    + "<personalized></personalized>\r\n            </cardTypeVO>\r\n            <!--Optional:-->\r\n            <clientVO>\r\n               <!--Optional:-->\r\n  "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <auto></auto>\r\n               <!--Optional:-->\r\n               <birthDate></birthDate>\r\n    "
                    + "<!--Optional:-->\r\n               <childrenAge></childrenAge>\r\n               <!--Optional:-->\r\n               <clientAddress>\r\n  "
                    + "<!--Optional:-->\r\n                  <appartment></appartment>\r\n                  <!--Optional:-->\r\n                  <building></building>\r\n "
                    + "<!--Optional:-->\r\n                  <city></city>\r\n                  <!--Optional:-->\r\n                  <district></district>\r\n "
                    + "<!--Optional:-->\r\n                  <districtArea></districtArea>\r\n                  <!--Optional:-->\r\n                  <house></house>\r\n   "
                    + "<!--Optional:-->\r\n                  <other></other>\r\n                  <!--Optional:-->\r\n                  <region></region>\r\n   "
                    + "<!--Optional:-->\r\n                  <street></street>\r\n                  <!--Optional:-->\r\n                  <zip></zip>\r\n   "
                    + "</clientAddress>\r\n               <!--Optional:-->\r\n               <creationDate></creationDate>\r\n               <!--Optional:-->\r\n   "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <email></email>\r\n               <!--Optional:-->\r\n  "
                    + "<firstName></firstName>\r\n               <!--Optional:-->\r\n               <fiscalNumber></fiscalNumber>\r\n               <!--Optional:-->\r\n    "
                    + "<guid></guid>\r\n               <!--Optional:-->\r\n               <isCompleted></isCompleted>\r\n               <!--Optional:-->\r\n    "
                    + "<lastChangeDate></lastChangeDate>\r\n               <!--Optional:-->\r\n               <lastName></lastName>\r\n               <!--Optional:-->\r\n  "
                    + "<marital></marital>\r\n               <!--Optional:-->\r\n               <middleName></middleName>\r\n               <!--Optional:-->\r\n    "
                    + "<mobileOperator></mobileOperator>\r\n               <!--Optional:-->\r\n               <mobilePhone></mobilePhone>\r\n               <!--Optional:-->\r\n    "
                    + "<passport>\r\n                  <!--Optional:-->\r\n                  <delivery></delivery>\r\n                  <!--Optional:-->\r\n    "
                    + "<deliveryDate></deliveryDate>\r\n                  <!--Optional:-->\r\n                  <departmentCode></departmentCode>\r\n   "
                    + "<!--Optional:-->\r\n                  <passNumber></passNumber>\r\n                  <!--Optional:-->\r\n                  <passSerie></passSerie>\r\n   "
                    + "</passport>\r\n               <!--Optional:-->\r\n               <phone></phone>\r\n               <receivePension></receivePension>\r\n "
                    + "<!--Optional:-->\r\n               <sendBy>\r\n                  <byEMail></byEMail>\r\n                  <byMail></byMail>\r\n  "
                    + "<byPhone></byPhone>\r\n                  <bySMS></bySMS>\r\n               </sendBy>\r\n               <sendCatalog></sendCatalog>\r\n   "
                    + "<!--Optional:-->\r\n               <sex></sex>\r\n               <!--Optional:-->\r\n               <shopNumber></shopNumber>\r\n    "
                    + "<!--Optional:-->\r\n               <snilsNumber></snilsNumber>\r\n            </clientVO>\r\n            <!--Optional:-->\r\n    "
                    + "<createDate></createDate>\r\n            <!--Optional:-->\r\n            <deleted></deleted>\r\n            <!--Optional:-->\r\n "
                    + "<expirationDate></expirationDate>\r\n            <!--Optional:-->\r\n            <guid></guid>\r\n            <!--Optional:-->\r\n   "
                    + "<mainCard/>\r\n            <!--Optional:-->\r\n            <newCardNumber></newCardNumber>\r\n            <!--Optional:-->\r\n   "
                    + "<newCardType>\r\n               <!--Optional:-->\r\n               <id></id>\r\n               <!--Optional:-->\r\n  "
                    + "<classType></classType>\r\n               <!--Optional:-->\r\n               <color>\r\n                  <blue></blue>\r\n  "
                    + "<green></green>\r\n                  <red></red>\r\n               </color>\r\n               <!--Optional:-->\r\n   "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n    "
                    + "<name></name>\r\n               <personalized></personalized>\r\n            </newCardType>\r\n            <!--Optional:-->\r\n  "
                    + "<number>{4}</number>\r\n            <!--Optional:-->\r\n            <oldCardNumber></oldCardNumber>\r\n  "
                    + "<signChangeOfCategoryCard></signChangeOfCategoryCard>\r\n            <signExtensionCard></signExtensionCard>\r\n            <!--Optional:-->\r\n "
                    + "<status></status>\r\n            <!--Optional:-->\r\n            <statusDescription></statusDescription>\r\n         </oldCard>\r\n         <!--Optional:-->\r\n "
                    + "<newCard>\r\n            <!--Optional:-->\r\n            <id></id>\r\n            <!--Optional:-->\r\n            <accountName></accountName>\r\n    "
                    + "<!--Optional:-->\r\n            <accountNumber></accountNumber>\r\n            <!--Optional:-->\r\n            <activationDate>{2}</activationDate>\r\n  "
                    + "<!--Optional:-->\r\n            <amount></amount>\r\n            <!--Optional:-->\r\n            <barcode></barcode>\r\n            <!--Optional:-->\r\n "
                    + "<blockDate></blockDate>\r\n            <blocked>false</blocked>\r\n            <!--Optional:-->\r\n            <cardTypeVO>\r\n               <!--Optional:-->\r\n   "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n               <!--Optional:-->\r\n               <color>\r\n  "
                    + "<blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n               </color>\r\n               <!--Optional:-->\r\n    "
                    + "<deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n               <name></name>\r\n    "
                    + "<personalized></personalized>\r\n            </cardTypeVO>\r\n            <!--Optional:-->\r\n            <clientVO>\r\n               <!--Optional:-->\r\n  "
                    + "<id></id>\r\n               <!--Optional:-->\r\n               <auto></auto>\r\n               <!--Optional:-->\r\n               <birthDate></birthDate>\r\n    "
                    + "<!--Optional:-->\r\n               <childrenAge></childrenAge>\r\n               <!--Optional:-->\r\n               <clientAddress>\r\n  "
                    + "<!--Optional:-->\r\n                  <appartment></appartment>\r\n                  <!--Optional:-->\r\n                  <building></building>\r\n "
                    + "<!--Optional:-->\r\n                  <city></city>\r\n                  <!--Optional:-->\r\n                  <district></district>\r\n "
                    + "<!--Optional:-->\r\n                  <districtArea></districtArea>\r\n                  <!--Optional:-->\r\n                  <house></house>\r\n   "
                    + "<!--Optional:-->\r\n                  <other></other>\r\n                  <!--Optional:-->\r\n                  <region></region>\r\n                  <!--Optional:-->\r\n "
                    + "<street></street>\r\n                  <!--Optional:-->\r\n                  <zip></zip>\r\n               </clientAddress>\r\n               <!--Optional:-->\r\n   "
                    + "<creationDate></creationDate>\r\n               <!--Optional:-->\r\n               <deleted></deleted>\r\n               <!--Optional:-->\r\n               <email></email>\r\n  "
                    + "<!--Optional:-->\r\n               <firstName></firstName>\r\n               <!--Optional:-->\r\n               <fiscalNumber></fiscalNumber>\r\n    "
                    + "<!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n               <isCompleted></isCompleted>\r\n               <!--Optional:-->\r\n "
                    + "<lastChangeDate></lastChangeDate>\r\n               <!--Optional:-->\r\n               <lastName></lastName>\r\n               <!--Optional:-->\r\n  "
                    + "<marital></marital>\r\n               <!--Optional:-->\r\n               <middleName></middleName>\r\n               <!--Optional:-->\r\n    "
                    + "<mobileOperator>{5}</mobileOperator>\r\n               <!--Optional:-->\r\n               <mobilePhone></mobilePhone>\r\n               <!--Optional:-->\r\n "
                    + "<passport>\r\n                  <!--Optional:-->\r\n                  <delivery></delivery>\r\n                  <!--Optional:-->\r\n    "
                    + "<deliveryDate></deliveryDate>\r\n                  <!--Optional:-->\r\n                  <departmentCode></departmentCode>\r\n                  <!--Optional:-->\r\n "
                    + "<passNumber></passNumber>\r\n                  <!--Optional:-->\r\n                  <passSerie></passSerie>\r\n               </passport>\r\n               <!--Optional:-->\r\n    "
                    + "<phone></phone>\r\n               <receivePension></receivePension>\r\n               <!--Optional:-->\r\n               <sendBy>\r\n                  <byEMail></byEMail>\r\n   "
                    + "<byMail></byMail>\r\n                  <byPhone></byPhone>\r\n                  <bySMS></bySMS>\r\n               </sendBy>\r\n               <sendCatalog></sendCatalog>\r\n    "
                    + "<!--Optional:-->\r\n               <sex></sex>\r\n               <!--Optional:-->\r\n               <shopNumber>{6}</shopNumber>\r\n               <!--Optional:-->\r\n  "
                    + "<snilsNumber></snilsNumber>\r\n            </clientVO>\r\n            <!--Optional:-->\r\n            <createDate></createDate>\r\n            <!--Optional:-->\r\n  "
                    + "<deleted>false</deleted>\r\n            <!--Optional:-->\r\n            <expirationDate></expirationDate>\r\n            <!--Optional:-->\r\n            <guid></guid>\r\n   "
                    + "<!--Optional:-->\r\n            <mainCard/>\r\n            <!--Optional:-->\r\n            <newCardNumber></newCardNumber>\r\n            <!--Optional:-->\r\n   "
                    + "<newCardType>\r\n               <!--Optional:-->\r\n               <id></id>\r\n               <!--Optional:-->\r\n               <classType></classType>\r\n    "
                    + "<!--Optional:-->\r\n               <color>\r\n                  <blue></blue>\r\n                  <green></green>\r\n                  <red></red>\r\n               </color>\r\n   "
                    + "<!--Optional:-->\r\n               <deleted></deleted>\r\n               <!--Optional:-->\r\n               <guid></guid>\r\n               <!--Optional:-->\r\n "
                    + "<name></name>\r\n               <personalized></personalized>\r\n            </newCardType>\r\n            <!--Optional:-->\r\n            <number>{1}</number>\r\n  "
                    + "<!--Optional:-->\r\n            <oldCardNumber></oldCardNumber>\r\n            <signChangeOfCategoryCard></signChangeOfCategoryCard>\r\n "
                    + "<signExtensionCard></signExtensionCard>\r\n            <!--Optional:-->\r\n            <status></status>\r\n            <!--Optional:-->\r\n "
                    + "<statusDescription></statusDescription>\r\n         </newCard>\r\n         <!--Optional:-->\r\n         <cause></cause>\r\n         <!--Optional:-->\r\n         <comment></comment>\r\n "
                    + "</car:changeInternalCard>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>"
                    , array);
                return this.SendRequest(request);
            }
            public DataSet getCardAction(string id)
            {
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{1}\">\r\n   <soapenv:Header/>\r\n    "
                    + "<soapenv:Body>\r\n      <car:getActionsByCard>\r\n         <!--Optional:-->\r\n         <sessionId></sessionId>\r\n         <!--Optional:-->\r\n "
                    + "<card>\r\n            <!--Optional:-->\r\n            <id>{0}</id>\r\n            <!--Optional:-->\r\n            <accountName/>\r\n "
                    + "<!--Optional:-->\r\n            <accountNumber/>\r\n            <!--Optional:-->\r\n            <activationDate/>\r\n            <!--Optional:-->\r\n    "
                    + "<amount/>\r\n            <!--Optional:-->\r\n            <barcode/>\r\n            <!--Optional:-->\r\n            <blockDate/>\r\n  "
                    + "<blocked/>\r\n            <!--Optional:-->\r\n            <cardTypeVO>\r\n               <!--Optional:-->\r\n               <id/>\r\n    "
                    + "<!--Optional:-->\r\n               <classType/>\r\n               <!--Optional:-->\r\n               <color>\r\n                  <blue/>\r\n    "
                    + "<green/>\r\n                  <red/>\r\n               </color>\r\n               <!--Optional:-->\r\n               <deleted/>\r\n  "
                    + "<!--Optional:-->\r\n               <guid/>\r\n               <!--Optional:-->\r\n               <name/>\r\n               <personalized/>\r\n    "
                    + "</cardTypeVO>\r\n            <!--Optional:-->\r\n            <clientVO>\r\n               <!--Optional:-->\r\n               <id/>\r\n   "
                    + "<!--Optional:-->\r\n               <auto/>\r\n               <!--Optional:-->\r\n               <birthDate/>\r\n               <!--Optional:-->\r\n  "
                    + "<childrenAge/>\r\n               <!--Optional:-->\r\n               <clientAddress>\r\n                  <!--Optional:-->\r\n                  <appartment/>\r\n "
                    + "<!--Optional:-->\r\n                  <building/>\r\n                  <!--Optional:-->\r\n                  <city/>\r\n                  <!--Optional:-->\r\n   "
                    + "<district/>\r\n                  <!--Optional:-->\r\n                  <districtArea/>\r\n                  <!--Optional:-->\r\n                  <house/>\r\n   "
                    + "<!--Optional:-->\r\n                  <other/>\r\n                  <!--Optional:-->\r\n                  <region/>\r\n                  <!--Optional:-->\r\n    "
                    + "<street/>\r\n                  <!--Optional:-->\r\n                  <zip/>\r\n               </clientAddress>\r\n               <!--Optional:-->\r\n    "
                    + "<creationDate/>\r\n               <!--Optional:-->\r\n               <deleted/>\r\n               <!--Optional:-->\r\n               <email/>\r\n "
                    + "<!--Optional:-->\r\n               <firstName/>\r\n               <!--Optional:-->\r\n               <guid/>\r\n               <!--Optional:-->\r\n  "
                    + "<isCompleted/>\r\n               <!--Optional:-->\r\n               <lastChangeDate/>\r\n               <!--Optional:-->\r\n               <lastName/>\r\n   "
                    + "<!--Optional:-->\r\n               <marital/>\r\n               <!--Optional:-->\r\n               <middleName/>\r\n               <!--Optional:-->\r\n  "
                    + "<mobileOperator/>\r\n               <!--Optional:-->\r\n               <mobilePhone/>\r\n               <!--Optional:-->\r\n               <passport>\r\n    "
                    + "<!--Optional:-->\r\n                  <delivery/>\r\n                  <!--Optional:-->\r\n                  <deliveryDate/>\r\n                  <!--Optional:-->\r\n   "
                    + "<departmentCode/>\r\n                  <!--Optional:-->\r\n                  <passNumber/>\r\n                  <!--Optional:-->\r\n                  <passSerie/>\r\n   "
                    + "</passport>\r\n               <!--Optional:-->\r\n               <phone/>\r\n               <!--Optional:-->\r\n               <sendBy>\r\n  "
                    + "<byEMail/>\r\n                  <byMail/>\r\n                  <byPhone/>\r\n                  <bySMS/>\r\n               </sendBy>\r\n               <sendCatalog/>\r\n "
                    + "<!--Optional:-->\r\n               <sex/>\r\n               <!--Optional:-->\r\n               <shopNumber/>\r\n            </clientVO>\r\n  "
                    + "<!--Optional:-->\r\n            <createDate/>\r\n            <!--Optional:-->\r\n            <deleted/>\r\n            <!--Optional:-->\r\n  "
                    + "<expirationDate/>\r\n            <!--Optional:-->\r\n            <guid/>\r\n            <!--Optional:-->\r\n            <mainCard/>\r\n            <!--Optional:-->\r\n  "
                    + "<newCardNumber/>\r\n            <!--Optional:-->\r\n            <newCardType>\r\n               <!--Optional:-->\r\n               <id/>\r\n "
                    + "<!--Optional:-->\r\n               <classType/>\r\n               <!--Optional:-->\r\n               <color>\r\n                  <blue/>\r\n                  <green/>\r\n  "
                    + "<red/>\r\n               </color>\r\n               <!--Optional:-->\r\n               <deleted/>\r\n               <!--Optional:-->\r\n               <guid/>\r\n   "
                    + "<!--Optional:-->\r\n               <name/>\r\n               <personalized/>\r\n            </newCardType>\r\n            <!--Optional:-->\r\n            <number/>\r\n  "
                    + "<!--Optional:-->\r\n            <oldCardNumber/>\r\n            <signChangeOfCategoryCard/>\r\n            <signExtensionCard/>\r\n            <!--Optional:-->\r\n  "
                    + "<status/>\r\n            <!--Optional:-->\r\n            <statusDescription/>\r\n         </card>\r\n      </car:getActionsByCard>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>"
                    , id, ServiceConst.targetNamespace);
                return this.SendRequest(request);
            }
            public DataSet getCardByFilter(CClientCard.CCardFilter filter)
            {
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{7}\" >\r\n   <soapenv:Header/>\r\n   "
                    + "<soapenv:Body>\r\n      <car:getCardsByFilter>\r\n         <!--Optional:-->\r\n         <sessionId></sessionId>\r\n         <!--Optional:-->\r\n "
                    + "<filter>\r\n            <!--Optional:-->\r\n            <birthDate>{4}</birthDate>\r\n            <!--Optional:-->\r\n            <blocked></blocked>\r\n    "
                    + "<!--Optional:-->\r\n            <cardTypeId></cardTypeId>\r\n            <!--Optional:-->\r\n            <cashNum></cashNum>\r\n            <!--Optional:-->\r\n "
                    + "<checkNum></checkNum>\r\n            <!--Optional:-->\r\n            <clientId>{8}</clientId>\r\n            <!--Optional:-->\r\n    "
                    + "<firstName>use_like:%{2}%</firstName>\r\n            <!--Optional:-->\r\n            <gangNum></gangNum>\r\n            <!--Optional:-->\r\n "
                    + "<lastName>use_like:%{1}%</lastName>\r\n            <!--Optional:-->\r\n            <middleName>use_like:%{3}%</middleName>\r\n            <!--Optional:-->\r\n   "
                    + "<passNumber>{6}</passNumber>\r\n            <!--Optional:-->\r\n            <passSerie>{5}</passSerie>\r\n            <!--Optional:-->\r\n   "
                    + "<number>{0}</number>\r\n            <!--Optional:-->\r\n            <phone></phone>\r\n            <!--Optional:-->\r\n            <shopIndex></shopIndex>\r\n   "
                    + "<showOnlyDeferredClient>false</showOnlyDeferredClient>\r\n         </filter>\r\n         <!--Optional:-->\r\n         <start>1</start>\r\n   "
                    + "<!--Optional:-->\r\n         <count>100</count>\r\n      </car:getCardsByFilter>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>"
                    , filter.Number/*0*/, filter.LastName/*2*/, filter.FirstName/*3*/, filter.MiddleName/*4*/, filter.BirthDate/*5*/, filter.PassSerie/*6*/, filter.PassNumber/*7*/, ServiceConst.targetNamespace/*8*/, filter.ClientID/*9*/);
                return this.SendRequest(request);
            }
            public DataSet storeClient(CClientCard.CClient client)
            {
                client.BirthDate = ServiceProcedure.DateToServerFormat(client.BirthDate);
                client.Passport.DeliveryDate = ServiceProcedure.DateToServerFormat(client.Passport.DeliveryDate);
                string request = string.Format("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:car=\"{30}\">\r\n   <soapenv:Header/>\r\n   "
                    + "<soapenv:Body>\r\n      <car:storeClient>\r\n         <!--Optional:-->\r\n         <sessionId>?</sessionId>\r\n         <!--Optional:-->\r\n "
                    + "<client>\r\n            <!--Optional:-->\r\n            <id>{0}</id>\r\n            <!--Optional:-->\r\n            <auto></auto>\r\n    "
                    + "<!--Optional:-->\r\n            <birthDate>{1}</birthDate>\r\n            <!--Optional:-->\r\n            <childrenAge></childrenAge>\r\n    "
                    + "<!--Optional:-->\r\n            <clientAddress>\r\n               <!--Optional:-->\r\n               <appartment>{2}</appartment>\r\n    "
                    + "<!--Optional:-->\r\n               <building>{3}</building>\r\n               <!--Optional:-->\r\n               <city>{4}</city>\r\n    "
                    + "<!--Optional:-->\r\n               <district>{5}</district>\r\n               <!--Optional:-->\r\n               <districtArea>{6}</districtArea>\r\n    "
                    + "<!--Optional:-->\r\n               <house>{7}</house>\r\n               <!--Optional:-->\r\n               <other>{27}</other>\r\n   "
                    + "<!--Optional:-->\r\n               <region>{8}</region>\r\n               <!--Optional:-->\r\n               <street>{9}</street>\r\n    "
                    + "<!--Optional:-->\r\n               <zip>{10}</zip>\r\n            </clientAddress>\r\n            <!--Optional:-->\r\n   "
                    + "<creationDate>{31}</creationDate>\r\n            <!--Optional:-->\r\n            <deleted></deleted>\r\n            <!--Optional:-->\r\n "
                    + "<email>{24}</email>\r\n            <!--Optional:-->\r\n            <firstName>{11}</firstName>\r\n            <!--Optional:-->\r\n   "
                    + "<guid></guid>\r\n            <!--Optional:-->\r\n            <isCompleted>true</isCompleted>\r\n            <!--Optional:-->\r\n "
                    + "<lastChangeDate>?</lastChangeDate>\r\n            <!--Optional:-->\r\n            <lastName>{12}</lastName>\r\n            <!--Optional:-->\r\n  "
                    + "<marital></marital>\r\n            <!--Optional:-->\r\n            <middleName>{13}</middleName>\r\n            <!--Optional:-->\r\n "
                    + "<mobileOperator>{29}</mobileOperator>\r\n            <!--Optional:-->\r\n            <mobilePhone>{14}</mobilePhone>\r\n            <!--Optional:-->\r\n "
                    + "<passport>\r\n               <!--Optional:-->\r\n               <delivery>{15}</delivery>\r\n               <!--Optional:-->\r\n "
                    + "<deliveryDate>{16}</deliveryDate>\r\n               <!--Optional:-->\r\n               <departmentCode>{17}</departmentCode>\r\n "
                    + "<!--Optional:-->\r\n               <passNumber>{18}</passNumber>\r\n               <!--Optional:-->\r\n               <passSerie>{19}</passSerie>\r\n    "
                    + "</passport>\r\n            <!--Optional:-->\r\n            <phone>{20}</phone>\r\n            <!--Optional:-->\r\n            <sendBy>\r\n   "
                    + "<byEMail>{28}</byEMail>\r\n               <byMail>{28}</byMail>\r\n               <byPhone>{28}</byPhone>\r\n               <bySMS>{28}</bySMS>\r\n  "
                    + "</sendBy>\r\n            <sendCatalog>{28}</sendCatalog>\r\n            <!--Optional:-->\r\n            <sex>{21}</sex>\r\n            <!--Optional:-->\r\n  "
                    + "<shopNumber>{23}</shopNumber>\r\n            <phone>{20}</phone>\r\n            <receivePension>{22}</receivePension>\r\n    "
                    + "<fiscalNumber>{25}</fiscalNumber>\r\n            <snilsNumber>{26}</snilsNumber>\r\n         </client>\r\n      </car:storeClient>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>"
                    , client.ID/*0*/, client.BirthDate/*1*/, client.ClientAddress.Appartment/*2*/, client.ClientAddress.Building/*3*/, client.ClientAddress.City/*4*/, client.ClientAddress.District/*5*/, client.ClientAddress.DistrictArea/*6*/
                    , client.ClientAddress.House/*7*/, client.ClientAddress.Region/*8*/, client.ClientAddress.Street/*9*/, client.ClientAddress.Zip/*10*/, client.FirstName/*11*/, client.LastName/*12*/, client.MiddleName/*13*/, client.MobilePhone/*14*/
                    , client.Passport.Delivery/*15*/, client.Passport.DeliveryDate/*16*/, client.Passport.DepartmentCode/*17*/, client.Passport.Number/*18*/, client.Passport.Serie/*19*/, client.Phone/*20*/, client.Sex/*21*/, client.ReceivePension/*22*/
                    , client.ShopNumber/*23*/, client.EMail/*24*/, client.FiscalNumber/*25*/, client.SnilsNumber/*26*/, client.ClientAddress.Other/*27*/, client.SendBy.ToString().ToLower()/*28*/, client.OperatorName/*29*/, ServiceConst.targetNamespace/*30*/, client.CreationDate/*31*/);
                return this.SendRequest(request);
            }
            private void GetSRUrl()
            {
                m_sURL = ConfigurationManager.AppSettings["CardManagerURL"];
            }
        }
        public class ServiceProcedure
        {
            public static string DateToServerFormat(string strDate)
            {
                string text = "";
                if (strDate.IndexOf("-") != 4)
                {
                    text = strDate.Substring(6, 4) + "-" + strDate.Substring(3, 2) + "-" + strDate.Substring(0, 2);
                    if (strDate.Length > 10)
                    {
                        text += strDate.Substring(11);
                    }
                }
                else text = strDate;
                return text;
            }
            public LoyaltyAPI.Objects.CAddress getClientAddress(DataRow row)
            {
                LoyaltyAPI.Objects.CAddress cAddress = new LoyaltyAPI.Objects.CAddress();
                cAddress.Zip = this.getFieldData(row, "zip");
                cAddress.Region = this.getFieldData(row, "region");
                cAddress.District = this.getFieldData(row, "district");
                cAddress.DistrictArea = this.getFieldData(row, "districtArea");
                cAddress.Street = this.getFieldData(row, "street");
                cAddress.House = this.getFieldData(row, "house");
                cAddress.City = this.getFieldData(row, "city");
                cAddress.Building = this.getFieldData(row, "building");
                cAddress.Appartment = this.getFieldData(row, "appartment");
                cAddress.Other = this.getFieldData(row, "other");
                return cAddress;
            }
            private string getFieldData(DataRow row, string fieldName)
            {
                if (row.Table.Columns.Contains(fieldName))
                {
                    return row[fieldName].ToString();
                }
                return string.Empty;
            }
            public static Type TypeName
            {
                get; set;
            }
        }
    }
}