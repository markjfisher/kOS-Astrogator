using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("kOS-Astrogator")]
[assembly: AssemblyDescription("kOS interface for Astrogator")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("kOS-Astrogator")]
[assembly: AssemblyCopyright("Copyright © Mark Fisher 2022")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a0799abf-b268-4bb7-a686-a26f3638af91")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
[assembly: AssemblyVersion("0.1.2.0")]
[assembly: AssemblyFileVersion("0.1.2.0")]

[assembly: KSPAssembly("kOS-Astrogator", 0, 1)]
[assembly: KSPAssemblyDependency("kOS", 1, 3)]
[assembly: KSPAssemblyDependency("Astrogator", 0, 10)]
