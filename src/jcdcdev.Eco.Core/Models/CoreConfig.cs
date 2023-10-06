using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jcdcdev.Eco.Core.Models;

public class CoreConfig
{
    [Description("The rate in seconds (5-120) at which store data is updated. Default: 10s")]
    [Category("Performance")]
    [Range(5, 120)]
    [DefaultValue(10)]
    public uint StoreUpdateFrequency { get; set; } = 10;
}