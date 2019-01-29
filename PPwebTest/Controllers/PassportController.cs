using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PPweb.Models;
using PPweb.Utils;
using FastReport;
using FastReport.Web;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Collections.Specialized;

namespace PPweb.Controllers
{
    public class PassportController : Controller
    {
        private CClientCard.CClient prCast = new CClientCard.CClient();
        private CClientCard.CCard prCard = new CClientCard.CCard();
        string filepath = "";
        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.DateTT = DateTime.Now.DayOfYear.ToString()+ '.'+ DateTime.Now.Year.ToString() + ' ' + DateTime.Now.TimeOfDay.ToString();
            return View();
        }

        [HttpPost]
        public ActionResult Index(Authorization autore)
        {
            if (autore.AutProc())
            {
                //ViewBag.MessageAutor = "Авторизация пройдена!";
                return View("MainForm", new FilterCust());
            }
            else
            {
                ViewBag.MessageAutor = "Некорректные учетные данные!";
                return View();
            }
        }

        // GET: Main
        [HttpGet]
        public ViewResult MainForm(string opsIndex, string opsOperName, string opsOperNameFull, string opsOperId, string opsTerminal)
        {
            OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
            if (!string.IsNullOrEmpty(opsIndex))
            { _opsInfo.OpsIndex = opsIndex; }
            if (!string.IsNullOrEmpty(opsOperName))
            { _opsInfo.OpsOperatorName = opsOperName; }
            if (!string.IsNullOrEmpty(opsOperNameFull))
            { _opsInfo.OpsOperatorFullName = opsOperNameFull; }
            if (!string.IsNullOrEmpty(opsOperId))
            { _opsInfo.OpsOperatorId = opsOperId; }
            if (!string.IsNullOrEmpty(opsTerminal))
            { _opsInfo.OpsShopId = opsTerminal; }
            if (_opsInfo != null)
            {
                if (HttpContext.Session["OpsInfo"] == null)
                {
                    HttpContext.Session["OpsInfo"] = _opsInfo;
                }
            }
            if (HttpContext.Session["OpsInfo"] != null)
            {
                ViewBag.OpsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
            }
            /*FilterCust fCust = new FilterCust(); 
            FillCustFromStat(ref fCust); 
            TestStore.FillTempTables ftt = new TestStore.FillTempTables(); 
            if (fCust.resultCust == null)
              ViewBag.rCust = ConvertDataTable(ftt.fillCust(true));
            else
                ViewBag.rCust = ConvertDataTable(fCust.resultCust);
            if(fCust.resultCard == null)
                ViewBag.rCard = ConvertDataTable(ftt.fillCard(true));
             else
                ViewBag.rCard = ConvertDataTable(fCust.resultCard);
            if (fCust.resultCardAction == null)
                ViewBag.rAction = ConvertDataTable(ftt.fillCardAction(true));
            else
                ViewBag.rAction = ConvertDataTable(fCust.resultCardAction);*/
            return View(new FilterCust());
        }

