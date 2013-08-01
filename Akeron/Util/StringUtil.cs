using System.Text.RegularExpressions;

namespace Styx.Util
{

    public static class StringUtil
    {
        #region IsAllNotNullOrEmpty
        public static bool IsNotNullOrEmpty(params string[] args)
        {
            foreach (string arg in args)
            {
                if (string.IsNullOrEmpty(arg))
                    return false;
            }
            return true;
        }
        #endregion
    }
}
