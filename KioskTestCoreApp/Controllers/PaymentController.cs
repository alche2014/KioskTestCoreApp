using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace KioskTestCoreApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ILogger<PaymentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public PaymentResponse Get(string Amount ,string UniqueId)
        {
            var serialPort = new SerialPort();
            serialPort.PortName = "COM5";
            serialPort.BaudRate = 115200;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
            serialPort.Write($"{Amount}");
            Thread.Sleep(40000);
            var b = 0;
            var response = serialPort.ReadExisting();
            var rng = new Random();
            if (response.Contains("Response Code = 00"))
            {
                return new PaymentResponse
                {
                    IsSuccess=true,
                    UniqueId= UniqueId
                };
            }
            return new PaymentResponse
            {
                IsSuccess = false,
                UniqueId = UniqueId
            };
        }
    }
}
