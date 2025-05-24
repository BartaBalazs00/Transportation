using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Entities.Dto;
using Transportation.Entities.Entity;

namespace Transportation.Logic.Dto
{
    public class DtoProvider
    {
        public Mapper Mapper { get;}
        public DtoProvider()
        {
            Mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Vehicle, VehicleViewDto>();
                cfg.CreateMap<VehicleCreateUpdateDto, Vehicle>();
            }));
        }
    }
}
