using System;

namespace ApiBridge.Core.RavenDBMembership
{
    public class RavenDBMembershipProviderThatDisposesStore : RavenDBMembershipProvider, IDisposable
    {
        public void Dispose()
        {
            if (DocumentStore != null)
                DocumentStore.Dispose();

            DocumentStore = null;
        }
    }
}