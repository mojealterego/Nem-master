using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewMaster.LiveOps
{
    public sealed class LiveOpsService
    {
        public LiveEventDefinition FindActive(IEnumerable<LiveEventDefinition> events, DateTime utcNow)
        {
            if (events == null)
                return null;

            foreach (var liveEvent in events)
            {
                if (liveEvent != null && liveEvent.IsActive(utcNow))
                    return liveEvent;
            }

            return null;
        }
    }
}
