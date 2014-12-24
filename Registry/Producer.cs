using System;
using System.Threading;

namespace Registry
{
	public class Producer
	{
		private Registry<int> container;
		private Random rnd;
		private const int MIN_SUBSCRIBERS = 10;
		private const int SLEEP_TIME = 500;
		private const int RAND_MIN = 0;
		private const int RAND_MAX = 100;

		public Producer (Registry<int> container)
		{
			this.container = container;
			rnd = new Random ();
		}

		public void Produce () {
			while (true) {
				if (container.NoOfRegistered <= MIN_SUBSCRIBERS) {
					int value = rnd.Next (RAND_MIN, RAND_MAX);
					Console.WriteLine ("Produce: " + value);
					container.Register (value);
				} else {
					Thread.Sleep (SLEEP_TIME);
				}
			}
		}
	}
}

