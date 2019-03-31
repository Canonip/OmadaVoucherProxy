using OmadaAPI.Model.DTO;
using System;

namespace OmadaVoucherProxy.Model
{

    public class VoucherGenerationSettings
    {
        public string CloudClientId { get; set; }
        public int CodeLength { get; set; }
        public int Amount { get; set; }
        public int DurationMinutes { get; set; }
        public NewVoucherParams ToNewVoucherParams()
        {
            return new NewVoucherParams()
            {
                VoucherCodeLength = CodeLength,
                Amount = Amount,
                DurationTime = TimeSpan.FromMinutes(DurationMinutes)
            };
        }
    }

}
