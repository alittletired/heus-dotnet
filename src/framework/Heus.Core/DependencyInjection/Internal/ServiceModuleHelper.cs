using System.Reflection;

namespace Heus.Core.DependencyInjection.Internal
{
    static internal class ServiceModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType);
            return moduleTypes;
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            CheckServiceModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependsOnAttributes = moduleType
                .GetCustomAttributes(typeof(ModuleDependsOnAttribute<>));


            foreach (var dependsOn in dependsOnAttributes)
            {
                var dependedModuleType = dependsOn.GetType().GetGenericArguments().First();
                dependencies.TryAdd(dependedModuleType);

            }

            return dependencies;
        }

        private static void CheckServiceModuleType(Type moduleType)
        {
            if (!IsModuleInitializer(moduleType))
            {
                throw new ArgumentException("Given type is not an ModuleInitializer: " + moduleType.AssemblyQualifiedName);
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

        private static bool IsModuleInitializer(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IModuleInitializer).GetTypeInfo().IsAssignableFrom(type);
        }
        private static void AddModuleAndDependenciesRecursively(
            List<Type> moduleTypes,
            Type moduleType,

            int depth = 0)
        {
            CheckServiceModuleType(moduleType);

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
