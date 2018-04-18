FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.0-runtime 
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "NetCoreConsole.dll"]