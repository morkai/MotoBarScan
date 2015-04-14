using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace MotoBarScan
{
    public class StdinWorker
    {
        private IDictionary<string, IAction> actions = new Dictionary<string, IAction>();

        private CancellationTokenSource tokenSource;

        private BlockingCollection<IAction> actionQueue;

        public StdinWorker(CancellationTokenSource tokenSource, BlockingCollection<IAction> actionQueue)
        {
            this.tokenSource = tokenSource;
            this.actionQueue = actionQueue;
        }

        public void RegisterAction(string id, IAction action)
        {
            actions[id] = action;
        }

        public void Run()
        {
            while (!tokenSource.IsCancellationRequested && !actionQueue.IsAddingCompleted)
            {
                string[] actionArgs;

                try
                {
                    actionArgs = Console.ReadLine().Trim().Split(' ');
                }
                catch (Exception)
                {
                    break;
                }

                if (!actions.ContainsKey(actionArgs[0]))
                {
                    continue;
                }

                var action = actions[actionArgs[0]];

                try
                {
                    action.ReadArgs(actionArgs);

                    actionQueue.Add(action);
                }
                catch (Exception) { }
            }
        }
    }
}
