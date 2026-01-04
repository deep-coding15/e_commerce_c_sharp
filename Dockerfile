FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["E-commerce_c_charp.csproj", "./"]
RUN dotnet restore "E-commerce_c_charp.csproj"
COPY . .
RUN dotnet build "E-commerce_c_charp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E-commerce_c_charp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-commerce_c_charp.dll"]