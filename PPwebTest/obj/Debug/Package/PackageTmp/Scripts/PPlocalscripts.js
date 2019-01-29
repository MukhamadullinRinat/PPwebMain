function SendByClick() {
    var rad = document.getElementsByName('radSendBy');
    if (rad != null)
    {
        for (var i = 0; i < rad.length; i++) {
            if (rad[i].checked) {
                document.getElementById('btnSave').disabled = false;;
            }
        }
    }    
};
function showHideRows(cardId) {
    var trCollection = $(document).find('.gridAct tr:not(.header)');
    $(trCollection).each(function (index, row) {
        if ($(row).find('div.grid' + cardId).length) {
            $(row).show();
        }
        else {
            $(row).hide();
        }
    });
}

function clickToCard(cardId, rowsClassName) {
    bodyMask(true);
    setRowsColor(cardId, rowsClassName);
    var isVBB = getVisibilityBlockButton(cardId, rowsClassName);
    var isVRB = getVisibilityReplaceButton(cardId, rowsClassName);
    var isENCB = getEnableNewCardButton(cardId, rowsClassName);
    var isActiveCardInTable = searchActiveCard();
    var isAllCardBlokedInTable = searchBlokedCard();
    //isVRB = isActiveCardInTable ? isVRB = false : isVRB;
    isENCB = isActiveCardInTable ? isENCB = false : isENCB;
    isENCB = isAllCardBlokedInTable;
    $('#cardActions').load("SearchCardActionsResult?cardId=" + cardId, function () {
        bodyMask(false);
        setTips("#tipsContainer3", "#tipsContainer4", "Чтобы вернуться к <b>Данным клиента</b>, прокрутите экран вверх.")
    });
    !isVBB && isVRB ? rebildButtonsContainer() : bildButtonsContainer(), setVisibilityBlockButton(isVBB);
    setVisibilityReplaceButton(isVRB);
    setEnableNewCardButton(isENCB);
}

function setVisibilityReplaceButton(isVisible) {
    var replaceButoon = $('.blockAndReplaceButton').filter('#replaceCardsButton');
    isVisible ? $(replaceButoon).css("visibility", "visible") : $(replaceButoon).css("visibility", "hidden");
}

function setVisibilityBlockButton(isVisible) {
    var blockButoon = $('.blockAndReplaceButton').filter('#blockCardsButton');
    isVisible ? $(blockButoon).css("visibility", "visible") : $(blockButoon).css("visibility", "hidden");
}

function setEnableNewCardButton(isVisible) {
    isVisible ? $('.clientsCards #issueCardButton').removeClass('disabled notClickable') : $('.clientsCards #issueCardButton').addClass('disabled notClickable');
}

function clickToClient(clientId, rowsClassName, cardId) {
    bodyMask(true);
    $('#clientInfo').load("GetClientInfo?clientId=" + clientId, function () {
        if (cardId) {
            var isDefaindedCard = false;
            $('.clientsCards table input').val(function (index, value) {
                if (value === cardId)
                    isDefaindedCard = true;
                return value;
            });
            if (isDefaindedCard)
                clickToCard(cardId, 'gridCardResults');
        }
        bodyMask(false);
        setTips("#tipsContainer2", "#tipsContainer3", "Выберите карту в <b>Картах клиента</b> для отображения истории операций по карте.")
    });
    setRowsColor(clientId, rowsClassName);
}

function openNewClientModal() {
    showVerticalLine(false);
    bodyMask(true);
    $('#modalBoxContainer').load("NewCust", function () {
        bodyMask(false);
        $('#clientModalBox').on('shown.bs.modal', function (e) {
            setMaskClientModal();
            var heightButtons = $('.buttonWithInput').css("height");
            $('.inputWithButton').css("height", heightButtons);
            $('#radSendBy1').attr("disabled", true);
            $('#radSendBy2').attr("disabled", true);
            $('#modalBoxContainer .blockedAfterShow').attr("disabled", true);
            $('#modalBoxContainer .blockedAfterShow').filter('button').addClass('notClickable');
            bildDateField('BirthDate');
            bildDateField('DeliveryDate');
        });
        $('#clientModalBox').on('hide.bs.modal', function (e) {
            showVerticalLine(true);
            $('#modalBoxContainer').html("");
        });
        $('#clientModalBox').modal('show');
    });
}

