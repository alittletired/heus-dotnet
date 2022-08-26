using System.Reflection;

namespace Heus.Ioc.Internal
{
    internal static class ServiceModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType);
            return moduleTypes;
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            CheckSeriveModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependsOnAttributes = moduleType
                .GetCustomAttributes()
                .OfType<DependsOnAttribute>();

            foreach (var dependsOn in dependsOnAttributes)
            {
                foreach (var dependedModuleType in dependsOn.DependedTypes)
                {
                    dependencies.TryAdd(dependedModuleType);
                }
            }

            return dependencies;
        }
        internal static void CheckSeriveModuleType(Type moduleType)
        {
            if (!IsSeriveModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }
        //  public IEnumerable<Type> Scan()
        // {
        //     var assemblies = _moduleContainer.Modules.Select(m => m.Type.Assembly).Distinct();
        //     foreach (var assembly in assemblies)
        //     {
        //         foreach (var type in assembly.GetTypes())
        //         {
        //             yield return type;
        //         }
        //     }

        public static bool IsSeriveModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IServiceModule).GetTypeInfo().IsAssignableFrom(type);
        }
        private static void AddModuleAndDependenciesRecursively(
            List<Type> moduleTypes,
            Type moduleType,

            int depth = 0)
        {
            CheckSeriveModuleType(moduleType);

            if (moduleTypes.Contains(moduleType))
            {
                return;
            }

            moduleTypes.Add(moduleType);

            foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            {
                AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleType, depth + 1);
            }
        }
    }
}
