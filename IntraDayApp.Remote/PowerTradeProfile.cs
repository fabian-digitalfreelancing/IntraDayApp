using AutoMapper;
using IntraDayApp.Domain.Models;
using Services;

namespace IntraDayApp.Remote
{
    public class PowerTradeProfile : Profile
    {
        public PowerTradeProfile()
        {
            CreateMap<PowerTrade, Trade>();
            CreateMap<PowerPeriod, TradePeriod>();
        }

    }
}
