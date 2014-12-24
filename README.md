registry
========

C# implementation of IRegistry interface. 

```C#
interface IRegistry<T>
{
	void Register (T o);
	void Unregister (T o);
	T[] GetSubscribers ();
	bool IsRegistryEmpty { get; }
	int NoOfRegistered { get; }
    string[] GetWatingThreadsList;
	bool IsRegistered (T subscriber);
	void WaitForSubscribers (int howMany)
}
```