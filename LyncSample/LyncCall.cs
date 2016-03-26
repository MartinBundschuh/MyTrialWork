using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Extensibility;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Data.Entity;
using System.Threading.Tasks;

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

            Parallel.ForEach(contactMailAddresses, sip =>
                {
                    if (sip == null)
                        throw new InvalidEMailException("Invalid Argument: Empty E-Mail addresss.");

                    contactMailAddressesString.Add(sip.ToString().Insert(0, "sip:"));
                });

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

            Parallel.ForEach(phoneNumbers, tel =>
                {
                    if (tel == null)
                        throw new InvalidPhoneNumberException("Invalid Argment: Phonenumber is empty.");

                    phoneNumbersString.Add(tel.ToLync());
                });

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

        #region Call and Logging
        private static void StartCall(List<string> participants)
        {
            try
            {
                var lyncCLient = LyncClient.GetClient();
                if (lyncCLient == null || lyncCLient.State != ClientState.SignedIn)
                    throw new NoSuccessfulCallException("Call not possible: CLient is not signed in.");               
                
                var automation = LyncClient.GetAutomation();
                automation.BeginStartConversation(AutomationModalities.Audio, participants, null, null, automation);

                Parallel.ForEach(participants, uri =>
                    {
                        LogCall(lyncCLient, uri);
                    });
            }
            catch (Exception e)
            {
                throw new NoSuccessfulCallException(e.Message, e.InnerException);
            }
        }

        private static void LogCall(LyncClient lyncClient, string uri)
        {
            var callHistory = new CallHistory
            {
                CallFrom = lyncClient.Uri,
                CallTo = uri,
                Date = DateTime.Now,
                IsIntern = lyncClient.ContactManager.GetContactByUri(uri) == null
            };

            using (var callHistoryLog = new LyncCallLogging())
            {
                callHistoryLog.CallHistory.Add(callHistory);
                callHistoryLog.SaveChangesAsync();
            }
        }
        #endregion
    }
}
