using NUnit.Framework;
using NewMaster.Raids;
using System.Collections.Generic;

namespace NewMaster.Tests
{
    public sealed class RaidTargetSelectorTests
    {
        [Test]
        public void FiltersSelfEmptyAndZeroLootTargets()
        {
            var targets = new List<RaidTarget>
            {
                new RaidTarget { TargetId = "self", AvailableLoot = 100 },
                new RaidTarget { TargetId = "empty", AvailableLoot = 0 },
                new RaidTarget { TargetId = "target-1", AvailableLoot = 250 }
            };

            var available = new RaidTargetSelector().FilterAvailable(targets, "self");

            Assert.That(available.Count, Is.EqualTo(1));
            Assert.That(available[0].TargetId, Is.EqualTo("target-1"));
        }

        [Test]
        public void SelectUsesFilteredIndex()
        {
            var targets = new List<RaidTarget>
            {
                new RaidTarget { TargetId = "a", AvailableLoot = 10 },
                new RaidTarget { TargetId = "b", AvailableLoot = 20 }
            };

            var selected = new RaidTargetSelector().Select(targets, "self", 1);

            Assert.That(selected.TargetId, Is.EqualTo("b"));
        }
    }
}
