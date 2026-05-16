FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OrderProductApi.csproj", "./"]
RUN dotnet restore "OrderProductApi.csproj"
COPY . .
RUN dotnet build "OrderProductApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderProductApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProductApi.dll"]