function openEditCustModalBox() {
    showVerticalLine(false);
    bodyMask(true);
    $('#modalBoxContainer').load("EditCust", function () {
        bodyMask(false);
        $('#clientModalBox').on('shown.bs.modal', function (e) {
            setMaskClientModal();
            $('#clientModalBox #radSendBy1').attr("disabled", true);
            $('#clientModalBox #radSendBy2').attr("disabled", true);
            var isSendBy = $('#clientModalBox #isSendBy').val();
            isSendBy === "" ? "" : isSendBy === "True" ? $('#editClientModalBox #radSendBy1').prop("checked", true) : $('#editClientModalBox #radSendBy2').prop("checked", true);
            bildDateField('BirthDate');
            bildDateField('DeliveryDate');
        });
        $('#clientModalBox').on('hide.bs.modal', function (e) {
            showVerticalLine(true);
            $('#modalBoxContainer').html("");
        });
        $('#clientModalBox').modal('show');
    });
}
function openNewCardModalBox() {
    showVerticalLine(false);
    bodyMask(true);
    //var cardName = getSelectedCardName().replace(" ", "_");
    $('#modalBoxContainer').load("NewCard", function (e) {
        bodyMask(false);
        $('#clientModalBox').on('shown.bs.modal', function (e) {
            setMaskClientModal();
            var heightButtons = $('.buttonWithInput').css("height");
            $('.inputWithButton').css("height", heightButtons);
            $('#radSendBy1').attr("disabled", true);
            $('#radSendBy2').attr("disabled", true);
            $('#modalBoxContainer .blockedAfterShow').attr("disabled", true);
            $('#modalBoxContainer .blockedAfterShow').filter('button').addClass('notClickable');
            bildDateField('BirthDate');
            bildDateField('DeliveryDate');
        });
        $('#clientModalBox').on('hide.bs.modal', function (e) {
            showVerticalLine(true);
            $('#modalBoxContainer').html("");
        });
        $('#clientModalBox').modal('show');
    });
}

function openBlockCardModalBox() {
    showVerticalLine(false);
    bodyMask(true);
    var number = getSelectedCardNumber();
    $('#modalBoxContainer').load("BlockCard?number=" + number, function () {
        bodyMask(false);
        $('#blockCardModalBox').on('shown.bs.modal', function (e) {
            $('#blockCardModalBox #Reason').addClass('defaultValueInSelectList');
            $('#blockCardModalBox option').each(function (index, elem) {
                if ($(elem).val() === "")
                    $(elem).addClass('defaultValueInSelectList');
                else
                    $(elem).addClass('optionalValueInSelectList');
            });

            $('#blockCardModalBox #Comment').attr("disabled", true);

            $('#blockCardModalBox #Reason').on("change", function (e) {
                var value = $('#blockCardModalBox #Reason').val();
                if (value === "") {
                    $('#blockCardModalBox #Reason').addClass('defaultValueInSelectList');
                    $('#blockCardModalBox #blockCardButton').addClass('notClickable disabled');
                }
                else {
                    $('#blockCardModalBox #Reason').removeClass('defaultValueInSelectList');
                    $('#blockCardModalBox #blockCardButton').removeClass('notClickable disabled');
                }

                if (value === "ANOTHER") {
                    $('#blockCardModalBox .commentsFields').removeClass("displayNone");
                    $('#blockCardModalBox #Comment').attr("disabled", false);
                }
                else {
                    $('#blockCardModalBox .commentsFields').addClass("displayNone");
                    $('#blockCardModalBox #Comment').attr("disabled", true);
                }
            });
        });
        $('#blockCardModalBox').on('hide.bs.modal', function (e) {
            showVerticalLine(true);
            $('#modalBoxContainer').html("");
        });
        $('#blockCardModalBox').modal('show');
    });
}

