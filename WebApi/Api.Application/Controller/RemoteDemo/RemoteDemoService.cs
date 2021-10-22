using Core.Infrastructure.Common;
using Furion.ClayObject;
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.Application.Http;

namespace Api.Application.Shipment
{
    /// <summary>
    /// 模拟调用远程请求
    /// 
    /// </summary>
    [ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public  class RemoteDemoService : IDynamicApiController
    {
        private readonly Isso _sso;
        private readonly IConfig _config;
        public RemoteDemoService(Isso sso, IConfig config)
        {
            _sso = sso;
            _config = config;
        }
   
        public async Task<ResultSet<UserResponse>> PostLogin(Api.Application.Http.User user)
        {
            //var user = new User
            //{
            //    UserName = "9",
            //    Password = "111111"
            //};
          var data = await  _sso.PostLoginAsync(user);
            return data;
        }

        public async Task<ResultSet<dynamic>> PostLoginAsClay(Api.Application.Http.User user)
        {
            //var user = new User
            //{
            //    UserName = "9",
            //    Password = "111111"
            //};
            var data = await _sso.PostLoginAsDynamicAsync(user);
            return data;
        }

        [HttpPost]
        public async Task<ResultSet<List<CustomerTree>>> GetCustomerTree(Api.Application.Http.User user)
        {
            //var user = new User
            //{
            //    UserName = "9",
            //    Password = "111111",
            //    Type=1

            //};
            var data = await _sso.PostLoginAsync(user);
            var token = data.Data.Token;//获取token
           
           var res=  await _config.GetCustomerTreeAsync(token);


            return res;
        }


    }
}
