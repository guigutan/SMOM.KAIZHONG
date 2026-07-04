FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["nuget.config", "Projects/SMOM/SIE.WebApiHost/"]
COPY ["Projects/SMOM/SIE.WebApiHost/SIE.WebApiHost.csproj", "Projects/SMOM/SIE.WebApiHost/"]
COPY ["Modules/Products/SIE.Inventory/SIE.Inventory.csproj", "Modules/Products/SIE.Inventory/"]
COPY ["Modules/CSM/SIE.CSM/SIE.CSM.csproj", "Modules/CSM/SIE.CSM/"]
COPY ["Modules/Products/SIE.Core/SIE.Core.csproj", "Modules/Products/SIE.Core/"]
COPY ["Modules/Items/SIE.Items/SIE.Items.csproj", "Modules/Items/SIE.Items/"]
COPY ["Modules/Resources/SIE.Resources/SIE.Resources.csproj", "Modules/Resources/SIE.Resources/"]
COPY ["Modules/Products/SIE.EventMessages/SIE.EventMessages.csproj", "Modules/Products/SIE.EventMessages/"]
COPY ["Modules/Products/SIE.Warehouses/SIE.Warehouses.csproj", "Modules/Products/SIE.Warehouses/"]
COPY ["Modules/Products/SIE.Equipments/SIE.Equipments.csproj", "Modules/Products/SIE.Equipments/"]
COPY ["Modules/Products/SIE.MES.TeamManagement/SIE.MES.TeamManagement.csproj", "Modules/Products/SIE.MES.TeamManagement/"]
COPY ["Modules/Products/SIE.SMT/SIE.SMT.csproj", "Modules/Products/SIE.SMT/"]
COPY ["Modules/Products/SIE.SPC/SIE.SPC.csproj", "Modules/Products/SIE.SPC/"]
COPY ["Modules/Products/SIE.WorkBenchChartBase/SIE.WorkBenchChartBase.csproj", "Modules/Products/SIE.WorkBenchChartBase/"]
COPY ["Modules/Products/SIE.PDCA/SIE.PDCA.csproj", "Modules/Products/SIE.PDCA/"]
COPY ["Modules/Products/SIE.Tech/SIE.Tech.csproj", "Modules/Products/SIE.Tech/"]
COPY ["Modules/Products/SIE.Defects/SIE.Defects.csproj", "Modules/Products/SIE.Defects/"]
COPY ["Modules/Products/SIE.Packages/SIE.Packages.csproj", "Modules/Products/SIE.Packages/"]
COPY ["Modules/Products/SIE.Quota/SIE.Quota.csproj", "Modules/Products/SIE.Quota/"]
COPY ["Modules/Products/SIE.ESop/SIE.ESop.csproj", "Modules/Products/SIE.ESop/"]
COPY ["Modules/Products/SIE.DIST/SIE.DIST.csproj", "Modules/Products/SIE.DIST/"]
COPY ["Modules/Products/SIE.WMS.Job/SIE.WMS.Job.csproj", "Modules/Products/SIE.WMS.Job/"]
COPY ["Modules/Products/SIE.WMS.Receipt/SIE.WMS.Receipt.csproj", "Modules/Products/SIE.WMS.Receipt/"]
COPY ["Modules/Products/SIE.WMS/SIE.WMS.csproj", "Modules/Products/SIE.WMS/"]
COPY ["Modules/Products/SIE.PO/SIE.PO.csproj", "Modules/Products/SIE.PO/"]
COPY ["Modules/Products/SIE.WMS.Portal/SIE.WMS.Portal.csproj", "Modules/Products/SIE.WMS.Portal/"]
COPY ["Modules/Products/SIE.WMS.Shipment/SIE.WMS.Shipment.csproj", "Modules/Products/SIE.WMS.Shipment/"]
COPY ["Modules/Products/SIE.MES.Workbench/SIE.MES.Workbench.csproj", "Modules/Products/SIE.MES.Workbench/"]
COPY ["Modules/APS/SIE.APS.Simulations/SIE.APS.Simulations.csproj", "Modules/APS/SIE.APS.Simulations/"]
COPY ["Modules/Products/SIE.MES.TeamManagement.Job/SIE.MES.TeamManagement.Job.csproj", "Modules/Products/SIE.MES.TeamManagement.Job/"]
COPY ["Modules/APS/SIE.APS.Scheduling/SIE.APS.Scheduling.csproj", "Modules/APS/SIE.APS.Scheduling/"]
COPY ["Modules/Products/SIE.ProductIntfc/SIE.ProductIntfc.csproj", "Modules/Products/SIE.ProductIntfc/"]
COPY ["Modules/Products/SIE.WMS.Statistics/SIE.WMS.Statistics.csproj", "Modules/Products/SIE.WMS.Statistics/"]
COPY ["Modules/Products/SIE.WMS.INV/SIE.WMS.INV.csproj", "Modules/Products/SIE.WMS.INV/"]
COPY ["Modules/Products/SIE.IQC/SIE.IQC.csproj", "Modules/Products/SIE.IQC/"]
COPY ["Modules/Products/SIE.QMS/SIE.QMS.csproj", "Modules/Products/SIE.QMS/"]
COPY ["Modules/Products/SIE.WorkBenchCommon/SIE.WorkBenchCommon.csproj", "Modules/Products/SIE.WorkBenchCommon/"]
COPY ["Modules/Products/SIE.Barcodes/SIE.Barcodes.csproj", "Modules/Products/SIE.Barcodes/"]
COPY ["Modules/APS/SIE.APS/SIE.APS.csproj", "Modules/APS/SIE.APS/"]
COPY ["Modules/Products/SIE.MES/SIE.MES.csproj", "Modules/Products/SIE.MES/"]
COPY ["Modules/Products/SIE.MES.DashBoard/SIE.MES.DashBoard.csproj", "Modules/Products/SIE.MES.DashBoard/"]
COPY ["Modules/Products/SIE.MI.QMS.Job/SIE.MI.QMS.Job.csproj", "Modules/Products/SIE.MI.QMS.Job/"]
COPY ["Modules/Products/SIE.PQC/SIE.PQC.csproj", "Modules/Products/SIE.PQC/"]
COPY ["Modules/Products/SIE.MI.QMS.WorkBench/SIE.MI.QMS.WorkBench.csproj", "Modules/Products/SIE.MI.QMS.WorkBench/"]
COPY ["Modules/Products/SIE.MES.Statistics/SIE.MES.Statistics.csproj", "Modules/Products/SIE.MES.Statistics/"]
COPY ["Modules/Products/SIE.OQC/SIE.OQC.csproj", "Modules/Products/SIE.OQC/"]
COPY ["Modules/Products/SIE.MI.CommonJob/SIE.MI.CommonJob.csproj", "Modules/Products/SIE.MI.CommonJob/"]
COPY ["Modules/Products/SIE.QMS.WorkBench/SIE.QMS.WorkBench.csproj", "Modules/Products/SIE.QMS.WorkBench/"]
COPY ["Modules/Products/SIE.ProductInsp.Job/SIE.ProductInsp.Job.csproj", "Modules/Products/SIE.ProductInsp.Job/"]
COPY ["Modules/Products/SIE.MES.Job/SIE.MES.Job.csproj", "Modules/Products/SIE.MES.Job/"]
RUN dotnet restore "Projects/SMOM/SIE.WebApiHost/SIE.WebApiHost.csproj" --configfile "Projects/SMOM/SIE.WebApiHost/nuget.config"
COPY . .
WORKDIR "/src/Projects/SMOM/SIE.WebApiHost"
RUN dotnet build "SIE.WebApiHost.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SIE.WebApiHost.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
WORKDIR .
# linux 连接oracle 时 timezone处理
ENV TIME_ZONE Asia/Shanghai               
RUN echo "${TIME_ZONE}" > /etc/timezone 
RUN ln -sf /usr/share/zoneinfo/${TIME_ZONE} /etc/localtime
ENTRYPOINT ["dotnet", "SIE.WebApiHost.dll"]