FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app

COPY . /app

# set up network
ENV ASPNETCORE_URLS http://+:80

ENTRYPOINT ["dotnet", "Example.Consul.ServiceA.dll"]