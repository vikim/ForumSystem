namespace ForumSystem.Web.Infrastructure.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    public class AutoMapperConfig
    {
        private Assembly assembly;

        public AutoMapperConfig(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public void Execute()
        {
            var types = this.assembly.GetExportedTypes();
            // the calling assembly
            // var types = Assembly.GetCallingAssembly().GetExportedTypes();

            LoadStandardMappings(types);

            LoadCustomMappings(types);
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            // All classes in the asembly which inherit IMapFrom 
            // ( All View Models which inherit IMapFrom )
            // gave generic arguments and the class
            // call static dictunary  Mapper.CreateMap
            var maps = from t in types
                       from i in t.GetInterfaces()
                       where
                           i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) && !t.IsAbstract
                           && !t.IsInterface
                       select new { Source = i.GetGenericArguments()[0], Destination = t };

            foreach (var map in maps)
            {
                Mapper.CreateMap(map.Source, map.Destination);
            }
        }

        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            // All classes in the asembly which inherit IHaveCustomMappings 
            // Call method CreateMappings for each of them
            var maps = from t in types
                       from i in t.GetInterfaces()
                       where typeof(IHaveCustomMappings).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface
                       select (IHaveCustomMappings)Activator.CreateInstance(t);

            foreach (var map in maps)
            {
                map.CreateMappings(Mapper.Configuration);
            }
        }
    }
}