FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 17875
EXPOSE 44318
EXPOSE 80

#OmadaAPI
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["OmadaAPI/OmadaAPI.csproj", "OmadaAPI/"]
RUN dotnet restore "OmadaAPI/OmadaAPI.csproj"
COPY . .
WORKDIR "/src/OmadaAPI"
RUN dotnet build "OmadaAPI.csproj" -c Release -o /app
WORKDIR /src
COPY ["OmadaVoucherProxy/OmadaVoucherProxy.csproj", "OmadaVoucherProxy/"]
RUN dotnet restore "OmadaVoucherProxy/OmadaVoucherProxy.csproj"
COPY . .
WORKDIR "/src/OmadaVoucherProxy"
RUN dotnet build "OmadaVoucherProxy.csproj" -c Release -o /app

FROM build AS publish
WORKDIR "/src/OmadaAPI"
RUN dotnet publish "OmadaAPI.csproj" -c Release -o /app
WORKDIR "/src/OmadaVoucherProxy"
RUN dotnet publish "OmadaVoucherProxy.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OmadaVoucherProxy.dll"]