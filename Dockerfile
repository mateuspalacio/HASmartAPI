FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /cert
RUN dotnet dev-certs https -ep hasmartapi.pfx -p c3rtp455w0rd

WORKDIR /app

# copy csproj and restore as distinct layers
COPY HaSmart.WebApi/*.csproj /app/
COPY HaSmart.Infrastructure/*.csproj /HaSmart.Infrastructure/
COPY HaSmart.Core/*.csproj /HaSmart.Core/
COPY HaSmart.WebApi/HaSmart.WebApi.sln .
# WORKDIR /app/dotnetapp
RUN dotnet restore

# copy and publish app and libraries
# WORKDIR /app/
COPY ./* .
#COPY HaSmart.Infrastructure/. ./HaSmart.Infrastructure/
#COPY HaSmart.Core/. ./HaSmart.Core/
# WORKDIR /app/HaSmart.WebApi
RUN dotnet publish HASmart.WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /certs/https
COPY --from=build /cert/hasmartapi.pfx ./

WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "HASmart.WebApi.dll"]