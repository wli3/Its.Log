﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Its.Log.Instrumentation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Disposables;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using Its.Log.Instrumentation.Extensions;
using Moq;
using NUnit.Framework;

namespace Recipes.Tests
{
    [TestFixture]
    public class WebApiTelemetryTests
    {
        private CompositeDisposable disposables;
        private IList<Telemetry> telemetryEvents;

        [SetUp]
        public void SetUp()
        {
            disposables = new CompositeDisposable();
            telemetryEvents = new List<Telemetry>();

            disposables.Add(Log.TelemetryEvents().Subscribe(e => { telemetryEvents.Add(e); }));
        }

        [TearDown]
        public void TearDown()
        {
            disposables.Dispose();
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_responses_indicate_HTTP_status_code()
        {
            HttpResponseMessage response = null;

            using (var activity = Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
                activity.MarkAsSuccessful();
            }

            telemetryEvents.Single()
                           .HttpStatusCode
                           .Should()
                           .Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_responses_indicate_success_if_the_code_is_20x()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }

            telemetryEvents.Single()
                           .Succeeded
                           .Should()
                           .BeTrue();
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_responses_indicate_failure_if_the_code_is_40x()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            telemetryEvents.Single()
                           .Succeeded
                           .Should()
                           .BeFalse();
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_responses_indicate_failure_if_the_code_is_50x()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => new { response }))
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            telemetryEvents.Single()
                           .Succeeded
                           .Should()
                           .BeFalse();
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_responses_contain_a_target_URI()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, @"http://contoso.com/")
                };
            }

            telemetryEvents.Single()
                           .RequestUri
                           .Should()
                           .Be(new Uri(@"http://contoso.com/"));
        }

        [Test]
        public async Task Telemetry_events_based_on_HTTP_request_contains_a_caller_IP_address()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, @"http://contoso.com/");
                request.Properties[RemoteEndpointMessageProperty.Name] = new RemoteEndpointMessageProperty("123.123.123.123", 80);

                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = request
                };
            }

            telemetryEvents.Single()
                           .IsIncomingRequest()
                           .Should()
                           .BeTrue();
            telemetryEvents.Single()
                           .CallerIpAddress()
                           .Should()
                           .Be("123.123.123.123");
        }

        [Test]
        public async Task Telemetry_operation_name_based_on_HTTP_request_contains_an_action_descriptor()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                var request = new HttpRequestMessage(HttpMethod.Get, @"http://contoso.com/operationName5");
                var actionDescriptor = new Mock<HttpActionDescriptor>();
                actionDescriptor.Setup(a => a.ActionName).Returns("operationName6");
                request.Properties[HttpPropertyKeys.HttpActionDescriptorKey] = actionDescriptor.Object;

                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = request
                };
            }

            telemetryEvents.Single()
                           .OperationName
                           .Should()
                           .Be("operationName6");
        }

        [Test]
        public async Task Telemetry_operation_name_based_on_HTTP_request_url_parsing()
        {
            HttpResponseMessage response = null;

            using (Log.With<Telemetry>(t => t.WithPropertiesBasedOn(response)).Enter(() => { }))
            {
                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = new HttpRequestMessage(HttpMethod.Get, @"http://contoso.com/operationName5")
                };
            }

            telemetryEvents.Single()
                           .OperationName
                           .Should()
                           .Be("operationName5");
        }
    }
}