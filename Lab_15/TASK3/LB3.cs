var instance = LazySingleton.Instance;


Console.WriteLine(LazySingleton.SomeVal);

Console.WriteLine(LazySingleton.SomeVal);

Console.WriteLine("------");


List<Task> tasks = new List<Task>();
for (int i = 0; i < 1; i++)
{
    Task t = Task.Run(() =>
    {
        Task.Delay(DateTime.Now.Second);
        var singleton = Check.Instance;
        Console.WriteLine(Check.SomeVal);
    });
    tasks.Add(t);
    Task.WhenAll(tasks).Wait();
}

Console.WriteLine("------");

public sealed class LazySingleton
{
    private static Random _random = new Random(DateTime.Now.Millisecond);

    private static int _someVal;
    private static readonly Lazy<LazySingleton> _instance =
    new Lazy<LazySingleton>(() =>
    {
        _someVal = _random.Next();
        return new LazySingleton();
    });
    LazySingleton() { }
    public static LazySingleton Instance { get { return _instance.Value; } }
    public static int SomeVal { get => _someVal; }
}

public sealed class Check
{
    private static Random _random = new Random(DateTime.Now.Millisecond);

    
    private static volatile Check _instance;
    private static readonly object _syncRoot = new object();
    private static int _someVal;

    Check()
    { }
    public static Check Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new Check();
                        _someVal = _random.Next();
                    }
                }
            }
            return _instance;
        }
    }
    public static int SomeVal { get => _someVal; }
}