using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;

namespace FileBox
{
    public class CustomResourceAssemblyProvider : IResourceAssemblyProvider
    {
        private IEnumerable<Assembly> filteredAssemblies;

        public IEnumerable<Assembly> GetAssembliesToScan()
        {
            return (this.filteredAssemblies ?? (this.filteredAssemblies = GetFilteredAssemblies()));
        }

        private static IEnumerable<Assembly> GetFilteredAssemblies()
        {
            return AppDomainAssemblyTypeScanner.Assemblies.Where(x => !x.IsDynamic);
        }
    }
}