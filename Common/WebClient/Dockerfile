FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Projects/SMOM/WebClient/WebClient.csproj", "Projects/SMOM/WebClient/"]
COPY ["nuget.config", "Projects/SMOM/WebClient/"]
COPY ["Modules/Products/SIE.Web.QMS/SIE.Web.QMS.csproj", "Modules/Products/SIE.Web.QMS/"]
COPY ["Modules/Products/SIE.Web.Defects/SIE.Web.Defects.csproj", "Modules/Products/SIE.Web.Defects/"]
COPY ["Modules/Products/SIE.Defects/SIE.Defects.csproj", "Modules/Products/SIE.Defects/"]
COPY ["Modules/Items/SIE.Items/SIE.Items.csproj", "Modules/Items/SIE.Items/"]
COPY ["Modules/Products/SIE.Core/SIE.Core.csproj", "Modules/Products/SIE.Core/"]
COPY ["Modules/Resources/SIE.Resources/SIE.Resources.csproj", "Modules/Resources/SIE.Resources/"]
COPY ["Modules/Products/SIE.EventMessages/SIE.EventMessages.csproj", "Modules/Products/SIE.EventMessages/"]
COPY ["Modules/Products/SIE.Web.Core/SIE.Web.Core.csproj", "Modules/Products/SIE.Web.Core/"]
COPY ["Modules/Products/SIE.QMS/SIE.QMS.csproj", "Modules/Products/SIE.QMS/"]
COPY ["Modules/Products/SIE.Tech/SIE.Tech.csproj", "Modules/Products/SIE.Tech/"]
COPY ["Modules/Products/SIE.Packages/SIE.Packages.csproj", "Modules/Products/SIE.Packages/"]
COPY ["Modules/CSM/SIE.CSM/SIE.CSM.csproj", "Modules/CSM/SIE.CSM/"]
COPY ["Modules/Products/SIE.Warehouses/SIE.Warehouses.csproj", "Modules/Products/SIE.Warehouses/"]
COPY ["Modules/Products/SIE.WorkBenchCommon/SIE.WorkBenchCommon.csproj", "Modules/Products/SIE.WorkBenchCommon/"]
COPY ["Modules/Items/SIE.Web.Items/SIE.Web.Items.csproj", "Modules/Items/SIE.Web.Items/"]
COPY ["Modules/Products/SIE.Web.OQC/SIE.Web.OQC.csproj", "Modules/Products/SIE.Web.OQC/"]
COPY ["Modules/Products/SIE.Web.IQC/SIE.Web.IQC.csproj", "Modules/Products/SIE.Web.IQC/"]
COPY ["Modules/Products/SIE.IQC/SIE.IQC.csproj", "Modules/Products/SIE.IQC/"]
COPY ["Modules/Products/SIE.OQC/SIE.OQC.csproj", "Modules/Products/SIE.OQC/"]
COPY ["Modules/Products/SIE.Web.WMS/SIE.Web.WMS.csproj", "Modules/Products/SIE.Web.WMS/"]
COPY ["Modules/Products/SIE.WMS/SIE.WMS.csproj", "Modules/Products/SIE.WMS/"]
COPY ["Modules/Products/SIE.Inventory/SIE.Inventory.csproj", "Modules/Products/SIE.Inventory/"]
COPY ["Modules/Products/SIE.Web.WMS.Shipment/SIE.Web.WMS.Shipment.csproj", "Modules/Products/SIE.Web.WMS.Shipment/"]
COPY ["Modules/CSM/SIE.Web.CSM/SIE.Web.CSM.csproj", "Modules/CSM/SIE.Web.CSM/"]
COPY ["Modules/Products/SIE.WMS.Shipment/SIE.WMS.Shipment.csproj", "Modules/Products/SIE.WMS.Shipment/"]
COPY ["Modules/Products/SIE.PO/SIE.PO.csproj", "Modules/Products/SIE.PO/"]
COPY ["Modules/Products/SIE.Web.WMS.Statistics/SIE.Web.WMS.Statistics.csproj", "Modules/Products/SIE.Web.WMS.Statistics/"]
COPY ["Modules/Products/SIE.WMS.Statistics/SIE.WMS.Statistics.csproj", "Modules/Products/SIE.WMS.Statistics/"]
COPY ["Modules/Products/SIE.WMS.INV/SIE.WMS.INV.csproj", "Modules/Products/SIE.WMS.INV/"]
COPY ["Modules/Products/SIE.WMS.Receipt/SIE.WMS.Receipt.csproj", "Modules/Products/SIE.WMS.Receipt/"]
COPY ["Modules/Products/SIE.Web.Warehouses/SIE.Web.Warehouses.csproj", "Modules/Products/SIE.Web.Warehouses/"]
COPY ["Modules/Products/SIE.Web.WMS.Receipt/SIE.Web.WMS.Receipt.csproj", "Modules/Products/SIE.Web.WMS.Receipt/"]
COPY ["Modules/Resources/SIE.Web.Resources/SIE.Web.Resources.csproj", "Modules/Resources/SIE.Web.Resources/"]
COPY ["Modules/Products/SIE.Web.PQC/SIE.Web.PQC.csproj", "Modules/Products/SIE.Web.PQC/"]
COPY ["Modules/Products/SIE.PQC/SIE.PQC.csproj", "Modules/Products/SIE.PQC/"]
COPY ["Modules/Products/SIE.Web.WMS.INV/SIE.Web.WMS.INV.csproj", "Modules/Products/SIE.Web.WMS.INV/"]
COPY ["Modules/Products/SIE.Web.Packages/SIE.Web.Packages.csproj", "Modules/Products/SIE.Web.Packages/"]
COPY ["Modules/Products/SIE.Web.Inventory/SIE.Web.Inventory.csproj", "Modules/Products/SIE.Web.Inventory/"]
RUN dotnet restore "Projects/SMOM/WebClient/WebClient.csproj" --configfile "Projects/SMOM/WebClient/nuget.config"
COPY . .
WORKDIR "/src/Projects/SMOM/WebClient"
RUN dotnet build "WebClient.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebClient.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
WORKDIR .
# linux 连接oracle 时 timezone处理
ENV TIME_ZONE Asia/Shanghai               
RUN echo "${TIME_ZONE}" > /etc/timezone 
RUN ln -sf /usr/share/zoneinfo/${TIME_ZONE} /etc/localtime
ENTRYPOINT ["dotnet", "WebClient.dll"]