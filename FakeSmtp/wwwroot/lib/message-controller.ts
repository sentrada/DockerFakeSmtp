export class MessageController {


    function

    getEmailDiv(email: any): string {

        var response = `<div class="panel panel-primary">
                    <a class="panel-heading" href='@(Url.Action("Message", "Home", new {id = ${email.Id
            }))' style="display: block" tabIndex="-1">
                        <b>Email # ${email.Id}</b> <span class="pull-right">Sent: ${email.SentDate}</span>
                    </a>
                    <ul class="list-group">
                        <li class="list-group-item">
                            <div class="row">
                                <div class="col-sm-4"><b class="text-primary" style="margin-right: 10px;">From:</b> ${
            email.From}</div>

                                <div class="col-sm-4"><b class="text-primary" style="margin-right: 5px;">To:</b> ${
            email.To}</div>

                                <div class="col-sm-4"><b class="text-primary" style="margin-right: 5px;">Cc:</b> ${
            email.Cc}</div>

                            </div>
                        </li>
                        <li class="list-group-item">
                            <div class="row">

                                <div class="col-sm-12"><b class="text-primary" style="margin-right: 5px;">Subject:</b> ${
            email.Subject}</div>
                            </div>
                        </li>`;
        if (email.Attachments.Count > 0) {
            response += `<li class="list-group-item" >
            <div class="row" >
            <div class="col-sm-12" >
            <b class="text-primary" style = "margin-right: 5px;" > Attachments: </b>`;

            for (var i = 0; i < email.Attachments.Count; i++) {
                response +=
                    `<a href='@(Url.Action("Download", "Home", new {id = email.Id, attachmentId = email.Attachments[i].Id}))' >@email.Attachments[i].Name(@email.Attachments[i].Size)</a>`;
                if (i < email.Attachments.Count - 1) {
                    response += `, `;
                }
            }
            response += `</div>
                < /div>
            < /li>`;
        }
        response += `<li class="list-group-item" >`;
        if (email.IsBodyHtml) {
            response += ` @Html.Raw(${email.Body})`;
        } else {
            response += `<pre>${email.Body}</pre>`;
        }
        response += `</li>
                    </ul>
                </div>`;

        return response;
    }
}