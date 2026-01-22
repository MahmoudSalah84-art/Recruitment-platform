using AutoMapper;
using Jobs.Application.Features.Users.Queries.GetUserProfile;

namespace Jobs.API.Mapping
{
    public class ApiMappingProfile : Profile
	{
        public ApiMappingProfile() 
        {
			CreateMap<GetUserByIdQuery, UserResponse>();
				


		}
    }
}