function setRowsColor(selectedId, rowsClassName) {
    var allGrid = $('.' + rowsClassName).each(function (index, element) {
        if (element.id === "g_" + selectedId) {
            $(element).addClass("selectedGridResults");
        }
        else
            $(element).removeClass("selectedGridResults");
    });
}

function openReplaceCardModalBox() {
    showVerticalLine(false);
    bodyMask(true);
    var cardNumber = getSelectedCardNumber();
    var oldId = getSelectedCardId();
    $('#modalBoxContainer').load("GetReplaceCardView?number=" + cardNumber + "&Id=" + oldId, function () {
        bodyMask(false);
        $('#replaceCardModalBox').on('shown.bs.modal', function (e) {
            var heightButtons = $('#checkingCard').css("height");
            $('#newNumber').css("height", heightButtons);
        });
        $('#replaceCardModalBox').on('hide.bs.modal', function (e) {
            $('#modalBoxContainer').html("");
            showVerticalLine(true);
        });
        $('#replaceCardModalBox').modal('show');
    });
}

function getSelectedCardNumber() {
    var selectedCard = $('.gridCardResults.selectedGridResults')[0];
    var cardNumber = $(selectedCard).find('.leftTd')[0].innerText;
    return cardNumber;
}

function getSelectedCardName() {
    var selectedCard = $('.gridCardResults.selectedGridResults')[0];
    var cardName = $(selectedCard).find('.cardName')[0].innerText;
    return cardName;
}


function hideModalBox() {
    $('#modalBoxContainer').children('div').modal('hide');
}

function getSelectedCardId() {
    var selectedCard = $('.gridCardResults.selectedGridResults')[0];
    if (selectedCard !== undefined) {
    var cardId = $(selectedCard).find('input')[0].value;
    return cardId;
}
    return null;
}

function clickToTypeSearch(activeTabId) {
    $('#searchContainer input').val("").removeClass('input-validation-error');
    var unActiveItem = "",
        activeItem = "";

    var unActiveTabId = activeTabId === "#cardTab" ?
        (activeItem = "#cardsRow", unActiveItem = "#passportRow", "#passportTab") :
        (activeItem = "#passportRow", unActiveItem = "#cardsRow", "#cardTab");

    $(unActiveTabId).removeClass('activeTab');
    $(activeTabId).addClass('activeTab');
    $(activeItem).removeClass('displayNone');
    $(unActiveItem).addClass('displayNone');
    $('#searchContainer span').html('');
    if ($('#searchResultClients .erroreMsgContainer').length > 0) $('#searchResultClients').html("");
}

function clickToCheckButton() {
    var cardNumber = $('#CardNumber')[0].value;
    $('#infoMsgContainer').load("CheckingNewCust?cardNumber=" + cardNumber);
}

function clearnSearchForm() {
    $('#searchContainer input').val(function (index, elem) {
        return "";
    });
    $('#searchContainer input').removeClass('input-validation-error');
    $('#searchContainer span').html('');
    $('#searchResultClients').html("");
    setTips(null, "#tipsContainer1", "Выберите тип <b>Поиска клиента</b> и заполните поля данных клиента.");
}

function insertClientInfo(clientInfo) {
    for (var item in clientInfo) {
        if (item !== "SendBy" && item !== "Sex")
            $('#newClientModalBox #' + item).val(clientInfo[item]);
        else if (item === "SendBy") {
            if (clientInfo[item] === null)
                continue;
            clientInfo[item] ? $('#newClientModalBox #radSendBy1').prop("checked", true) : $('#newClientModalBox #radSendBy2').prop("checked", true);
        }
        else if (item === "Sex") {
            clientInfo[item] !== "Female" ? $('#newClientModalBox #Sex0').prop("checked", true) : $('#newClientModalBox #Sex1').prop("checked", true);
        }
    }
}

function checkingReplaceResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#replaceCardModalBox #checkingReplaceCardMsgContainer .erroreMsg').html(erroreMsg);
        $('#replaceCardModalBox #checkingReplaceCardMsgContainer .infoMsg').html("");
        if (!$('#replaceCardModalBox #replaceCardButton').hasClass("disabled notClickable"))
            $('#replaceCardModalBox #replaceCardButton').addClass("disabled notClickable");
    }
    else {
        $('#replaceCardModalBox #checkingReplaceCardMsgContainer .infoMsg').html(infoMsg);
        $('#replaceCardModalBox #checkingReplaceCardMsgContainer .erroreMsg').html("");
        $('#replaceCardModalBox #replaceCardButton').removeClass("disabled notClickable");
    }
}

function replaceResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#replaceCardModalBox #replaceCardMsgContainer .erroreMsg').html(erroreMsg);
    }
    else {
        $('#replaceCardModalBox #replaceCardMsgContainer .infoMsg').html(infoMsg);
        reloadGrid();
        $('#replaceCardModalBox').modal('hide');
    }
}

function saveClientResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#clientModalBox #saveMsgContainer .erroreMsg').html(erroreMsg);
    }
    else {
        $('#clientModalBox #saveMsgContainer .infoMsg').html(infoMsg);
        if ($('#searchResultClients .table').length !== 0)
            reloadGrid();
        $('#clientModalBox').modal('hide');
    }
}

var oldCardNumber = "";
function checkingClientCardResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#clientModalBox #checkMsgContainer .erroreMsg').html(erroreMsg);
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", true);
        $('#modalBoxContainer .blockedAfterShow').filter('button').addClass('notClickable');
        $('#clientModalBox #radSendBy1').attr("disabled", true);
        $('#clientModalBox #radSendBy2').attr("disabled", true);
        $('#clientModalBox #saveButton').addClass("disabled notClickable");
        $('#clientModalBox #checkMsgContainer .infoMsg').html("");
        $('#clientModalBox .inputWithButton').off("input");
    }
    else {
        var params = infoMsg.split('&');
        if (params.length > 1) {
            $('#clientModalBox #checkMsgContainer .infoMsg').html("Карта \"" + params[0] + "\", статус \"" + params[1] + "\"");
        }
        else {
            $('#clientModalBox #checkMsgContainer .infoMsg').html(infoMsg);
        }
        $('#clientModalBox #checkMsgContainer .erroreMsg').html("");
        oldCardNumber = $('#clientModalBox .inputWithButton').val();
        $('#clientModalBox .inputWithButton').on("input", function (e) {
            disableFields($(this).val());
        });
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", false);
        $('#modalBoxContainer .blockedAfterShow').filter('button').removeClass('notClickable');
    }
}

function printResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#clientModalBox #saveMsgContainer .erroreMsg').html(erroreMsg);
    }
    else {
        var param = "menubar=no,location=no,resizable=yes,scrollbars=yes,status=yes"
        window.open(infoMsg, "Печать", param);
        $('#clientModalBox #radSendBy1').attr("disabled", false);
        $('#clientModalBox #radSendBy2').attr("disabled", false);
        $('#clientModalBox #saveButton').removeClass("disabled notClickable");
        $('#clientModalBox .inputWithButton').on("input", function (e) {
            disableAllFields($(this).val());
        });
    }
}

function blockCardResponse(erroreMsg, infoMsg) {
    bodyMask(false);
    if (erroreMsg) {
        $('#blockCardModalBox #blockCardMsgContainer .erroreMsg').html(erroreMsg);
    }
    else {
        $('#blockCardModalBox #blockCardMsgContainer .infoMsg').html(infoMsg);
        reloadGrid();
        $('#blockCardModalBox').modal('hide');
    }
}

