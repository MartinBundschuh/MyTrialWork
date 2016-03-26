using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyncSample
{
    /// <summary>
    /// Phonenumber, wich you can use to start an intern/extern Lync-Call.
    /// </summary>
    public class PhoneNumber
    {
        private int AreaCodeInternational;
        private int? AreaCode;
        private int Number;

        private string PhoneNumberString;
        private string PhoneNumberStringSave;

        #region Constructors
        /// <summary>
        /// Creates the Phone number, International Area Code is set to 41 (CH).
        /// </summary>
        /// <param name="areaCode">Area Code, Format: 044 or 44 (Zürich, CH).</param>
        /// <param name="number">Number, Format: 3886000.</param>
        public PhoneNumber(int areaCode, int number) 
            : this(41, areaCode, number) { }

        /// <summary>
        /// Creates the Phone number.
        /// </summary>
        /// <param name="areaCodeInternational">International Area Code, Format: 41 (CH).</param>
        /// <param name="areaCode">Area Code, Format: 044 or 44 (Zürich, GER).</param>
        /// <param name="number">Number, Format: 3886000.</param>
        public PhoneNumber(int areaCodeInternational, int areaCode, int number) 
        {
            if (areaCodeInternational <= 0 || areaCode <= 0 || number <= 0)
                throw new InvalidPhoneNumberException("TelefonNr Instance Error: Missing input parameters");

            AreaCodeInternational = areaCodeInternational;
            AreaCode = areaCode;
            Number = number;            
        }

        /// <summary>
        /// Creates an Phonenumber de to a given String.
        /// </summary>
        /// <param name="phoneNumber">Phonenumber with international-/ area code e.g.: +41 44 - 688 60 00, default int. area code will be 41 (CH).</param>
        public PhoneNumber(string phoneNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new NoSuccessfulCallException("TelefonNr Instance Error: Phone number is empty.");

                PhoneNumberStringSave = phoneNumber;
                PhoneNumberString = phoneNumber.Replace("(", string.Empty).Replace(")", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(PhoneNumberString))
                    throw new NoSuccessfulCallException("TelefonNr Instance Error: Phone number is empty.");

                if (!PhoneNumberString.StartsWith("+"))
                {
                    PhoneNumberString = PhoneNumberString.Substring(0, 2).Equals("00") ? PhoneNumberString.Substring(2, PhoneNumberString.Length - 2) : PhoneNumberString.TrimStart('0').Insert(0, "41");
                    PhoneNumberString = PhoneNumberString.Insert(0, "+");
                }

                int.TryParse(phoneNumber.Substring(1, 2), out AreaCodeInternational);
                // Wont decide between int. and city area code. To Complex and not even necessary to handle
                AreaCode = null;
                int.TryParse(phoneNumber.Substring(3, phoneNumber.Length - 3), out Number);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Setter
        /// <summary>
        /// Sets the Internatonal Area Code.
        /// </summary>
        /// <param name="areaCodeInternational">International area code, Format: 0041 or 41 (CH).</param>
        public void SetAreaCodeInternational(int areaCodeInternational)
        {
            if (areaCodeInternational > 0)
                AreaCodeInternational = areaCodeInternational;
        }

        /// <summary>
        /// Sets the area code.
        /// </summary>
        /// <param name="areaCode">Area Code, Format: 044 or 44 (Zürich, CH).</param>
        public void SetAreaCode(int areaCode)
        {
            if (areaCode > 0)
                AreaCode = areaCode;            
        }

        /// <summary>
        /// Setzt die TelefonNr (ohne (Länder-)Vorwahl !)
        /// </summary>
        /// <param name="number">Number, Format: 6886000.</param>
        public void SetNumber(int number)
        {            
            if (number > 0)
                Number = number;
        }
        #endregion

        #region Getter
        /// <summary>
        /// Return the international Area Code, Format 49.
        /// </summary>
        /// <returns>International Area Code, 2 Digits.</returns>
        public int GetAreaCodeInternational()
        {
            return AreaCodeInternational;
        }

        /// <summary>
        /// Return the Area Code, will be Null if String-Constructor has been used.
        /// </summary>
        /// <returns>Area Code.</returns>
        public int? GetAreaCode()
        {
            return AreaCode;
        }

        /// <summary>
        /// Returns the phoneNumber without international- / area code.
        /// </summary>
        /// <returns>Simple Phone Number.</returns>
        public int GetNumber()
        {
            return Number;
        }
        #endregion

        #region ToString / ToLync
        /// <summary>
        /// Returns the whole Phonenumber, Format +44 (0)44 - 6886000.
        /// </summary>
        /// <returns>Frmatted and complete Phonenumber.</returns>
        public override string ToString()
        {
            try
            {
                return string.IsNullOrEmpty(PhoneNumberStringSave) ? string.Concat("+", AreaCodeInternational, " (0)", AreaCode, " - ", Number) : PhoneNumberStringSave;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the whole Phonenumber, Format tel:+41446886000.
        /// </summary>
        /// <returns>Returns the whole Phonenumber, Formatted for Lync.</returns>
        internal string ToLync()
        {
            try
            {
                return string.IsNullOrEmpty(PhoneNumberString) ? string.Concat("tel:+", AreaCodeInternational, AreaCode, Number) : "tel:" + PhoneNumberString;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
