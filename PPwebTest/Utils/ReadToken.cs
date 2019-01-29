using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using PPweb.Models.StorePP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using RutokenPkcs11Interop;
using System.Configuration;

namespace PPweb.Utils
{
    public class ReadToken
    {
        static readonly List<ObjectAttribute> RsaPrivateKeyAttributes = new List<ObjectAttribute>
        {
        };
        private bool CertValidation(X509Certificate2 _cert)
        {

            bool _certValid = false;
            DateTime _currDate = DateTime.Now;

            int ndate = _currDate.CompareTo(_cert.NotBefore);
            int aDate = _currDate.CompareTo(_cert.NotAfter);
            //1 - текущая дата больше, -1 - текущая дата меньше
            _certValid = (ndate == 1 & aDate == -1);   //действителен по периоду, если не действителен, дальнейшая проверка не нужна
            if (!_certValid)
            { return _certValid; }


            string _issurer = _cert.Issuer;
            X509Store _store = new X509Store(StoreName.CertificateAuthority | StoreName.TrustedPeople | StoreName.My | StoreName.AuthRoot | StoreName.Root, StoreLocation.LocalMachine);

            _store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection _certCollection = _store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, _issurer, true);
            _store.Close();
            _certValid = (_certCollection.Count != 0);  //действителен по Центру Сертификации

            return _certValid;
        }
        public byte[] ReadTokenCert()
        {
            byte[] _certByte = null;
            try
            {
                // Инициализировать библиотеку
                var attributes = new List<CKA>
                    {
                        CKA.CKA_VALUE
                    };
                using (var pkcs11 = new Pkcs11(Settings.RutokenEcpDllDefaultPath, Settings.OsLockingDefault))
                {
                    // Получить слоты
                    List<Slot> slots = pkcs11.GetSlotList(true);
                    // Проверить, что слоты найдены
                    if (slots == null)
                        throw new NullReferenceException("No available slots");
                    // Проверить, что число слотов больше 0
                    if (slots.Count <= 0)
                        throw new InvalidOperationException("No available slots");

                    var slot = slots[0];
                    // Получить информацию о токене
                    // Открыть RW сессию в первом доступном слоте
                    using (Session session = slot.OpenSession(false))
                    {
                        try
                        {
                            List<ObjectHandle> privateKeys = session.FindAllObjects(RsaPrivateKeyAttributes);
                            if (privateKeys.Count <= 0)
                                throw new InvalidOperationException("No private keys found");
                            foreach (ObjectHandle key in privateKeys)
                            {
                                List<ObjectAttribute> attribList = session.GetAttributeValue(key, attributes);
                                byte[] val = null;
                                foreach (ObjectAttribute attV in attribList)
                                {
                                    if (!attV.CannotBeRead)
                                    {
                                        val = attV.GetValueAsByteArray();

                                        X509Certificate2 cert = new X509Certificate2();
                                        cert.Import(val);
                                        if (CertValidation(cert))
                                            _certByte = cert.Export(X509ContentType.Cert);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            // Сбросить права доступа как в случае исключения,
                            // так и в случае успеха.
                            // Сессия закрывается автоматически.
                            //session.Logout();
                        }
                    }
                }
            }
            catch
            {

            }

            return _certByte;
        }
        public X509Certificate2 ReadCertFromStore(out string certErr)
        {
            X509Certificate2 _certReturn = null;
            certErr = string.Empty;

            string _serverName = Environment.MachineName.ToLower();
            Logger.Log.Info(_serverName);
            string _certSN = ConfigurationManager.AppSettings[_serverName];
            if (_certSN != null)
            {
                X509Store _store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                _store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection _collection = _store.Certificates.Find(X509FindType.FindBySerialNumber, _certSN, true);
                _store.Close();
                if (_collection.Count > 0)
                {
                    _certReturn = _collection[0];
                    Logger.Log.Info(_certReturn.GetSerialNumberString());
                }
                else
                    certErr = "Сертификат отсутствует в Хранилище сертификатов!";
            }
            else
                certErr = "Не указан Сертификат для обмена в Настройках!";
            return _certReturn;
        }

        public bool ReadTokenCertStatus(out string tokenError)
        {
            tokenError = string.Empty;
            bool certOK = true;
            try
            {
                // Инициализировать библиотеку
                var attributes = new List<CKA> { CKA.CKA_VALUE };
                using (var pkcs11 = new Pkcs11(Settings.RutokenEcpDllDefaultPath, Settings.OsLockingDefault))
                {
                    // Получить слоты
                    List<Slot> slots = pkcs11.GetSlotList(true);
                    // Проверить, что слоты найдены
                    if (slots == null)
                        throw new NullReferenceException("No available slots");
                    // Проверить, что число слотов больше 0
                    if (slots.Count <= 0)
                        throw new InvalidOperationException("No available slots");

                    var slot = slots[0];
                    // Получить информацию о токене
                    // Открыть RW сессию в первом доступном слоте
                    using (Session session = slot.OpenSession(false))
                    {
                        try
                        {
                            List<ObjectHandle> privateKeys = session.FindAllObjects(RsaPrivateKeyAttributes);
                            if (privateKeys.Count <= 0)
                                throw new InvalidOperationException("No private keys found");
                            foreach (ObjectHandle key in privateKeys)
                            {
                                List<ObjectAttribute> attribList = session.GetAttributeValue(key, attributes);
                                byte[] val = null;
                                foreach (ObjectAttribute attV in attribList)
                                {
                                    if (!attV.CannotBeRead)
                                    {
                                        val = attV.GetValueAsByteArray();

                                        X509Certificate2 cert = new X509Certificate2();
                                        cert.Import(val);
                                        certOK = CertValidation(cert);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            // Сбросить права доступа как в случае исключения,
                            // так и в случае успеха.
                            // Сессия закрывается автоматически.
                            //session.Logout();
                        }
                    }
                }
            }
            catch
            {
                certOK = false;
                tokenError = "Не найден Токен!";
            }
            return certOK;
        }
    }
}