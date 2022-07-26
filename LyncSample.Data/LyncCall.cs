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
        /// <param name="contactMailAddresses">E-Mail Addresses of Lync-registered contacts.</param>
        public static void CallRegisteredUser(IReadOnlyCollection<MailAddress> contactMailAddresses)
        {
            if (contactMailAddresses == null || contactMailAddresses.Count == 0)
            {
                throw new ArgumentException($"{nameof(contactMailAddresses)} must not be null or empty");
            }

            var contactMailAddresseStrings = new List<string>();

            Parallel.ForEach(contactMailAddresses, e =>
            {
                if (e == null)
                {
                    throw new InvalidEMailException("Invalid argument: Ee-mail address is null.");
                }

                contactMailAddresseStrings.Add(e.ToString().Insert(0, "sip:"));
            });

            StartCall(contactMailAddresseStrings);
        }

        /// <summary>
        /// Starts a Lync Call with a Lync-registered contact.
        /// </summary>
        /// <param name="contactMailAddress">E-mail address of a Lync registered contact.</param>
        public static void CallRegisteredUser(MailAddress contactMailAddress) =>
            CallRegisteredUser(new List<MailAddress>
            {
                contactMailAddress,
            });

        /// <summary>
        /// Calls a given phone number.
        /// </summary>
        /// <param name="phoneNumber">Phone number you wat to call.</param>
        public static void Call(PhoneNumber phoneNumber) =>
            Call(new List<PhoneNumber>
            {
                phoneNumber,
            });
        
        /// <summary>
        /// Calls one ore more phone numbers.
        /// </summary>
        /// <param name="phoneNumbers">Phone number you want to call.</param>
        public static void Call(IReadOnlyCollection<PhoneNumber> phoneNumbers)
        {
            if (phoneNumbers == null || phoneNumbers.Count == 0)
            {
                throw new ArgumentException($"{nameof(phoneNumbers)} must not be null or empty");
            }

            var phoneNumberStrings = new List<string>();

            Parallel.ForEach(phoneNumbers, p =>
            {
                if (p == null)
                {
                    throw new InvalidPhoneNumberException("Invalid argument: Phone number is null.");
                }

                phoneNumberStrings.Add(p.ToLync());
            });

            StartCall(phoneNumberStrings);
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
