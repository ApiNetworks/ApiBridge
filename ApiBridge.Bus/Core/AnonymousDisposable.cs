using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ApiBridge.Bus.Core
{
    internal sealed class AnonymousDisposable : IDisposable
    {
        readonly Action dispose;
        int isDisposed;

        public AnonymousDisposable(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref this.isDisposed, 1) == 0)
            {
                this.dispose();
            }
        }
    }
}
