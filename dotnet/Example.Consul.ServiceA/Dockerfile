FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
EXPOSE 80

COPY . /app

ENTRYPOINT ["dotnet", "Example.Consul.ServiceA.dll"]