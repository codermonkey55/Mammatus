using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Enums
{
    public enum FileSize : long
    {
        Scale = 1024,
        KiloByte = 1 * Scale,
        MegaByte = KiloByte * Scale,
        GigaByte = MegaByte * Scale,
        TeraByte = GigaByte * Scale
    }
}
