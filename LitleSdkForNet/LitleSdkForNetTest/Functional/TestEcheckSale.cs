﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Litle.Sdk;

namespace Litle.Sdk.Test.Functional
{
    [TestFixture]
    class TestEcheckSale
    {
        private LitleOnline litle;

        [TestFixtureSetUp]
        public void setUp()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            config.Add("url", "https://www.testlitle.com/sandbox/communicator/online");
            config.Add("reportGroup", "Default Report Group");
            config.Add("username", "DOTNET");
            config.Add("version", "8.13");
            config.Add("timeout", "65");
            config.Add("merchantId", "101");
            config.Add("password", "TESTCASE");
            config.Add("printxml", "true");
            litle = new LitleOnline(config);
        }

        [Test]
        public void SimpleEcheckSaleWithEcheck()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.amount = 123456;
            echeckSaleObj.orderId = "12345";
            echeckSaleObj.orderSource = orderSourceType.ecommerce;

            echeckType echeckTypeObj = new echeckType();
            echeckTypeObj.accType = echeckAccountTypeEnum.Checking;
            echeckTypeObj.accNum = "12345657890";
            echeckTypeObj.routingNum = "123456789";
            echeckTypeObj.checkNum = "123455";
            
            contact contactObj = new contact();
            contactObj.name = "Bob";
            contactObj.city = "lowell";
            contactObj.state = "MA";
            contactObj.email = "litle.com";

            echeckSaleObj.echeck = echeckTypeObj;
            echeckSaleObj.billToAddress = contactObj;

            echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            StringAssert.AreEqualIgnoringCase("Approved", response.message);
        }

        [Test]
        public void NoAmount()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.reportGroup = "Planets";
            
            try
            {
                //expected exception;
                echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            }
            catch (LitleOnlineException e)
            {
                Assert.True(e.Message.StartsWith("Error validating xml data against the schema"));
            }
        }

        [Test]
        public void EcheckSaleWithShipTo()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.reportGroup = "Planets";
            echeckSaleObj.amount = 123456;
            echeckSaleObj.verify = true;
            echeckSaleObj.orderId = "12345";
            echeckSaleObj.orderSource = orderSourceType.ecommerce;

            echeckType echeckTypeObj = new echeckType();
            echeckTypeObj.accType = echeckAccountTypeEnum.Checking;
            echeckTypeObj.accNum = "12345657890";
            echeckTypeObj.routingNum = "123456789";
            echeckTypeObj.checkNum = "123455";

            contact contactObj = new contact();
            contactObj.name = "Bob";
            contactObj.city = "lowell";
            contactObj.state = "MA";
            contactObj.email = "litle.com";

            echeckSaleObj.echeck = echeckTypeObj;
            echeckSaleObj.billToAddress = contactObj;
            echeckSaleObj.shipToAddress = contactObj;

            echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            StringAssert.AreEqualIgnoringCase("Approved", response.message);
        }

        [Test]
        public void EcheckSaleWithEcheckToken()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.reportGroup = "Planets";
            echeckSaleObj.amount = 123456;
            echeckSaleObj.verify = true;
            echeckSaleObj.orderId = "12345";
            echeckSaleObj.orderSource = orderSourceType.ecommerce;

            echeckTokenType echeckTokenTypeObj = new echeckTokenType();
            echeckTokenTypeObj.accType = echeckAccountTypeEnum.Checking;
            echeckTokenTypeObj.litleToken = "1234565789012";
            echeckTokenTypeObj.routingNum = "123456789";
            echeckTokenTypeObj.checkNum = "123455";

            customBilling customBillingObj = new customBilling();
            customBillingObj.phone = "123456789";
            customBillingObj.descriptor = "good";

            contact contactObj = new contact();
            contactObj.name = "Bob";
            contactObj.city = "lowell";
            contactObj.state = "MA";
            contactObj.email = "litle.com";

            echeckSaleObj.token = echeckTokenTypeObj;
            echeckSaleObj.customBilling = customBillingObj;
            echeckSaleObj.billToAddress = contactObj;

            echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            StringAssert.AreEqualIgnoringCase("Approved", response.message);
        }

        [Test]
        public void EcheckSaleMissingBilling()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.amount = 123456;
            echeckSaleObj.orderId = "12345";
            echeckSaleObj.orderSource = orderSourceType.ecommerce;

            echeckType echeckTypeObj = new echeckType();
            echeckTypeObj.accType = echeckAccountTypeEnum.Checking;
            echeckTypeObj.accNum = "12345657890";
            echeckTypeObj.routingNum = "123456789";
            echeckTypeObj.checkNum = "123455";

            echeckSaleObj.echeck = echeckTypeObj;

            try
            {
                //expected exception;
                echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            }
            catch (LitleOnlineException e)
            {
                Assert.True(e.Message.StartsWith("Error validating xml data against the schema"));
            }
        }

        [Test]
        public void SimpleEcheckSale()
        {
            echeckSale echeckSaleObj = new echeckSale();
            echeckSaleObj.reportGroup = "Planets";
            echeckSaleObj.litleTxnId = 123456789101112;
            echeckSaleObj.amount = 12;

            echeckSalesResponse response = litle.EcheckSale(echeckSaleObj);
            StringAssert.AreEqualIgnoringCase("Approved", response.message);
        }
            
    }
}
