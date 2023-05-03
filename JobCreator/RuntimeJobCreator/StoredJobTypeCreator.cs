using Quartz;
using System.Reflection.Emit;
using System.Reflection;

public partial class JobDeJobs
{
    public class StoredJobTypeCreator
    {
        public Type Create(StoredJob existentJob)
        {
            ValidateJob(existentJob);
            TypeSpecs typeSpec = TypeSpecs.For(existentJob);
            //Definición de assembly que se crea por reflection
            AssemblyName assemblyName = new AssemblyName("DynamicJobsAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
            //Definición del tipo --> este se pasa a Quartz cuando se crea el Job
            TypeBuilder typeBuilder = moduleBuilder.DefineType($"{existentJob.Name}", TypeAttributes.Public);
            typeBuilder.SetParent(typeSpec.BaseType);
            //Parámetros del constructor            
            ConstructorBuilder constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, typeSpec.ConstructorArgs);
            //Agarro el primer constructor porque no pude obtenerlo de otro modo
            AddIntermediateLanguageInstructions(typeSpec.BaseType.GetConstructors()[0], constructor);
            return typeBuilder.CreateType();
        }

        private void AddIntermediateLanguageInstructions(ConstructorInfo baseConstructor, ConstructorBuilder constructor)
        {
            //Acá se agregan las instrucciones de código intermedio al ensamblado para que el constructor llame a la clase base
            Type stringType = Type.GetType("System.String");
            ConstructorInfo objCtor = stringType.GetConstructor(new Type[0]);
            ILGenerator constructorIL = constructor.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Ldarg_1);
            constructorIL.Emit(OpCodes.Call, baseConstructor);
            constructorIL.Emit(OpCodes.Nop);             
            constructorIL.Emit(OpCodes.Ret);
        }

        private void ValidateJob(StoredJob job)
        {
            CronExpression.ValidateExpression(job.CronExpression);
            if (string.IsNullOrEmpty(job.Name) || IsNotOnlyLetters(job.Name))
            {
                throw new FormatException($"El nombre del job {job.Name} no es válido");
            }
        }
        private bool IsNotOnlyLetters(string jobName) => !jobName.All(c => char.IsLetter(c) || c == '_');
    }
}
