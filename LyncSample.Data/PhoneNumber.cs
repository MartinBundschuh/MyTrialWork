using System;

namespace LyncSample.Data
{
    /// <summary>
    /// Phonenumber, wich you can use to start an intern/extern Lync-Call.
    /// </summary>
    public class PhoneNumber
    {
        private int _areaCodeInternational;
        private int? _areaCode;
        private int _number;

        private readonly string _phoneNumberString;
        private readonly string _phoneNumberStringSave;

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
            {
                throw new InvalidPhoneNumberException("TelefonNr Instance Error: Missing input parameters");
            }

            _areaCodeInternational = areaCodeInternational;
            _areaCode = areaCode;
            _number = number;            
        }

        /// <summary>
        /// Creates an Phonenumber de to a given String.
        /// </summary>
        /// <param name="phoneNumber">Phonenumber with international-/ area code e.g.: +41 44 - 688 60 00, default int. area code will be 41 (CH).</param>
        public PhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new NoSuccessfulCallException("TelefonNr Instance Error: Phone number is empty.");

            _phoneNumberStringSave = phoneNumber;
            _phoneNumberString = phoneNumber
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("/", string.Empty)
                .Replace("-", string.Empty)
                .Trim();

            if (string.IsNullOrWhiteSpace(_phoneNumberString))
            {
                throw new NoSuccessfulCallException("TelefonNr Instance Error: Phone number is empty.");
            }

            if (!_phoneNumberString.StartsWith("+"))
            {
                _phoneNumberString = _phoneNumberString.Substring(0, 2).Equals("00") 
                    ? _phoneNumberString.Substring(2, _phoneNumberString.Length - 2) 
                    : _phoneNumberString.TrimStart('0').Insert(0, "41");
                _phoneNumberString = _phoneNumberString.Insert(0, "+");
            }

            if (!int.TryParse(phoneNumber.Substring(1, 2), out _areaCodeInternational)
                || int.TryParse(phoneNumber.Substring(3, phoneNumber.Length - 3), out _number))
            {
                throw new InvalidPhoneNumberException("TelefonNr Instance Error: Invalid format");
            }
        }

        /// <summary>
        /// Sets the Internatonal Area Code.
        /// </summary>
        /// <param name="areaCodeInternational">International area code, Format: 0041 or 41 (CH).</param>
        public void SetAreaCodeInternational(int areaCodeInternational)
        {
            if (areaCodeInternational <= 0)
            {
                throw new InvalidPhoneNumberException($"{nameof(areaCodeInternational)} must be grater than 0");
            }
            
            _areaCodeInternational = areaCodeInternational;
        }

        /// <summary>
        /// Sets the area code.
        /// </summary>
        /// <param name="areaCode">Area Code, Format: 044 or 44 (Zürich, CH).</param>
        public void SetAreaCode(int areaCode)
        {
            if (areaCode <= 0)
            {
                throw new InvalidPhoneNumberException($"{nameof(areaCode)} must be grater than 0");
            }
            
            _areaCode = areaCode;
        }

        /// <summary>
        /// Setzt die TelefonNr (ohne (Länder-)Vorwahl !)
        /// </summary>
        /// <param name="number">Number, Format: 6886000.</param>
        public void SetNumber(int number)
        {
            if (number <= 0)
            {
                throw new InvalidPhoneNumberException($"{nameof(number)} must be grater than 0");
            }
            
            _number = number;
        }

        /// <summary>
        /// Return the international Area Code, Format 49.
        /// </summary>
        /// <returns>International Area Code, 2 Digits.</returns>
        public int GetAreaCodeInternational() => _areaCodeInternational;

        /// <summary>
        /// Return the Area Code, will be Null if String-Constructor has been used.
        /// </summary>
        /// <returns>Area Code.</returns>
        public int? GetAreaCode() => _areaCode;

        /// <summary>
        /// Returns the phoneNumber without international- / area code.
        /// </summary>
        /// <returns>Simple Phone Number.</returns>
        public int GetNumber() => _number;

        /// <summary>
        /// Returns the whole Phonenumber, Format +44 (0)44 - 6886000.
        /// </summary>
        /// <returns>Frmatted and complete Phonenumber.</returns>
        public override string ToString() => string.IsNullOrEmpty(_phoneNumberStringSave) 
            ? $"+{_areaCodeInternational} (0){_areaCode} - {_number}"
            : _phoneNumberStringSave;

        /// <summary>
        /// Returns the whole Phonenumber, Format tel:+41446886000.
        /// </summary>
        /// <returns>Returns the whole Phonenumber, Formatted for Lync.</returns>
        internal string ToLync() => string.IsNullOrEmpty(_phoneNumberString)
            ? $"tel:+{_areaCodeInternational}{_areaCode}{_number}"
            : $"tel:{_phoneNumberString}";
    }
}