function reloadGrid() {
    var selectedCardId = getSelectedCardId();
    var selectedClientId = getSelectedClientId();
    $.post("MainForm", { btnComm: "runSearch", isReloadGrid: true }, function (view) {
        $('#searchResultClients').html(view);
        clickToClient(selectedClientId, 'gridClientResults', selectedCardId);
    });
}

function getSelectedClientId() {
    var selectedClient = $('.gridClientResults').filter('.selectedGridResults');
    var clientId = $(selectedClient).find('.clientId').html();
    return clientId;
}

function normalizeCardCreateDate(tableClass) {
    $(tableClass + " .activationDate").each(function (index, elem) {
        if ($(elem).html().indexOf('T') !== -1) {
            var arrDate = $(elem).html().split('T');
            var date = normalizeDate(arrDate[0]);
            date += " " + arrDate[1].substring(0, 8);
            $(elem).html(date);
        }
    });
}

function normalizeDate(serverDate) {
    if (serverDate !== null) {
        var normalDate = serverDate.split('-').reverse().join('.');
        return normalDate;
    }
}

var responseIsSend = false;

function bodyMask(isShow, isRecursive) {
    isRecursive = isRecursive === undefined ? null : isRecursive;
    var validateErrore = $('.field-validation-error').length;

    if (isShow && responseIsSend) {
        responseIsSend = false;
        $('#preloaderContainer').html("");
    }
    else if (isShow && validateErrore !== 0) {
        $('#preloaderContainer').html("");
        responseIsSend = false;
    }

    else if (isShow && isRecursive) {
        setTimeout(function () {
            bodyMask(true, true);
        }, 500);
    }
    else if (isShow) {
        var htmlText = "<div id=\"bodyMask\">" +
            "<img id=\"loader\" class=\"centered\" src=\"../Content/Images/ajax-loader.gif\" alt=\"Loading, Loading!\" />" +
            "</div>";
        $('#preloaderContainer').html(htmlText);
        bodyMask(true, true);
    }
    else {
        $('#preloaderContainer').html("");
        responseIsSend = true;
    }
}

$(document).ready(function () {
    $('#searchContainer #fPassSeria').mask("?9999").on("change", function () {
        $('#searchContainer #fPassSeriaNumber').val($(this).val() + " " + $('#searchContainer #fPassNumber').val());
    });
    $('#searchContainer #fPassNumber').mask("?999999").on("change", function () {
        $('#searchContainer #fPassSeriaNumber').val($('#searchContainer #fPassSeria').val() + " " + $(this).val());
    });
});

function setMaskClientModal() {
    $('#clientModalBox #MobilePhone').mask("+7 ?999 999 99 99");
    $('#clientModalBox #SnilsNumber').mask("?999-999-999 99");
    $('#clientModalBox #FiscalNumber').mask("?999999999999");
    $('#clientModalBox #Serie').mask("?9999");
    $('#clientModalBox #Number').mask("?999999");
    $('#clientModalBox #DepartmentCode').mask("?999-999");
    $('#clientModalBox #Zip').mask("?999999");
}

function getTipsHtml(text) {
    return "<div class=\"tips\">" +
        "<p>" + text + "</p>" +
        "</div >";
}

function setTips(deleteContainerId, insertContainerId, text) {
    if (deleteContainerId)
        $(deleteContainerId).html("");
    $(insertContainerId).html(getTipsHtml(text));
}

function showVerticalLine(isShow) {
    isShow ? $('#verticalLineContainer').html("<div id=\"verticalLine\"></div>") : $('#verticalLineContainer').html("");
}

function getVisibilityReplaceButton(cardId, rowsClassName) {
    var selectedRow = $('.' + rowsClassName).filter('.selectedGridResults');
    var cardName = $(selectedRow).find('.cardName').html();
    var cardNumber = $(selectedRow).find('.leftTd').html();
    return ((cardName === "Почтовый паспорт" || cardName === "Любимый клиент") && cardNumber !== "Отсутствует у клиента");
}

