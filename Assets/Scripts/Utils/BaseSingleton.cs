using System;

public abstract class BaseSingleton<T> where T : class
{
    #region Members

    /// <summary>
    /// Static instance. Needs to use lambda expression
    /// to construct an instance (since constructor is private).
    /// </summary>
    private static readonly Lazy<T> sInstance = new Lazy<T>(() => CreateInstanceOfT());

    private bool isInitialized = false;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance of this singleton.
    /// </summary>
    public static T Instance { get { return sInstance.Value; } }

    #endregion

    #region Methods
    
    /// <summary>
    /// Creates an instance of T via reflection since T's constructor is expected to be private.
    /// </summary>
    /// <returns></returns>
    private static T CreateInstanceOfT()
    {
        return Activator.CreateInstance(typeof(T), true) as T;
    }

    protected virtual void Initialize ()
    {
        if (!isInitialized)
        {
            isInitialized = true;
        }
    }

    #endregion
}
