using System;
using System.Threading;

namespace Registry
{
	public class Consumer
	{
		private Registry<int> container;
		private const int MIN_SUBSCRIBERS = 10;

		public Consumer (Registry<int> container)
		{
			this.container = container;
		}

		public void Consume () {
			while (true) {
				container.WaitForSubscribers (MIN_SUBSCRIBERS);
				int[] values = container.GetSubscribers ();
				foreach (int value in values) {
					Console.WriteLine ("Consume: " + value);
					container.Unregister (value);
				}
			}
		}
	}
}

