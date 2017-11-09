﻿using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.TestSample;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Askmethat.Aspnet.JsonLocalizer.Test.Localizer
{
    [TestClass]
    public class JsonStringLocalizerTest
    {
        IServiceCollection services;
        TestServer server;

        [TestInitialize]
        public void Init()
        {
            var builder = new WebHostBuilder()
                            .ConfigureServices(serv =>
                            {
                                serv.AddJsonLocalization();
                                this.services = serv;
                            })
                            .UseStartup<Startup>();

            server = new TestServer(builder);

        }


        [TestMethod]
        public void Should_Read_Json_From_Ressource()
        {
            // Arrange
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));
        }


        [TestMethod]
        public void Should_Read_Base_Name1()
        {
            // Arrange
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            CultureInfo.CurrentCulture = new CultureInfo("fr-FR");

            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));


            var result = localizer.GetString("BaseName1");

            Assert.AreEqual("Mon Nom de Base 1", result);
        }

        [TestMethod]
        public void Should_Read_Base_NotFound()
        {
            // Arrange
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            var result = localizer.GetString("Nop");

            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void Should_Read_Base_Name1_US()
        {
            // Arrange
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            var result = localizer.GetString("BaseName1");

            Assert.AreEqual("My Base Name 1", result);
        }
    }
}
