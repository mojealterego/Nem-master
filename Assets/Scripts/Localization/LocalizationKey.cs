namespace NewMaster.Localization
{
    public readonly struct LocalizationKey
    {
        public string Value { get; }

        public LocalizationKey(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;
    }
}
