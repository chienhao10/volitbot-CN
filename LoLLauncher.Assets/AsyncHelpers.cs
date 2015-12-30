using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LoLLauncher.Assets
{
	public static class AsyncHelpers
	{
		private class ExclusiveSynchronizationContext : SynchronizationContext
		{
			private bool done;

			private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);

			private readonly Queue<Tuple<SendOrPostCallback, object>> items = new Queue<Tuple<SendOrPostCallback, object>>();

			public Exception InnerException
			{
				get;
				set;
			}

			public override void Send(SendOrPostCallback d, object state)
			{
				throw new NotSupportedException("We cannot send to our same thread");
			}

			public override void Post(SendOrPostCallback d, object state)
			{
				lock (this.items)
				{
					this.items.Enqueue(Tuple.Create<SendOrPostCallback, object>(d, state));
				}
				this.workItemsWaiting.Set();
			}

			public void EndMessageLoop()
			{
				this.Post(delegate(object _)
				{
					this.done = true;
				}, null);
			}

			public void BeginMessageLoop()
			{
				while (!this.done)
				{
				}
			}
		}

		public static void RunSync(Func<Task> task)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			AsyncHelpers.ExclusiveSynchronizationContext synch = new AsyncHelpers.ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synch);
			synch.Post(delegate(object _)
			{
				try
				{
					await task();
				}
				catch (Exception innerException)
				{
					synch.InnerException = innerException;
					throw;
				}
				finally
				{
					synch.EndMessageLoop();
				}
			}, null);
			synch.BeginMessageLoop();
			SynchronizationContext.SetSynchronizationContext(current);
		}

		public static T RunSync<T>(Func<Task<T>> task)
		{
			SynchronizationContext current = SynchronizationContext.Current;
			AsyncHelpers.ExclusiveSynchronizationContext synch = new AsyncHelpers.ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synch);
			T ret = default(T);
			synch.Post(delegate(object _)
			{
				try
				{
					ret = await task();
				}
				catch (Exception innerException)
				{
					synch.InnerException = innerException;
					throw;
				}
				finally
				{
					synch.EndMessageLoop();
				}
			}, null);
			synch.BeginMessageLoop();
			SynchronizationContext.SetSynchronizationContext(current);
			return ret;
		}
	}
}
