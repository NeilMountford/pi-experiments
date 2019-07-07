using System;
using System.Device.Gpio;

namespace ButtonLamp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var gpioController = new GpioController(PinNumberingScheme.Board))
            {
                var ledPin = 11;
                var buttonPin = 12;

                var ledState = PinValue.Low;
                var buttonState = PinValue.High;
                var lastButtonState = PinValue.High;
                PinValue reading;
                var lastChangeTime = DateTime.UtcNow;
                var captureTime = TimeSpan.FromMilliseconds(50);
                var exitTime = TimeSpan.FromSeconds(2);

                gpioController.OpenPin(ledPin, PinMode.Output);
                gpioController.OpenPin(buttonPin, PinMode.InputPullUp);

                try
                {
                    while (true)
                    {
                        reading = gpioController.Read(buttonPin);

                        if(reading != lastButtonState)
                        {
                            lastChangeTime = DateTime.UtcNow;
                        }
                        else if (reading == PinValue.Low && DateTime.UtcNow - lastChangeTime > exitTime)
                        {
                            System.Console.WriteLine("Pressed and held to exit");
                            break;
                        }

                        if(DateTime.UtcNow - lastChangeTime > captureTime)
                        {
                            if(reading != buttonState)
                            {
                                buttonState = reading;

                                if(buttonState == PinValue.Low)
                                {
                                    System.Console.WriteLine("Button pressed");
                                    ledState = ledState == PinValue.Low ? PinValue.High : PinValue.Low;

                                    if(ledState == PinValue.High)
                                    {
                                        System.Console.WriteLine("Turning on LED");
                                    }
                                    else
                                    {
                                        System.Console.WriteLine("Turning off LED");
                                    }
                                }
                                else
                                {
                                    System.Console.WriteLine("Button released");
                                }
                            }
                        }

                        gpioController.Write(ledPin, ledState);
                        lastButtonState = reading;
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
