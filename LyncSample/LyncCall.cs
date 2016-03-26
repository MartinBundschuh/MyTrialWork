using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Extensibility;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace LyncSample
{
    /// <summary>
    /// Class wich u can use to start Lync Calls and Conferecnes
    /// </summary>
    public static class LyncCall
    {
        #region Call Lync-registered contact(s)
        /// <summary>
        /// Starts a phone call with one or more Lync-registered contacts.
        /// </summary>
        /// <param name="contactMailAddresses">E-Mail Adresses of Lync-registered contacts.</param>
        public static void CallRegisteredUser(List<MailAddress> contactMailAddresses)
        {
            if (contactMailAddresses == null || contactMailAddresses.Count == 0)
                throw new InvalidEMailException("Invalid Argument: No given E-Mail addresses.");

            var contactMailAddressesString = new List<string>();

            foreach (var sip in contactMailAddresses)
            {
                if (sip == null)
                    throw new InvalidEMailException("Invalid Argument: Empty E-Mail addresss.");

                contactMailAddressesString.Add(sip.ToString().Insert(0, "sip:"));                
            }

            try
            {
                StartCall(contactMailAddressesString);
            }
            catch (NoSuccessfulCallException)
            {
                throw;
            }
        }

        /// <summary>
        /// Starts a Lync Call with a Lync-registered contact.
        /// </summary>
        /// <param name="contactMailAddress">E-Mail Adress of a Lync registered contact.</param>
        public static void CallRegisteredUser(MailAddress contactMailAddress)
        {
            var kontaktMails = new List<MailAddress>();
            kontaktMails.Add(contactMailAddress);
            CallRegisteredUser(kontaktMails);
        }
        #endregion

        #region Call non-registered contact(s)
        /// <summary>
        /// Calls one ore more phonenumbers.
        /// </summary>
        /// <param name="phoneNumbers">Phonenumber you want to call.</param>
        public static void Call(List<PhoneNumber> phoneNumbers)
        {
            if (phoneNumbers == null || phoneNumbers.Count == 0)
                throw new InvalidPhoneNumberException("Invalid Argument: No phonenumbers found.");

            var phoneNumbersString = new List<string>();

            foreach (var tel in phoneNumbers)
            {
                if (tel == null)
                    throw new InvalidPhoneNumberException("Invalid Argment: Phonenumber is empty.");

                phoneNumbersString.Add(tel.ToLync());
            }

            try
            {
                StartCall(phoneNumbersString);
            }
            catch (NoSuccessfulCallException)
            {
                throw;
            }
        }
        
        /// <summary>
        /// Calls a given phonenumber.
        /// </summary>
        /// <param name="phoneNumber">Phonenumber you wat to call.</param>
        public static void Call(PhoneNumber phoneNumber)
        {
            var phoneNumbers = new List<PhoneNumber>();
            phoneNumbers.Add(phoneNumber);
            Call(phoneNumbers);
        }
        #endregion

        private static void StartCall(List<string> participants)
        {
            try
            {
                var automation = LyncClient.GetAutomation();
                automation.BeginStartConversation(AutomationModalities.Audio, participants, null, null, automation);
            }
            catch (Exception e)
            {
                throw new NoSuccessfulCallException(e.Message, e.InnerException);
            }
        }
    }
}
