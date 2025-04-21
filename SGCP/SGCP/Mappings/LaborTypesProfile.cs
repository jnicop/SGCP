using AutoMapper;
using SGCP.DTOs.LaborCost;
using SGCP.Models;

namespace SGCP.Mappings
{
    public class LaborTypesProfile:Profile
    {
        public LaborTypesProfile()
        {
            CreateMap<LaborType, LaborTypeDto>().ReverseMap();

        }
    }
}
