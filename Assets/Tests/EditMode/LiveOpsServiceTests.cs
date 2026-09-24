using System;
using NUnit.Framework;
using UnityEngine;
using NewMaster.LiveOps;

namespace NewMaster.Tests
{
    public sealed class LiveOpsServiceTests
    {
        [Test]
        public void ActiveEventIsResolvedByUtcWindow()
        {
            var liveEvent = ScriptableObject.CreateInstance<LiveEventDefinition>();
            try
            {
                var service = new LiveOpsService();
                Assert.That(service.FindActive(
                    new[] { liveEvent },
                    DateTime.UtcNow), Is.Not.Null);
            }
            finally
            {
                Object.DestroyImmediate(liveEvent);
            }
        }
    }
}
