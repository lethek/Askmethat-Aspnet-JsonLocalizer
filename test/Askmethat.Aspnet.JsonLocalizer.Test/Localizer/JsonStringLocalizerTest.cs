﻿using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.TestSample;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Linq;


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
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");

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
            Assert.IsTrue(result.ResourceNotFound);
        }

        [TestMethod]
        public void Should_Read_Base_UseDefault()
        {
            // Arrange
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));
            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");

            var result = localizer.GetString("NoFrench");
            Assert.AreEqual("NoFrench", result);
        }

        [TestMethod]
        public void Should_Read_Base_Name1_US()
        {
            // Arrange
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            var result = localizer.GetString("BaseName1");

            Assert.AreEqual("My Base Name 1", result);
        }

        [TestMethod]
        public void Should_Read_CaseInsensitive_CultureName()
        {
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var result = localizer.GetString("CaseInsensitiveCultureName");
            Assert.AreEqual("French", result);
        }

        [TestMethod]
        public void Should_Read_CaseInsensitive_UseDefault()
        {
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            CultureInfo.CurrentUICulture = new CultureInfo("de-DE");
            var result = localizer.GetString("CaseInsensitiveCultureName");
            Assert.AreEqual("CaseInsensitiveCultureName", result);
            Assert.IsTrue(result.ResourceNotFound);
        }

        [TestMethod]
        public void Should_GetAllStrings_ByCaseInsensitiveCultureName()
        {
            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create(typeof(IStringLocalizer));

            CultureInfo.CurrentUICulture = new CultureInfo("fr-FR");
            var expected = new[] {
                "Mon Nom de Base 1",
                "Mon Nom de Base 2",
                "NoFrench",
                "French"
            };
            var results = localizer.GetAllStrings().Select(x => x.Value).ToArray();
            CollectionAssert.AreEquivalent(expected, results);
        }


        [TestMethod]
        public void Should_Read_Base_Name1_WithCultureFR()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            var sp = services.BuildServiceProvider();
            var factory = sp.GetService<IStringLocalizerFactory>();
            var defaultLocalizer = factory.Create(typeof(IStringLocalizer));
            
            var enLocalizer = defaultLocalizer.WithCulture(new CultureInfo("fr-FR"));

            var result = enLocalizer.GetString("BaseName1");

            Assert.AreEqual("Mon Nom de Base 1", result);
        }
    }
}
