using AutoMapper;
using Newtonsoft.Json.Linq;
using StocksComparisonApp.Models;

namespace StocksComparisonApp.App_Start.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<JToken, Stock>()
                .ForMember(dest => dest.Open, act => act.MapFrom(src => src["1. open"]))
                .ForMember(dest => dest.High, act => act.MapFrom(src => src["2. high"]))
                .ForMember(dest => dest.Low, act => act.MapFrom(src => src["3. low"]))
                .ForMember(dest => dest.Close, act => act.MapFrom(src => src["4. close"]))
                .ForMember(dest => dest.Volume, act => act.MapFrom(src => src["5. volume"]))
                .ForMember(dest => dest.Date, act => act.MapFrom(src => ((JProperty) src.Parent).Name));
        }
    }
}
