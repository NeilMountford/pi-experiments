using System;
using System.Device.Gpio;
using System.Threading;

namespace LedBlink
{
    class Program
    {
        private const int NumberOfTimesToBlink = 4;

        static void Main(string[] args)
        {
            Console.WriteLine($"Blinking the LED {NumberOfTimesToBlink} times");

            using(var gpioController = new GpioController(PinNumberingScheme.Board))
            {
                var pin = 11;

                gpioController.OpenPin(pin, PinMode.Output);

                try
                {
                    for (var i = 0; i < NumberOfTimesToBlink; i++)
                    {
                        gpioController.Write(pin, PinValue.High);
                        Thread.Sleep(500);
                        gpioController.Write(pin, PinValue.Low);
                        Thread.Sleep(500);
                    }
                }
                finally
                {
                    gpioController.ClosePin(pin);
                }
            }
        }
    }
}
