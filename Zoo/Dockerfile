FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out 

FROM microsoft/dotnet:runtime
WORKDIR /app 
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "Zoo.dll"] 
