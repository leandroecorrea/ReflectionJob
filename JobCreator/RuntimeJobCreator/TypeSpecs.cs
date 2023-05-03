public partial class JobDeJobs
{
    public class TypeSpecs
    {
        public Type BaseType { get; init; }
        public Type[] ConstructorArgs { get; init; }

        private TypeSpecs(Type baseType, Type[] constructorArgs)
        {
            BaseType = baseType;
            ConstructorArgs = constructorArgs;
        }

        public static TypeSpecs For(StoredJob job) =>
            new TypeSpecs(GetBaseType(), GetConstructorArgs());

        private static Type GetBaseType() => typeof(GenericJob);
        private static Type[] GetConstructorArgs() => new[] { typeof(ILogger<GenericJob>)};
    }
}
