using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Utils
{
    public class Common
    {
        public enum LayoutType { UpDown, LeftRight, ErrorType };


        public static string getLayoutString(LayoutType lt)
        {
            switch (lt) { 
                case LayoutType.UpDown:
                    return "UpDown";
                case LayoutType.LeftRight:
                    return "LeftRight";
                default:
                    return "ErrorType";
            }
        }
        public static LayoutType getLayout(string str)
        {
            if (string.IsNullOrEmpty(str))
                return LayoutType.ErrorType;

            switch (str.ToLower())
            {
                case "updown":
                    return LayoutType.UpDown;
                case "leftright":
                    return LayoutType.LeftRight;
                default:
                    return LayoutType.ErrorType;
            }
        }
    }
}
