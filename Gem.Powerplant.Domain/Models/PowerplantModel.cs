using Gem.Powerplant.Domain.Enums;

namespace Gem.Powerplant.Domain.Models;

public class PowerplantModel
{
    public required string Name { get; set; }
    public PowerplantType Type { get; set; }
    public double Efficiency { get; set; }
    public double PMin { get; set; }
    public double PMax { get; set; }
}
