namespace dotnet_rpg
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Models.Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Models.Character>();
            CreateMap<UpdateCharacterDto, Models.Character>();
        }
    }
}
