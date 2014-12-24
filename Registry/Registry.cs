using System;
using System.Threading;
using System.Collections.Generic;

namespace Registry
{
	public class Registry<T> : IRegistry<T>
	{
		private const bool DEBUG = true;
		private List<T> subscribers;
		private List<string> waitingThreadsNames;
		private Dictionary<int, ManualResetEvent> waitingThreads;

		public Registry ()
		{
			subscribers = new List<T> ();
			waitingThreadsNames = new List<string> ();
			waitingThreads = new Dictionary<int, ManualResetEvent> ();
		}

		public void Register (T o)
		{
			lock (subscribers) {
				if (DEBUG)
					Console.WriteLine ("Register value: " + o);

				subscribers.Add (o);
				lock (waitingThreads) {
					if (waitingThreads.ContainsKey (subscribers.Count)) {

						if (DEBUG)
							Console.WriteLine ("Waking up threads. Count: " + subscribers.Count);

						waitingThreads [subscribers.Count].Set ();
					}
				}
			}
		}

		public void Unregister (T o)
		{
			lock (subscribers) {
				if (waitingThreads.ContainsKey (subscribers.Count)) {

					if (DEBUG)
						Console.WriteLine ("Reseting signal for threads. Count: " + subscribers.Count);

					waitingThreads [subscribers.Count].Reset ();
				}
				subscribers.Remove (o);
			}
		}

		public T[] GetSubscribers ()
		{
			lock (subscribers) {
				return subscribers.ToArray ();
			}
		}

		public bool IsRegistered (T subscriber)
		{
			lock (subscribers) {
				return subscribers.Contains (subscriber);
			}
		}

		public void WaitForSubscribers (int howMany)
		{
			bool wait = false;

			lock (subscribers) {
				if (subscribers.Count < howMany) {
					wait = true;

					lock (waitingThreadsNames)
						waitingThreadsNames.Add (Thread.CurrentThread.Name);
				
					if (!waitingThreads.ContainsKey (howMany)) {
						waitingThreads.Add (howMany, new ManualResetEvent (false));
					}
				}
			}

			if (wait) {

				if (DEBUG)
					Console.WriteLine ("Thread [" + Thread.CurrentThread.Name + "] is going to sleep");

				while (this.NoOfRegistered < howMany)
					waitingThreads [howMany].WaitOne ();

				lock (waitingThreadsNames)
					waitingThreadsNames.Remove (Thread.CurrentThread.Name);

				if (DEBUG)
					Console.WriteLine ("Thread [" + Thread.CurrentThread.Name + "] is waking up");
			}
		}

		public bool IsRegistryEmpty {
			get {
				lock (subscribers) {
					return subscribers.Count.Equals (0) ? true : false;
				}
			}
		}

		public int NoOfRegistered {
			get {
				lock (subscribers) {
					return subscribers.Count;
				}
			}
		}

		public string[] GetWatingThreadsList () {
			lock (waitingThreadsNames) {
				return waitingThreadsNames.ToArray ();
			}
		}
	}
}

