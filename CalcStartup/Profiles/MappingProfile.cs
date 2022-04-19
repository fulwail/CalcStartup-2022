using Mapster;

namespace CalcStartup.Profiles
{
    public class MappingProfile: IRegister
{
     
        public void Register(Mapster.TypeAdapterConfig config)
        {
           // config.NewConfig<ModelA, ModelB>();
        }

 
    }
}
