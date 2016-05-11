using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lightspeed_Product_Management
{
    class HelperFunctions
    {
        public static bool CheckForEquality(ABSItem x, ABSItem y)
        {
            bool status = false;

            if ("NARC ".Contains(x.Status) && y.Status == "false")
            {
                status = true;
            }
            else if ("NARC ".Contains(x.Status) && y.Status != "false")
            {
                status = false;
            }
            else if (!"NARC ".Contains(x.Status) && y.Status == "false")
            {
                //Insert error
            }
            else if (!"NARC ".Contains(x.Status) && y.Status != "false")
            {
                status = true;
            }

            if (x.UPC == y.UPC && decimal.Parse(x.MAP) == decimal.Parse(y.MAP) && decimal.Parse(x.MSRP) == decimal.Parse(y.MSRP) && x.CustomSKU == y.CustomSKU && decimal.Parse(x.Cost) == decimal.Parse(y.Cost) && x.Category.Split('>')[0].Replace(":", ": ").Replace("/", "") == y.Category.Replace("/", "") && x.Manufacturer == y.Manufacturer)
            {
                if (status)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }
    }
}
