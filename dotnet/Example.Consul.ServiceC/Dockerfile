FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Example.Consul.ServiceC/Example.Consul.ServiceC.csproj", "Example.Consul.ServiceC/"]
RUN dotnet restore "Example.Consul.ServiceC/Example.Consul.ServiceC.csproj"
COPY . .
WORKDIR "/src/Example.Consul.ServiceC"
RUN dotnet build "Example.Consul.ServiceC.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Example.Consul.ServiceC.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Example.Consul.ServiceC.dll"]