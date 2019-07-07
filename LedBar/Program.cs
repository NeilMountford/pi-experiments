using System;
using System.Device.Gpio;
using System.Threading;

namespace LedBar
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var gpioController = new GpioController(PinNumberingScheme.Board))
            {
                var ledPins = new[] { 11, 12, 13, 15, 16, 18, 22, 3, 5, 24 };
                
                foreach(var ledPin in ledPins)
                {
                    gpioController.OpenPin(ledPin, PinMode.Output);
                }

                var exitAt = DateTime.UtcNow.AddSeconds(10);

                try
                {
                    while(DateTime.UtcNow < exitAt)
                    {
                        foreach(var ledPin in ledPins)
                        {
                            gpioController.Write(ledPin, PinValue.High);
                            Thread.Sleep(100);
                            gpioController.Write(ledPin, PinValue.Low);
                        }
                    }
                }
                finally
                {
                    foreach (var ledPin in ledPins)
                    {
                        gpioController.Write(ledPin, PinValue.High);
                        gpioController.ClosePin(ledPin);
                    }
                    System.Console.WriteLine("Closed pins");
                }
            }
        }
    }
}
