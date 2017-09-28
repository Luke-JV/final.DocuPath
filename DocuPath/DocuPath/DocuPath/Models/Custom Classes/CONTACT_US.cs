using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models.Custom_Classes
{
    public class CONTACT_US
    {
        // Members:
        [Required]
        private string mNameSurname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        private string mEmailAddress { get; set; }

        [Required]
        private string mSubject { get; set; }

        [Required]
        private string mMessage { get; set; }

        // Properties:
        public string NameSurname { get { return mNameSurname; } set { mNameSurname = value; } }
        public string EmailAddress { get { return mEmailAddress; } set { mEmailAddress = value; } }
        public string Subject { get { return mSubject; } set { mSubject = value; } }
        public string Message { get { return mMessage; } set { mMessage = value; } }

        // Default Constructor:
        public CONTACT_US()
        {
            NameSurname = "";
            EmailAddress = "";
            Subject = "";
            Message = "";
        }

        public CONTACT_US(string inNameSurname, string inEmailAddress, string inSubject, string inMessage)
        {
            NameSurname = inNameSurname;
            EmailAddress = inEmailAddress;
            Subject = inSubject;
            Message = inMessage;
        }
    }
}