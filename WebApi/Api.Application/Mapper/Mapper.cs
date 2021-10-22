using Core.Infrastructure.Enum;
using Furion.DataEncryption;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Dtos;
using Api.Application.Dtos.Notificaiton;
using Api.Application.Dtos.User;
using Api.Application.Vo;
using Api.Application.Vo.Notification;
using Api.Core;

namespace Api.Application.Mapper
{
    /// <summary>
    /// sample mapper
    /// </summary>
    public class Mapper : IRegister
    {
        /// <summary>
        /// 注册entity和dto的映射关系（用的是Mapster）
        /// </summary>
        /// <param name="config"></param>
        public void Register(TypeAdapterConfig config)
        {
            //config.ForType<SLogPrintT, LogPrintDto>()
            //    .Map(dest => dest.PrintInfo, src => $"{DateTime.Now}-{src.UserId} print {src.Copies}");


            config.ForType<User, LoginUserInfoResponse>()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest=>dest.Username,src=>src.Name);

            config.ForType<UserDto, Staff>()
                .IgnoreNullValues(true)
                .Ignore(dest=>dest.Id)//userdto里的id是userid，不是staffid，所以不用继承
                .Map(dest => dest.UserId, src => src.Id)//staff.userid=userdto.id
                .Map(dest => dest.No, src => src.JobNo)//staff.no=userdto.jobno
                .Map(dest => dest.Phone, src => src.Mobile);//staff.Phone=userdto.Mobile
                
            config.ForType<UserDto, User>()
                .Map(dest=>dest.Id,src=>src.Id??Guid.NewGuid().ToString())
                .IgnoreNullValues(true)
                .Map(dest => dest.Name, src => src.Name ?? src.Username)
                .Map(dest=>dest.Status,src=>src.Available);

            config.ForType<UserDto, Account>()
               .IgnoreNullValues(true)
               .Ignore(dest => dest.Id)//userdto里的id是userid，不是staffid，所以不用继承
               .Map(dest => dest.UserId, src => src.Id)
               .Map(dest => dest.LoginName, src => src.Username?? src.Name)
               .Map(dest=>dest.AccountType,src=>src.AccountType?? (int)EnumAccountType.Local);

            config.ForType<User, QueryUserResponse>()
              .Map(dest => dest.Username, src => src.Name)
              .Map(dest=>dest.Available, src=>src.Status);

            config.ForType<Staff, QueryUserResponse>()
              .Map(dest => dest.JobNo, src => src.No)
              .Map(dest => dest.Mobile, src => src.Phone);

            config.ForType<CompanyDto, Company>()
              .IgnoreNullValues(true)
              .TwoWays()
              .Map(dest=>dest.TimeZoneId,src=>src.TimezoneId);

            #region 通知映射
            //通知基础信息
            config.ForType<NotficationDto<ReleaseNtf>, Notification_Base>()
               .Map(dest => dest.ID, src =>  Guid.NewGuid().ToString())
               .IgnoreNullValues(true)
               .Map(dest => dest.PublishType, src => (int)src.PublishType)
               .Map(dest => dest.NotficationType, src => (int)src.Type)
               .Map(dest => dest.PublishTime, src => src.TimePublish)
               .Map(dest => dest.Status, src => (int)src.PublishType == 0 ? 0 : -1); 

            config.ForType<NotficationDto<MaintenanceNtf>, Notification_Base>()
                .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
                .IgnoreNullValues(true)
                .Map(dest => dest.PublishType, src => (int)src.PublishType)
                .Map(dest => dest.NotficationType, src => (int)src.Type)
                .Map(dest => dest.PublishTime, src => src.TimePublish)
                .Map(dest => dest.Status, src => (int)src.PublishType == 0 ? 0 : -1); 

            config.ForType<NotficationDto<OtherNtf>, Notification_Base>()
                .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
                .IgnoreNullValues(true)
                .Map(dest => dest.PublishType, src => (int)src.PublishType)
                .Map(dest => dest.NotficationType, src => (int)src.Type)
                .Map(dest => dest.PublishTime, src => src.TimePublish)
                .Map(dest=>dest.Status, src=> (int)src.PublishType==0?0:-1);

            //通知内容映射
            config.ForType<ReleaseNtf, NT_Release>()
                 .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
                .IgnoreNullValues(true)
                .Map(dest => dest.Time, src => src.ReleaseTime)
                .Map(dest => dest.Version_No, src => src.Version)
                .Map(dest => dest.Version_Name, src => src.VersionName)
                .Map(dest => dest.Content, src => string.Join(",", src.Details));

            config.ForType<MaintenanceNtf, NT_Maintenance>()
              .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
              .IgnoreNullValues(true)
              .Map(dest => dest.Time, src => src.MaintenanceTime)
              .Map(dest => dest.Content, src => src.Content)
              .Map(dest => dest.Title, src => src.Title);

            config.ForType<OtherNtf, NT_Other>()
             .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
             .IgnoreNullValues(true)
             .Map(dest => dest.Time, src => DateTime.Now)
             .Map(dest => dest.Content, src => src.Content)
             .Map(dest => dest.Title, src => src.Title);

            config.ForType<ReleaseDetail, NT_Release_Detail>()
                 .Map(dest => dest.ID, src => Guid.NewGuid().ToString())
                 .IgnoreNullValues(true)
                .Map(dest => dest.Release_Id, src => src.ReleaseId)
                .Map(dest => dest.Type, src => src.ReleaseType)
                .Map(dest => dest.Content, src => src.Content);

            //返回值映射
            config.ForType<Notification_Base, NtfReleaseResponse>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Id, src => src.ID)
                .Map(dest => dest.Time, src => src.PublishTime)
                .Map(dest => dest.Type, src => src.NotficationType)
                .Map(dest => dest.Sent, src => src.Status == 0 ? true : false);

            config.ForType<Notification_Base, NtfMaintenanceResponse>()
               .IgnoreNullValues(true)
               .Map(dest => dest.Id, src => src.ID)
               .Map(dest => dest.Time, src => src.PublishTime)
               .Map(dest => dest.Type, src => src.NotficationType)
               .Map(dest => dest.Sent, src => src.Status == 0 ? true : false);

            config.ForType<Notification_Base, NtfOtherResponse>()
              .IgnoreNullValues(true)
              .Map(dest => dest.Id, src => src.ID)
              .Map(dest => dest.Time, src => src.PublishTime)
              .Map(dest => dest.Type, src => src.NotficationType)
              .Map(dest => dest.Sent, src => src.Status == 0 ? true : false);
            #endregion


        }
    }
}
