FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /webapihost
EXPOSE 80

COPY ./ /webapihost
# linux 连接oracle 时 timezone处理
ENV TIME_ZONE Asia/Shanghai
RUN echo "${TIME_ZONE}" > /etc/timezone
RUN ln -sf /usr/share/zoneinfo/${TIME_ZONE} /etc/localtime
ENTRYPOINT ["dotnet", "SIE.WebApiHost.dll"]