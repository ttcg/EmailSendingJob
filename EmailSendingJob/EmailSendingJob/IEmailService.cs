﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSendingJob
{
    public interface IEmailService
    {
        void SendEmail(string email);
    }
}
