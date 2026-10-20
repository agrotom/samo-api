FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY . ./samo-api/
WORKDIR /source/samo-api
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
RUN mkdir -p wwwroot/uploads
CMD ["dotnet", "ef", "database", "update"]
ENTRYPOINT ["dotnet", "samo-api.dll"]