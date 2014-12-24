using System;
using System.Threading;

namespace Registry
{
	class MainClass
	{
		public const int MAIN_THREAD_SLEEP_TIME = 2000;

		public static void Main (string[] args)
		{
			/*
			 * Simple Producer - Consumer example
			 * 
			 * Producer produces elements if there is less than 10 elements in the register. Otherwise, it sleeps for 
			 * 500 miliseconds just to make results more readable.
			 * 
			 * Consumer starts consuming elements if there is ten or more elements in the register. 
			 */

			Console.WriteLine("Program has started");
			Console.WriteLine();

			Registry<int> registry = new Registry<int> ();
			Producer producer = new Producer (registry);
			Consumer consumer = new Consumer (registry);

			Thread producerThread = new Thread (new ThreadStart (producer.Produce));
			Thread consumerThread = new Thread (new ThreadStart (consumer.Consume));

			producerThread.Name = "Producer";
			consumerThread.Name = "Consumer";

			producerThread.Start ();
			consumerThread.Start ();

			while (!producerThread.IsAlive);
			while (!consumerThread.IsAlive);

			Thread.Sleep (MAIN_THREAD_SLEEP_TIME);

			producerThread.Abort ();
			consumerThread.Abort ();

			producerThread.Join ();
			consumerThread.Join ();

			Console.WriteLine();
			Console.WriteLine("Program has finished");
		}
	}
}
