FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["Src/MaleFashion.Web/MaleFashion.Web.csproj", "MaleFashion.Web/"]
COPY ["Src/MaleFashion.Domain/MaleFashion.Domain.csproj", "MaleFashion.Domain/"]
COPY ["Src/MaleFashion.Application/MaleFashion.Application.csproj", "MaleFashion.Application/"]
COPY ["Src/MaleFashion.Infrastructure/MaleFashion.Infrastructure.csproj", "MaleFashion.Infrastructure/"]
RUN dotnet restore "MaleFashion.Web/MaleFashion.Web.csproj"

COPY ./Src/ .
WORKDIR "/src/MaleFashion.Web"
RUN dotnet build "MaleFashion.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MaleFashion.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "MaleFashion.Web.dll"]