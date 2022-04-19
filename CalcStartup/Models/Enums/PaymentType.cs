using System.ComponentModel;

namespace CalcStartup.Models.Enums
{
    public enum PaymentType
    {
        [Description("Ежемесячный")]
        Monthly,
        [Description("Ежегодный")]
        Yearly,
        [Description("Единоразовая")]
        Single,

    }
}
