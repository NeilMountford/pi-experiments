using System;
using System.Device.Gpio;
using System.Threading;

namespace ButtonLed
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var gpioController = new GpioController(PinNumberingScheme.Board))
            {
                var ledPin = 11;
                var buttonPin = 12;

                gpioController.OpenPin(ledPin, PinMode.Output);
                gpioController.OpenPin(buttonPin, PinMode.InputPullUp);

                var exitAt = DateTime.UtcNow.AddSeconds(20);

                try
                {
                    while(DateTime.UtcNow < exitAt)
                    {
                        if(gpioController.Read(buttonPin) == PinValue.Low)
                        {
                            gpioController.Write(ledPin, PinValue.High);
                        }
                        else
                        {
                            gpioController.Write(ledPin, PinValue.Low);
                        }
                    }
                }
                finally
                {
                    gpioController.Write(ledPin, PinValue.Low);
                    gpioController.ClosePin(ledPin);
                    gpioController.ClosePin(buttonPin);
                    System.Console.WriteLine("Closed pins");
                }
            }
        }
    }
}
