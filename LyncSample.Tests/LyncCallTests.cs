using Microsoft.VisualStudio.TestTools.UnitTesting;
using LyncSample.Data;

namespace LyncSample.Tests
{
    [TestClass]
    public class LyncCallTests
    {
        [TestMethod]
        public void CallTest()
        {
            var phoneNumber = new PhoneNumber(44, 6886000);
            try
            {
                LyncCall.Call(phoneNumber);
            }
            catch (NoSuccessfulCallException e)
            {
                Assert.AreEqual("Call not possible: Lync is not started.",
                    e.Message, true);

                return;
            }

            Assert.Fail();
        }
    }
}