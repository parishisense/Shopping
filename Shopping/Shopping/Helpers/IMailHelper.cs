﻿using Shopping.Common;

namespace Shopping.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subjet, string body);
    }
}