function getVisibilityBlockButton(cardId, rowsClassName) {
    var selectedRow = $('.' + rowsClassName).filter('.selectedGridResults');
    var cardStatus = $(selectedRow).find('.rightTd').html();
    return cardStatus === "Активна";
}

function getEnableNewCardButton(cardId, rowsClassName) {
    var selectedRow = $('.' + rowsClassName).filter('.selectedGridResults');
    var cardName = $(selectedRow).find('.cardName').html();
    var cardNumber = $(selectedRow).find('.leftTd').html();
    var cardStatus = $(selectedRow).find('.rightTd').html();
    return (cardName === "Почтовый паспорт" && (cardNumber === "Отсутствует у клиента" || cardStatus === "Заблокирована"));
}

function rebildButtonsContainer() {
    var htmlText = "<button type=\"button\" class=\"btn-default btn col-lg-3 blockAndReplaceButton\" id=\"replaceCardsButton\" onclick=\"openReplaceCardModalBox()\">Заменить карту</button>" +
        "<button id=\"issueCardButton\" type=\"button\" class=\"btn-primary btn col-lg-3 col-lg-offset-6 disabled notClickable\" onclick=\"openNewCardModalBox()\">Оформить карту</button>";
    $('.clientsCards .buttonsContainer').html(htmlText);
}

function bildButtonsContainer() {
    var htmlText = "<button type=\"button\" class=\"btn-default btn col-lg-3 blockAndReplaceButton\" id=\"blockCardsButton\" onclick=\"openBlockCardModalBox()\">Блокировать карту</button>" +
        "<button type=\"button\" class=\"btn-default btn col-lg-3 toRight10px blockAndReplaceButton\" id=\"replaceCardsButton\" onclick=\"openReplaceCardModalBox()\">Заменить карту</button>" +
        "<button id=\"issueCardButton\" type=\"button\" class=\"btn-primary btn col-lg-3 col-lg-offset-3 disabled notClickable\" onclick=\"openNewCardModalBox()\">Оформить карту</button>";
    $('.clientsCards .buttonsContainer').html(htmlText);
}

function searchActiveCard() {
    var isActive = false;
    $('.cardsTable tr').each(function (index, elem) {
        if(!isActive)
            isActive = $(elem).find('.cardName').html() === "Почтовый паспорт" && $(elem).find('.rightTd').html() === "Активна";
    });
    return isActive;
}

function searchBlokedCard() {
    if (!searchActiveCard()) {
        var isBloked = true;
        $('.cardsTable tr').each(function (index, elem) {
            if (isBloked && $(elem).find('.cardName').html() === "Почтовый паспорт")
                isBloked = $(elem).find('.rightTd').html() === "Заблокирована" || $(elem).find('.rightTd').html() === "-";
        });
        return isBloked;
    }
    else return false;
}


function disableFields(newCardNumber) {
    if (newCardNumber !== oldCardNumber) {
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", true);
        $('#modalBoxContainer .blockedAfterShow').filter('button').addClass('notClickable');
    }
    else {
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", false);
        $('#modalBoxContainer .blockedAfterShow').filter('button').removeClass('notClickable');
    }
}

function disableAllFields(newCardNumber) {
    if (newCardNumber !== oldCardNumber) {
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", true);
        $('#modalBoxContainer .blockedAfterShow').filter('button').addClass('notClickable');
        $('#clientModalBox #radSendBy1').attr("disabled", true);
        $('#clientModalBox #radSendBy2').attr("disabled", true);
        $('#clientModalBox #saveButton').addClass("disabled notClickable");
    }
    else {
        $('#modalBoxContainer .blockedAfterShow').attr("disabled", false);
        $('#modalBoxContainer .blockedAfterShow').filter('button').removeClass('notClickable');
        $('#clientModalBox #radSendBy1').attr("disabled", false);
        $('#clientModalBox #radSendBy2').attr("disabled", false);
        $('#clientModalBox #saveButton').removeClass("disabled notClickable");
    }
}

