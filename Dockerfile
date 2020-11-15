FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as runtime
COPY --from=build /app/publish /app/publish
WORKDIR /app/publish
EXPOSE 80
CMD ["dotnet", "BervProject.WebApi.Boilerplate.dll"]