namespace OmadaVoucherProxy
{
    public class VoucherGenerationSettings
    {
        public int CodeLength { get; set; } = 6;
        public int Amount { get; set; } = 6;
        public int DurationMinutes { get; set; }
        public NewVoucherParams ToNewVoucherParams()
        {
            return new NewVoucherParams()
            {
                Amount = Amount,
                CodeLength = CodeLength,
                DurationMinutes = DurationMinutes
            };
        }
    }
}
