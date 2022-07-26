using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Extensibility;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LyncSample.Data
{
    /// <summary>
    /// Class which u can use to start Lync Calls and Conferences
    /// </summary>
    public static class LyncCall
    {
        private static readonly LyncClient LyncClient = LyncClient.GetClient();
        
        public static readonly bool IsSignedIn = LyncClient?.State == ClientState.SignedIn;
        
        /// <summary>
        /// Starts a phone call with one or more Lync-registered contacts.
        /// </summary>
        /// <param name="contactMailAddresses">E-Mail Adresses of Lync-registered contacts.</param>
        public static void CallRegisteredUser(List<MailAddress> contactMailAddresses)
        {
            if (contactMailAddresses == null || contactMailAddresses.Count == 0)
            {
                throw new InvalidEMailException("Invalid Argument: No given E-Mail addresses.");
            }

            var contactMailAddressesString = new List<string>();

            Parallel.ForEach(contactMailAddresses, sip =>
            {
                if (sip == null)
                {
                    throw new InvalidEMailException("Invalid Argument: Empty E-Mail addresss.");
                }

                contactMailAddressesString.Add(sip.ToString().Insert(0, "sip:"));
            });

            StartCall(contactMailAddressesString);
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

        /// <summary>
        /// Calls a given phonenumber.
        /// </summary>
        /// <param name="phoneNumber">Phonenumber you wat to call.</param>
        public static void Call(PhoneNumber phoneNumber) =>
            Call(new List<PhoneNumber>
            {
                phoneNumber,
            });
        
        /// <summary>
        /// Calls one ore more phonenumbers.
        /// </summary>
        /// <param name="phoneNumbers">Phonenumber you want to call.</param>
        public static void Call(IReadOnlyCollection<PhoneNumber> phoneNumbers)
        {
            if (phoneNumbers == null || phoneNumbers.Count == 0)
            {
                throw new InvalidPhoneNumberException("Invalid Argument: No phonenumbers found.");
            }

            var phoneNumbersString = new List<string>();

            Parallel.ForEach(phoneNumbers, tel =>
            {
                if (tel == null)
                {
                    throw new InvalidPhoneNumberException("Invalid Argment: Phonenumber is empty.");
                }

                phoneNumbersString.Add(tel.ToLync());
            });

            StartCall(phoneNumbersString);
        }

        private static void StartCall(IReadOnlyCollection<string> participants)
        {
            try
            {              
                if (!IsSignedIn)
                {
                    throw new NoSuccessfulCallException("Call not possible: Client is not signed in.");
                }

                var automation = LyncClient.GetAutomation();
                automation.BeginStartConversation(AutomationModalities.Audio, participants, null, null, automation);

                Parallel.ForEach(participants, LogCall);
            }
            catch (Exception e)
            {
                throw new NoSuccessfulCallException("Call not possible: Lync is not started.", e.InnerException);
            }
        }

        private static void LogCall(string uri)
        {
            var callHistory = new CallHistory
            {
                CallFrom = LyncClient.Uri,
                CallTo = uri,
                Date = DateTime.Now,
                IsIntern = LyncClient.ContactManager.GetContactByUri(uri) != null,
            };

            using var callHistoryLog = new LyncCallLogging();
            callHistoryLog.CallHistory.Add(callHistory);
            callHistoryLog.SaveChangesAsync();
        }
    }
}