function UniqPassCheck() {
    var lCustID = $('#clientModalBox #custId').val();
    if (lCustID === null || lCustID === "")
    { lCustID = "new"; }

    var lSerie = $('#clientModalBox #Serie').val();
    var lNumber = $('#clientModalBox #Number').val();
    if (lSerie !== null && lNumber !== null) {
        if (lSerie.length === 4 && lNumber.length === 6) {
            $('#clientModalBox #PassportForValid').val(lCustID + "_" + lSerie + "_" + lNumber);
            var lPassControl = $('#clientModalBox #PassportForValid').val();
            $('#clientModalBox #PassportForValid').load("ValidPassportUniq?PassportForValid=" + lPassControl);
        }
    }
};

function UniqPassCheckMessage(errMsg) {
    bodyMask(false);
    $('#clientModalBox #checkingPassportUniqMsgContainer .errPassMsg').html("");
    if (errMsg !== null) {
        if (errMsg !== "null") {
            $('#clientModalBox #checkingPassportUniqMsgContainer .errPassMsg').html(errMsg);
            $('#clientModalBox #printButton').addClass("disabled notClickable");
        } else {
            if ($('#clientModalBox #printButton').hasClass(["disabled"])) {
                $('#clientModalBox #printButton').removeClass("disabled notClickable");
            }
        }
    } else {
        if ($('#clientModalBox #printButton').hasClass(["disabled"])) {
            $('#clientModalBox #printButton').removeClass("disabled notClickable");
        }
    }
};

function setCurrentDate() {
    var currentDate = new Date();
    var date = (currentDate.getDate() < 10 ? "0" + currentDate.getDate() : currentDate.getDate())
        + "." + (currentDate.getMonth() + 1 < 10 ? "0" + (currentDate.getMonth() + 1) : currentDate.getMonth() + 1)
        + "." + currentDate.getFullYear() + ".";
    $('.currentDate').html(date);
    var time = (currentDate.getHours() < 10 ? "0" + currentDate.getHours() : currentDate.getHours()) + ":" +
        (currentDate.getMinutes() < 10 ? "0" + currentDate.getMinutes() : currentDate.getMinutes());
    $('.currentTime').html(time);
    var interval = 60000 - currentDate.getSeconds()*1000;
    setTimeout(function () {
        setCurrentDate();
    }, interval);
}
$(document).ready(function () {
    setCurrentDate();
});

$(document).ready(function () {
    bildDateField('fBirthDate');
});

function bildDateField(id) {
    $('#' + id).datepicker({
        dateFormat: "dd.mm.yy",
        changeYear: true,
        changeMonth: true,
        dayNamesMin: ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"],
        monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
        monthNamesShort: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
        yearRange: getYearRange(id)
    });
    $('#' + id).mask("?99.99.9999");

    $('#' + id).on("change", function (e) {
        $('.' + id + 'Validate').load(getRouteValidate(id) + $(this).val(), function (text) {
            if ($('#' + id).val() === "") {
                $(this).html("");
                $('#' + id).removeClass("input-validation-error");
            }
            else if (text == "true") {
                $(this).html("");
                $('#' + id).removeClass("input-validation-error");
            }
            else {
                $(this).html(text.replace("\"", "").replace("\"", ""));
                $('#' + id).addClass("input-validation-error");
            }
        });
    });
}

function getYearRange(id) {
    return id === "fBirthDate" || id === "BirthDate" ? "-100: -14" : "-100: -0";
}

function getRouteValidate(id) {
    return id === "fBirthDate" ? 'ValidFilterBirthDate?fBirthDate=' : id === "BirthDate" ? 'ValidBirthDate?BirthDate=' : 'ValidPassportDate?DeliveryDate=';
}

//document.body.scrollTop = document.body.scrollHeight;
function openConfirmedModalBox() {
    $('#confirmationModalBoxContainer').load('GetConfirmedView', function () {
        $('#confirmationModalBox').modal('show');
    });
}

