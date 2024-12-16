using System;
using System.Threading;

namespace ConsoleStopwatch
{
    public delegate void StopwatchEventHandler(string message);

    public class Stopwatch
    {
        private TimeSpan _timeElapsed;
        private bool _isRunning;
        private Timer _timer;

        public event StopwatchEventHandler OnStarted;
        public event StopwatchEventHandler OnStopped;
        public event StopwatchEventHandler OnReset;

        public Stopwatch()
        {
            _timeElapsed = TimeSpan.Zero;
            _isRunning = false;
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");

            _timer = new Timer(Tick, null, 0, 1000); 
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;
            _timer?.Dispose();
            OnStopped?.Invoke("Stopwatch Stopped!");
        }

        public void Reset()
        {
            Stop();
            _timeElapsed = TimeSpan.Zero;
            OnReset?.Invoke("Stopwatch Reset!");
        }

        private void Tick(object state)
        {
            _timeElapsed = _timeElapsed.Add(TimeSpan.FromSeconds(1));
            Console.Clear();
            Console.WriteLine($"Time Elapsed: {_timeElapsed}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.OnStarted += HandleEvent;
            stopwatch.OnStopped += HandleEvent;
            stopwatch.OnReset += HandleEvent;

            Console.WriteLine("Console Stopwatch Application\n");

            while (true)
            {
                Console.WriteLine("Press S to Start, T to Stop, R to Reset, Q to Quit.");
                char input = Console.ReadKey(true).KeyChar;

                switch (char.ToUpper(input))
                {
                    case 'S':
                        stopwatch.Start();
                        break;
                    case 'T':
                        stopwatch.Stop();
                        break;
                    case 'R':
                        stopwatch.Reset();
                        break;
                    case 'Q':
                        stopwatch.Stop();
                        return;
                    default:
                        Console.WriteLine("Invalid input. Try again.");
                        break;
                }
            }
        }

        static void HandleEvent(string message)
        {
            Console.WriteLine(message);
        }
    }
}
