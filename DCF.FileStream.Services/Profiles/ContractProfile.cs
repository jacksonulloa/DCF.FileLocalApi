using AutoMapper;
using DCF.FileStream.Dtos.Response;
using DCF.FileStream.Entities.Generic;

namespace DCF.FileStream.Services.Profiles
{
    public class ContractProfile : Profile
    {
        public ContractProfile() 
        {
            CreateMap<BaseResponse, SearchFileRes>();
            CreateMap<BaseResponse, DownFileRes>();
        }
    }
}
