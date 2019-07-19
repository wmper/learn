FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Example.Consul.ServiceB/Example.Consul.ServiceB.csproj", "Example.Consul.ServiceB/"]
RUN dotnet restore "Example.Consul.ServiceB/Example.Consul.ServiceB.csproj"
COPY . .
WORKDIR "/src/Example.Consul.ServiceB"
RUN dotnet build "Example.Consul.ServiceB.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Example.Consul.ServiceB.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Example.Consul.ServiceB.dll"]