"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var MessageController = /** @class */ (function () {
    function MessageController() {
    }
    MessageController.prototype.getEmailDiv = function (email) {
        var response = "<div class=\"panel panel-primary\">\n                    <a class=\"panel-heading\" href='@(Url.Action(\"Message\", \"Home\", new {id = " + email.Id + "))' style=\"display: block\" tabIndex=\"-1\">\n                        <b>Email # " + email.Id + "</b> <span class=\"pull-right\">Sent: " + email.SentDate + "</span>\n                    </a>\n                    <ul class=\"list-group\">\n                        <li class=\"list-group-item\">\n                            <div class=\"row\">\n                                <div class=\"col-sm-4\"><b class=\"text-primary\" style=\"margin-right: 10px;\">From:</b> " + email.From + "</div>\n\n                                <div class=\"col-sm-4\"><b class=\"text-primary\" style=\"margin-right: 5px;\">To:</b> " + email.To + "</div>\n\n                                <div class=\"col-sm-4\"><b class=\"text-primary\" style=\"margin-right: 5px;\">Cc:</b> " + email.Cc + "</div>\n\n                            </div>\n                        </li>\n                        <li class=\"list-group-item\">\n                            <div class=\"row\">\n\n                                <div class=\"col-sm-12\"><b class=\"text-primary\" style=\"margin-right: 5px;\">Subject:</b> " + email.Subject + "</div>\n                            </div>\n                        </li>";
        if (email.Attachments.Count > 0) {
            response += "<li class=\"list-group-item\" >\n            <div class=\"row\" >\n            <div class=\"col-sm-12\" >\n            <b class=\"text-primary\" style = \"margin-right: 5px;\" > Attachments: </b>";
            for (var i = 0; i < email.Attachments.Count; i++) {
                response +=
                    "<a href='@(Url.Action(\"Download\", \"Home\", new {id = email.Id, attachmentId = email.Attachments[i].Id}))' >@email.Attachments[i].Name(@email.Attachments[i].Size)</a>";
                if (i < email.Attachments.Count - 1) {
                    response += ", ";
                }
            }
            response += "</div>\n                < /div>\n            < /li>";
        }
        response += "<li class=\"list-group-item\" >";
        if (email.IsBodyHtml) {
            response += " @Html.Raw(" + email.Body + ")";
        }
        else {
            response += "<pre>" + email.Body + "</pre>";
        }
        response += "</li>\n                    </ul>\n                </div>";
        return response;
    };
    return MessageController;
}());
exports.MessageController = MessageController;
//# sourceMappingURL=message-controller.js.map