FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /src
COPY BAuth.csproj ./
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
RUN dotnet publish -c Release -o /app -r linux-x64

FROM microsoft/dotnet:2.1-aspnetcore-runtime
LABEL maintainer="khuziz@hotmail.com"
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "BAuth.dll"]