function closeClientModalBox() {
    $('#modalBoxContainer div').modal('hide');
}

$(function () {
    jQuery.validator.addMethod('validpassseria', function (value, element, params) {
        var number = $('#fPassNumber').val();
        if (value === "____" || value === "") {
            if (number === "______" || number === "") {
                $('#fPassNumber').removeClass('input-validation-error');
                $('#fPassNumber-error').remove();
            }
            return number === "______" || number === "";
        }
        return true;
    }, '');

    jQuery.validator.unobtrusive.adapters.add('validpassseria', function (options) {
        options.rules['validpassseria'] = {};
        options.messages['validpassseria'] = options.message;
    });


    jQuery.validator.addMethod('validpassnumber', function (value, element, params) {
        var seria = $('#fPassSeria').val();
        if (value === "______" || value === "") {
            if (seria === "____" || seria === "") {
                $('#fPassSeria').removeClass('input-validation-error');
                $('#fPassSeria-error').remove();
            }
            return seria === "____" || seria === "";
        }
        return true;
    }, '');

    jQuery.validator.unobtrusive.adapters.add('validpassnumber', function (options) {
        options.rules['validpassnumber'] = {};
        options.messages['validpassnumber'] = options.message;
    });

}(jQuery));

function OnChangeFederal() {
    RequiredCity();
    var RegionCode = $('#clientModalBox #RegionCode').val();
    var City = $('#clientModalBox #City').val();

    if (RegionCode != undefined && RegionCode != "" && RegionCode != " ") {
        $(this).load("CheckFederal?RegionCode=" + encodeURI(RegionCode), function (text) {
            if (text == "true") {
                $('#clientModalBox #District').val(" ");
                $('#clientModalBox #District').attr("disabled", true);
                $('#clientModalBox #City').attr("required", false);
                if (City != undefined && City != "") {
                    $(this).load("CheckCityFederal?RegionCode=" + encodeURI(RegionCode) + "&City=" + encodeURI(City), function (text2) {
                        if (text2 == "true") {

                            if ($('#clientModalBox #City').hasClass("valid"))
                                $('#clientModalBox #City').removeClass("valid");
                            $('#clientModalBox #City').attr("data-val", true);
                            $('#clientModalBox #City').addClass("input-validation-error");
                            if ($('.CityValidate').hasClass("field-validation-valid"))
                                $('.CityValidate').removeClass("field-validation-valid");
                            $('.CityValidate').addClass("field-validation-error");
                            $('.CityValidate').html('Значение полей Регион и Населенный пункт не должны совпадать');
                            $('#clientModalBox #printButton').addClass("disabled notClickable");
                        }
                        else {
                            ReplaceCityValid();
                        }
                    });
                }
                else {
                    ReplaceCityValid();
                }
            }
            else {
                ReplaceCityValid();
                $('#clientModalBox #District').attr("disabled", false);
                RequiredCity();
                if ($('#clientModalBox #printButton').hasClass("disabled"))
                    $('#clientModalBox #printButton').removeClass("disabled notClickable");
            }
        });
    }
}
function ReplaceCityValid() {
    if ($('#clientModalBox #City').hasClass("input-validation-error"))
        $('#clientModalBox #City').removeClass("input-validation-error");
    $('.CityValidate').html("");
    if ($('.CityValidate').hasClass("field-validation-error"))
        $('.CityValidate').removeClass("field-validation-error");
    $('.CityValidate').addClass("field-validation-valid");
    if ($('#clientModalBox #printButton').hasClass("disabled"))
        $('#clientModalBox #printButton').removeClass("disabled notClickable");
}
function RequiredCity() {
    $('#clientModalBox #City').attr("required", true);
    $('#clientModalBox #City').attr("data-val", true);
    $('#clientModalBox #City').attr("data-val-required", "Поле является обязательным для заполнения");
}