        [HttpPost]
        public ActionResult MainForm(FilterCust fCust, string btnComm, bool? isReloadGrid)
        {
            if(isReloadGrid == true)
            {
                fCust = (FilterCust)HttpContext.Session["filterCust"];
            }
            var tag = new TagBuilder("p");
            tag.AddCssClass("erroreMsg col-lg-offset-1");                
            if (btnComm == "runSearch")
            {
                if (string.IsNullOrEmpty(fCust.fBirthDate) && string.IsNullOrEmpty(fCust.fCardNumber) && string.IsNullOrEmpty(fCust.fFirstName)
                    && string.IsNullOrEmpty(fCust.fLastName) && string.IsNullOrEmpty(fCust.fMiddleName) && string.IsNullOrEmpty(fCust.fPassNumber)
                    && string.IsNullOrEmpty(fCust.fPassSeria))
                {
                    tag.InnerHtml = "Заполните поля для поиска";
                    ViewBag.ErroreMsg = tag.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(fCust.fFirstName))
                    {
                        if(fCust.fFirstName.Contains(" ")) fCust.fFirstName = fCust.fFirstName.Replace(" ","");
                    }
                    if (!string.IsNullOrEmpty(fCust.fMiddleName))
                    {
                        if (fCust.fMiddleName.Contains(" ")) fCust.fMiddleName = fCust.fMiddleName.Replace(" ", "");
                    }
                    if (!string.IsNullOrEmpty(fCust.fLastName))
                    {
                        if (fCust.fLastName.Contains(" ")) fCust.fLastName = fCust.fLastName.Replace(" ", "");
                    }
                    string err = "";
                    FindProcess fp = new FindProcess();
                    fp.letFindSomeData(ref fCust, out err);
                    if (fCust.resultCust == null && err == "")
                    {
                        tag.InnerHtml = "Не найдено ни одной записи. Просьба изменить критерии и повторить поиск.";
                        ViewBag.ErroreMsg = tag.ToString();
                    }
                    else if (err != "")
                    {
                        tag.InnerHtml = err;
                        ViewBag.ErroreMsg = tag.ToString();
                        /*TestStore.FillTempTables ftt = new TestStore.FillTempTables();
                        ViewBag.ErrorMsg = err;
                        ViewBag.rCust = ConvertDataTable(ftt.fillCust(true));
                        ViewBag.rCard = ConvertDataTable(ftt.fillCard(true)); ;
                        ViewBag.rAction = ConvertDataTable(ftt.fillCardAction(true));*/
                    }
                    else
                    {
                        if (fCust.resultCust.Rows.Count > 0)
                        {
                            if (!fCust.resultCust.Columns.Contains("passdata"))
                                fCust.resultCust.Columns.Add("passdata");
                            if (fCust.resultCard.Rows.Count > 0)
                            {
                                foreach (DataRow row in fCust.resultCust.Rows)
                                {
                                    string id = row["id"].ToString();
                                    string cardsSum = "";
                                    string pass = string.Empty;
                                    foreach (DataRow cardRow in fCust.resultCard.Rows)
                                    {
                                        if (cardRow["CustId"].ToString().Equals(id))
                                        {
                                            cardsSum += cardRow["number"].ToString() + "; ";
                                        }
                                    }
                                    row["cards"] = cardsSum;
                                    foreach (DataRow passRow in fCust.resultCustPassport.Rows)
                                    {
                                        if (passRow["CustId"].ToString().Equals(id))
                                        {
                                            pass = passRow["passSerie"].ToString() + " " + passRow["passNumber"].ToString();
                                        }
                                    }
                                    if (pass != string.Empty)
                                        row["passdata"] = pass;
                                    fCust.resultCust.AcceptChanges();
                                }
                            }
                            ViewBag.rCust = ConvertDataTable(fCust.resultCust);

                        }
                        /*if (fCust.resultCard.Rows.Count > 0)
                        {
                            ViewBag.rCard = ConvertDataTable(fCust.resultCard);
                        }
                        if (fCust.resultCardAction.Rows.Count > 0)
                        {
                            ViewBag.rAction = ConvertDataTable(fCust.resultCardAction);
                        }*/
                    }
                }
                HttpContext.Session["filterCust"] = fCust;
                ViewBag.ErroreMsg = tag.ToString();
                return PartialView("SearchResultClients");
            }
            if (btnComm == "NewCust")
            {
                return RedirectToAction("NewCust", "Passport");
            }
            /*if (btnComm.Contains("rsCustId:"))
            {
                fCust = (FilterCust)HttpContext.Session["filterCust"];
                string tmpId = btnComm.Remove(0, 9);
                EditCClient eCust = new EditCClient();
                FillEditCustFromSearch(ref eCust, fCust, tmpId);
                ViewBag.rCard = ConvertDataTable(eCust.resultCard);
                //ViewBag.rAction = ConvertDataTable(eCust.resultCardAction);
                return PartialView("ClientInfo", eCust);
                //return RedirectToAction("EditCust", "Passport");
            } */          
            return View(fCust);
        }

        [HttpPost]
        public MvcHtmlString ProcessingClient(Client client, string commBtn)
        {
            if(client.ModalBoxType == "NewClient")
            {
                var tClient = DownCustingClient(client, new TempClient());
                return NewCust(tClient, commBtn);
            }
            else if (client.ModalBoxType == "EditClient")
            {
                var tClient = DownCustingClient(client, new EditCClient());
                return EditCust(tClient, commBtn);
            }
            else if (client.ModalBoxType == "NewCard")
            {
                var tClient = DownCustingClient(client, new EditClientNewCard());
                return NewCard(tClient, commBtn);
            }
            throw new ArgumentOutOfRangeException();
        }

        [HttpGet]
        public ActionResult SearchCardActionsResult(string cardId)
        {
            var eCust = (EditCClient)HttpContext.Session["editCust"];
            var rAction = ConvertDataTable(eCust.resultCardAction);
            for(var i = 0; i < rAction.Count; i++)
            {
                if(rAction[i].CardId != cardId)
                {
                    rAction.RemoveAt(i);
                    i--;
                }
            }
            ViewBag.rAction = rAction;
            return PartialView("CardActionsGrid");
        }

        [HttpGet]
        public ActionResult GetClientInfo(string clientId)
        {
            var fCust = (FilterCust)HttpContext.Session["filterCust"];
            EditCClient eCust = new EditCClient();
            FillEditCustFromSearch(ref eCust, fCust, clientId);
            ViewBag.rCard = ConvertDataTable(eCust.resultCard);
            HttpContext.Session["editCust"] = eCust;
            return PartialView("ClientInfo", eCust);
        }

        [HttpGet]
        public ActionResult NewCust()
        {
            ViewBag.Regions = RegionList();
            ViewBag.CurrRegion = " ";
            return PartialView("ClientModal", new TempClient());
        }

        [HttpGet]
        public ActionResult EditCust()
        {
            var eCust = (EditCClient)HttpContext.Session["editCust"];

            var _regions = RegionList();
            string currRegionName = (eCust.Region == null ? string.Empty : eCust.Region);
            string selectedReg = string.Empty;
            string currRegionValue = GetCurrValueFromRegions(_regions, currRegionName, out selectedReg);
            if (!string.IsNullOrEmpty(selectedReg))
            {
                ViewBag.Regions = RegionList(selectedReg);
            }
            else
            {
                ViewBag.Regions = _regions;
            }

            ViewBag.CurrRegion = currRegionValue;

            eCust.Header = "Редактирование данных клиента";
            eCust.ModalBoxType = "EditClient";
            return PartialView("ClientModal", eCust);
        }

        [HttpGet]
        public ActionResult NewCard()
        {
            var client = (Client)(EditCClient)HttpContext.Session["editCust"];

            var _regions = RegionList();
            string currRegionName = (client.Region == null ? string.Empty : client.Region);
            string selectedReg = string.Empty;
            string currRegionValue = GetCurrValueFromRegions(_regions, currRegionName, out selectedReg);
            if (!string.IsNullOrEmpty(selectedReg))
            {
                ViewBag.Regions = RegionList(selectedReg);
            }
            else
            {
                ViewBag.Regions = _regions;
            }

            ViewBag.CurrRegion = currRegionValue;

            client.Header = "Оформление карты \"Почтовый паспорт\" ";
            client.ModalBoxType = "NewCard";
            return PartialView("ClientModal", client);
        }

        [HttpGet]
        public ActionResult GetReplaceCardView(string number, string oldId)
        {
            return PartialView("ReplaceCardModal", new ModifyCard {oldNumber = number, oldId = oldId });
        }

        [HttpPost]
        public MvcHtmlString ReplaceCard(ModifyCard mCard, string btn)
        {
            var eroreMsg = string.Empty;
            var infoMsg = string.Empty;
            var callback = string.Empty;
            var scriptTag = new TagBuilder("script");
            if(btn == "check")
            {
                CheckingCard(mCard.newNumber, ref eroreMsg, ref infoMsg);
                callback = "checkingReplaceResponse(" +(string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }
            else
            {
                OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
                if (HttpContext.Session["OpsInfo"] != null)
                {
                    _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
                }
                EditCClient eCust = new EditCClient();
                if (HttpContext.Session["editCust"] != null)
                {
                    eCust = (EditCClient)HttpContext.Session["editCust"];
                    foreach (DataRow row in eCust.resultCard.Rows)
                    {
                        if (row["number"].ToString().Equals(mCard.oldNumber))
                        {
                            mCard.oldId = row["Id"].ToString();
                            mCard.CustId = row["CustId"].ToString();
                        }
                    }
                }
                FindData fd = new FindData();
                var result = false;
                var msg = fd.ChangeCard(ref mCard, _opsInfo, out result);
                infoMsg = result ? msg : string.Empty;
                eroreMsg = result ? string.Empty : msg;
                callback = "replaceResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }


        [HttpGet]
        public PartialViewResult BlockCard(string number)
        {
            var mCard = new ModifyCard { oldNumber = number };
            var reasonList = new CClientCard.CCardBlockingReasonList();
            var selectList = new SelectList(reasonList, "Value", "Name");
            ViewBag.Reasons = selectList;
            return PartialView("BlockCardModal", mCard);
        }
        [HttpPost]
        public MvcHtmlString BlockCard(ModifyCard mCard, string Reason)
        {
            var eroreMsg = string.Empty;
            var infoMsg = string.Empty;
            var callback = string.Empty;
            var scriptTag = new TagBuilder("script");

            var reasonList = new CClientCard.CCardBlockingReasonList();
            mCard.Reason = reasonList.Find(r => r.Value == Reason);
            var msg = string.Empty;
            mCard.Comment = mCard.Comment == null ? string.Empty : mCard.Comment;
            var result = false;
            var fd = new FindData();
            OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
            if (HttpContext.Session["OpsInfo"] != null)
            {
                _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
            }
            msg = fd.BlockCard(mCard, out result, _opsInfo);

            if (result)
            {
                infoMsg = "Карта успешно заблокирована";
            }
            else
            {
                eroreMsg = msg;
            }


            callback = "blockCardResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }

        [HttpGet]
        public ActionResult FitPageView()
        {
            var webReport =(WebReport) HttpContext.Cache["webReport"];
            ViewBag.WebReport = webReport;
            HttpContext.Cache.Remove("webReport");
            return View("FitPage");
        }

        [HttpGet]
        public PartialViewResult GetConfirmedView()
        {
            return PartialView("ConfirmedModal");
        }

        private MvcHtmlString NewCard(EditClientNewCard enCust, string btnComm)
        {
            CClientCard.CClient nCust = new CClientCard.CClient();
            CClientCard.CCard nCard = new CClientCard.CCard();
            var scriptTag = new TagBuilder("script");
            var eroreMsg = string.Empty;
            var infoMsg = string.Empty;
            var callback = string.Empty;

            if (btnComm == "runCheck")
            {
                var msg = string.Empty;
                FindData fd = new FindData();
                DataTable tmpTB = new DataTable();
                tmpTB = fd.FindCard(enCust.CardNumber, out msg);
                if (tmpTB != null)
                {
                    if (tmpTB.Rows.Count > 0)
                    {
                        DataRow row = tmpTB.Rows[0];
                        if (row != null)
                        {
                            infoMsg = "Тип карты: " + row["name"].ToString();
                            string _status = string.Empty;
                            if (row["status"].ToString().Equals("Активна"))
                            {
                                _status = ", статус: Доступна";
                                infoMsg += _status;
                            }
                            else
                            {
                                _status = ", статус: " + row["status"].ToString();
                                infoMsg += _status;
                            }
                        }
                    }
                    else
                    {
                        if (msg != string.Empty)
                            eroreMsg = msg;
                        else
                        eroreMsg = "Карта выдана другому клиенту";
                    }

                }
                callback = "checkingClientCardResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            else if (btnComm == "runPrint")
            {
                PreSaveCust(nCust, enCust, 0);
                PreSaveCard(nCard, enCust);

                var filepath = GetFilePath();
                var webReport = FitPage(nCust, nCard, filepath);
                HttpContext.Cache["webReport"] = webReport;
                infoMsg = "FitPageView";
                callback = "printResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            else if (btnComm == "runSave")
            {
                OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
                if (HttpContext.Session["OpsInfo"] != null)
                {
                    _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
                }
                EditCClient eCust = new EditCClient();
                if (HttpContext.Session["editCust"] != null)
                {
                    eCust = (EditCClient)HttpContext.Session["editCust"];
                }
                DataSet ds = new DataSet();
                LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();

                string oldCardID = string.Empty;
                string oldCardNumber = string.Empty;
                if (eCust.resultCard.Rows.Count == 1)
                {
                    DataRow oldCard = eCust.resultCard.Rows[0];
                    oldCardID = oldCard["id"].ToString();
                    oldCardNumber = oldCard["number"].ToString();
                }
                else if (eCust.resultCard.Rows.Count > 1)
                {
                    DataRow[] _cards = eCust.resultCard.Select("name <> 'Почтовый паспорт' and status = 'Активна'");
                    if (_cards.Length != 0)
                    {
                        DataRow oldCard = _cards[0];
                        oldCardID = oldCard["id"].ToString();
                        oldCardNumber = oldCard["number"].ToString();
                    }
                    else
                    {
                        _cards = eCust.resultCard.Select("name = 'Почтовый паспорт' and status <> 'Активна' and number <> 'Отсутствует у клиента'");
                        if (_cards.Length != 0)
                        {
                            DataRow oldCard = _cards[0];
                            oldCardID = oldCard["id"].ToString();
                            oldCardNumber = oldCard["number"].ToString();
                        }
                    }
                }

                string OperOps = string.IsNullOrEmpty(_opsInfo.OpsOperatorName) ? "" : _opsInfo.OpsOperatorName;
                string IndexOps = string.IsNullOrEmpty(_opsInfo.OpsIndex) ? "" : _opsInfo.OpsIndex;

                ds = ls.changeCard(oldCardID, oldCardNumber, enCust.CardNumber, OperOps, IndexOps);
                if (ds.Tables.Contains("return"))
                {
                    infoMsg = "Сохранено!";
                }
                else
                {
                    try
                    {
                        string text = ds.Tables["Fault"].Rows[0]["faultString"].ToString();
                        string text2 = text;
                        if (ds.Tables.Contains("CardsException"))
                        {
                            string text3 = ds.Tables["CardsException"].Rows[0]["type"].ToString();
                            switch (text3)
                            {
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
                            eroreMsg = text2;
                        }
                        eroreMsg = "Ошибка сохранения!";
                    }
                    catch
                    {
                        eroreMsg = "Неизвестная ошибка сохранения!";
                    }
                }
                callback = "saveClientResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }

        private MvcHtmlString EditCust(EditCClient eCust, string btnComm)
        {
            if (!string.IsNullOrEmpty(eCust.FirstName))
            {
                if (eCust.FirstName.Contains(" ")) eCust.FirstName = eCust.FirstName.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(eCust.MiddleName))
            {
                if (eCust.MiddleName.Contains(" ")) eCust.MiddleName = eCust.MiddleName.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(eCust.LastName))
            {
                if (eCust.LastName.Contains(" ")) eCust.LastName = eCust.LastName.Replace(" ", "");
            }
            var eCustSession = (EditCClient)HttpContext.Session["editCust"];
            var props = HttpContext.Request.Params;
            eCust = MergeCust(eCustSession, eCust, props);

            var _regions = RegionList();
            IEnumerable<SelectListItem> region = _regions.Where(x => x.Value == eCust.RegionCode);
            foreach (SelectListItem reg in region)
            {
                eCust.Region = reg.Text;
            }

            var scriptTag = new TagBuilder("script");
            var eroreMsg = string.Empty;
            var infoMsg = string.Empty;
            var callback = string.Empty;

            if (btnComm == "runSave")
            {
                CClientCard.CClient nCust = new CClientCard.CClient();
                eCust.SendBy = eCust.SendByTemp;
                PreSaveCustEdit(nCust, eCust, 1);

                CClientCard.ValidPP valid = new CClientCard.ValidPP();
                string err = valid.LetValidSome(nCust);
                if (err == string.Empty)
                {
                    DataSet ds = new DataSet();
                    LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
                    ds = ls.storeClient(nCust);
                    if (ds != null)
                    {
                        infoMsg = "Сохранено!";
                    }
                    else
                    {
                        eroreMsg = "Ошибка сохранения!";
                    }
                }
                else
                {
                    eroreMsg = err;
                }
                callback = "saveClientResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }
            else if(btnComm == "runPrint")
            {
                CClientCard.CClient nCust = new CClientCard.CClient();
                CClientCard.CCard nCard = new CClientCard.CCard();
                PreSaveCustEdit(nCust, eCust, 0);
                string number = string.Empty;
                var rows = eCust.resultCard.Select("name = \'Почтовый паспорт\' and status = \'Активна\'"); //and status = \'Активна\'
                if (rows.Length > 0)
                {
                    number = rows[0]["number"].ToString();
                }
                else
                {
                    number = " ";
                    //eroreMsg = "Ошибка выбора Карт";
                }
                PreSaveCardEdit(nCard, number);
                filepath = GetFilePath();
                var webReport = FitPage(nCust, nCard, filepath);
                infoMsg = "FitPageView";
                HttpContext.Cache["webReport"] = webReport;
                callback = "printResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }
            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }

        private MvcHtmlString NewCust(TempClient tClient, string commBtn)
        {
            if (!string.IsNullOrEmpty(tClient.FirstName))
            {
                if (tClient.FirstName.Contains(" ")) tClient.FirstName = tClient.FirstName.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(tClient.MiddleName))
            {
                if (tClient.MiddleName.Contains(" ")) tClient.MiddleName = tClient.MiddleName.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(tClient.LastName))
            {
                if (tClient.LastName.Contains(" ")) tClient.LastName = tClient.LastName.Replace(" ", "");
            }
            var _regions = RegionList();
            IEnumerable<SelectListItem> region = _regions.Where(x => x.Value == tClient.RegionCode);
            foreach (SelectListItem reg in region)
            {
                tClient.Region = reg.Text;
            }

            CClientCard.CClient nCust = new CClientCard.CClient();
            CClientCard.CCard nCard = new CClientCard.CCard();
            var scriptTag = new TagBuilder("script");
            var eroreMsg = string.Empty;
            var infoMsg = string.Empty;
            var callback = string.Empty;

            if (commBtn == "runSave")
            {
                tClient.SendBy = tClient.SendByTemp;
                PreSaveCust(nCust, tClient, 1);
                PreSaveCard(nCard, tClient);
                CClientCard.ValidPP valid = new CClientCard.ValidPP();
                string err = valid.LetValidSome(nCust);
                if (err == string.Empty)
                {
                    DataSet ds = new DataSet();
                    LoyaltyService.RequestGenerator ls = new LoyaltyService.RequestGenerator();
                    ds = ls.addClientAndSetToCard(nCard.CardNumber, nCust);
                    if (ds != null)
                    {
                        infoMsg = "Сохранено";
                    }
                    else
                    {
                        eroreMsg = "Ошибка сохранения!";
                    }
                }
                else
                {
                    eroreMsg = err;
                }

                callback = "saveClientResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }
            else if (commBtn == "runCheck")
            {
                var msg = string.Empty;
                FindData fd = new FindData();
                DataTable tmpTB = new DataTable();
                tmpTB = fd.FindCard(tClient.CardNumber, out msg);
                if (tmpTB != null)
                {
                    if (tmpTB.Rows.Count > 0)
                    {
                        DataRow row = tmpTB.Rows[0];
                        if (row != null)
                        {
                            //infoMsg = "Карта свободна";
                            infoMsg = "Тип карты: " + row["name"].ToString();
                            string _status = string.Empty;
                            if (row["status"].ToString().Equals("Активна"))
                            {
                                _status = ", статус: Доступна";
                                infoMsg += _status;
                            }
                            else
                            {
                                _status = ", статус: " + row["status"].ToString();
                                infoMsg += _status;
                            }
                        }
                    }
                    else
                    {
                        if (msg != string.Empty)
                            eroreMsg = msg;
                        else
                            eroreMsg = "Карта выдана другому клиенту";
                    }

                }
                callback = "checkingClientCardResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            else if (commBtn == "runPrint")
            {
                PreSaveCust(nCust, tClient, 0);
                PreSaveCard(nCard, tClient);

                var filepath = GetFilePath();
                var webReport = FitPage(nCust, nCard, filepath);
                HttpContext.Cache["webReport"] = webReport;
                infoMsg = "FitPageView";
                callback = "printResponse(" + (string.IsNullOrEmpty(eroreMsg) ? "null" : "'" + eroreMsg + "'") + ", " + (string.IsNullOrEmpty(infoMsg) ? "null" : "'" + infoMsg + "'") + ")";
            }

            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }


        private void CheckingCard(string newNumber, ref string erroreMsg, ref string infoMsg)
        {
            var msg = string.Empty;
            FindData fd = new FindData();
            var tmpTB = fd.FindCard(newNumber, out msg);
            if (tmpTB != null)
            {
                if (tmpTB.Rows.Count > 0)
                {
                    DataRow row = tmpTB.Rows[0];
                    if (row != null)
                    {
                        //infoMsg = "Карта свободна";
                        infoMsg = "Тип карты: " + row["name"].ToString(); ;
                        string _status = string.Empty;
                        if (row["status"].ToString().Equals("Активна"))
                        {
                            _status = ", статус: Доступна";
                            infoMsg += _status;
                        }
                        else
                        {
                            _status = ", статус: " + row["status"].ToString();
                            infoMsg += _status;
                        }
                    }
                }
                else
                {
                    if (msg != string.Empty)
                        erroreMsg = msg;
                    else
                        erroreMsg = "Карта выдана другому клиенту";
                }
            }
        }

        public JsonResult ValidMobilePhone(string MobilePhone)
        {
            if (MobilePhone != "+7 ")
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json("Поле является обязательным для заполнения", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidBirthDate(string BirthDate)
        {
            if (BirthDate == "__.__.____") return Json(true, JsonRequestBehavior.AllowGet);

            string errorMsg = CheckDateInFuture(BirthDate, 2);
            if(string.IsNullOrEmpty(errorMsg))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(errorMsg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidFilterBirthDate(string fBirthDate)
        {
            if(fBirthDate == "__.__.____") return Json(true, JsonRequestBehavior.AllowGet);

            string errorMsg = CheckDateInFuture(fBirthDate, 2);
            if (string.IsNullOrEmpty(errorMsg))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(errorMsg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidPassportDate(string DeliveryDate)
        {
            if (DeliveryDate == "__.__.____") return Json(true, JsonRequestBehavior.AllowGet);

            string errorMsg = CheckDateInFuture(DeliveryDate, 1);
            if (string.IsNullOrEmpty(errorMsg))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(errorMsg, JsonRequestBehavior.AllowGet);
        }
        public MvcHtmlString ValidPassportUniq(string PassportForValid)
        {
            var callback = string.Empty;
            var scriptTag = new TagBuilder("script");
            string errMessage = string.Empty;

            if (!string.IsNullOrEmpty(PassportForValid))
            {
                string[] _passport = PassportStringParser(PassportForValid);
                FilterCust fCust = new FilterCust { fCardNumber = _passport[0], fPassSeria = _passport[1], fPassNumber = _passport[2] };
                //fCardNumber - в данном случае для передачи ID клиента
                FindProcess findProcess = new FindProcess();

                bool checkStatus = findProcess.letCheckPassportUniq(ref fCust, out errMessage);
                if (!checkStatus)
                {
                    callback = "UniqPassCheckMessage('" + errMessage + "')";
                }
                else
                {
                    callback = "UniqPassCheckMessage('null')";
                }

            }
            else
            {
                callback = "UniqPassCheckMessage('null')";
            }
            scriptTag.InnerHtml = callback;
            return MvcHtmlString.Create(scriptTag.ToString());
        }
        public JsonResult CheckFederal(string RegionCode)  
        {
            var _regions = RegionList();
            IEnumerable<SelectListItem> federalEnum = _regions.Where(x => x.Value.ToLower().Contains(" город"));
            IEnumerable<SelectListItem> checkCurRegion = federalEnum.Where(x => x.Value == RegionCode);
            if (checkCurRegion.Count() > 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckCityFederal(string RegionCode, string City) 
        {
            var _regions = RegionList();
            SelectListItem checkCurRegion = _regions.Where(x => x.Value == RegionCode).First();
            if (checkCurRegion.Text.ToLower().Equals(City.ToLower()))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }
        private string CheckDateInFuture(string _date, int _par)
        {
            DateTime parsedDate;
            string errorMsg = string.Empty;
            DateTime controlDate = DateTime.Now;
            DateTime controlDate2 = new DateTime();
            string errorMsgTamplate = string.Empty;
            string errorMsgTamplate2 = string.Empty;

            switch (_par)
            {
                case 1:
                    {
                        errorMsgTamplate = "Дата выдачи паспорта не должна быть в будущем";
                        break;
                    }
                case 2:
                    {
                        controlDate2 = new DateTime(DateTime.Now.Year - 14, DateTime.Now.Month, DateTime.Now.Day);
                        errorMsgTamplate = "Дата рождения не должна быть в будущем";
                        errorMsgTamplate2 = "Клиент должен быть старше 14 лет";
                        break;
                    }
            }

            if (DateTime.TryParse(_date, out parsedDate))
            {
                if (controlDate < parsedDate)
                    errorMsg = errorMsgTamplate;
                else if (_par == 2)
                {
                    if(controlDate2 < parsedDate)
                        errorMsg = errorMsgTamplate2;
                }
            } else
                errorMsg = "Некорректная дата";
            return errorMsg; 
        }
        private string[] PassportStringParser(string _pass)
        {
            string[] readyData = new[] { "", "", "" };
            if (_pass.Contains("_"))
            {
                for (int i = 0; (_pass.Contains("_")) || (i < 3); i++)
                {
                    if (_pass.IndexOf("_") > 0)
                    {
                        string temp = _pass.Remove(_pass.IndexOf("_"));
                        _pass = _pass.Remove(0, _pass.IndexOf("_") + 1);
                        readyData[i] = temp;
                    }
                    else
                    {
                        string temp = _pass;
                        readyData[i] = temp;
                    }

                }
            }
            return readyData;
        }

        private string GetFilePath()
        {
            string path = ConfigurationManager.AppSettings["ClientFormName"];
            string path2 = ConfigurationManager.AppSettings["TemplatePath"];
            string fileName = this.Server.MapPath(path2 + '/' + path);
            return fileName;
        }
        private void PreSaveCust(CClientCard.CClient nCust, Client tCust, int Stage)
        {
            nCust.BirthDate = tCust.BirthDate;
            nCust.FirstName = tCust.FirstName;
            nCust.MiddleName = tCust.MiddleName;
            nCust.LastName = tCust.LastName;
            nCust.CreationDate = DateTime.Now.ToString();
            nCust.SnilsNumber = tCust.SnilsNumber;
            nCust.MobilePhone = tCust.MobilePhone;
            nCust.Phone = tCust.Phone;
            nCust.FiscalNumber = tCust.FiscalNumber;
            nCust.EMail = tCust.EMail;
            switch (tCust.Sex)
            {
                case "0":
                    nCust.Sex = "Male";
                    break;
                case "1":
                    nCust.Sex = "Female";
                    break;
            }
            if(Stage == 1)
                nCust.SendBy = (bool)tCust.SendBy;

            CClientCard.CAddress tAddress = new CClientCard.CAddress();
            tAddress.Appartment = tCust.Appartment;
            tAddress.Building = tCust.Building;
            tAddress.City = tCust.City;
            tAddress.Corp = tCust.Corp;
            tAddress.District = tCust.District;
            tAddress.DistrictArea = tCust.DistrictArea;
            tAddress.House = tCust.House;
            tAddress.Region = tCust.Region;
            tAddress.Street = tCust.Street;
            tAddress.Zip = tCust.Zip;
            tAddress.Other = tCust.Corp;
            nCust.ClientAddress = tAddress;

            CClientCard.CDocument tPassport = new CClientCard.CDocument();
            tPassport.Delivery = tCust.Delivery;
            tPassport.DeliveryDate = tCust.DeliveryDate;
            tPassport.DepartmentCode = tCust.DepartmentCode;
            tPassport.Number = tCust.Number;
            tPassport.Serie = tCust.Serie;
            nCust.Passport = tPassport;

            OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
            if (HttpContext.Session["OpsInfo"] != null)
            {
                _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
            }
            if (_opsInfo.OpsOperatorName != null)
                nCust.OperatorName = _opsInfo.OpsOperatorName;
            if (_opsInfo.OpsIndex != null)
                nCust.ShopNumber = _opsInfo.OpsIndex;
        }
        private void PreSaveCard(CClientCard.CCard nCard, Client tCust)
        {
            nCard.CardNumber = tCust.CardNumber;
        }
        private void PreSaveCustEdit(CClientCard.CClient nCust, EditCClient eCust, int Stage)
        {
            nCust.ID = eCust.ID;
            nCust.BirthDate = eCust.BirthDate;
            nCust.FirstName = eCust.FirstName;
            nCust.MiddleName = eCust.MiddleName;
            nCust.LastName = eCust.LastName;
            nCust.CreationDate = DateTime.Now.ToString();
            nCust.SnilsNumber = eCust.SnilsNumber;
            nCust.MobilePhone = eCust.MobilePhone;
            nCust.Phone = eCust.Phone;
            nCust.FiscalNumber = eCust.FiscalNumber;
            nCust.EMail = eCust.EMail;
            switch (eCust.Sex)
            {
                case "0":
                    nCust.Sex = "Male";
                    break;
                case "1":
                    nCust.Sex = "Female";
                    break;
            } 
            if (Stage == 1)    
              nCust.SendBy = (bool)eCust.SendBy;

            CClientCard.CAddress tAddress = new CClientCard.CAddress();
            tAddress.Appartment = eCust.Appartment;
            tAddress.Building = eCust.Building;
            tAddress.City = eCust.City;
            tAddress.Corp = eCust.Corp;
            tAddress.District = eCust.District;
            tAddress.DistrictArea = eCust.DistrictArea;
            tAddress.House = eCust.House;
            tAddress.Region = eCust.Region;
            tAddress.Street = eCust.Street;
            tAddress.Zip = eCust.Zip;
            tAddress.Other = eCust.Corp;
            nCust.ClientAddress = tAddress;

            CClientCard.CDocument tPassport = new CClientCard.CDocument();
            tPassport.Delivery = eCust.Delivery;
            tPassport.DeliveryDate = eCust.DeliveryDate;
            tPassport.DepartmentCode = eCust.DepartmentCode;
            tPassport.Number = eCust.Number;
            tPassport.Serie = eCust.Serie;
            nCust.Passport = tPassport;

            OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
            if (HttpContext.Session["OpsInfo"] != null)
            {
                _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
            }
            if (_opsInfo.OpsOperatorName != null)
                nCust.OperatorName = _opsInfo.OpsOperatorName;
            if (_opsInfo.OpsIndex != null)
                nCust.ShopNumber = _opsInfo.OpsIndex;
        }
        private void PreSaveCardEdit(CClientCard.CCard nCard, string cardNumber)
        {
            nCard.CardNumber = cardNumber;
            
        }
        private WebReport FitPage(CClientCard.CClient prCast, CClientCard.CCard prCard, string filepath)
        {
            WebReport webReport = new WebReport();
            OpsInfo.COpsInfo _opsInfo = new OpsInfo.COpsInfo();
            if (HttpContext.Session["OpsInfo"] != null)
            {
                _opsInfo = (OpsInfo.COpsInfo)HttpContext.Session["OpsInfo"];
            }
            webReport.Width = Unit.Percentage(100);
            webReport.Height = Unit.Percentage(100);
            webReport.ToolbarIconsStyle = ToolbarIconsStyle.Black;
            webReport.ShowToolbar = true;
            webReport.SinglePage = true;

            webReport.Report.Load(filepath);
            SetReportParamValue(webReport, "LastName", prCast.LastName.ToUpper());
            SetReportParamValue(webReport, "FirstName", prCast.FirstName.ToUpper());
            SetReportParamValue(webReport, "MiddleName", prCast.MiddleName == null ? "" : prCast.MiddleName.ToUpper());
            SetReportParamValue(webReport, "BirthDate", DateToDocFormat(prCast.BirthDate));
            if (prCast.Sex != null)
            {
                switch (prCast.Sex)
                {
                    case "Male":
                        SetReportParamValue(webReport, "Sex", "М");
                        break;
                    case "Female":
                        SetReportParamValue(webReport, "Sex", "Ж");
                        break;
                    default:
                        SetReportParamValue(webReport, "Sex", "");
                        break;
                }
            }
            else
            {
                SetReportParamValue(webReport, "Sex", "");
            }       
            SetReportParamValue(webReport, "Appartment", prCast.ClientAddress.Appartment);
            SetReportParamValue(webReport, "Building", prCast.ClientAddress.Building);
            SetReportParamValue(webReport, "City", prCast.ClientAddress.City);
            SetReportParamValue(webReport, "District", prCast.ClientAddress.District);
            SetReportParamValue(webReport, "DistrictArea", prCast.ClientAddress.DistrictArea);
            SetReportParamValue(webReport, "House", prCast.ClientAddress.House);
            SetReportParamValue(webReport, "Region", prCast.ClientAddress.Region);
            SetReportParamValue(webReport, "Street", prCast.ClientAddress.Street);
            SetReportParamValue(webReport, "Zip", prCast.ClientAddress.Zip);
            SetReportParamValue(webReport, "Other", prCast.ClientAddress.Other);
            if (prCast.Passport.Delivery != null)
                SetReportParamValue(webReport, "Delivery", prCast.Passport.Delivery.ToUpper());
            SetReportParamValue(webReport, "DeliveryDate", DateToDocFormat(prCast.Passport.DeliveryDate));
            if (prCast.Passport.DepartmentCode != null && prCast.Passport.DepartmentCode.Length > 6)
            {
                SetReportParamValue(webReport, "DepartmentCode", prCast.Passport.DepartmentCode);
            }
            else
            {
                SetReportParamValue(webReport, "DepartmentCode", "   -   ");
            }
            SetReportParamValue(webReport, "Number", prCast.Passport.Number);
            SetReportParamValue(webReport, "Serie", prCast.Passport.Serie);
            SetReportParamValue(webReport, "MobilePhone", SetMobileToPrintFormat(prCast.MobilePhone));
            SetReportParamValue(webReport, "Phone", prCast.Phone);
            SetReportParamValue(webReport, "EMail", prCast.EMail);
            if (prCast.ShopNumber != null && prCast.ShopNumber.Length >= 6)
            {
                this.SetReportParamValue(webReport, "ShopNumber", prCast.ShopNumber);
            }
            else
            {
                if(_opsInfo.OpsShopId != null)
                    SetReportParamValue(webReport, "ShopNumber", _opsInfo.OpsIndex);
            }
            SetReportParamValue(webReport, "CardNumber", prCard.CardNumber);
            if (_opsInfo.OpsOperatorName != null)
                SetReportParamValue(webReport, "OperatorName", _opsInfo.OpsOperatorName);
            SetReportParamValue(webReport, "SnilsNumber", prCast.SnilsNumber);
            SetReportParamValue(webReport, "InnNumber", prCast.FiscalNumber);

            prCast = null;
            prCard = null;
            return webReport;
        }
        private void SetReportParamValue(WebReport rep, string paramName, string value)
        {
            try
            {
                if (rep.Report.Parameters.FindByName(paramName) != null)
                {
                    rep.Report.SetParameterValue(paramName, value);
                }
            }
            catch
            {
            }
        }
        private string SetMobileToPrintFormat(string mPhone)
        {
            string newMobile = string.Empty;
            if (!string.IsNullOrEmpty(mPhone))
            {
                if (mPhone.Contains("+7 "))
                    mPhone = mPhone.Replace("+7 ", "");
                if (mPhone.Contains(" "))
                    mPhone = mPhone.Replace(" ", "");
                newMobile = mPhone;
            }
            return newMobile;
        }
        private List<dynamic> ConvertDataTable(DataTable tb)
        {
            var result = new List<dynamic>();
            foreach (DataRow row in tb.Rows)
            {
                var obj = (IDictionary<string, object>)new ExpandoObject();
                foreach (DataColumn col in tb.Columns)
                {
                    obj.Add(col.ColumnName, row[col.ColumnName]);
                }
                result.Add(obj);
            }
            return result;
        }
        private void FillEditCustFromSearch(ref EditCClient eCust, FilterCust fCust, string sId)
        {
            DataRow row = null;
            foreach (DataRow tr in fCust.resultCust.Rows)
            {
                if (tr["Id"].ToString().Equals(sId))
                { row = tr; }
            }
            DataRow rowPass = null;
            foreach (DataRow tr in fCust.resultCustPassport.Rows)
            {
                if (tr["CustId"].ToString().Equals(sId))
                { rowPass = tr; }
            }
            DataRow rowAddress = null;
            foreach (DataRow tr in fCust.resultCustAddress.Rows)
            {
                if (tr["CustId"].ToString().Equals(sId))
                { rowAddress = tr; }
            }

            eCust.ID = row["Id"].ToString();
            eCust.LastName = row["lastName"].ToString();
            eCust.FirstName = row["firstName"].ToString();
            eCust.MiddleName = row["middleName"].ToString();
            eCust.BirthDate = DateToPointFormat(row["birthDate"].ToString());
            if (fCust.resultCust.Columns.Contains("fiscalNumber"))
            {
                if (row["fiscalNumber"] != null)
                    eCust.FiscalNumber = row["fiscalNumber"].ToString();
                else
                    eCust.FiscalNumber = string.Empty;
            }
            else
                eCust.FiscalNumber = string.Empty;
            if (fCust.resultCust.Columns.Contains("snilsNumber"))
            {
                if (row["snilsNumber"] != null)
                    eCust.SnilsNumber = row["snilsNumber"].ToString();
                else
                    eCust.SnilsNumber = string.Empty;
            }
            else
                eCust.SnilsNumber = string.Empty;
            if (fCust.resultCust.Columns.Contains("sex"))
                eCust.Sex = row["sex"].ToString();
            else
                eCust.Sex = "2";
            if (fCust.resultCust.Columns.Contains("phone"))
            {
                if (row["phone"] != null)
                    eCust.Phone = row["phone"].ToString();
                else
                    eCust.Phone = string.Empty;
            }
            else
                eCust.Phone = string.Empty;
            if (fCust.resultCust.Columns.Contains("mobilephone"))
            {
                if (row["mobilephone"] != null)
                    eCust.MobilePhone = row["mobilephone"].ToString();
                else
                    eCust.MobilePhone = string.Empty;
            }
            else
                eCust.MobilePhone = string.Empty;
            if (fCust.resultCust.Columns.Contains("email"))
            {
                if (row["email"] != null)
                    eCust.EMail = row["email"].ToString();
                else
                    eCust.EMail = string.Empty;
            }
            else
                eCust.EMail = string.Empty;
            switch (row["sendCatalog"].ToString())
            {
                case "true":
                    eCust.SendBy = true;
                    break;
                case "false":
                    eCust.SendBy = false;
                    break;
                default:
                    eCust.SendBy = null;
                    break;
            }

            eCust.Serie = rowPass["passSerie"].ToString();
            eCust.Number = rowPass["passNumber"].ToString();
            eCust.Delivery = rowPass["delivery"].ToString();
            eCust.DeliveryDate = DateToPointFormat(rowPass["deliveryDate"].ToString());
            eCust.DepartmentCode = rowPass["departmentCode"].ToString();

            eCust.Zip = rowAddress["zip"].ToString();
            eCust.Region = rowAddress["region"].ToString();
            eCust.District = rowAddress["district"].ToString();
            eCust.DistrictArea = rowAddress["districtArea"].ToString();
            eCust.City = rowAddress["city"].ToString();
            eCust.Street = rowAddress["street"].ToString();
            eCust.House = rowAddress["house"].ToString();
            eCust.Building = rowAddress["building"].ToString();
            eCust.Appartment = rowAddress["appartment"].ToString();
            eCust.Other = rowAddress["other"].ToString();
            eCust.Corp = rowAddress["other"].ToString();

            string RegionCode = string.Empty;
            string CurrRegionName = GetCurrValueFromRegions(RegionList(), eCust.Region, out RegionCode);
            eCust.RegionCode = RegionCode;

            DataTable tTableC = new DataTable();

            DataTable tTableCA = new DataTable();
            tTableC = fCust.resultCard.Copy();

            tTableC.Columns.Add("flag");
            tTableCA = fCust.resultCardAction.Copy();
            tTableCA.Columns.Add("flag");

            foreach (DataRow tr in tTableC.Rows)
            {
                if (tr["CustId"].ToString().Equals(sId))
                {
                    string trc = tr["Id"].ToString();
                    foreach (DataRow trca in tTableCA.Rows)
                    {
                        if (trca["CardId"].ToString().Equals(trc))
                        {
                            trca["flag"] = "F";
                        }
                        else
                        {
                            if (trca["flag"].ToString() != "F")
                            {
                                trca["flag"] = "D";
                            }
                        }
                    }
                }
                else
                {
                    tr["flag"] = "D";
                }
            }
            tTableC.AcceptChanges();
            tTableCA.AcceptChanges();

            DataRow[] rowarr = tTableC.Select("flag = 'D'");
            for (int i = rowarr.Length - 1; i >= 0; i--)
            {
                tTableC.Rows.Remove(rowarr[i]);
            }
            rowarr = tTableCA.Select("flag = 'D'");
            for (int i = rowarr.Length - 1; i >= 0; i--)
            {
                tTableCA.Rows.Remove(rowarr[i]);
            }
            tTableC.Columns.Remove("flag");
            tTableCA.Columns.Remove("flag");
            tTableC.AcceptChanges();
            tTableCA.AcceptChanges();

            eCust.resultCard = tTableC;
            eCust.resultCardAction = tTableCA;
        }
        
        private T MergeCust<T>(T eCustSession, T eCust, NameValueCollection propsFromCard)
        {
            var typeObject = eCustSession.GetType();
            var properties = typeObject.GetProperties();
            foreach(var property in properties)
            {
                if(propsFromCard.Get(property.Name) != null)
                {
                    var valueNew = property.GetValue(eCust);
                    var valueOld = property.GetValue(eCustSession);
                    if ((valueNew != null && !valueNew.Equals(valueOld)) || (valueOld != null && !valueOld.Equals(valueNew)))
                    {
                        property.SetValue(eCustSession, valueNew);
                    }
                }
            }
            return eCustSession;
        }

        private T DownCustingClient<T>(Client client, T downCustClient)
        {
            var proporties = client.GetType().GetProperties();
            foreach(var prop in proporties)
            {
                prop.SetValue(downCustClient, prop.GetValue(client));
            }
            return downCustClient;
        }
        
        private ModifyCard FillModifyCardFromEdit(DataRow row)
        {
            ModifyCard mCard = new ModifyCard();
            mCard.CustId = row["CustId"].ToString();
            mCard.oldId = row["id"].ToString();
            mCard.oldNumber = row["number"].ToString();
            return mCard;
        }
        private string DateToDocFormat(string strDate)
        {
            string text = string.Empty;
            if (strDate.IndexOf("-") == 4)
            {
                text = strDate.Substring(8, 2) + strDate.Substring(5, 2) + strDate.Substring(0, 4);
            }
            else if (strDate.IndexOf("-") == 2)
            {
                text = strDate.Substring(0, 2) + strDate.Substring(3, 2) + strDate.Substring(6, 4);
            }
            else
            {
                if (strDate.Length > 10)
                {
                    text = strDate.Remove(10, strDate.Length - 10);
                }
                else
                {
                    text = strDate;
                }
                text = text.Replace(".", "");
            }
            return text;
        }
        private string DateToPointFormat(string strDate)
        {
            string text = string.Empty;
            if (strDate.IndexOf("-") == 4)
            {
                text = strDate.Substring(8, 2) +"."+ strDate.Substring(5, 2) + "." + strDate.Substring(0, 4);
            }
            else if (strDate.IndexOf("-") == 2)
            {
                text = strDate.Substring(0, 2) + "." + strDate.Substring(3, 2) + "." + strDate.Substring(6, 4);
            }
            else
            {
                if (strDate.Length > 10)
                {
                    text = strDate.Remove(10, strDate.Length - 10);
                }
                else
                {
                    text = strDate;
                }
            }
            return text;
        }
        private SelectList RegionList()
        {
            if (HttpContext.Session["regionList"] == null)
            {
                string path = ConfigurationManager.AppSettings["AddressDictionaryFile"];
                string path2 = ConfigurationManager.AppSettings["ResourcePath"];
                string fileName = Server.MapPath(path2 + '/' + path);

                var regionList = new AddressDictionary.RegionList(fileName);
                HttpContext.Session["regionList"] = regionList;
                var selectList = new SelectList(regionList, "Value", "Name");
                return selectList;
            }
            else
            {
                var selectList = new SelectList((AddressDictionary.RegionList)HttpContext.Session["regionList"], "Value", "Name");
                return selectList;
            }
        }
        private SelectList RegionList(string selectedValue)
        {
            if (HttpContext.Session["regionList"] == null)
            {
                string path = ConfigurationManager.AppSettings["AddressDictionaryFile"];
                string path2 = ConfigurationManager.AppSettings["ResourcePath"];
                string fileName = Server.MapPath(path2 + '/' + path);

                var regionList = new AddressDictionary.RegionList(fileName);
                var selectList = new SelectList(regionList, "Value", "Name", selectedValue);
                return selectList;
            }
            else
            {
                var selectList = new SelectList((AddressDictionary.RegionList)HttpContext.Session["regionList"], "Value", "Name", selectedValue);
                return selectList;
            }
        }
        private string GetCurrValueFromRegions(SelectList Regions, string CurrRegName, out string selectedReg)
        {
            string result = " ";
            selectedReg = string.Empty;
            foreach (var _region in Regions)
            {
                if (_region.Text.ToLower().Equals(CurrRegName.ToLower()))
                {
                    result = _region.Text;
                    selectedReg = _region.Value;
                }
            }
            return result;
        }
    }
}