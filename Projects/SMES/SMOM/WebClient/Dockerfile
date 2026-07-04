#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /webclient
EXPOSE 80

RUN apt-get update
RUN apt-get install -y libgdiplus

COPY ./ /webclient
# linux 连接oracle 时 timezone处理
ENV TIME_ZONE Asia/Shanghai
RUN echo "${TIME_ZONE}" > /etc/timezone
RUN ln -sf /usr/share/zoneinfo/${TIME_ZONE} /etc/localtime
ENTRYPOINT ["dotnet", "WebClient.dll"]