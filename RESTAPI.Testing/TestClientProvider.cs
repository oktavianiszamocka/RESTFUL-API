
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using RESTAPI.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RESTAPI.Testing
{
    public class TestClientProvider
    {

        private readonly HttpClient Client;
        public TestClientProvider()
        {
            // var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            // Client = server.CreateClient();
            var appFactory = new WebApplicationFactory<Startup>();
           
            Client = appFactory.CreateClient();
        }

        [Fact]
        public async Task GetAll()
        {
           
            var response = await Client.GetAsync("/api/v1/Note");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddNote()
        {
                var response = await Client.PostAsync("/api/v1/Note", new StringContent(
                JsonConvert.SerializeObject(new Note()
                {
                    Title = "hello from the test",
                    Content = "im created from test"

                }), 
            Encoding.UTF8,
            "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
