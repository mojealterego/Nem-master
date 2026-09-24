using NUnit.Framework;
using NewMaster.Worlds;
using UnityEngine;

namespace NewMaster.Tests
{
    public sealed class WorldCatalogRuntimeTests
    {
        [Test]
        public void RuntimeFallbackCreatesRequestedWorldRange()
        {
            var catalog = WorldCatalog.CreateRuntimeFallback(365);

            try
            {
                Assert.That(catalog.Worlds.Count, Is.EqualTo(365));
                Assert.That(catalog.Find(1), Is.Not.Null);
                Assert.That(catalog.Find(365), Is.Not.Null);
                Assert.That(catalog.Find(366), Is.Null);
                Assert.That(catalog.Find(360).bossId, Is.EqualTo("boss.360"));
                Assert.That(catalog.Find(20).seasonalTag, Is.EqualTo("season.01"));
            }
            finally
            {
                Object.DestroyImmediate(catalog);
            }
        }
    }
}
