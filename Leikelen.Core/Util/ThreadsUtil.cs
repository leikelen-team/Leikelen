using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Util
{
    /// <summary>
    /// Utility class with functions related to threads.
    /// </summary>
    public static class ThreadsUtil
    {
        /// <summary>
        /// Starts an action in a new thread in STA mode (Single thread apartment).
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        /// <returns></returns>
        public static Task StartSTATask(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(new object());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}
