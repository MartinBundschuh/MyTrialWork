using Microsoft.VisualStudio.TestTools.UnitTesting;
using LyncSample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyncSample.Tests
{
    [TestClass()]
    public class PhoneNumberTests
    {
        [TestMethod()]
        public void PhoneNumberTest()
        {
            var phoneNumber = new PhoneNumber(44, 6806000);

            Assert.IsTrue(phoneNumber != null);
        }

        [TestMethod()]
        public void PhoneNumberTestString()
        {
            var number = "+41 44 - 688 60 00";
            var phoneNumber = new PhoneNumber(number);
            Assert.AreEqual(phoneNumber.GetAreaCodeInternational(), 41);
            Assert.AreEqual(phoneNumber.ToString(), number);
        }
    }
}