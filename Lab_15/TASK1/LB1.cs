using System;
using System.Collections.Generic;
using System.Timers;

namespace Lab_15
{
    public class DirMonitoring
    {
        private List<string> _files;
        private readonly string _directory;

        public DirMonitoring(string directory)
        {
            _directory = directory;

        }


        public bool StartMonitor()
        {
            if (!Directory.Exists(_directory))
            {
                return false;
            }

            _files = new List<string>();

            var directoryInfo = new DirectoryInfo(_directory);
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                _files.Add(fileInfo.FullName);
            }

            return true;
        }


        public List<string> DeletedFiles()
        {
            var result = new List<string>();
            foreach (var file in _files.ToArray())
            {
                if (!File.Exists(file))
                {
                    _files.Remove(file);
                    result.Add(file);
                }
            }

            return result;
        }
    }


    public class Subscriber
    {
        private readonly string _name;


        public Subscriber(string name)
        {
            _name = name;
        }

        public void ItIsSubscriber(string filename)//deleted file
        {
            Console.WriteLine($"{_name} {filename}  was deleted!");
        }
    }


    class FileAnalogDelegate : IDisposable
    {
        private readonly Action<string> _watcher;
        private readonly System.Timers.Timer _timer;
        private readonly DirMonitoring _dirmonitoring; //checking
        public FileAnalogDelegate(string directory, Action<string> watcher)//path + delegate
        {
            _watcher = watcher;
            _dirmonitoring = new DirMonitoring(directory);

            if (_dirmonitoring.StartMonitor())
            {
                _timer = new System.Timers.Timer(1000);
                _timer.Elapsed += CheckRemoval;
                _timer.Start();
            }
            else
            {
                Console.WriteLine("This directory doesn't exist");
                Dispose();
            }

        }


        private void CheckRemoval(Object source, ElapsedEventArgs e)
        {
            foreach (var filename in _dirmonitoring.DeletedFiles())
            {
                _watcher(filename);
            }
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

    }


    class LB1
    {
        static void Main(string[] args)
        {
            var directory = @"C:\Users\user\OneDrive\Рабочий стол\Test";

            var FileAnalogDelegate = new FileAnalogDelegate(directory, new Subscriber(String.Empty).ItIsSubscriber);

            Console.ReadKey();
        }
    }

}

