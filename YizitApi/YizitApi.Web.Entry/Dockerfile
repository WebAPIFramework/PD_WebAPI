FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# 将publish下的所有文件拷贝到工作目录
COPY . .
# 端口
EXPOSE 80
WORKDIR ./Api
# 启动webapi
ENTRYPOINT ["dotnet", "YizitApi.Web.Entry.dll"]