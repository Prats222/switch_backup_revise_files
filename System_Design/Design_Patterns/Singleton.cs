public sealed class Singleton // class is accesible but not inheritable
{
    private static volatile Singleton? _instance; // private=no one can change it directly, static = belongs only to singleton class, volatile = accesible by multiple threads.
    private static readonly object _lock = new object();// to support synchronization , A,B threads should wait for eachother, when this readonly(cant be unclocked) lock is applied.
    private string _data;

    private Singleton(string data)
    {
        //private constructor, so cant be initialized anywhere else like new Singleton(data)
        _data=data;
    }

    public static Singleton GetInstance(string data)//allows outside code to get instance only.
    {
        //First check - just check if instance exists or nt, if not skip locking mechanism and go to the return instance.
        if (_instance == null)
        {
            //if instance is null , A,B ,... threads can attempt to create an instance, so syncronized, B has to wait for A.
            lock (_lock)
            {
                // ** If A,B pass first check , and enter the lock , A gets in B wait, A creates an instance and gets out,
                //  while B wont know there is an Instance, so to avoid this we have 2nd check so B doesn't create duplicate instance
                if (_instance == null)
                {
                    _instance= new Singleton(data);
                }
            }
        }
        return _instance;
    }
}
