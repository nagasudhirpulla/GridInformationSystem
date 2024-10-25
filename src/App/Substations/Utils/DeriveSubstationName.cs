using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Substations.Utils;
public static class SubstationUtils
{
    public static string DeriveSubstationName(string voltageLevel, string location)
    {
        return $"{voltageLevel} {location}";
    }
}