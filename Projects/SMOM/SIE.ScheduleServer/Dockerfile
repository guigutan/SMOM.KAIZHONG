FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /scheduleserver
EXPOSE 80

COPY ./ /scheduleserver
# linux 连接oracle 时 timezone处理
ENV TIME_ZONE Asia/Shanghai               
RUN echo "${TIME_ZONE}" > /etc/timezone 
RUN ln -sf /usr/share/zoneinfo/${TIME_ZONE} /etc/localtime
ENTRYPOINT ["dotnet", "SIE.ScheduleServer.dll"]
