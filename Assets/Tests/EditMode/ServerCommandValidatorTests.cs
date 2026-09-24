using NUnit.Framework;
using NewMaster.Online;
namespace NewMaster.Tests
{
    public sealed class ServerCommandValidatorTests
    {
        [Test]
        public void CurrencyAndWorldBoundsAreEnforced()
        {
            var validator = new ServerCommandValidator();
            Assert.That(validator.IsValidCurrencyDelta(100), Is.True);
            Assert.That(validator.IsValidCurrencyDelta(-1), Is.False);
            Assert.That(validator.IsValidWorldId(365), Is.True);
            Assert.That(validator.IsValidWorldId(366), Is.False);
        }
    }
}
