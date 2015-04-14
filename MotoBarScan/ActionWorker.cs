using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MotoBarScan
{
    public class ActionWorker
    {
        private CancellationTokenSource tokenSource;

        private BlockingCollection<IAction> actionQueue;

        public ActionWorker(CancellationTokenSource tokenSource, BlockingCollection<IAction> actionQueue)
        {
            this.tokenSource = tokenSource;
            this.actionQueue = actionQueue;
        }

        public void Run()
        {
            while (!tokenSource.IsCancellationRequested && !actionQueue.IsAddingCompleted)
            {
                try
                {
                    actionQueue.Take().Execute();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception) {}
            }
        }
    }
}
