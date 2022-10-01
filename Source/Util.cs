using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace kOS.AddOns.kOSAstrogator
{
    /// <summary>
    /// Everyone needs a util class
    /// </summary>
    public class Util
    {
        ///<summary>
        /// checks if the mod with "assemblyName" is loaded into KSP. Taken from KOS-Scansat
        ///</summary>
        public static bool IsModInstalled(string assemblyName)
        {
            Assembly assembly = (from a in AssemblyLoader.loadedAssemblies
                                 where a.name.ToLower().Equals(assemblyName.ToLower())
                                 select a).FirstOrDefault().assembly;
            return assembly != null;
        }
    }
}
