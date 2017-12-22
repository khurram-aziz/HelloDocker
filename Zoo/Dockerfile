FROM microsoft/dotnet:2.0-sdk AS builder
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out 

FROM microsoft/dotnet:2.0-runtime
WORKDIR /app 
COPY --from=builder /app/out ./
ENTRYPOINT ["dotnet", "Zoo.dll"] 
